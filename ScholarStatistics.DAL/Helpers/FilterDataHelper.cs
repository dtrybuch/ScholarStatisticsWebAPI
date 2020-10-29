using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using ScholarStatistics.DAL.Interfaces;
using ScholarStatistics.DAL.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace ScholarStatistics.DAL.Helpers
{
    public class FilterDataHelper : IFilterDataHelper
    {
        private readonly IAuthorsRepository _authorsRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IAffiliationsRepository _affiliationsRepository;
        private readonly IPublicationsRepository _publicationsRepository;
        private readonly ILogger<FilterDataHelper> _logger;
        private readonly string scorpusApiKey = "7dbe54dab14260feabebf2b085778135";
        private readonly DateTime startDateTime = new DateTime(1, 1, 1);
        public FilterDataHelper(IAuthorsRepository authorsRepository, ICategoriesRepository categoriesRepository,
            IAffiliationsRepository affiliationsRepository, IPublicationsRepository publicationsRepository,
            ILogger<FilterDataHelper> logger)
        {
            _authorsRepository = authorsRepository;
            _categoriesRepository = categoriesRepository;
            _affiliationsRepository = affiliationsRepository;
            _publicationsRepository = publicationsRepository;
            _logger = logger;
            SetRatioPublications();
        }

        public void SaveArxivData()
        {
            int step = 2000;
            var categories = _categoriesRepository.GetCategories().ToArray();
            var index = Array.FindIndex<Category>(categories, category => category.Code == "physics.acc-ph");
            var leftCategories = categories.TakeLast(categories.Length - index).ToList();
            foreach (var mainCategory in leftCategories)
            {
                for (int i = 0; i < 10000; i += step)
                {
                    try
                    {
                        Debug.WriteLine($"\nDateTime: {DateTime.Now}. \n Main Category: {mainCategory.Code}, i = {i}\n");
                        using (XmlReader reader = XmlReader.Create("https://export.arxiv.org/api/query?search_query=cat:" + mainCategory.Code + $"&max_results={step}&start={i}&sortBy=lastUpdatedDate"))
                        {
                            var feed = SyndicationFeed.Load(reader);
                            foreach (var item in feed.Items)
                            {
                                var authorsList = new List<Author>();
                                foreach (var author in item.Authors)
                                {
                                    var (firstName, lastName) = GetAuthorsNames(author.Name);
                                    authorsList.Add(new Author()
                                    {
                                        FirstName = firstName,
                                        LastName = lastName
                                    });
                                }
                                var unaccepted = authorsList.Where(author => author.FirstName.EndsWith(".") && author.FirstName.Length < 6);
                                if (unaccepted.Count() > 0) break;
                                else
                                {
                                    var categorieIds = new List<int>();
                                    foreach (var categoryItem in item.Categories)
                                    {
                                        var categoryFromDB = _categoriesRepository.QueryCategories(category => category.Code == categoryItem.Name).ToArray();
                                        if (categoryFromDB.Count() > 0) categorieIds.Add(categoryFromDB[0].CategoryId);
                                    }
                                    var authorIds = _authorsRepository.AddAuthors(authorsList);
                                    var publication = new Publication()
                                    {
                                        AuthorsFK = authorIds,
                                        Title = item.Title.Text,
                                        DateOfAddedToArxiv = item.PublishDate.DateTime,
                                        CategoriesFK = categorieIds
                                    };
                                    _publicationsRepository.AddPublication(publication);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                    }  
                }
            }
        }

        //First Name first
        private (string, string) GetAuthorsNames(string name)
        {
            var names = name.Split();
            var firstName = "";
            if (names.Count() > 2)
                firstName = string.Join(" ", names, 0, names.Count() - 1);
            else if (names.Count() == 2)
                firstName = names[0];
            var lastName = names[^1];
            return (firstName, lastName);
        }

        //Last Name first
        private (string, string) GetLastAuthorsNames(string name)
        {
            var names = name.Split();
            var firstName = "";
            if (names.Count() > 2)
                firstName = string.Join(" ", names, 1, names.Count() - 1);
            else if (names.Count() == 2)
                firstName = names[^1];
            var lastName = names[0];
            return (firstName, lastName);
        }

        private void AddAffiliationToAuthor(XElement ele, Author author)
        {
            string name = "", city = "", country = "";
            foreach (var value in ele.Elements())
            {
                if (value.Name.LocalName == "affilname")
                    name = value.Value;
                if (value.Name.LocalName == "affiliation-city")
                    city = value.Value;
                if (value.Name.LocalName == "affiliation-country")
                    country = value.Value;
            }
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(city) && !string.IsNullOrEmpty(country))
            {
                var affiliation = new Affiliation()
                {
                    Name = name,
                    City = city,
                    Country = country
                };
                var id = _affiliationsRepository.AddAffiliation(affiliation);
                if (id != 0)
                {
                    author.AffiliationFK = id;
                    _authorsRepository.UpdateAuthor(author);
                }
            }
        }

        public void SaveAuthorsAffiliationFromScopus()
        {
            var authors = _authorsRepository.QueryAuthors(author => author.AffiliationFK == 0);
            var i = 0;
            var addedAffiliations = 0;
            var authorsCount = authors.Count();
            foreach (var author in authors)
            {
                Debug.WriteLine($"Remained clients: {authorsCount - i}");
                Debug.WriteLine($"Added affiliations: {addedAffiliations}");
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/atom+xml"));
                        var url = "https://api.elsevier.com/content/search/scopus?query=AUTH" + HttpUtility.UrlEncode("(" + author.FirstName + " " + author.LastName + ")") + $"&apiKey={scorpusApiKey}";
                        Debug.WriteLine(url);
                        i++;
                        using (var response = client.GetAsync(url).Result)
                        {

                            response.EnsureSuccessStatusCode();
                            string responseBody = response.Content.ReadAsStringAsync().Result;
                            TextReader sr = new StringReader(responseBody);
                            XmlReader reader = XmlReader.Create(sr);
                            var feed = SyndicationFeed.Load(reader);
                            var isProperlyAuthor = false;
                            foreach (var item in feed.Items)
                            {
                                if (isProperlyAuthor)
                                    break;
                                foreach (var extension in item.ElementExtensions)
                                {
                                    XElement ele = extension.GetObject<XElement>();
                                    if (ele.Name.LocalName == "creator")
                                    {
                                        var (firstName, lastName) = GetLastAuthorsNames(ele.Value);
                                        if (firstName.Length > 0)
                                        {
                                            if (author.FirstName.StartsWith(firstName[0]) && string.Compare(author.LastName, lastName, CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace) == 0)
                                            {
                                                isProperlyAuthor = true;
                                            }
                                        }
                                    }
                                    if (ele.Name.LocalName == "affiliation")
                                    {
                                        if (isProperlyAuthor)
                                        {
                                            AddAffiliationToAuthor(ele, author);
                                            addedAffiliations++;
                                        }
                                    }
                                }
                            } 
                        }
                    }
                    catch(Exception e)
                    {
                        _logger.LogError(e.Message);
                    }


                }
            }
                    
        }
        public void SavePublicationsFromScopus()
        {

            var publications = _publicationsRepository.QueryPublications(publication => publication.DateOfPublished == startDateTime).ToArray();
            int countOfAddedDate = 0;
            foreach (var publication in publications)
            {

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/atom+xml"));
                        var url = "https://api.elsevier.com/content/search/scopus?query=TITLE" + HttpUtility.UrlEncode("(" + publication.Title.Replace("\n", "").Replace("\r", "") + ")") + $"&apiKey={scorpusApiKey}";
                        Debug.WriteLine(url);
                        Debug.WriteLine(countOfAddedDate);
                        using (var response = client.GetAsync(url).Result)
                        {

                            response.EnsureSuccessStatusCode();
                            string responseBody = response.Content.ReadAsStringAsync().Result;
                            TextReader sr = new StringReader(responseBody);
                            XmlReader reader = XmlReader.Create(sr);
                            var feed = SyndicationFeed.Load(reader);
                            foreach (var item in feed.Items)
                            {
                                Author mainAuthor = null;
                                foreach (var extension in item.ElementExtensions)
                                {
                                    XElement ele = extension.GetObject<XElement>();
                                    //if (ele.Name.LocalName == "title")
                                    //{
                                    //    var val1 = string.Concat(ele.Value.Replace("\n", "").Replace("\r", "").ToLower().Where(c => !Char.IsWhiteSpace(c)));
                                    //    var val2 = string.Concat(publication.Title.Replace("\n", "").Replace("\r", "").ToLower().Where(c => !Char.IsWhiteSpace(c)));
                                    //}
                                    //if (ele.Name.LocalName == "title" && String.Concat(ele.Value.Replace("\n", "").Replace("\r", "").ToLower().Where(c => !Char.IsWhiteSpace(c))) != string.Concat(publication.Title.Replace("\n", "").Replace("\r", "").ToLower().Where(c => !Char.IsWhiteSpace(c)))) break;
                                    if (ele.Name.LocalName == "coverDate")
                                    {
                                        countOfAddedDate++;
                                        publication.DateOfPublished = DateTime.Parse(ele.Value);
                                        _publicationsRepository.UpdatePublication(publication);
                                    }
                                    if (ele.Name.LocalName == "creator")
                                    {
                                        var (firstName, lastName) = GetLastAuthorsNames(ele.Value);
                                        if (firstName.Length > 0)
                                        {
                                            var authorFromDB = _authorsRepository.QueryAuthors(author => author.FirstName.StartsWith(firstName[0]) && author.LastName == lastName);
                                            if (authorFromDB.Count() == 1)
                                            {
                                                mainAuthor = authorFromDB.ToArray()[0];
                                            }
                                        }
                                    }
                                    if (ele.Name.LocalName == "affiliation")
                                    {
                                        if (mainAuthor != null)
                                        {
                                            AddAffiliationToAuthor(ele, mainAuthor);
                                        }
                                    }

                                }
                            }
                       
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                    }
                }

            }

        }
        public void SetDifferenceBetweenPublications()
        {
            foreach (var category in _categoriesRepository.GetCategories())
            {
                var publicatedPublications = _publicationsRepository.QueryPublications(publication => publication.DateOfPublished != startDateTime 
                                            && publication.DateOfPublished > publication.DateOfAddedToArxiv
                                            && publication.CategoriesFK.Contains(category.CategoryId)).ToList();
                var average = 0D;
                foreach (var publication in publicatedPublications)
                {
                    var dateOffset1 = publication.DateOfPublished.Ticks;
                    var dateOffset2 = publication.DateOfAddedToArxiv.Ticks;
                    average += (dateOffset1 - dateOffset2)/ (double)publicatedPublications.Count();
                }
                var time = new DateTime((long)average);
                category.DifferenceBetweenPublications = time;
                _categoriesRepository.UpdateCategory(category);
            }
        }
        public void SetRatioPublications()
        {
            foreach (var category in _categoriesRepository.GetCategories())
            {
                var allPublications = _publicationsRepository.QueryPublications(publication => publication.CategoriesFK.Contains(category.CategoryId)).ToList();
                var notAddedPublications = allPublications.Where(publication => publication.DateOfPublished == startDateTime);
                category.RatioPublications = (float)notAddedPublications.Count() / (float)allPublications.Count();
                _categoriesRepository.UpdateCategory(category);
            }
        }
    }
}

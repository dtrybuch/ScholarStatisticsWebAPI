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
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IAffiliationsRepository _affiliationsRepository;
        private readonly IPublicationsRepository _publicationsRepository;
        private readonly ILogger<FilterDataHelper> _logger;
        private readonly List<string> scopusApiKeys = new List<string>()
        {
            //"ebc18d915ccd203672b713a312ee498b",
            //"83ddf2650464dd50d9509eb5c655043a",
            //"7dbe54dab14260feabebf2b085778135",
            //"9a3a38e36bde9d48403be5af2f89d7fa",
            //"a3203f9083266cacc6dcc57dfa337956",
            "41ebcdbc13804f194d086b3e721a7fb1",
            "aa8e9c4f1dda149bf5c9ebe32de1cb9c"
        };
        private string scopusApiKey;
        private readonly DateTime startDateTime = new DateTime(1, 1, 1);
        public FilterDataHelper(ICategoriesRepository categoriesRepository,
            IAffiliationsRepository affiliationsRepository, IPublicationsRepository publicationsRepository,
            ILogger<FilterDataHelper> logger)
        {
            _categoriesRepository = categoriesRepository;
            _affiliationsRepository = affiliationsRepository;
            _publicationsRepository = publicationsRepository;
            _logger = logger;
            scopusApiKey = GetAPIKey();
            //SetDifferenceBetweenPublicationsInDays();
            //SetRatioPublications();
            //SetCountryToCategories();
            //SetAffiliationLatAndLong();
            //SaveCountOfDays();
            //AddCountOfPublicationsToCategory();
            //AddCountOfTopTenCategoriesToAffiliation();
            AddCategoriesToAffiliation();
        }

        public void SaveArxivDataByCategories()
        {
            int step = 500;
            var categories = _categoriesRepository.GetCategories().ToArray();
            var index = Array.FindIndex<Category>(categories, category => category.Code == "q-fin.CP");
            var leftCategories = categories.TakeLast(categories.Length - index).ToList();
            //var leftCategories = categories.TakeLast(categories.Length).ToList();
            var publications = _publicationsRepository.GetPublications();
            foreach (var mainCategory in leftCategories)
            {
                for (int i = 0; i < 500; i += step)
                {
                    try
                    {
                        Debug.WriteLine($"\nDateTime: {DateTime.Now}. \n Main Category: {mainCategory.Code}, i = {i}\n");
                        using (XmlReader reader = XmlReader.Create("https://export.arxiv.org/api/query?search_query=cat:" + mainCategory.Code + $"&max_results={step}&start={i}&sortBy=lastUpdatedDate"))
                        {
                            Debug.WriteLine($"\nDateTime: {DateTime.Now}. \n Main Category: {mainCategory.Code}, i = {i}, get from website done.\n");
                            var feed = SyndicationFeed.Load(reader);
                            foreach (var item in feed.Items)
                            {
                                if (publications.Where(publication => GetCuttedString(publication.Title) == GetCuttedString(item.Title.Text)).Any()) continue;                                                         
                                var categorieIds = new List<int>();
                                var publication = new Publication();
                                foreach (var categoryItem in item.Categories)
                                {
                                    var categoryFromDB = _categoriesRepository.QueryCategories(category => category.Code == categoryItem.Name).ToArray();
                                    if (categoryFromDB.Count() > 0) categorieIds.Add(categoryFromDB[0].CategoryId);
                                }    
                                publication = new Publication()
                                {
                                    Title = item.Title.Text,
                                    DateOfAddedToArxiv = item.PublishDate.DateTime,
                                    CategoriesFK = categorieIds
                                };                               
                                _publicationsRepository.AddPublication(publication);
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

        private static string GetCuttedString(string value)
        {
            return string.Concat(value.Replace("\n", "").Replace("\r", "").ToLower().Where(c => !char.IsWhiteSpace(c)));
        }

        private void AddAffiliationToPublication(XElement ele, Publication publication)
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
                    publication.AffiliationFK = id;
                    _publicationsRepository.UpdatePublication(publication);
                }
            }
        }
        public void SavePublicationsFromScopus()
        {
            var rnd = new Random();
            var publications = _publicationsRepository.QueryPublications(publication => publication.WasCheckedInScopus == false).ToArray();
            int countOfAddedDate = 0;
            int allPub = publications.Count();
            foreach (var publication in publications)
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/atom+xml"));
                        var url = "https://api.elsevier.com/content/search/scopus?query=TITLE" + HttpUtility.UrlEncode("(" + publication.Title.Replace("\n", "").Replace("\r", "") + ")") + $"&apiKey={scopusApiKey}";
                        Debug.WriteLine(url);
                        Debug.WriteLine($"countOfAddedDate : {countOfAddedDate}");
                        using (var response = client.GetAsync(url).Result)
                        {
                            Debug.WriteLine($"Left: {allPub--}");
                            if(response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                                scopusApiKey = GetAPIKey();
                            if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                            {
                                publication.WasCheckedInScopus = true;
                                _publicationsRepository.UpdatePublication(publication);
                            }
                            response.EnsureSuccessStatusCode();
                            string responseBody = response.Content.ReadAsStringAsync().Result;
                            TextReader sr = new StringReader(responseBody);
                            XmlReader reader = XmlReader.Create(sr);
                            var feed = SyndicationFeed.Load(reader);
                            if(feed.Items.Any())
                            {
                                var item = feed.Items.ToArray()[0];
                                foreach (var extension in item.ElementExtensions)
                                {
                                    XElement ele = extension.GetObject<XElement>();
                                    //if (ele.Name.LocalName == "title")
                                    //{
                                    //    var val1 = string.Concat(ele.Value.Replace("\n", "").Replace("\r", "").ToLower().Where(c => !Char.IsWhiteSpace(c)));
                                    //    var val2 = string.Concat(publication.Title.Replace("\n", "").Replace("\r", "").ToLower().Where(c => !Char.IsWhiteSpace(c)));
                                    //}
                                    //if (ele.Name.LocalName == "title" && String.Concat(ele.Value.Replace("\n", "").Replace("\r", "").ToLower().Where(c => !Char.IsWhiteSpace(c))) != string.Concat(publication.Title.Replace("\n", "").Replace("\r", "").ToLower().Where(c => !Char.IsWhiteSpace(c)))) break;
                                    if (ele.Name.LocalName == "error")
                                    {
                                        Debug.WriteLine(ele.Value);
                                        break;
                                    }
                                    if (ele.Name.LocalName == "coverDate")
                                    {
                                        countOfAddedDate++;
                                        publication.DateOfPublished = DateTime.Parse(ele.Value);
                                    }
                                    if (ele.Name.LocalName == "citedby-count")
                                    {
                                        publication.CountOfCited = int.Parse(ele.Value);
                                    }
                                    if (ele.Name.LocalName == "affiliation")
                                    {
                                        AddAffiliationToPublication(ele, publication);
                                    }
                                }
                            }
                            publication.WasCheckedInScopus = true;
                            _publicationsRepository.UpdatePublication(publication);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                    }
                }

            }

        }
        private string GetAPIKey()
        {
            var rnd = new Random();
            return scopusApiKeys[rnd.Next(0, scopusApiKeys.Count - 1)];
        }
        public void SetDifferenceBetweenPublicationsInDays()
        {
            foreach (var category in _categoriesRepository.GetCategories())
            {
                var publicatedPublications = _publicationsRepository.QueryPublications(publication => publication.DateOfPublished != startDateTime 
                                            && publication.DateOfPublished > publication.DateOfAddedToArxiv
                                            && publication.CategoriesFK.Contains(category.CategoryId)).ToList();
                var average = 0D;
                foreach (var publication in publicatedPublications)
                {
                    var dateOffset1 = publication.DateOfPublished;
                    var dateOffset2 = publication.DateOfAddedToArxiv;
                    average += (dateOffset1 - dateOffset2).TotalDays/ (double)publicatedPublications.Count();
                }
                category.DifferenceBetweenPublicationsInDays = average;
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
        public void SetCountryToCategories()
        {
            foreach (var category in _categoriesRepository.GetCategories())
            {
                var allPublications = _publicationsRepository.QueryPublications(publication => publication.CategoriesFK.Contains(category.CategoryId) && publication.AffiliationFK != 0).ToList();
                var affilliations = new List<Affiliation>();
                foreach (var publication in allPublications)
                {
                    affilliations.AddRange(_affiliationsRepository.QueryAffiliations(affilliation => affilliation.AffiliationId == publication.AffiliationFK));
                }
                var country = affilliations.GroupBy(i => i.Country).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
                category.MainCountry = country;
                _categoriesRepository.UpdateCategory(category);
            }
        }
        public void SetAffiliationLatAndLong()
        {
            var affiliationsQuery = _affiliationsRepository.QueryAffiliations(affiliation => affiliation.Lattitude == 0 && affiliation.Longitude == 0);
            //var affiliationsQuery = _affiliationsRepository.GetAffiliations();
            var left = affiliationsQuery.Count();
            foreach (var affiliation in affiliationsQuery)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        //var url = $"https://api.opencagedata.com/geocode/v1/json?q={HttpUtility.UrlEncode(affiliation.City)}%2C%{HttpUtility.UrlEncode(affiliation.Country)}&language=en&key=5cc3df6bad704909ad1e80dcc014234b";
                        var url = $"https://api.opencagedata.com/geocode/v1/json?q={HttpUtility.UrlEncode(affiliation.Name)}&language=en&key=5cc3df6bad704909ad1e80dcc014234b";
                        Debug.WriteLine($"Left: {left--}");
                        Debug.WriteLine(url);
                        using (var response = client.GetAsync(url).Result)
                        {
                            response.EnsureSuccessStatusCode();
                            string responseBody = response.Content.ReadAsStringAsync().Result;
                            var jsonObject = JObject.Parse(responseBody);
                            var result = ((JArray)jsonObject.GetValue("results"))[0];
                            var geometry = result["geometry"];
                            affiliation.Lattitude = (double)geometry["lat"];
                            affiliation.Longitude = (double)geometry["lng"];
                            _affiliationsRepository.UpdateAffiliation(affiliation);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        public void SaveCountOfDays()
        {
            foreach (var category in _categoriesRepository.GetCategories())
            {
                ClearCategory(category);
                var publicatedPublications = _publicationsRepository.QueryPublications(publication => publication.DateOfPublished != startDateTime
                                            && publication.DateOfPublished > publication.DateOfAddedToArxiv
                                            && publication.CategoriesFK.Contains(category.CategoryId)).ToList();
                foreach (var publication in publicatedPublications)
                {
                    SaveToProperlyDay(category, publication.DateOfPublished.ToString("dddd", new CultureInfo("en-US")));
                }
                SavePercentageOfDays(category, publicatedPublications.Count);
                _categoriesRepository.UpdateCategory(category);
            }
        }
        private void SavePercentageOfDays(Category category, int size)
        {
            category.PercentageOfMondays = (float) category.CountOfMondays / (float) size * 100;
            category.PercentageOfTuesdays = (float) category.CountOfTuesdays / (float) size * 100;
            category.PercentageOfWednesdays = (float) category.CountOfWednesdays / (float) size * 100;
            category.PercentageOfThursdays = (float) category.CountOfThursdays / (float) size * 100;
            category.PercentageOfFridays = (float) category.CountOfFridays / (float) size * 100;
            category.PercentageOfSaturdays = (float) category.CountOfSaturdays / (float) size * 100;
            category.PercentageOfSundays = (float) category.CountOfSundays / (float) size * 100;
        }
        private void ClearCategory(Category category)
        {
            category.CountOfMondays = 0;
            category.CountOfTuesdays = 0;
            category.CountOfWednesdays = 0;
            category.CountOfThursdays = 0;
            category.CountOfFridays = 0;
            category.CountOfSaturdays = 0;
            category.CountOfSundays = 0;
        }
        private void SaveToProperlyDay(Category category, string day)
        {
            switch (day)
            {
                case "Monday":
                    category.CountOfMondays++;
                    break;
                case "Tuesday":
                    category.CountOfTuesdays++;
                    break;
                case "Wednesday":
                    category.CountOfWednesdays++;
                    break;
                case "Thursday":
                    category.CountOfThursdays++;
                    break;
                case "Friday":
                    category.CountOfFridays++;
                    break;
                case "Saturday":
                    category.CountOfSaturdays++;
                    break;
                case "Sunday":
                    category.CountOfSundays++;
                    break;
                default:
                    break;
            }
        }
        private void AddCountOfPublicationsToCategory()
        {
            foreach (var category in _categoriesRepository.GetCategories())
            {
                var publications = _publicationsRepository.QueryPublications(publication => 
                                                    publication.CategoriesFK.Contains(category.CategoryId)).ToList();
                category.CountOfPublications = publications.Count();
                _categoriesRepository.UpdateCategory(category);
            }
        }

        private void AddCountOfTopTenCategoriesToAffiliation()
        {
            var categoryTopList = _categoriesRepository.GetCategories()
                .OrderByDescending(category => category.CountOfPublications).Take(10);
            var affiliations = _affiliationsRepository.GetAffiliations();
            foreach (var affiliation in affiliations)
            {
                affiliation.CountOfTopTenCategories = 0;
                foreach (var publication in _publicationsRepository.QueryPublications(publication => publication.AffiliationFK == affiliation.AffiliationId))
                {
                    if (publication.CategoriesFK.Any(id => categoryTopList.Any(category => category.CategoryId == id)))
                        affiliation.CountOfTopTenCategories++;
                }
            }
            _affiliationsRepository.UpdateAffiliations(affiliations);
        }
        private void AddCategoriesToAffiliation()
        {
            var affiliations = _affiliationsRepository.GetAffiliations();
            foreach (var affiliation in affiliations)
            {
                var categoriesIds = new List<int>();
                var publications = _publicationsRepository.QueryPublications(publication => publication.AffiliationFK == affiliation.AffiliationId);
                foreach (var publication in publications)
                {
                    categoriesIds.AddRange(publication.CategoriesFK);
                }
                affiliation.CategoriesUsingInThisAffiliationFK = categoriesIds.Distinct().ToList();
            }
            _affiliationsRepository.UpdateAffiliations(affiliations);
        }
    }
}

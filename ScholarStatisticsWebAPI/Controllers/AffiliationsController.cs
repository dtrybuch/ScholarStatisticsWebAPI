using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ScholarStatistics.DAL.Helpers;
using ScholarStatistics.DAL.Interfaces;
using ScholarStatistics.DAL.Models;

namespace ScholarStatisticsWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AffiliationsController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly IAffiliationsRepository _affiliationsRepository;
        private readonly IPublicationsRepository _publicationsRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IFilterDataHelper _filterDataHelper;
        public AffiliationsController(ILogger<CategoriesController> logger,
            IAffiliationsRepository affiliationsRepository, IPublicationsRepository publicationsRepository,
            ICategoriesRepository categoriesRepository, IFilterDataHelper filterDataHelper)
        {
            _logger = logger;
            _affiliationsRepository = affiliationsRepository;
            _publicationsRepository = publicationsRepository;
            _categoriesRepository = categoriesRepository;
            _filterDataHelper = filterDataHelper;
        }

        // GET: /Affiliations
        [HttpGet]
        public IEnumerable<Affiliation> Get()
        {
            return _affiliationsRepository.GetAffiliations();
        }

        //// GET: /Affiliations/5
        //[HttpGet("{id}")]
        //public List<Category> Get(int id)
        //{
        //    var categoriesIds = new List<int>();
        //    var publications = _publicationsRepository.QueryPublications(publication => publication.AffiliationFK == id);
        //    foreach (var publication in publications)
        //    {
        //        categoriesIds.AddRange(publication.CategoriesFK);
        //    }
        //    var categories = new List<Category>();
        //    foreach (var categoryId in categoriesIds.Distinct())
        //    {
        //        categories.Add(_categoriesRepository.GetCategoryById(categoryId));
        //    }
        //    return categories;
        //}
    }
}
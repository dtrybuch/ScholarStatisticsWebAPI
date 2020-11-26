using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScholarStatistics.DAL.Interfaces;
using ScholarStatistics.DAL.Models;

namespace ScholarStatisticsWebAPI.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class HeatMapController : Controller
    {
        private readonly IAffiliationsRepository _affiliationsRepository;
        private readonly IPublicationsRepository _publicationsRepository;
        private readonly IAffiliationCategoryRepository _affiliationCategoryRepository;
        public HeatMapController(IAffiliationsRepository affiliationsRepository, IPublicationsRepository publicationsRepository,
            IAffiliationCategoryRepository affiliationCategoryRepository)
        {
            _affiliationsRepository = affiliationsRepository;
            _publicationsRepository = publicationsRepository;
            _affiliationCategoryRepository = affiliationCategoryRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var model = _affiliationsRepository.GetAffiliations();
            return View(model);
        }
        [HttpGet("{categoryId}")]
        public IActionResult CategoryHeatmap(int categoryId)
        {
            var affiliationsCategory = _affiliationCategoryRepository.QueryAffiliationCategories(affcat => affcat.CategoriesFK == categoryId);
            var model = new List<Affiliation>();
            foreach (var affcat in affiliationsCategory)
            {
                var affiliation = _affiliationsRepository.GetAffiliationById(affcat.AffiliationFK);
                if (affiliation == null) continue;
                affiliation.CountOfTopTenCategories = affcat.CountOfCategoryPublications;
                model.Add(affiliation);
            }
           return View("Index", model);
        }
    }
}
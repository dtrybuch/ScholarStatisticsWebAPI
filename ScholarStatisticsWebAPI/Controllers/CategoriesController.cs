﻿using System;
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
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IPublicationsRepository _publicationsRepository;
        private readonly IFilterDataHelper _filterDataHelper;
        public CategoriesController(ILogger<CategoriesController> logger,
            ICategoriesRepository categoriesRepository, IFilterDataHelper filterDataHelper,
            IPublicationsRepository publicationsRepository)
        {
            _logger = logger;
            _categoriesRepository = categoriesRepository;
            _filterDataHelper = filterDataHelper;
            _publicationsRepository = publicationsRepository;
        }
        // GET: api/Categories
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            return _categoriesRepository.GetCategories().OrderBy(category => category.Name);
        }
    }
}

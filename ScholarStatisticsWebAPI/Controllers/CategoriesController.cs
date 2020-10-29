using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public CategoriesController(ILogger<CategoriesController> logger,
            ICategoriesRepository categoriesRepository)
        {
            _logger = logger;
            _categoriesRepository = categoriesRepository;
        }
        // GET: api/Categories
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            return _categoriesRepository.GetCategories().OrderBy(category => category.Name);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public Category Get(int id)
        {
            return _categoriesRepository.GetCategoryById(id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScholarStatistics.DAL.Interfaces;

namespace ScholarStatisticsWebAPI.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class HeatMapController : Controller
    {
        private readonly IAffiliationsRepository _affiliationsRepository;
        public HeatMapController(IAffiliationsRepository affiliationsRepository)
        {
            _affiliationsRepository = affiliationsRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var model = _affiliationsRepository.GetAffiliations();
            return View(model);
        }
    }
}
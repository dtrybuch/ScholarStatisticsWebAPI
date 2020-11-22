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
            var restoredaff = _affiliationsRepository.QueryAffiliations(aff => aff.Lattitude == 0 && aff.Longitude == 0 && aff.City == "Copenhagen");
            foreach (var affi in restoredaff)
            {
                affi.Lattitude = 55.682758219701945;
                affi.Longitude = 12.572532381415828;
            }
            _affiliationsRepository.UpdateAffiliations(restoredaff);
            var model = _affiliationsRepository.GetAffiliations();
            return View(model);
        }
    }
}
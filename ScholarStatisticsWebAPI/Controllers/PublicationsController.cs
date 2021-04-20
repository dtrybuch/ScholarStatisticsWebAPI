using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScholarStatistics.DAL.Interfaces;

namespace ScholarStatisticsWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PublicationsController : ControllerBase
    {
        IPublicationsRepository _publicationsRepository;
        public PublicationsController(IPublicationsRepository publicationsRepository)
        {
            _publicationsRepository = publicationsRepository;
        }
        // GET: Publications
        [HttpGet]
        public int Get()
        {
            return _publicationsRepository.GetCount();
        }

        // GET: Publications/5
        [HttpGet]
        [Route("Scopus")]
        public int GetScopus()
        {
            return _publicationsRepository.GetScopusCount();
        }
    }
}

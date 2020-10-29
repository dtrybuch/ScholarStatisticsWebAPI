using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ScholarStatistics.DAL;
using ScholarStatistics.DAL.Helpers;
using ScholarStatistics.DAL.Interfaces;
using ScholarStatistics.DAL.Models;

namespace ScholarStatisticsWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorsController : ControllerBase
    {

        private readonly ILogger<AuthorsController> _logger;
        private readonly IAuthorsRepository _authorsRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IPublicationsRepository _publicationsRepository;

        public AuthorsController(ILogger<AuthorsController> logger, IFilterDataHelper filterHelper,
            IAuthorsRepository authorsRepository, ICategoriesRepository categoriesRepository,
            IPublicationsRepository publicationsRepository)
        {
            _logger = logger;
            _authorsRepository = authorsRepository;
            _categoriesRepository = categoriesRepository;
            _publicationsRepository = publicationsRepository;
        }

        [HttpGet]
        public IEnumerable<Publication> GetAuthors()
        {
            try
            {
                var authors = _authorsRepository.QueryAuthors(author => author.LastName.ToLower().Contains("krawczyk"));
                var publications = _publicationsRepository.QueryPublications(publication => publication.AuthorsFK.Contains(1533));
                return publications.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }

        }
        // GET: api/Authors/5
        [HttpGet("{id}")]
        public Author GetAuthor(int id)
        {
            try
            {
                var author = _authorsRepository.GetAuthorById(id);
                return author;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
    }
}

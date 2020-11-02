using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using AdventureWorks.Domain.Entity;
using AdventureWorks.Domain.Interfaces;

namespace AdventureWorks.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        #region Members

        private readonly IPersonRepository _PersonRepository;

        #endregion

        #region Constructor

        public PersonController(IPersonRepository personRepository)
        {
            _PersonRepository = personRepository;
        }

        #endregion

        #region Actions

        /// <summary>
        /// Get all Persons
        /// </summary>
        /// <param name="rows">OPTIONAL Count of rows</param>
        /// <param name="page">OPTIONAL Page number</param>
        /// <returns>Collection of Person objects</returns>
        /// <sample>GET /Person/GetAll</sample>
        [HttpGet]
        [Route("GetAll/{rows?}/{page?}")]
        public async Task<IActionResult> Search(int rows = 50, int page = 0)
        {
            try
            {
                IEnumerable<Person> result = null;

                await Task.Run(() =>
                {
                    result = _PersonRepository.GetAll(rows, page);
                });

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get Person matching provided ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>Person object</returns>
        /// <sample>GET /Person/1</sample>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                Person result = null;

                await Task.Run(() =>
                {
                    result = _PersonRepository.Get(id);
                });

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}

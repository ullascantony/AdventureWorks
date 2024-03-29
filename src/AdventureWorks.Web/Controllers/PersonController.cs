﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using AdventureWorks.Domain.Entity;
using AdventureWorks.Domain.Interfaces;

namespace AdventureWorks.Web.Controllers
{
    /// <summary>
    /// Manage Person data
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        #region Members

        private readonly ILogger<PersonController> _Logger;

        private readonly IPersonRepository _PersonRepository;

        #endregion

        #region Constructor

        public PersonController(
            ILogger<PersonController> logger,
            IPersonRepository personRepository)
        {
            _Logger = logger;
            _PersonRepository = personRepository;
        }

        #endregion

        #region Action methods

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
                _Logger.LogInformation("Person controller: Get All action invoked.");

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
                _Logger.LogInformation("Person controller: Get action invoked.");

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

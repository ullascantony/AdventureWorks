using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using AdventureWorks.Domain.Interfaces;

namespace AdventureWorks.Web.Controllers
{
    /// <summary>
    /// Manage Employee data
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        #region Members

        private readonly ILogger<EmployeeController> _Logger;

        private readonly IEmployeeRepository _EmployeeRepository;

        #endregion

        #region Constructor

        public EmployeeController(
            ILogger<EmployeeController> logger,
            IEmployeeRepository employeeRepository)
        {
            _Logger = logger;
            _EmployeeRepository = employeeRepository;
        }

        #endregion

        #region Action methods

        /// <summary>
        /// Get all Employees
        /// </summary>
        /// <param name="rows">OPTIONAL Count of rows</param>
        /// <param name="page">OPTIONAL Page number</param>
        /// <returns>Collection of Dynamic Employee objects</returns>
        /// <sample>GET /Employee/GetAll</sample>
        [HttpGet]
        [Route("GetAll/{rows?}/{page?}")]
        public async Task<IActionResult> GetAll(int rows = 50, int page = 0)
        {
            try
            {
                _Logger.LogInformation("Employee controller: Get All action invoked.");

                IEnumerable<ExpandoObject> result = null;

                await Task.Run(() =>
                {
                    result = _EmployeeRepository.GetAll(rows, page);
                });

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get Employee matching provided ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>Dynamic Employee object</returns>
        /// <sample>GET /Employee/1</sample>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                _Logger.LogInformation("Employee controller: Get action invoked.");

                ExpandoObject result = null;

                await Task.Run(() =>
                {
                    result = _EmployeeRepository.Get(id);
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

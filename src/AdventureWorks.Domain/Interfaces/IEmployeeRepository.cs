using System.Collections.Generic;
using System.Dynamic;

namespace AdventureWorks.Domain.Interfaces
{
    /// <summary>
    /// Repository to manage Employee objects
    /// </summary>
    public interface IEmployeeRepository
    {
        #region Methods

        /// <summary>
        /// Get all Employees
        /// </summary>
        /// <param name="rows">OPTIONAL Count of rows</param>
        /// <param name="page">OPTIONAL Page number</param>
        /// <returns>Collection of Dynamic Employee objects</returns>
        IList<ExpandoObject> GetAll(int rows = 50, int page = 0);

        /// <summary>
        /// Get Employee matching provided ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>Dynamic Employee object</returns>
        ExpandoObject Get(int id);

        #endregion
    }
}
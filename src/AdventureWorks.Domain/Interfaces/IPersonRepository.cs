using System.Collections.Generic;

using AdventureWorks.Domain.Entity;

namespace AdventureWorks.Domain.Interfaces
{
    /// <summary>
    /// Repository to manage Person objects
    /// </summary>
    public interface IPersonRepository
    {
        #region Methods

        /// <summary>
        /// Get all Persons
        /// </summary>
        /// <param name="rows">OPTIONAL Count of rows</param>
        /// <param name="page">OPTIONAL Page number</param>
        /// <returns>Collection of Person objects</returns>
        IList<Person> GetAll(int rows = 50, int page = 0);

        /// <summary>
        /// Get person matching provided ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>Person object</returns>
        Person Get(int id);

        #endregion
    }
}

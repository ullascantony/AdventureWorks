using System.Collections.Generic;
using System.Linq;

using AdventureWorks.Domain.Entity;
using AdventureWorks.Domain.Interfaces;

namespace AdventureWorks.Repository
{
    public class PersonRepository : IPersonRepository
    {
        #region Members

        private readonly IDataAccess _DataAccess;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public PersonRepository(IDataAccess dataAccess)
        {
            _DataAccess = dataAccess;
        }

        #endregion

        #region Methods

        public IList<Person> GetAll(int rows, int page)
        {
            const string query = @"
                SELECT
                     [BusinessEntityID]
                    ,[PersonType]
                    ,[NameStyle]
                    ,[Title]
                    ,[FirstName]
                    ,[MiddleName]
                    ,[LastName]
                    ,[Suffix]
                    ,[RowGuid]
                    ,[ModifiedDate]
                    ,COUNT(1) OVER() AS[ResultCount]
                FROM
                    [Person].[Person]
                ORDER BY
                    [BusinessEntityID] ASC
                OFFSET(@Rows * @Page) ROWS
                FETCH NEXT @Rows ROWS ONLY;";

            var parameters = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@Rows", rows),
                new KeyValuePair<string, object>("@Page", page)
            };

            return _DataAccess.ExecuteQuery<Person>(query, parameters);
        }

        public Person Get(int id)
        {
            const string query = @"
                SELECT
                     [BusinessEntityID]
                    ,[PersonType]
                    ,[NameStyle]
                    ,[Title]
                    ,[FirstName]
                    ,[MiddleName]
                    ,[LastName]
                    ,[Suffix]
                    ,[RowGuid]
                    ,[ModifiedDate]
                FROM
                    [Person].[Person]
                WHERE
                    [BusinessEntityID] = @BusinessEntityID;";

            var parameters = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@BusinessEntityID", id)
            };

            return _DataAccess.ExecuteQuery<Person>(query, parameters).FirstOrDefault();
        }

        #endregion
    }
}

using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

using AdventureWorks.Domain.Interfaces;

namespace AdventureWorks.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        #region Members

        private readonly IDataAccess _DataAccess;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public EmployeeRepository(IDataAccess dataAccess)
        {
            _DataAccess = dataAccess;
        }

        #endregion

        #region Methods

        public IList<ExpandoObject> GetAll(int rows = 50, int page = 0)
        {
            const string query = @"
                SELECT
                     emp.[BusinessEntityID]
                    ,emp.[Title]
                    ,emp.[FirstName]
                    ,emp.[MiddleName]
                    ,emp.[LastName]
                    ,emp.[Suffix]
                    ,emp.[JobTitle]
                    ,emp.[PhoneNumber]
                    ,emp.[PhoneNumberType]
                    ,emp.[EmailAddress]
                    ,emp.[AddressLine1]
                    ,emp.[AddressLine2]
                    ,emp.[City]
                    ,emp.[StateProvinceName]
                    ,emp.[PostalCode]
                    ,emp.[CountryRegionName]
                    ,per.[RowGuid]
                    ,per.[ModifiedDate]
                    ,COUNT(1) OVER() AS[ResultCount]
                FROM
                    [HumanResources].[vEmployee] AS emp
                    INNER JOIN [Person].[Person] AS per
                        ON per.[BusinessEntityID] = emp.[BusinessEntityID]
                ORDER BY
                    emp.[BusinessEntityID] ASC
                OFFSET(@Rows * @Page) ROWS
                FETCH NEXT @Rows ROWS ONLY;";

            var parameters = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@Rows", rows),
                new KeyValuePair<string, object>("@Page", page)
            };

            return _DataAccess.ExecuteQueryExpando(query, parameters);
        }

        public ExpandoObject Get(int id)
        {
            const string query = @"
                SELECT
                     emp.[BusinessEntityID]
                    ,emp.[Title]
                    ,emp.[FirstName]
                    ,emp.[MiddleName]
                    ,emp.[LastName]
                    ,emp.[Suffix]
                    ,emp.[JobTitle]
                    ,emp.[PhoneNumber]
                    ,emp.[PhoneNumberType]
                    ,emp.[EmailAddress]
                    ,emp.[AddressLine1]
                    ,emp.[AddressLine2]
                    ,emp.[City]
                    ,emp.[StateProvinceName]
                    ,emp.[PostalCode]
                    ,emp.[CountryRegionName]
                    ,per.[RowGuid]
                    ,per.[ModifiedDate]
                FROM
                    [HumanResources].[vEmployee] AS emp
                    INNER JOIN [Person].[Person] AS per
                        ON per.[BusinessEntityID] = emp.[BusinessEntityID]
                WHERE
                    per.[BusinessEntityID] = @BusinessEntityID;";

            var parameters = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@BusinessEntityID", id)
            };

            return _DataAccess.ExecuteQueryExpando(query, parameters).FirstOrDefault();
        }

        #endregion
    }
}

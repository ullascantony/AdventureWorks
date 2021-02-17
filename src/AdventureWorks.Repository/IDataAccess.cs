using System.Collections.Generic;
using System.Dynamic;

namespace AdventureWorks.Repository
{
    /// <summary>
    /// Interface to manage Data Access
    /// </summary>
    public interface IDataAccess
    {
        #region Methods

        /// <summary>
        /// Execute database command and return generic list
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">OPTIONAL parameter list</param>
        /// <param name="isSqlStoredProc">OPTIONAL flag when command is stored procedure</param>
        /// <param name="multiple">OPTIONAL flag for multiple resultsets</param>
        /// <returns>Generic list</returns>
        List<T> ExecuteQuery<T>(
            string commandText,
            List<KeyValuePair<string, object>> parameters = null,
            bool isSqlStoredProc = false,
            bool multiple = false);

        /// <summary>
        /// Execute database command and return expando object list
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">OPTIONAL parameter list</param>
        /// <param name="isSqlStoredProc">OPTIONAL flag when command is stored procedure</param>
        /// <returns>Expando object list</returns>
        List<ExpandoObject> ExecuteQueryExpando(
            string commandText,
            List<KeyValuePair<string, object>> parameters = null,
            bool isSqlStoredProc = false);

        /// <summary>
        /// Execute database command and return generic object result
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">OPTIONAL parameter list</param>
        /// <param name="isSqlStoredProc">OPTIONAL flag when command is stored procedure</param>
        /// <returns>Generic object result</returns>
        T ExecuteScalar<T>(
            string commandText,
            List<KeyValuePair<string, object>> parameters = null,
            bool isSqlStoredProc = false);

        /// <summary>
        /// Execute database command and return affected row count
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">OPTIONAL parameter list</param>
        /// <param name="isSqlStoredProc">OPTIONAL flag when command is stored procedure</param>
        /// <returns>Affected row count</returns>
        int ExecuteCommand(
            string commandText,
            List<KeyValuePair<string, object>> parameters = null,
            bool isSqlStoredProc = false);

        /// <summary>
        /// Execute database command and return return specified return value
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Parameter list</param>
        /// <param name="returnValueName">Name of the return value parameter</param>
        /// <param name="isSqlStoredProc">OPTIONAL flag when command is stored procedure</param>
        /// <returns>Affected row count</returns>
        T ExecuteCommandReturn<T>(
            string commandText,
            List<KeyValuePair<string, object>> parameters,
            string returnValueName,
            bool isSqlStoredProc = false);

        /// <summary>
        /// Bulk insert data into database
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="data">Data as generic list</param>
        /// <param name="tableName">Table name</param>
        /// returns>Success flag</returns>
        bool BulkInsert<T>(
            IList<T> data,
            string tableName);

        #endregion
    }
}

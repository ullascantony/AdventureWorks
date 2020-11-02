using System.Data.SqlClient;

namespace AdventureWorks.Repository
{
    /// <summary>
    /// Interface for Database Connection Context
    /// </summary>
    public interface IDatabaseContext
    {
        #region Properties

        /// <summary>
        /// SQL Command Timeout in seconds
        /// </summary>
        /// <returns>Timeout as integer</returns>
        int SqlCommandTimeoutSeconds { get; }

        /// <summary>
        /// SQL bulk insert batch size
        /// </summary>
        /// <returns>Batch size as integer</returns>
        int SqlBulkInsertBatchSize { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Get SQL Client Connection object
        /// </summary>
        /// <returns>SQL Client Connection object</returns>
        SqlConnection GetConnection();

        #endregion
    }
}

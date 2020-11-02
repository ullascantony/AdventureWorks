using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AdventureWorks.Repository
{
    public class DatabaseContext : IDatabaseContext
    {
        #region Constants

        private const string Connection_Strings = "ConnectionStrings";
        private const string App_Settings = "AppSettings";
        private const string Base_Connection = "BaseConnection";
        private const string SQL_Command_TimeoutSeconds = "Sql.Command.TimeoutSeconds";
        private const string SQL_BulkInsert_BatchSize = "Sql.BulkInsert.BatchSize";

        #endregion

        #region Members

        private readonly IConfiguration Configuration;

        #endregion

        #region Properties

        public int SqlCommandTimeoutSeconds { get; }

        public int SqlBulkInsertBatchSize { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public DatabaseContext(IConfiguration configuration)
        {
            Configuration = configuration;

            _ = int.TryParse(Configuration.GetSection(App_Settings).GetSection(SQL_Command_TimeoutSeconds).Value.Trim(), out var sqlCommandTimeoutSeconds);
            SqlCommandTimeoutSeconds = sqlCommandTimeoutSeconds;

            _ = int.TryParse(Configuration.GetSection(App_Settings).GetSection(SQL_BulkInsert_BatchSize).Value.Trim(), out var sqlBulkInsertBatchSize);
            SqlBulkInsertBatchSize = sqlBulkInsertBatchSize;
        }

        #endregion

        #region Methods

        public SqlConnection GetConnection()
        {
            return new SqlConnection(Configuration.GetSection(Connection_Strings).GetSection(Base_Connection).Value.Trim());
        }

        #endregion
    }
}

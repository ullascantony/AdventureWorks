using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace AdventureWorks.Repository
{
    public class DataAccess : IDataAccess
    {
        #region Members

        private readonly IDatabaseContext _Context;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">Database Context instance</param>
        public DataAccess(IDatabaseContext context)
        {
            _Context = context;
        }

        #endregion

        #region Methods

        public List<T> ExecuteQuery<T>(
            string commandText,
            List<KeyValuePair<string, object>> parameters = null,
            bool isSqlStoredProc = false,
            bool multiple = false)
        {
            var collection = new List<T>();

            var commandType = isSqlStoredProc ? CommandType.StoredProcedure : CommandType.Text;

            if (multiple)
            {
                var resultSet = ExecuteReaderMultiple(
                    commandText,
                    FromKeyValues(parameters),
                    commandType);

                foreach (DataTable result in resultSet.Tables)
                {
                    var col = ListFromDataTable<T>(result);
                    collection = collection.Union(col).ToList();
                }
            }
            else
            {
                var result = ExecuteReader(commandText, FromKeyValues(parameters), commandType);
                if (result != null)
                {
                    collection = ListFromDataTable<T>(result);
                }
            }

            return collection;
        }

        public List<ExpandoObject> ExecuteQueryExpando(
            string commandText,
            List<KeyValuePair<string, object>> parameters = null,
            bool isSqlStoredProc = false)
        {
            var commandType = isSqlStoredProc ? CommandType.StoredProcedure : CommandType.Text;

            var result = ExecuteReader(
                commandText,
                FromKeyValues(parameters),
                commandType);

            return ExpandoFromDataTable(result);
        }

        public T ExecuteScalar<T>(
            string commandText,
            List<KeyValuePair<string, object>> parameters = null,
            bool isSqlStoredProc = false)
        {
            var commandType = isSqlStoredProc ? CommandType.StoredProcedure : CommandType.Text;

            return (T)ExecuteScalar(
                commandText,
                FromKeyValues(parameters),
                commandType);
        }

        public int ExecuteCommand(
            string commandText,
            List<KeyValuePair<string, object>> parameters = null,
            bool isSqlStoredProc = false)
        {
            var commandType = isSqlStoredProc ? CommandType.StoredProcedure : CommandType.Text;

            return ExecuteNonQuery(
                commandText,
                FromKeyValues(parameters),
                commandType);
        }

        public T ExecuteCommandReturn<T>(
            string commandText,
            List<KeyValuePair<string, object>> parameters,
            string returnValueName,
            bool isSqlStoredProc = false)
        {
            var commandType = isSqlStoredProc ? CommandType.StoredProcedure : CommandType.Text;

            return ExecuteNonQueryReturn<T>(
                commandText,
                FromKeyValues(parameters),
                returnValueName,
                commandType);
        }

        public bool BulkInsert<T>(
            IList<T> data,
            string tableName)
        {
            var result = true;

            try
            {
                if (data.Any())
                {
                    BulkInsert(DataTableFromList(data, tableName));
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Execute database command and return results
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">OPTIONAL SQL parameter array</param>
        /// <param name="commandType">OPTIONAL Command type</param>
        /// <returns>Results as DataTable</returns>
        private DataTable ExecuteReader(
            string commandText,
            SqlParameter[] parameters = null,
            CommandType commandType = CommandType.StoredProcedure)
        {
            var result = new DataTable();

            using var con = _Context.GetConnection();
            con.Open();

            using var cmd = new SqlCommand(commandText, con)
            {
                CommandTimeout = _Context.SqlCommandTimeoutSeconds,
                CommandType = commandType
            };

            if (parameters != null)
            {
                if (parameters.Any())
                {
                    foreach (var parameter in parameters)
                    {
                        if (parameter.Value == null)
                        {
                            parameter.Value = DBNull.Value;
                        }
                    }

                    cmd.Parameters.AddRange(parameters);
                }
            }

            var rdr = cmd.ExecuteReader();
            result.Load(rdr);

            return result;
        }

        /// <summary>
        /// Execute database command and return results
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">OPTIONAL SQL parameter array</param>
        /// <param name="commandType">OPTIONAL Command type</param>
        /// <returns>Results as DataTable</returns>
        private DataSet ExecuteReaderMultiple(
            string commandText,
            SqlParameter[] parameters = null,
            CommandType commandType = CommandType.StoredProcedure)
        {
            var resultSet = new DataSet();

            using var con = _Context.GetConnection();
            con.Open();

            using var cmd = new SqlCommand(commandText, con)
            {
                CommandTimeout = _Context.SqlCommandTimeoutSeconds,
                CommandType = commandType
            };

            if (parameters != null)
            {
                if (parameters.Any())
                {
                    foreach (var parameter in parameters)
                    {
                        if (parameter.Value == null)
                        {
                            parameter.Value = DBNull.Value;
                        }
                    }

                    cmd.Parameters.AddRange(parameters);
                }
            }

            using var adr = new SqlDataAdapter(cmd);
            adr.Fill(resultSet);

            return resultSet;
        }

        /// <summary>
        /// Execute database command and return object result
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">OPTIONAL SQL parameter array</param>
        /// <param name="commandType">OPTIONAL Command type</param>
        /// <returns>Generic object result</returns>
        private object ExecuteScalar(
            string commandText,
            SqlParameter[] parameters = null,
            CommandType commandType = CommandType.StoredProcedure)
        {
            using var con = _Context.GetConnection();
            con.Open();

            using var cmd = new SqlCommand(commandText, con)
            {
                CommandTimeout = _Context.SqlCommandTimeoutSeconds,
                CommandType = commandType
            };

            if (parameters != null)
            {
                if (parameters.Any())
                {
                    foreach (var parameter in parameters)
                    {
                        if (parameter.Value == null)
                        {
                            parameter.Value = DBNull.Value;
                        }
                    }

                    cmd.Parameters.AddRange(parameters);
                }
            }

            return cmd.ExecuteScalar();
        }

        /// <summary>
        /// Execute database command and return affected row count
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">OPTIONAL SQL parameter array</param>
        /// <param name="commandType">OPTIONAL Command type</param>
        /// <returns>Affected row count</returns>
        private int ExecuteNonQuery(
            string commandText,
            SqlParameter[] parameters = null,
            CommandType commandType = CommandType.StoredProcedure)
        {
            using var con = _Context.GetConnection();
            con.Open();

            using var cmd = new SqlCommand(commandText, con)
            {
                CommandTimeout = _Context.SqlCommandTimeoutSeconds,
                CommandType = commandType
            };

            if (parameters != null)
            {
                if (parameters.Any())
                {
                    foreach (var parameter in parameters)
                    {
                        if (parameter.Value == null)
                        {
                            parameter.Value = DBNull.Value;
                        }
                    }

                    cmd.Parameters.AddRange(parameters);
                }
            }

            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Execute database command and return specified return value
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">SQL parameter array</param>
        /// <param name="returnValueName">Name of the return value parameter</param>
        /// <param name="commandType">OPTIONAL Command type</param>
        /// <returns>Affected row count</returns>
        private T ExecuteNonQueryReturn<T>(
            string commandText,
            SqlParameter[] parameters,
            string returnValueName,
            CommandType commandType = CommandType.StoredProcedure)
        {
            using var con = _Context.GetConnection();
            con.Open();

            using var cmd = new SqlCommand(commandText, con)
            {
                CommandTimeout = _Context.SqlCommandTimeoutSeconds,
                CommandType = commandType
            };

            foreach (var parameter in parameters)
            {
                if (parameter.Value == null)
                {
                    parameter.Value = DBNull.Value;
                }
            }

            cmd.Parameters.AddRange(parameters);

            _ = cmd.ExecuteNonQuery();

            return (T)cmd.Parameters[returnValueName].Value;
        }

        /// <summary>
        /// Bulk insert data into database
        /// </summary>
        /// <param name="dataTable">DataTable instance</param>
        private void BulkInsert(DataTable dataTable)
        {
            using var con = _Context.GetConnection();
            con.Open();

            using var bulk = new SqlBulkCopy(con)
            {
                BatchSize = _Context.SqlBulkInsertBatchSize,
                BulkCopyTimeout = _Context.SqlCommandTimeoutSeconds,
                DestinationTableName = dataTable.TableName
            };

            bulk.WriteToServer(dataTable);
        }

        private static SqlParameter[] FromKeyValues(List<KeyValuePair<string, object>> parameters)
        {
            if (parameters == null) { return null; }

            var result = new List<SqlParameter>();

            foreach (var param in parameters)
            {
                result.Add(new SqlParameter(param.Key, param.Value ?? DBNull.Value));
            }

            return result.ToArray();
        }

        /// <summary>
        /// Generate generic list from database result
        /// </summary>
        /// <typeparam name="T">Type parameter</typeparam>
        /// <param name="data">Data to convert</param>
        /// <returns>Generic list</returns>
        private static List<T> ListFromDataTable<T>(DataTable data)
        {
            var collection = new List<T>();

            if (data != null && data.Rows.Count > 0)
            {
                foreach (DataRow row in data.Rows)
                {
                    Type temp = typeof(T);
                    T obj = Activator.CreateInstance<T>();

                    var cols = row.Table.Columns;
                    foreach (DataColumn col in cols)
                    {
                        var props = temp.GetProperties();
                        foreach (var prop in props)
                        {
                            if (prop.Name.Equals(col.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                            {
                                var pValue = row[col.ColumnName];
                                prop.SetValue(obj, pValue == DBNull.Value ? null : pValue);
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }

                    // Add the object to the collection
                    collection.Add(obj);
                }
            }

            return collection;
        }

        /// <summary>
        /// Generate list of expando objects from database result
        /// </summary>
        /// <param name="data">Data to convert</param>
        /// <returns>Expando object list</returns>
        private static List<ExpandoObject> ExpandoFromDataTable(DataTable data)
        {
            var collection = new List<ExpandoObject>();

            if (data != null && data.Rows.Count > 0)
            {
                foreach (var row in data.Rows)
                {
                    // Create an instance of Expando object
                    var item = new ExpandoObject() as IDictionary<string, object>;

                    // Set the object's properties
                    foreach (var column in data.Columns)
                    {
                        var pName = (column as DataColumn).ColumnName;
                        var pValue = (row as DataRow)[(column as DataColumn).ColumnName];

                        item.Add(pName, pValue.GetType() == DBNull.Value.GetType() ? null : pValue);
                    }

                    // Add the object to the collection
                    collection.Add(item as ExpandoObject);
                }
            }

            return collection;
        }

        /// <summary>
        /// Generate DataTable from generic list
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="data">Data as generic list</param>
        /// <param name="tableName">Table name</param>
        /// <returns>DataTable instance</returns>
        private static DataTable DataTableFromList<T>(
            IList<T> data,
            string tableName)
        {
            var table = new DataTable(tableName);

            var properties = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(
                    prop.Name,
                    Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }

            return table;
        }

        #endregion
    }
}

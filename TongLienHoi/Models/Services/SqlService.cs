using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace HTTLVN.QLTLH.Models.Services
{
    internal class SqlService
    {
        #region Protected Member Variables
        protected string _connectionString = String.Empty;
        protected SqlParameterCollection _parameterCollection;
        protected ArrayList _parameters = new ArrayList();
        protected bool _isSingleRow = false;
        protected bool _convertEmptyValuesToDbNull = true;
        protected bool _convertMinValuesToDbNull = true;
        protected bool _convertMaxValuesToDbNull = false;
        protected bool _autoCloseConnection = true;
        protected SqlConnection _connection;
        protected SqlTransaction _transaction;
        protected int _commandTimeout = 30;
        #endregion Protected Member Variables

        #region Contructors
        public SqlService()
        {
            _connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        }

        public SqlService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlService(string server, string database, string user, string password)
        {
            ConnectionString = "Server=" + server + ";Database=" + database + ";User ID=" + user + ";Password=" + password + ";";
        }

        public SqlService(string server, string database)
        {
            ConnectionString = "Server=" + server + ";Database=" + database + ";Integrated Security=true;";
        }

        public SqlService(SqlConnection connection)
        {
            Connection = connection;
            AutoCloseConnection = false;
        }
        #endregion Contructors

        #region Properties
        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        public int CommandTimeout
        {
            get
            {
                return _commandTimeout;
            }
            set
            {
                _commandTimeout = value;
            }
        }

        public bool IsSingleRow
        {
            get
            {
                return _isSingleRow;
            }
            set
            {
                _isSingleRow = value;
            }
        }

        public bool AutoCloseConnection
        {
            get
            {
                return _autoCloseConnection;
            }
            set
            {
                _autoCloseConnection = value;
            }
        }

        public SqlConnection Connection
        {
            get
            {
                return _connection;
            }
            set
            {
                _connection = value;
                ConnectionString = _connection.ConnectionString;
            }
        }

        public SqlTransaction Transaction
        {
            get
            {
                return _transaction;
            }
            set
            {
                _transaction = value;
            }
        }

        public bool ConvertEmptyValuesToDbNull
        {
            get
            {
                return _convertEmptyValuesToDbNull;
            }
            set
            {
                _convertEmptyValuesToDbNull = value;
            }
        }

        public bool ConvertMinValuesToDbNull
        {
            get
            {
                return _convertMinValuesToDbNull;
            }
            set
            {
                _convertMinValuesToDbNull = value;
            }
        }

        public bool ConvertMaxValuesToDbNull
        {
            get
            {
                return _convertMaxValuesToDbNull;
            }
            set
            {
                _convertMaxValuesToDbNull = value;
            }
        }

        public SqlParameterCollection Parameters
        {
            get
            {
                return _parameterCollection;
            }
        }

        public int ReturnValue
        {
            get
            {
                if (_parameterCollection.Contains("@ReturnValue"))
                {
                    return (int)_parameterCollection["@ReturnValue"].Value;
                }
                throw new Exception("You must call the AddReturnValueParameter method before executing your request.");
            }
        }
        #endregion Properties

        #region Execute Methods
        public void ExecuteSql(string sql)
        {
            var cmd = new SqlCommand();
            Connect();

            cmd.CommandTimeout = CommandTimeout;
            cmd.CommandText = sql;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandType = CommandType.Text;
            CopyParameters(cmd);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            if (AutoCloseConnection) Disconnect();
        }

        public SqlDataReader ExecuteSqlReader(string sql)
        {
            var cmd = new SqlCommand();
            Connect();

            cmd.CommandTimeout = CommandTimeout;
            cmd.CommandText = sql;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandType = CommandType.Text;
            CopyParameters(cmd);

            var behavior = CommandBehavior.Default;

            if (AutoCloseConnection) behavior = behavior | CommandBehavior.CloseConnection;
            if (_isSingleRow) behavior = behavior | CommandBehavior.SingleRow;

            var reader = cmd.ExecuteReader(behavior);
            cmd.Dispose();

            return reader;
        }

        public SafeDataReader ExecuteSafeReader(string sql)
        {
            return new SafeDataReader(ExecuteSqlReader(sql));
        }

        public XmlReader ExecuteSqlXmlReader(string sql)
        {
            Connect();
            var cmd = new SqlCommand { CommandTimeout = CommandTimeout, CommandText = sql, Connection = _connection };
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandType = CommandType.Text;
            var reader = cmd.ExecuteXmlReader();
            cmd.Dispose();
            return reader;
        }

        public DataSet ExecuteSqlDataSet(string sql)
        {
            Connect();
            var da = new SqlDataAdapter();
            var ds = new DataSet();
            var cmd = new SqlCommand
            {
                CommandTimeout = CommandTimeout,
                Connection = _connection,
                CommandText = sql,
                CommandType = CommandType.Text
            };
            if (_transaction != null) cmd.Transaction = _transaction;
            da.SelectCommand = cmd;
            da.Fill(ds);
            da.Dispose();
            cmd.Dispose();

            if (AutoCloseConnection) Disconnect();

            return ds;
        }

        public DataSet ExecuteSqlDataSet(string sql, string tableName)
        {
            Connect();
            var da = new SqlDataAdapter();
            var ds = new DataSet();
            var cmd = new SqlCommand
            {
                CommandTimeout = CommandTimeout,
                Connection = _connection,
                CommandText = sql,
                CommandType = CommandType.Text
            };

            if (_transaction != null) cmd.Transaction = _transaction;
            da.SelectCommand = cmd;

            da.Fill(ds, tableName);
            da.Dispose();
            cmd.Dispose();

            if (AutoCloseConnection) Disconnect();

            return ds;
        }

        public void ExecuteSqlDataSet(ref DataSet dataSet, string sql, string tableName)
        {
            Connect();
            var da = new SqlDataAdapter();
            var cmd = new SqlCommand
            {
                CommandTimeout = CommandTimeout,
                Connection = _connection,
                CommandText = sql,
                CommandType = CommandType.Text
            };
            if (_transaction != null) cmd.Transaction = _transaction;
            da.SelectCommand = cmd;

            da.Fill(dataSet, tableName);
            da.Dispose();
            cmd.Dispose();

            if (AutoCloseConnection) Disconnect();
        }

        public DataSet ExecuteSPDataSet(string procedureName)
        {
            Connect();
            var da = new SqlDataAdapter();
            var ds = new DataSet();
            var cmd = new SqlCommand
            {
                CommandTimeout = CommandTimeout,
                CommandText = procedureName,
                Connection = _connection,
                CommandType = CommandType.StoredProcedure
            };
            if (_transaction != null) cmd.Transaction = _transaction;
            CopyParameters(cmd);
            da.SelectCommand = cmd;
            da.Fill(ds);
            _parameterCollection = cmd.Parameters;
            da.Dispose();
            cmd.Dispose();

            if (AutoCloseConnection) Disconnect();

            return ds;
        }

        public DataSet ExecuteSPDataSet(string procedureName, string tableName)
        {
            Connect();
            var da = new SqlDataAdapter();
            var ds = new DataSet();
            var cmd = new SqlCommand
            {
                CommandTimeout = CommandTimeout,
                CommandText = procedureName,
                Connection = _connection,
                CommandType = CommandType.StoredProcedure
            };

            if (_transaction != null) cmd.Transaction = _transaction;

            CopyParameters(cmd);
            da.SelectCommand = cmd;
            da.Fill(ds, tableName);
            _parameterCollection = cmd.Parameters;
            da.Dispose();
            cmd.Dispose();
            if (AutoCloseConnection) Disconnect();
            return ds;
        }

        public void ExecuteSPDataSet(ref DataSet dataSet, string procedureName, string tableName)
        {
            Connect();
            var da = new SqlDataAdapter();
            var cmd = new SqlCommand
            {
                CommandTimeout = CommandTimeout,
                CommandText = procedureName,
                Connection = _connection,
                CommandType = CommandType.StoredProcedure
            };


            if (_transaction != null) cmd.Transaction = _transaction;
            CopyParameters(cmd);

            da.SelectCommand = cmd;

            da.Fill(dataSet, tableName);

            _parameterCollection = cmd.Parameters;
            da.Dispose();
            cmd.Dispose();

            if (AutoCloseConnection) Disconnect();
        }

        public void ExecuteSP(string procedureName)
        {
            Connect();
            var cmd = new SqlCommand
            {
                CommandTimeout = CommandTimeout,
                CommandText = procedureName,
                Connection = _connection,
                CommandType = CommandType.StoredProcedure
            };
            if (_transaction != null) cmd.Transaction = _transaction;
            CopyParameters(cmd);
            cmd.ExecuteNonQuery();
            _parameterCollection = cmd.Parameters;
            cmd.Dispose();

            if (AutoCloseConnection) Disconnect();
        }

        public SqlDataReader ExecuteSPReader(string procedureName)
        {
            Connect();
            var cmd = new SqlCommand
            {
                CommandTimeout = CommandTimeout,
                CommandText = procedureName,
                Connection = _connection,
                CommandType = CommandType.StoredProcedure
            };

            if (_transaction != null) cmd.Transaction = _transaction;

            CopyParameters(cmd);
            var behavior = CommandBehavior.Default;
            if (AutoCloseConnection) behavior = behavior | CommandBehavior.CloseConnection;
            if (_isSingleRow) behavior = behavior | CommandBehavior.SingleRow;
            var reader = cmd.ExecuteReader(behavior);
            _parameterCollection = cmd.Parameters;
            cmd.Dispose();
            return reader;
        }

        public XmlReader ExecuteSPXmlReader(string procedureName)
        {
            Connect();
            var cmd = new SqlCommand
            {
                CommandTimeout = CommandTimeout,
                CommandText = procedureName,
                Connection = _connection,
                CommandType = CommandType.StoredProcedure
            };
            if (_transaction != null) cmd.Transaction = _transaction;
            CopyParameters(cmd);
            var reader = cmd.ExecuteXmlReader();

            _parameterCollection = cmd.Parameters;
            cmd.Dispose();

            return reader;
        }
        #endregion Execute Methods

        #region AddParameter
        public SqlParameter AddParameter(string name, SqlDbType type, object value)
        {
            var prm = new SqlParameter
            {
                Direction = ParameterDirection.Input,
                ParameterName = name,
                SqlDbType = type,
                Value = PrepareSqlValue(value)
            };

            _parameters.Add(prm);

            return prm;
        }

        public SqlParameter AddParameter(string name, SqlDbType type, object value, bool convertZeroToDBNull)
        {
            var prm = new SqlParameter
            {
                Direction = ParameterDirection.Input,
                ParameterName = name,
                SqlDbType = type,
                Value = PrepareSqlValue(value, convertZeroToDBNull)
            };

            _parameters.Add(prm);

            return prm;
        }

        public SqlParameter AddParameter(string name, DbType type, object value, bool convertZeroToDBNull)
        {
            var prm = new SqlParameter
            {
                Direction = ParameterDirection.Input,
                ParameterName = name,
                DbType = type,
                Value = PrepareSqlValue(value, convertZeroToDBNull)
            };

            _parameters.Add(prm);

            return prm;
        }

        public SqlParameter AddParameter(string name, SqlDbType type, object value, int size)
        {
            var prm = new SqlParameter
            {
                Direction = ParameterDirection.Input,
                ParameterName = name,
                SqlDbType = type,
                Size = size,
                Value = PrepareSqlValue(value)
            };

            _parameters.Add(prm);

            return prm;
        }

        public SqlParameter AddParameter(string name, SqlDbType type, object value, ParameterDirection direction)
        {
            var prm = new SqlParameter
            {
                Direction = direction,
                ParameterName = name,
                SqlDbType = type,
                Value = PrepareSqlValue(value)
            };

            _parameters.Add(prm);

            return prm;
        }

        public SqlParameter AddParameter(string name, SqlDbType type, object value, int size, ParameterDirection direction)
        {
            var prm = new SqlParameter
            {
                Direction = direction,
                ParameterName = name,
                SqlDbType = type,
                Size = size,
                Value = PrepareSqlValue(value)
            };

            _parameters.Add(prm);

            return prm;
        }

        public void AddParameter(SqlParameter parameter)
        {
            _parameters.Add(parameter);
        }
        #endregion AddParameter

        #region Specialized AddParameter Methods
        public SqlParameter AddOutputParameter(string name, SqlDbType type)
        {
            var prm = new SqlParameter { Direction = ParameterDirection.Output, ParameterName = name, SqlDbType = type };

            _parameters.Add(prm);

            return prm;
        }

        public SqlParameter AddOutputParameter(string name, SqlDbType type, int size)
        {
            var prm = new SqlParameter
            {
                Direction = ParameterDirection.Output,
                ParameterName = name,
                SqlDbType = type,
                Size = size
            };

            _parameters.Add(prm);

            return prm;
        }

        public SqlParameter AddReturnValueParameter()
        {
            var prm = new SqlParameter
            {
                Direction = ParameterDirection.ReturnValue,
                ParameterName = "@ReturnValue",
                SqlDbType = SqlDbType.Int
            };

            _parameters.Add(prm);

            return prm;
        }

        public SqlParameter AddStreamParameter(string name, Stream value)
        {
            return AddStreamParameter(name, value, SqlDbType.Image);
        }

        public SqlParameter AddStreamParameter(string name, Stream value, SqlDbType type)
        {
            var prm = new SqlParameter { Direction = ParameterDirection.Input, ParameterName = name, SqlDbType = type };
            value.Position = 0;
            var data = new byte[value.Length];
            value.Read(data, 0, (int)value.Length);
            prm.Value = data;
            _parameters.Add(prm);
            return prm;
        }

        public SqlParameter AddTextParameter(string name, string value)
        {
            var prm = new SqlParameter
            {
                Direction = ParameterDirection.Input,
                ParameterName = name,
                SqlDbType = SqlDbType.Text,
                Value = PrepareSqlValue(value)
            };
            _parameters.Add(prm);

            return prm;
        }
        #endregion Specialized AddParameter Methods

        #region Private Methods
        private void CopyParameters(SqlCommand command)
        {
            foreach (var t in _parameters)
            {
                command.Parameters.Add(t);
            }
        }

        private object PrepareSqlValue(object value, bool convertZeroToDBNull = false)
        {
            if (value is String)
            {
                if (ConvertEmptyValuesToDbNull && (string)value == String.Empty)
                {
                    return DBNull.Value;
                }
                return value;
            }
            if (value is Guid)
            {
                if (ConvertEmptyValuesToDbNull && (Guid)value == Guid.Empty)
                {
                    return DBNull.Value;
                }
                return value;
            }
            if (value is DateTime)
            {
                if ((ConvertMinValuesToDbNull && (DateTime)value == DateTime.MinValue)
                    || (ConvertMaxValuesToDbNull && (DateTime)value == DateTime.MaxValue))
                {
                    return DBNull.Value;
                }
                return value;
            }
            if (value is Int16)
            {
                if ((ConvertMinValuesToDbNull && (Int16)value == Int16.MinValue)
                    || (ConvertMaxValuesToDbNull && (Int16)value == Int16.MaxValue)
                    || (convertZeroToDBNull && (Int16)value == 0))
                {
                    return DBNull.Value;
                }
                return value;
            }
            if (value is Int32)
            {
                if ((ConvertMinValuesToDbNull && (Int32)value == Int32.MinValue)
                    || (ConvertMaxValuesToDbNull && (Int32)value == Int32.MaxValue)
                    || (convertZeroToDBNull && (Int32)value == 0))
                {
                    return DBNull.Value;
                }
                return value;
            }
            if (value is Int64)
            {
                if ((ConvertMinValuesToDbNull && (Int64)value == Int64.MinValue)
                    || (ConvertMaxValuesToDbNull && (Int64)value == Int64.MaxValue)
                    || (convertZeroToDBNull && (Int64)value == 0))
                {
                    return DBNull.Value;
                }
                return value;
            }
            if (value is Single)
            {
                if ((ConvertMinValuesToDbNull && (Single)value == Single.MinValue)
                    || (ConvertMaxValuesToDbNull && (Single)value == Single.MaxValue)
                    || (convertZeroToDBNull && (Single)value == 0))
                {
                    return DBNull.Value;
                }
                return value;
            }
            if (value is Double)
            {
                if ((ConvertMinValuesToDbNull && (Double)value == Double.MinValue)
                    || (ConvertMaxValuesToDbNull && (Double)value == Double.MaxValue)
                    || (convertZeroToDBNull && (Double)value == 0))
                {
                    return DBNull.Value;
                }
                return value;
            }
            if (value is Decimal)
            {
                if ((ConvertMinValuesToDbNull && (Decimal)value == Decimal.MinValue)
                    || (ConvertMaxValuesToDbNull && (Decimal)value == Decimal.MaxValue)
                    || (convertZeroToDBNull && (Decimal)value == 0))
                {
                    return DBNull.Value;
                }
                return value;
            }
            return value;
        }

        private Hashtable ParseConfigString(string config)
        {
            var attributes = new Hashtable(10, StringComparer.InvariantCultureIgnoreCase);
            var keyValuePairs = config.Split(';');
            foreach (var pair in keyValuePairs)
            {
                var keyValuePair = pair.Split('=');
                if (keyValuePair.Length == 2)
                {
                    attributes.Add(keyValuePair[0].Trim(), keyValuePair[1].Trim());
                }
                else
                {
                    attributes.Add(pair.Trim(), null);
                }
            }

            return attributes;
        }

        #endregion Private Methods

        #region Public Methods
        public void Connect()
        {
            if (_connection != null)
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
            }
            else
            {
                if (_connectionString != String.Empty)
                {
                    var initKeys = new StringCollection();
                    initKeys.AddRange(new string[]
                                          {
                                              "ARITHABORT", "ANSI_NULLS", "ANSI_WARNINGS", "ARITHIGNORE", "ANSI_DEFAULTS",
                                              "ANSI_NULL_DFLT_OFF", "ANSI_NULL_DFLT_ON", "ANSI_PADDING", "ANSI_WARNINGS"
                                          });

                    var initStatements = new StringBuilder();
                    var connectionString = new StringBuilder();

                    var attribs = ParseConfigString(_connectionString);
                    foreach (string key in attribs.Keys)
                    {
                        if (initKeys.Contains(key.Trim().ToUpper()))
                        {
                            initStatements.AppendFormat("SET {0} {1};", key, attribs[key]);
                        }
                        else if (key.Trim().Length > 0)
                        {
                            connectionString.AppendFormat("{0}={1};", key, attribs[key]);
                        }
                    }

                    _connection = new SqlConnection(connectionString.ToString());
                    _connection.Open();

                    if (initStatements.Length <= 0) return;
                    var cmd = new SqlCommand
                    {
                        CommandTimeout = CommandTimeout,
                        CommandText = initStatements.ToString(),
                        Connection = _connection,
                        CommandType = CommandType.Text
                    };
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    throw new InvalidOperationException(
                        "You must set a connection object or specify a connection string before calling Connect.");
                }
            }
        }

        public void Disconnect()
        {
            if ((_connection != null) && (_connection.State != ConnectionState.Closed))
            {
                _connection.Close();
            }

            if (_connection != null) _connection.Dispose();
            if (_transaction != null) _transaction.Dispose();

            _transaction = null;
            _connection = null;
        }

        public void BeginTransaction()
        {
            if (_connection != null)
            {
                _transaction = _connection.BeginTransaction();
            }
            else
            {
                throw new InvalidOperationException("You must have a valid connection object before calling BeginTransaction.");
            }
        }

        public void CommitTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
            }
            else
            {
                throw new InvalidOperationException("You must call BeginTransaction before calling CommitTransaction.");
            }
        }

        public void RollbackTransaction()
        {

            if (_transaction != null)
            {
                _transaction.Rollback();
            }
            else
            {
                throw new InvalidOperationException("You must call BeginTransaction before calling RollbackTransaction.");
            }
        }

        public void Reset()
        {
            if (_parameters != null)
            {
                _parameters.Clear();
            }

            if (_parameterCollection != null)
            {
                _parameterCollection = null;
            }
        }
        #endregion
    }
}
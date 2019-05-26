using System;
using System.Collections.Concurrent;
using System.Data;
using System.Linq.Expressions;

namespace DbSqlHelper
{

    //Open Api
    public static partial class Db
    {
        private static readonly ConcurrentDictionary<string, DbCache> _DbCache = new ConcurrentDictionary<string, DbCache>();

        #region Open Api
        /// <summary>
        /// Determines whether the connection cache contains the specified key.
        /// </summary>
        public static bool ContainsKey(this string key) => _DbCache.ContainsKey(key);

        /// <summary>
        /// Add Default Connection Like "".AddConnection(connectionString)
        /// </summary>
        public static string AddConnection<TDbType>(string connectionString) => "".AddConnectionImpl(typeof(TDbType), connectionString);

        public static string AddConnection<TDbType>(this string key, string connectionString) => key.AddConnectionImpl(typeof(TDbType), connectionString);

        public static string AddConnection(Type connectionType, string connectionString) => "".AddConnectionImpl(connectionType, connectionString);

        public static string AddConnection(this string key, Type connectionType, string connectionString) => key.AddConnectionImpl(connectionType, connectionString);

        public static IDbConnection GetConnection(bool autoOpen = true) => "".GetConnectionImpl(autoOpen);

        public static IDbConnection GetConnection(this string key, bool autoOpen = true) => key.GetConnectionImpl(autoOpen);
        #endregion

        #region Impl
        private static string AddConnectionImpl(this string key, Type connectionType, string connectionString)
        {
            var model = new DbCache();
            _DbCache[key] = model;
            model.Key = key;

            //Connection Cache
            {
                var type = connectionType;
                var constructor = type.GetConstructor(new[] { typeof(string) });
                var @new = Expression.New(constructor, Expression.Constant(connectionString));
                var cast = Expression.TypeAs(@new, typeof(IDbConnection));
                var func = Expression.Lambda<Func<IDbConnection>>(cast).Compile();

                model.ConnectionFunc = func;
            }


            using (var cn = key.GetConnection())
            using (var cmd = cn.CreateCommand())
            {
                //Parameter Cache
                {
                    var parameter = cmd.CreateParameter();
                    var parameterType = parameter.GetType();

                    var type = parameterType;
                    var @new = Expression.New(type);
                    var cast = Expression.TypeAs(@new, typeof(IDbDataParameter));
                    var func = Expression.Lambda<Func<IDbDataParameter>>(cast).Compile();

                    model.ParameterFunc = func;
                }

                //DbConnectionType
                model.DBConnectionType = cn.GetDbConnectionType();
                model.Type = connectionType;

                //ParameterPrefix QuotePrefix QuoteSuffix
                switch (cn.GetDbConnectionType())
                {
                    case DBConnectionType.SqlCeServer:
                    case DBConnectionType.SqlServer:
                    case DBConnectionType.SQLite:
                        model.ParameterPrefix = "@";
                        model.QuotePrefix = "[";
                        model.QuoteSuffix = "]";
                        break;
                    case DBConnectionType.MySql:
                        model.ParameterPrefix = "@";
                        model.QuotePrefix = "`";
                        model.QuoteSuffix = "`";
                        break;
                    case DBConnectionType.Firebird:
                        model.ParameterPrefix = "?";
                        model.QuotePrefix = "\"";
                        model.QuoteSuffix = "\"";
                        break;
                    case DBConnectionType.Postgres:
                        model.ParameterPrefix = "@";
                        model.QuotePrefix = "\"";
                        model.QuoteSuffix = "\"";
                        break;
                    case DBConnectionType.Oracle:
                        model.ParameterPrefix = ":";
                        model.QuotePrefix = "\"";
                        model.QuoteSuffix = "\"";
                        break;
                    default:
                        model.ParameterPrefix = "@";
                        model.QuotePrefix = "";
                        model.QuoteSuffix = "";
                        break;
                }
            }

            return key;
        }

        private static IDbConnection GetConnectionImpl(this string key, bool autoOpen)
        {
            var func = _DbCache[key].ConnectionFunc;
            var connection = func();

            if (autoOpen)
            {
                bool wasClosed = connection.State == ConnectionState.Closed;
                if (wasClosed) connection.Open();
            }
            return connection;
        }
        #endregion

        public static DbCache GetDbCache(this string key) => _DbCache[key];

        public static DbCache GetDbCache() => _DbCache[""];
    }

    //Connection Query
    public static partial class Db
    {
        public static T SqlQuery<T>(Func<IDbConnection,T> func)
        {
            using (var cn = Db.GetConnection())
            {
                return func(cn);
            }
        }
    }

    //Parameter
    public static partial class Db
    {
        public static IDbDataParameter CreateParameter(string name, object value)
            => "".CreateParameter(name, value);

        public static IDbDataParameter CreateParameter(this string key, string name, object value)
        {
            var parameter = _DbCache[key].ParameterFunc();
            parameter.ParameterName = name;
            parameter.Value = value;
            return parameter;
        }
    }

    //String Format
    public static partial class Db
    {
        /// <summary>
        /// {0} = ParameterPrefix , {1} = QuotePrefix , {2} = QuoteSuffix , 
        /// e.g
        /// 1. <code>"SqlServerDb".SqlFormat("select * from {1}orders{2} where id = {0}id")</code> equals <code>select * from [orders] where id = @id</code>
        /// 2. <code>"OracleDb".SqlFormat("select * from {1}orders{2} where id = {0}id")</code> equals <code>select * from "orders" where id = :id</code>
        /// </summary>
        /// <param name="key">DataBase Cache Key</param>
        /// <param name="sql">Format Sql</param>
        public static string SqlFormat(this string sql) => "".SqlFormat(sql);

        /// <summary>
        /// {0} = ParameterPrefix , {1} = QuotePrefix , {2} = QuoteSuffix , 
        /// e.g
        /// 1. <code>"SqlServerDb".SqlFormat("select * from {1}orders{2} where id = {0}id")</code> equals <code>select * from [orders] where id = @id</code>
        /// 2. <code>"OracleDb".SqlFormat("select * from {1}orders{2} where id = {0}id")</code> equals <code>select * from "orders" where id = :id</code>
        /// </summary>
        /// <param name="key">DataBase Cache Key</param>
        /// <param name="sql">Format Sql</param>
        public static string SqlFormat(this string key, string sql)
        {
            var cache = key.GetDbCache();
            sql = sql.Replace("{0}", cache.ParameterPrefix)
                .Replace("{1}", cache.QuotePrefix)
                .Replace("{2}", cache.QuoteSuffix)
                ;
            return sql;
        }

        /// <summary>
        /// oracle connection replace `@` by `:` 
        /// </summary>
        public static string SqlSimpleFormat(this string sql)
        {
            var cache = "".GetDbCache();
            if(cache.DBConnectionType==DBConnectionType.Oracle)
                sql = sql.Replace("@", cache.ParameterPrefix);
            return sql;
        }
    }
}

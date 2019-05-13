using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;

namespace DbSqlHelper
{
    internal class DbModel
    {
        public Func<IDbConnection> ConnectionFunc { get; set; }
        public Func<IDbDataParameter> ParameterFunc { get; set; }
    }

    public static partial class Db
    {
        private static readonly ConcurrentDictionary<string, DbModel> _ConnectionCache = new ConcurrentDictionary<string, DbModel>();

        public static string AddConnection<TDbType>(string connectionString) => "".AddConnection(typeof(TDbType), connectionString);

        public static string AddConnection<TDbType>(this string key, string connectionString) => key.AddConnection(typeof(TDbType), connectionString);

        public static string AddConnection(Type connectionType,string connectionString) => "".AddConnection(connectionType, connectionString);

        public static string AddConnection(this string key, Type connectionType, string connectionString)
        {
            var model = new DbModel();
            _ConnectionCache[key] = model;
            //Connection Cache
            {
                var type = connectionType;
                var constructor = type.GetConstructor(new[] { typeof(string) });
                var @new = Expression.New(constructor, Expression.Constant(connectionString));
                var cast = Expression.TypeAs(@new, typeof(IDbConnection));
                var func = Expression.Lambda<Func<IDbConnection>>(cast).Compile();

                model.ConnectionFunc = func;
            }

            //Parameter Cache
            {
                using (var cn = key.GetConnection())
                using (var cmd = cn.CreateCommand())
                {
                    var parameter = cmd.CreateParameter();
                    var parameterType = parameter.GetType();

                    var type = parameterType;
                    var @new = Expression.New(type);
                    var cast = Expression.TypeAs(@new, typeof(IDbDataParameter));
                    var func = Expression.Lambda<Func<IDbDataParameter>>(cast).Compile();

                    model.ParameterFunc = func;
                }
            }

            return key;
        }

        public static IDbConnection GetConnection(bool autoOpen = true) => "".GetConnection(autoOpen);

        public static IDbConnection GetConnection(this string key, bool autoOpen = true)
        {
            var func = _ConnectionCache[key].ConnectionFunc;
            var connection = func();

            if (autoOpen)
            {
                bool wasClosed = connection.State == ConnectionState.Closed;
                if (wasClosed) connection.Open();
            }
            return connection;
        }
    }

    //Parameter
    public static partial class Db
    {
        public static IDbDataParameter CreateParameter(string name, object value)
            => "".CreateParameter(name, value);

        public static IDbDataParameter CreateParameter(this string key, string name, object value)
        {
            var parameter = _ConnectionCache[key].ParameterFunc();
            parameter.ParameterName = name;
            parameter.Value = value;
            return parameter;
        }
    }
}

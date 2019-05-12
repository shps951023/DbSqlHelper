using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;

namespace DbSqlHelper
{
    public static class Db
    {
        private static readonly ConcurrentDictionary<string, Func<IDbConnection>> _connectionCache = new ConcurrentDictionary<string, Func<IDbConnection>>();

        public static string AddConnection<TDbType>(string connectionString) => "".AddConnection(typeof(TDbType), connectionString);

        public static string AddConnection<TDbType>(this string key, string connectionString) => key.AddConnection(typeof(TDbType), connectionString);

        public static string AddConnection(Type connectionType,string connectionString) => "".AddConnection(connectionType, connectionString);

        public static string AddConnection(this string key, Type connectionType, string connectionString)
        {
            var type = connectionType;
            var constructor = type.GetConstructor(new[] { typeof(string) });
            var @new = Expression.New(constructor, Expression.Constant(connectionString));
            var cast = Expression.TypeAs(@new, typeof(IDbConnection));
            var func = Expression.Lambda<Func<IDbConnection>>(cast).Compile();

            _connectionCache[key] = func;

            return key;
        }

        public static IDbConnection GetConnection(bool autoOpen = true) => "".GetConnection(autoOpen);

        public static IDbConnection GetConnection(this string key, bool autoOpen = true)
        {
            var func = _connectionCache[key];
            var connection = func();

            if (autoOpen)
            {
                bool wasClosed = connection.State == ConnectionState.Closed;
                if (wasClosed) connection.Open();
            }
            return connection;
        }
    }
}

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
        private static readonly ConcurrentDictionary<string, Func<DbConnection>> _connectionCache = new ConcurrentDictionary<string, Func<DbConnection>>();

        public static string AddConnection<TDbType>(string connectionString) => "".AddConnection(typeof(TDbType), connectionString);

        public static string AddConnection<TDbType>(this string key, string connectionString) => key.AddConnection(typeof(TDbType), connectionString);

        public static string AddConnection(this string key, Type dbType, string connectionString)
        {
            var type = dbType;
            var constructor = type.GetConstructor(new[] { typeof(string) });
            var @new = Expression.New(constructor, Expression.Constant(connectionString));
            var cast = Expression.TypeAs(@new, typeof(DbConnection));
            var func = Expression.Lambda<Func<DbConnection>>(cast).Compile();

            _connectionCache[key] = func;

            return key;
        }

        public static DbConnection GetConnection(bool autoOpen = true) => "".GetConnection(autoOpen);

        public static DbConnection GetConnection(this string key, bool autoOpen = true)
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

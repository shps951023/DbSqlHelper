using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DbSqlHelper
{
    public static class DbConnectionExtension
    {
        private static readonly DBConnectionType DefaultAdapter = DBConnectionType.Unknown;
        private static readonly Dictionary<string, DBConnectionType> AdapterDictionary
               = new Dictionary<string, DBConnectionType>
               {
                   ["oracleconnection"] = DBConnectionType.Oracle,
                   ["sqlconnection"] = DBConnectionType.SqlServer,
                   ["sqlceconnection"] = DBConnectionType.SqlCeServer,
                   ["npgsqlconnection"] = DBConnectionType.Postgres,
                   ["sqliteconnection"] = DBConnectionType.SQLite,
                   ["mysqlconnection"] = DBConnectionType.MySql,
                   ["fbconnection"] = DBConnectionType.Firebird
               };

        public static DBConnectionType GetDbConnectionType(this Type connectionType)
        {
            var name = connectionType.Name.ToLower();
            return !AdapterDictionary.ContainsKey(name) ? DefaultAdapter : AdapterDictionary[name];
        }

        public static DBConnectionType GetDbConnectionType(this IDbConnection connection)
        {
            var name = connection.GetType().Name.ToLower();
            return !AdapterDictionary.ContainsKey(name) ? DefaultAdapter : AdapterDictionary[name];
        }
    }

    public enum DBConnectionType
    {
        SqlServer, SqlCeServer, Postgres, SQLite, MySql, Oracle, Firebird, Unknown
    }
}

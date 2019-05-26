using System;
using System.Collections.Generic;
using System.Data;

namespace DbSqlHelper
{
    public static class DbConnectionExtension
    {
        private static readonly DBConnectionType _DefaultAdapter = DBConnectionType.Unknown;
        private static readonly Dictionary<string, DBConnectionType> _AdapterDictionary
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
            return !_AdapterDictionary.ContainsKey(name) ? _DefaultAdapter : _AdapterDictionary[name];
        }

        public static DBConnectionType GetDbConnectionType(this IDbConnection connection)
        {
            string name = GetDbConnectionTypeLowerName(connection);
            return !_AdapterDictionary.ContainsKey(name) ? _DefaultAdapter : _AdapterDictionary[name];
        }

        public static string GetDbConnectionTypeLowerName(this IDbConnection connection)
        {
            return connection.GetType().Name.ToLower();
        }

        private static readonly string[] DefaultSchemaKey
            = new[] { "ColumnName", "ColumnOrdinal", "ColumnSize", "DataType", "AllowDBNull", "DataTypeName", "ProviderSpecificDataType",
                "IsReadOnly" };

        private static readonly Dictionary<string, string[]> DbColumnsSchemas = new Dictionary<string, string[]> {
            {"sqlconnection", DefaultSchemaKey },
            {"sqlceserver", DefaultSchemaKey },
            {"sqliteconnection", DefaultSchemaKey },
            {"oracleconnection", new [] {"ColumnName","ColumnOrdinal","ColumnSize","DataType","AllowDBNull","DataTypeName"} },
            {"mysqlconnection", DefaultSchemaKey },
            {"npgsqlconnection", DefaultSchemaKey }
        };

        public static IEnumerable<DbColumnsSchema> GetDbColumnsSchema(this IDbConnection connection,string sql)
        {
            connection.OpenCloseConnection();
            var ds = DbColumnsSchemas[connection.GetDbConnectionTypeLowerName()];
            using (var command = connection.CreateCommand(sql))
            {
                var dt = command.ExecuteReader(CommandBehavior.SchemaOnly).GetSchemaTable();
                foreach (DataRow s in dt.Rows)
                {
                    yield return new DbColumnsSchema
                    {
                        ColumnName = s[ds[0]] as string,
                        ColumnOrdinal = (int)s[ds[1]],
                        ColumnSize = (int)s[ds[2]],
                        DataType = s[ds[3]] as Type,
                        AllowDBNull = (bool)s[ds[4]],
                        DataTypeName = s[ds[5]] as string,
                        ProviderSpecificDataType = s[ds[6]] as Type,
                        IsReadOnly = (bool)s[ds[7]],
                    };
                }
            }
        }

        public static void OpenCloseConnection(this IDbConnection connection)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();
        }
    }

    public class DbColumnsSchema
    {
        public string ColumnName { get; internal set; }
        public int ColumnOrdinal { get; internal set; }
        public int ColumnSize { get; internal set; }
        public System.Type DataType { get; internal set; }
        public System.Type ProviderSpecificDataType { get; internal set; }
        public bool AllowDBNull { get; internal set; }
        public string DataTypeName { get; internal set; }
        public bool IsReadOnly { get; internal set; }
    }

    public enum DBConnectionType
    {
        SqlServer, SqlCeServer, Postgres, SQLite, MySql, Oracle, Firebird, Unknown
    }
}

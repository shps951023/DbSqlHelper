using System;
using System.Data;

namespace DbSqlHelper
{
    public class DbCache
    {
        internal Func<IDbConnection> ConnectionFunc { get; set; }
        internal Func<IDbDataParameter> ParameterFunc { get; set; }
        public string ParameterPrefix { get; set; } = "@";
        public string QuotePrefix { get; set; } = "";
        public string QuoteSuffix { get; set; } = "";
        public DBConnectionType DBConnectionType { get; set; } = DBConnectionType.Unknown;
        public System.Type Type { get; set; }
        public string Key { get; set; }
    }
}

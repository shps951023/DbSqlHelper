using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DbSqlHelper
{
    public static class ExectueExtension
    {
        public static int ExecuteNonQuery(this DbConnection connection, string sql, params object[] parameters)
            => connection.CreateCommand(sql, parameters).ExecuteNonQuery();
    }
}

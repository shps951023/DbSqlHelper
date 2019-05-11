using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbSqlHelper
{
    public static partial class StringSqlExtension
    {
        public static int SqlExecute(this string sql, params object[] parameters)
        {
            using (var connection = Db.GetConnection())
            {
                return connection.CreateCommand(sql, parameters).ExecuteNonQuery();
            }
        }
    }
}

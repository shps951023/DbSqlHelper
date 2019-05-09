using System;
using System.Data.Common;

namespace DbSqlHelper
{
    public static class CommandExtension
    {
        public static DbCommand CreateCommand(this DbConnection cnn, string sql, params object[] parameters)
        {
            var command = cnn.CreateCommand();
            command.CommandText = sql;
            if (parameters.Length > 0)
                command.AddParams(parameters);
            return command;
        }
    }
}

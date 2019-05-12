using System;
using System.Data;
using System.Data.Common;

namespace DbSqlHelper
{
    public static class CommandExtension
    {
        public static IDbCommand CreateCommand(this IDbConnection cnn, string sql, params object[] parameters)
        {
            var command = cnn.CreateCommand();
            command.CommandText = sql;
            command.CommandTimeout = DbSqlHelperSetting.DefaultCommandTimeout;
            if (parameters.Length > 0)
                command.AddParams(parameters);
            return command;
        }
    }
}

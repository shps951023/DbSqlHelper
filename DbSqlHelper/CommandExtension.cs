using System;
using System.Data;
using System.Data.Common;

namespace DbSqlHelper
{
    public static class CommandExtension
    {
        private const int DefaultCommandTimeout = 60;
        public static IDbCommand CreateCommand(this IDbConnection cnn, string sql, params object[] parameters)
            => cnn._CreateCommand(sql, transaction: null, commandTimeout: DefaultCommandTimeout, commandType: CommandType.Text, parameters: parameters);

        public static IDbCommand CreateCommand(this IDbConnection cnn, string sql, IDbTransaction transaction, params object[] parameters) 
            => cnn._CreateCommand(sql, transaction: transaction, commandTimeout: DefaultCommandTimeout, commandType: CommandType.Text, parameters: parameters);

        public static IDbCommand CreateCommand(this IDbConnection cnn, string sql, IDbTransaction transaction,
            CommandType commandType, params object[] parameters)
            => cnn._CreateCommand(sql, transaction: transaction, commandTimeout: DefaultCommandTimeout, commandType: commandType, parameters: parameters);

        public static IDbCommand CreateCommand(this IDbConnection cnn, string sql, IDbTransaction transaction,
            CommandType commandType, int commandTimeout, params object[] parameters)
            => cnn._CreateCommand(sql, transaction: transaction, commandTimeout: commandTimeout, commandType: commandType, parameters: parameters);

        private static IDbCommand _CreateCommand(this IDbConnection cnn, string sql, 
            IDbTransaction transaction = null, int commandTimeout = DefaultCommandTimeout, 
            CommandType commandType = CommandType.Text, params object[] parameters)
        {
            var command = cnn.CreateCommand();
            command.CommandText = sql;
            command.CommandTimeout = commandTimeout;
            command.CommandType = commandType;
            if (transaction != null)
                command.Transaction = transaction;
            if (parameters.Length == 1)
                command.AddParams(parameters[0]);
            else if (parameters.Length > 0)
                command.AddParams(parameters);
            return command;
        }
    }
}

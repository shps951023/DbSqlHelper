using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DbSqlHelper
{
    public static class ParameterExtension
    {
        public static void AddParams(this IDbCommand cmd, params object[] parameters)
        {
            foreach (var item in parameters)
                AddParam(cmd, item);
        }

        public static void AddParam(this IDbCommand cmd, string key, object item)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = key;
            AddParam(cmd, item, p);
        }

        public static void AddParam(this IDbCommand cmd, object item)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = string.Format("p{0}", cmd.Parameters.Count);
            AddParam(cmd, item, p);
        }

        #region Private
        private static void AddParam(IDbCommand cmd, object item, IDbDataParameter p)
        {
            if (item == null)
            {
                p.Value = DBNull.Value;
            }
            else
            {
                if (item.GetType() == typeof(Guid))
                {
                    p.Value = item.ToString();
                    p.DbType = DbType.String;
                    p.Size = 4000;
                }
                else if (item.GetType() == typeof(System.Dynamic.ExpandoObject))
                {
                    var d = (IDictionary<string, object>)item;
                    p.Value = d.Values.FirstOrDefault();
                }
                else
                {
                    p.Value = item;
                }
                if (item.GetType() == typeof(string))
                    p.Size = 4000;
            }
            cmd.Parameters.Add(p);
        }
        #endregion
    }
}

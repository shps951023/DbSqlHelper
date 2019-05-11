using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using ValueGetter;

namespace DbSqlHelper
{
    public static class ParameterExtension
    {
        public static IDbCommand AddParams(this IDbCommand cmd, object parameters) 
        {
            if (parameters.GetType().IsValueType)
            {
                cmd.AddParam("p0", parameters);
                return cmd;
            }

            var values = parameters.GetObjectValues();
            foreach (var item in values)
                cmd.AddParam(item.Key, item.Value);
            return cmd;
        }

        public static IDbCommand AddParams(this IDbCommand cmd, params object[] parameters)
        {
            foreach (var item in parameters)
                AddParam(cmd, item);
            return cmd;
        }

        public static IDbCommand AddParam(this IDbCommand cmd, string key, object item)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = key;
            AddParam(cmd, item, p);
            return cmd;
        }

        public static IDbCommand AddParam(this IDbCommand cmd, object item)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = string.Format("p{0}", cmd.Parameters.Count);
            AddParam(cmd, item, p);
            return cmd;
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

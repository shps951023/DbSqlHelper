using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace DbSqlHelper
{
    public static class DataReaderExtension
    {
        public static IEnumerable<dynamic> ToDynamic(this IDataReader reader)
        {
            while (reader.Read())
                yield return reader._ToExpandoObject();
        }

        public static IEnumerable<Dictionary<string, object>> ToDictionary(this IDataReader reader)
        {
            while (reader.Read())
                yield return reader._ToDictionary();
        }

        private static Dictionary<string, object> _ToDictionary(this IDataReader reader)
        {
            var d = new Dictionary<string, object>();
            for (int i = 0; i < reader.FieldCount; i++)
                d.Add(reader.GetName(i), reader[i]);
            return d;
        }

        private static dynamic _ToExpandoObject(this IDataReader reader)
        {
            var d = new ExpandoObject() as IDictionary<string, object>;
            for (int i = 0; i < reader.FieldCount; i++)
                d.Add(reader.GetName(i), reader[i]);
            return d as dynamic;
        }
    }
}

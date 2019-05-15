using DbSqlHelper;
using System.Data.SqlClient;

namespace DbSqlHelperTest
{
    public class BaseTest
    {
        static BaseTest()
        {
            var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=SSPI;Initial Catalog=master;";
            Db.AddConnection<SqlConnection>(connectionString);
            Db.AddConnection(typeof(SqlConnection), connectionString);
        }
    }
}

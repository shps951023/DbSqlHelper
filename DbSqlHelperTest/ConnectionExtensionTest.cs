using DbSqlHelper;
using Xunit;

namespace DbSqlHelperTest
{
    public class ConnectionExtensionTest : BaseTest
    {
        [Fact]
        public void GetDbConnectionType()
        {
            using (var cn = Db.GetConnection())
            {
                var result = cn.GetDbConnectionType();
                Assert.Equal(DBConnectionType.SqlServer, result);
            }

            {
                var result = typeof(System.Data.SqlClient.SqlConnection).GetDbConnectionType();
                Assert.Equal(DBConnectionType.SqlServer, result);
            }
        }
    }
}

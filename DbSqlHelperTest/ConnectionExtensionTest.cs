using DbSqlHelper;
using Xunit;

namespace DbSqlHelperTest
{
    public class DbCacheTest : BaseTest
    {
        [Fact]
        public void GetDbCache()
        {
            var cache = Db.GetDbCache(); //or "".GetDbCache();
            Assert.Equal(DBConnectionType.SqlServer, cache.DBConnectionType);
            Assert.Equal("@", cache.ParameterPrefix);
            Assert.Equal("[", cache.QuotePrefix);
            Assert.Equal("]", cache.QuoteSuffix);
        }
    }

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

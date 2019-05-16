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

        [Fact]
        public void ContainsKey()
        {
            {
                var result = "sqlserver".ContainsKey();
                Assert.True(result);
            }

            {
                var result = "NotExistDb".ContainsKey();
                Assert.False(result);
            }
        }
    }
}

using DbSqlHelper;
using Xunit;

namespace DbSqlHelperTest
{
    public class DbTest : BaseTest
    {
        [Fact]
        public void SqlFormatTest()
        {
            using (var cn = Db.GetConnection())
            {
                var result = Db.SqlFormat("select * from {1}orders{2} where id = {0}id");
                Assert.Equal("select * from [orders] where id = @id", result);
            }
        }
    }
}

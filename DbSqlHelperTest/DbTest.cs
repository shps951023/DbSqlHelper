using DbSqlHelper;
using Xunit;

namespace DbSqlHelperTest
{
    public class DbTest : BaseTest
    {
        [Fact]
        public void SqlFormatTest()
        {
            {
                var sql = Db.SqlFormat("select * from {1}orders{2} where id = {0}id");
                //if db is sqlserver
                Assert.Equal("select * from [orders] where id = @id", sql);

                //if db is oracle
                //Assert.Equal("select * from "orders" where id = :id", sql); 
            }

            {
                var sql = "sqlserver".SqlFormat("select * from {1}orders{2} where id = {0}id");
                Assert.Equal("select * from [orders] where id = @id", sql);
            }
        }
    }
}

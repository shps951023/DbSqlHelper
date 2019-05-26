using DbSqlHelper;
using Xunit;
using Dapper;

namespace DbSqlHelperTest
{
    public class DbTest : BaseTest
    {
        [Fact]
        public void SqlFormat()
        {
            //default Database
            {
                var sql = "select * from {1}orders{2} where id = {0}id".SqlFormat();
                //if db is sqlserver
                Assert.Equal("select * from [orders] where id = @id", sql);

                //if db is oracle
                //Assert.Equal("select * from "orders" where id = :id", sql); 
            }

            //Mutiple Database
            {
                var sql = "sqlserver".SqlFormat("select * from {1}orders{2} where id = {0}id");
                Assert.Equal("select * from [orders] where id = @id", sql);
            }
        }

        [Fact]
        public void SqlQuery()
        {
            var result = Db.SqlQuery(connection => connection.QueryFirst<string>("select 'Hello Github'"));
            Assert.Equal("Hello Github", result);
        }
    }
}

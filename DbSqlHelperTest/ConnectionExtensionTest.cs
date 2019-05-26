using DbSqlHelper;
using Xunit;
using System.Linq;

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

        [Fact]
        public void GetDbColumnsSchema()
        {
            using (var cn = Db.GetConnection())
            {
                var result = cn.GetDbColumnsSchema("select 1 id,'hello github' val").ToArray();

                Assert.Equal("id", result[0].ColumnName);
                Assert.Equal("val", result[1].ColumnName);
                Assert.Equal(typeof(int), result[0].DataType);
                Assert.Equal(typeof(string), result[1].DataType);
            }
        }
    }
}

using DbSqlHelper;
using Xunit;

namespace DbSqlHelperTest
{
    public class ParameterTest : BaseTest
    {
        [Fact]
        public void CreateParameter()
        {
            using (var cn = Db.GetConnection())
            using (var cmd = cn.CreateCommand("select @val"))
            {
                var param = Db.CreateParameter("val", 123);
                cmd.Parameters.Add(param);
                var result = cmd.ExecuteScalar();

                Assert.Equal(123, result);
            }
        }

        [Fact]
        public void AddParams()
        {
            using (var cn = Db.GetConnection())
            using (var cmd = cn.CreateCommand("select @val"))
            {
                var param = Db.CreateParameter("val", 123);
                cmd.AddParam(param);
                var result = cmd.ExecuteScalar();

                Assert.Equal(123, result);
            }

            using (var cn = Db.GetConnection())
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "select @val1 + @val2";
                cmd.AddParams(new { val1 = 1, val2 = 2 });
                var result = cmd.ExecuteScalar();
                Assert.Equal(3, result);
            }

            using (var cn = Db.GetConnection())
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "select @val1 + @val2";
                cmd.AddParam("val1", 5).AddParam("val2", 10);
                var result = cmd.ExecuteScalar();
                Assert.Equal(15, result);
            }

            using (var cn = Db.GetConnection())
            using (var cmd = cn.CreateCommand())
            {
                {

                    cmd.CommandText = "select @p0";
                    cmd.AddParams(5);
                    var result = cmd.ExecuteScalar();
                    Assert.Equal(5, result);
                }
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "select @p0 + @p1";
                    cmd.AddParams(5, 10);
                    var result = cmd.ExecuteScalar();
                    Assert.Equal(15, result);
                }
            }
        }
    }
}

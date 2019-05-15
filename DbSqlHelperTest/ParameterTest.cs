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
            }
        }
    }
}

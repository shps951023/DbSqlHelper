[![NuGet](https://img.shields.io/nuget/v/DbSqlHelper.svg)](https://www.nuget.org/packages/DbSqlHelper)
![](https://img.shields.io/nuget/dt/DbSqlHelper.svg)

---
### Features

1. DbSqlHelper with other package library like Dapper
2. Just addConnection one time then you can get it any where,and it support mutiple connection type.
3. The simplest way to SQL Execute

---
### Online Demo
- [DbSqlHelper Demo : Easy Add/Get Connection ](https://dotnetfiddle.net/VcDt2Y)
- [DbSqlHelper Demo : Easy Execute SQL](https://dotnetfiddle.net/YWuQGb)
- [DbSqlHelper Demo : GetDbConnectionType](https://dotnetfiddle.net/1ida8T)


---
### Installation

You can install the package [from NuGet](https://www.nuget.org/packages/DbSqlHelper) using the Visual Studio Package Manager or NuGet UI:

```cmd
PM> install-package DbSqlHelper
```

or `dotnet` command line:

```cmd
dotnet add package DbSqlHelper
```

---

## Get Start

### Easy Sql Execute (Use Default Connection)

1. SqlExecute
```C#
"create table #T (ID int,Name nvarchar(20))".SqlExecute();
```

2. SqlExecute with parameters 
```C#
@"create table #T (ID int,Name nvarchar(20))
insert into #T (ID,Name) values (1,@p0),(2,@p1);
".SqlExecute("Github","Microsoft");
```

#### Easy Add/Get Connection (Default Connection)
- Just AddConnection One Time Then You Can Get AnyWhere
- Default Auto Open Connection
```C#
var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=SSPI;Initial Catalog=master;";
Db.AddConnection<System.Data.SqlClient.SqlConnection>(connectionString);
using (var cn = Db.GetConnection()) 
{
    //Sql Query..
}
```

#### Support Mutiple Connection
```C#
"SqlServerDb".AddConnection<System.Data.SqlClient.SqlConnection>(connectionString);
"OracleDb".AddConnection<Oracle.ManagedDataAccess.Client.OracleConnection>(connectionString);
using (var cn = "SqlServerDb".GetConnection())
{
    //Sql Query..
}
using (var cn = "OracleDb".GetConnection())
{
    //Sql Query..
}
```

#### GetDbConnectionType

```C#
var result = Db.GetConnection().GetDbConnectionType();
Assert.Equal(DBConnectionType.SqlServer, result);
```

----

### Extension

#### ParameterExtension
1.Builder Style    
```C#
using (var cn = Db.GetConnection())
using (var cmd = cn.CreateCommand())
{
    cmd.CommandText = "select @val1 + @val2";
    cmd.AddParam("val1", 5).AddParam("val2", 10);
    var result = cmd.ExecuteScalar();
    Assert.Equal(15, result);
}
```

2.EF SqlQuery Index Parameter Style (@p0,@p1...)  
> this is faster than Dapper Style because it doesn't use Reflection
```C#
using (var cn = Db.GetConnection())
using (var cmd = cn.CreateCommand())
{
    cmd.Parameters.Clear();
    cmd.CommandText = "select @p0 + @p1";
    cmd.AddParams(5,10);
    var result = cmd.ExecuteScalar();
    Assert.Equal(15, result);
}
```

3.Dapper Style
```C#
using (var cn = Db.GetConnection())
using (var cmd = cn.CreateCommand())
{
    cmd.CommandText = "select @val1 + @val2";
    cmd.AddParams(new { val1 = 1, val2 = 2 });
    var result = cmd.ExecuteScalar();
    Assert.Equal(3, result);
}
```





<!--
#### Command Extension
```C#
using (var cn = Db.GetConnection())
using (var command = cn.CreateCommand("select @p0 + @p1",1,2))
{
    var result = command.ExecuteScalar();
    Assert.Equal(3, result);
}
```

#### ExecuteNonQuery Extension

```C#
var sql = @"
with cte as ( 
    select @p0 + @p1 val union all 
    select @p2 + @p3
) 
select * into #T from cte
";
using (var cn = Db.GetConnection())
{
    var result = cn.ExecuteNonQuery(sql, 1, 2, 3, 4);
    Assert.Equal(2, result);
}
```
-->
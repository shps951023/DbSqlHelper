[![NuGet](https://img.shields.io/nuget/v/DbSqlHelper.svg)](https://www.nuget.org/packages/DbSqlHelper)
![](https://img.shields.io/nuget/dt/DbSqlHelper.svg)

---

### Features

1. DbSqlHelper can be used with other third-party package like Dapper easily
2. Just addConnection one time then you can get it any where,and it support mutiple connection type.
3. The simplest way to SQL Execute
4. Support mutiple RDBMS (SqlServer,Oracle,MySQL..)
5. Support `net40;net45;net451;net46;netstandard2.0;` framework

---
### Online Demo
- [DbSqlHelper Demo : Easy Add/Get Connection ](https://dotnetfiddle.net/VcDt2Y)
- [DbSqlHelper Demo : Easy Execute SQL](https://dotnetfiddle.net/YWuQGb)
- [DbSqlHelper Demo : GetDbConnectionType](https://dotnetfiddle.net/1ida8T)
- [DbSqlHelper Demo : SqlFormat](https://dotnetfiddle.net/kjZ2nn)

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

### Simplest Sql Execute (Use Default Connection)

1. SqlExecute
```C#
"create table #T (ID int,Name nvarchar(20))".SqlExecute();
```

2. SqlExecute with Index parameters (EF SqlQuery Parameter Style)
```C#
@"  create table #T (ID int,Name nvarchar(20))
    insert into #T (ID,Name) values (1,@p0),(2,@p1);
".SqlExecute("Github","Microsoft");
```

#### Easy Add/Get Connection (Default Connection)
- Just AddConnection One Time Then You Can Get Any Where
- Default Auto Open Connection
```C#
var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=SSPI;Initial Catalog=master;";
Db.AddConnection<SqlConnection>(connectionString); // or Db.AddConnection(typeof(SqlConnection),connectionString);
using (var cn = Db.GetConnection()) 
{
    //Sql Query..
}
```

#### Support Mutiple Connection
```C#
"SqlServerDb".AddConnection<SqlConnection>(connectionString);
"OracleDb".AddConnection<OracleConnection>(connectionString);
using (var sqlCn = "SqlServerDb".GetConnection())
using (var oracleCn = "OracleDb".GetConnection())
{
    //Sql Query..
}
```

#### SQL Format For ParameterPrefix,QuotePrefix,QuoteSuffix
- Automatically give ParameterPrefix, QuotePrefix,QuoteSuffix values as per the database to resolve the SQL dialect problem of different databases
- {0} = ParameterPrefix , {1} = QuotePrefix , {2} = QuoteSuffix
```C#
var sql = Db.SqlFormat("select * from {1}orders{2} where id = {0}id");
//if db is sqlserver
Assert.Equal("select * from [orders] where id = @id", sql); 

//if db is oracle
Assert.Equal("select * from \"orders\" where id = :id", sql); 
```

#### GetDbConnection Cache Model(Get Connection ParameterPrefix,QuotePrefix,QuoteSuffix)
```C#
var cache = Db.GetDbCache(); //or "".GetDbCache();
Assert.Equal(DBConnectionType.SqlServer, cache.DBConnectionType);
Assert.Equal("@", cache.ParameterPrefix);
Assert.Equal("[", cache.QuotePrefix);
Assert.Equal("]", cache.QuoteSuffix);
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
    cmd.CommandText = "select @p0 + @p1";
    cmd.AddParams(5,10);
    var result = cmd.ExecuteScalar();
    Assert.Equal(15, result);
}
```

3.Dapper Style
> it use valuegetter properties cache 
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

### Example
DbSqlHelper and Dapper
```C#
using (var cn = Db.GetConnection())
{
    var result = cn.QueryFirst<int>("select 1");
    Assert.Equal(1, result);
}
```
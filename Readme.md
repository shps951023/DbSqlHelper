[![NuGet](https://img.shields.io/nuget/v/DbSqlHelper.svg)](https://www.nuget.org/packages/DbSqlHelper)
![](https://img.shields.io/nuget/dt/DbSqlHelper.svg)

---
### Features

1. You can use DbSqlHelper with other package kibrary like Dapper
2. Just addConnection one time then you can get it any where,and it support mutiple connection type.
3. The simplest way to SQL Execute
4. Native SQL Extension

---
### Online Demo
- [DbSqlHelper Demo : Easy Add/Get Connection ](https://dotnetfiddle.net/VcDt2Y)
- [DbSqlHelper Demo : Easy Execute SQL](https://dotnetfiddle.net/YWuQGb)

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

#### Support Mutiple RDBMS Connection
e.g : oracle
```C#
Db.AddConnection<Oracle.ManagedDataAccess.Client.OracleConnection>(connectionString);
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

### Easy Sql Execute (Use Default Connection)

1. SqlExecute
```C#
"create table #T (ID int,Name nvarchar(20))".SqlExecute();
```

2. with index parameter 
```C#
@"create table #T (ID int,Name nvarchar(20))
insert into #T (ID,Name) values (1,@p0),(2,@p1);
".SqlExecute("Github","Microsoft");
```

#### GetDbType

```C#
{
    "SqlServerDb".AddConnection<System.Data.SqlClient.SqlConnection>(connectionString);
    var dbType = "SqlServerDb".GetConnection().GetDbConnectionType();
    Assert.Equal(DBConnectionType.SqlServer, result);
}
{
    "OracleDb".AddConnection<Oracle.ManagedDataAccess.Client.OracleConnection>(connectionString);
    var dbType = "OracleDb".GetConnection().GetDbConnectionType();
    Assert.Equal(DBConnectionType.Oracle, result);
}
```

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
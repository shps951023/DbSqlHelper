#### Version 0.9.6
- [X] Add SqlFormat , e.g:
```C#
var sql = Db.SqlFormat("select * from {1}orders{2} where id = {0}id");
//if db is sqlserver
Assert.Equal("select * from [orders] where id = @id", sql);

//if db is oracle
Assert.Equal("select * from \"orders\" where id = :id", sql); 
```

#### Version 0.9.5
- [X] Add DbCache Model for ParameterPrefix,QuotePrefix,QuoteSuffix
- [X] Add Common Connection CreateParameter Without DbCommand
- [X] Add `public static DBConnectionType GetDbConnectionType(this Type connectionType)`

#### Version 0.9.4
- [X] Add ParameterExtension Dapper Style AddParams
- [X] Support AddConnection(Type connectionType,string connectionString) 
- [X] Add Editable DefaultCommandTimeout

#### Version 0.9.3
- [X] Add String Extension For Easy Execute Sql

#### Version 0.9.2
- [X] Add Db.AddConnection and GetConnection
using Microsoft.EntityFrameworkCore;
using static Dragon.Provider.ConnectionProvider;

namespace Dragon.Data
{
    public class MySqlContext : ContextTables
    {
        public MySqlContext() : base() { CurrentConnection = new Connection { DatabaseType = (byte)DatabaseType.MySql, Server = "localhost", Database = "Aarna", Password = "Avni@003", User = "admin", Port = 3306 }; }
        public MySqlContext(Connection connection) : base() { CurrentConnection = connection; }
        public MySqlContext(Connection connection, bool isMigrate) : base() { CurrentConnection = connection; if (isMigrate) { Database.Migrate(); } }
    }
    public class SqlServerContext : ContextTables
    {
        public SqlServerContext() : base() { CurrentConnection = new Connection { DatabaseType = (byte)DatabaseType.SqlServer, Server = ".", Database = "GenerationDB", User = "sa", Password = "Krishna@003" }; }
        public SqlServerContext(Connection connection) : base() { CurrentConnection = connection; }
        public SqlServerContext(Connection connection, bool isMigrate) : base() { CurrentConnection = connection; if (isMigrate) { Database.Migrate(); } }
    }
}

//Add-Migration FirstMigration -Context MySqlContext -o Migrations\MySql
//Add-Migration SecondMigration -Context MySqlContext -o Migrations\MySql

//Add-Migration FirstMigration -Context SqlServerContext -o Migrations\SqlServer
//Add-Migration SecondMigration -Context SqlServerContext -o Migrations\SqlServer

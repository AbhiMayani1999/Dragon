using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Npgsql;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dragon.Provider
{
    public sealed class ConnectionProvider
    {
        private readonly ConcurrentDictionary<string, Connection> _tenantConnections = new();
        private static readonly Lazy<ConnectionProvider> _connectionProvider = new(() => new ConnectionProvider());
        public static ConnectionProvider Provider => _connectionProvider.Value;
        private ConnectionProvider() { }
        public Connection GetConnection(string tenantCode)
        {
            List<Connection> connectionList = _tenantConnections.IsEmpty ? ReloadAllConnection() : [.. _tenantConnections.Values];
            return connectionList.FirstOrDefault(d => d.TenantCode == tenantCode);
        }
        public List<Connection> GetAllConnection()
        {
            return _tenantConnections.IsEmpty ? ReloadAllConnection() : [.. _tenantConnections.Values];
        }
        public List<Connection> ReloadAllConnection()
        {
            List<Connection> connectionList = [];
            lock (_tenantConnections)
            {
                _tenantConnections.Clear();
                if (SetConnection(ConfigProvider.Settings.MasterConnection.ToJson().FromJson<Connection>()))
                {
                    using ContextProvider.ConnectionContext connectionContext = new(GetConnection(ConfigProvider.MasterTenantName));
                    if (connectionContext.Database.CanConnect()) { connectionContext.Connection.ToList().ForEach(connection => SetConnection(connection)); }
                    connectionList = [.. _tenantConnections.Values];
                }
            }
            return connectionList;
        }
        private bool SetConnection(Connection connections)
        {
            return _tenantConnections.AddOrUpdate(connections.TenantCode, connections, (key, value) => connections) != null;
        }

        [Table($"Config{nameof(Connection)}")]
        public class Connection
        {
            [Key] public int Id { get; set; }
            [Required] public byte DatabaseType { get; set; }
            [StringLength(25)] public string TenantCode { get; set; }
            [StringLength(100)] public string Server { get; set; }
            [StringLength(100)] public string Database { get; set; }
            [StringLength(100)] public string User { get; set; }
            [StringLength(100)] public string Password { get; set; }
            [Required] public bool IsActive { get; set; } = true;
            [Required] public bool IsDeleted { get; set; } = false;
            public int? Port { get; set; } = 0;
        }
        public class ContextProvider : DbContext
        {
            internal class ConnectionContext : ContextProvider { public ConnectionContext(Connection connection) : base() { CurrentConnection = connection; } public DbSet<Connection> Connection { get; set; } }

            public enum DatabaseType : byte { SqlServer = 1, MySql = 2, Postgres = 3, Oracle = 4, SQLite = 5 }
            public static Connection CurrentConnection { get; set; }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder = (DatabaseType)CurrentConnection.DatabaseType switch
                {
                    DatabaseType.MySql => optionsBuilder.UseMySQL(ConnectionBuilder.GetMySqlConnection(CurrentConnection)),
                    DatabaseType.SQLite => optionsBuilder.UseSqlite(ConnectionBuilder.GetSQLiteConnection(CurrentConnection)),
                    DatabaseType.SqlServer => optionsBuilder.UseSqlServer(ConnectionBuilder.GetSqlServerConnection(CurrentConnection)),
                    DatabaseType.Postgres => optionsBuilder.UseNpgsql(ConnectionBuilder.GetPostgresSqlConnection(CurrentConnection)),
                    _ => optionsBuilder.UseSqlServer(ConnectionBuilder.GetSqlServerConnection(CurrentConnection))
                };
                base.OnConfiguring(optionsBuilder);
            }
            public static class ConnectionBuilder
            {
                public static string GetMySqlConnection(Connection connection)
                {
                    MySqlConnectionStringBuilder connectionBuilder = new()
                    {
                        Server = connection.Server,
                        UserID = connection.User,
                        Password = connection.Password,
                        Database = connection.Database,
                        Port = (uint)connection.Port,
                    };
                    return connectionBuilder.ToString();
                }
                public static string GetSQLiteConnection(Connection connection)
                {
                    SqliteConnectionStringBuilder connectionBuilder = new()
                    {
                        DataSource = connection.Database
                    };
                    return connectionBuilder.ConnectionString;
                }
                public static string GetSqlServerConnection(Connection connection)
                {
                    SqlConnectionStringBuilder connectionBuilder = new()
                    {
                        ConnectTimeout = 0,
                        IntegratedSecurity = false,
                        TrustServerCertificate = true,
                        DataSource = connection.Server,
                        InitialCatalog = connection.Database,
                        UserID = connection.User,
                        Password = connection.Password
                    };
                    return connectionBuilder.ConnectionString;
                }
                public static string GetPostgresSqlConnection(Connection connection)
                {
                    NpgsqlConnectionStringBuilder connectionBuilder = new()
                    {
                        Host = connection.Server,
                        Database = connection.Database,
                        Username = connection.User,
                        Password = connection.Password
                    };
                    return connectionBuilder.ConnectionString;
                }
            }
        }
    }
}
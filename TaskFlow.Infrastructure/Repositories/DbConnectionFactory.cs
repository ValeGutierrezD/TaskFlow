using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;
using TaskFlow.Core.Enum;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Infrastructure.Repositories
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _config;
        private readonly string _sqlConn;
        private readonly string _mySqlConn;
        public DatabaseProvider Provider { get; }

        public DbConnectionFactory(IConfiguration config)
        {
            _config = config;
            _sqlConn = _config.GetConnectionString("DefaultConnectionSqlServer") ?? string.Empty;
            _mySqlConn = _config.GetConnectionString("DefaultConnection") ?? string.Empty;
            var providerStr = _config.GetSection("DatabaseProvider").Value ?? "MySql";
            Provider = providerStr.Equals("MySql", StringComparison.OrdinalIgnoreCase) ? DatabaseProvider.MySql : DatabaseProvider.SqlServer;
        }

        public IDbConnection CreateConnection()
        {
            return Provider switch
            {
                DatabaseProvider.MySql => new MySqlConnection(_mySqlConn),
                _ => new SqlConnection(_sqlConn)
            };
        }
    }
}

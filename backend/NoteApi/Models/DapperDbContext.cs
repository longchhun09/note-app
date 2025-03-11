using Microsoft.Data.SqlClient;
using System.Data;

namespace NoteApi.Models
{
    public interface IDapperDbContext
    {
        IDbConnection CreateConnection();
    }

    public class DapperDbContext : IDapperDbContext
    {
        private readonly string _connectionString;

        public DapperDbContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
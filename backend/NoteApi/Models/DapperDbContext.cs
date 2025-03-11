using Microsoft.Data.SqlClient;
using System.Data;

namespace NoteApi.Models
{
    public interface IDapperDbContext
    {
        IDbConnection CreateConnection();
    }

    public class DapperDbContext(IConfiguration configuration) : IDapperDbContext
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found in configuration. " +
                "Please ensure your connection string is properly set in the application configuration.");

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
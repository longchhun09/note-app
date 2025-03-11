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
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
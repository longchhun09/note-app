using Dapper;
using NoteApi.Models;
using NoteApi.Repositories.Interfaces;

namespace NoteApi.Repositories
{
    public class NotesRepository : INotesRepository
    {
        private readonly IDapperDbContext _dbContext;

        private const string SQL_CREATE = @"
            INSERT INTO Notes (UserId, Title, Content, CreatedAt, UpdatedAt) 
            VALUES (@UserId, @Title, @Content, @CreatedAt, @UpdatedAt);
            SELECT CAST(SCOPE_IDENTITY() as int);";

        private string BuildGetAllByUserQuery(string searchTerm, string sortField, string sortOrder)
        {
            var sql = @"
                SELECT Id, UserId, Title, Content, CreatedAt, UpdatedAt 
                FROM Notes 
                WHERE UserId = @UserId";

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                sql += @" AND (Title LIKE @SearchTerm OR Content LIKE @SearchTerm)";
            }

            sql += " ORDER BY ";

            if (string.IsNullOrWhiteSpace(sortField))
            {
                sql += "UpdatedAt DESC";
            }
            else
            {
                switch (sortField.ToLower())
                {
                    case "title":
                        sql += "Title";
                        break;
                    case "createdat":
                        sql += "CreatedAt";
                        break;
                    case "updatedat":
                        sql += "UpdatedAt";
                        break;
                    default:
                        sql += "UpdatedAt";
                        break;
                }

                if (sortOrder?.ToLower() == "asc")
                {
                    sql += " ASC";
                }
                else
                {
                    sql += " DESC";
                }
            }

            return sql;
        }

        private const string SQL_GET_BY_ID = @"
            SELECT Id, UserId, Title, Content, CreatedAt, UpdatedAt 
            FROM Notes 
            WHERE Id = @Id AND UserId = @UserId";

        private const string SQL_UPDATE = @"
            UPDATE Notes
            SET Title = @Title,
                Content = @Content,
                UpdatedAt = @UpdatedAt
            WHERE Id = @Id AND UserId = @UserId";

        private const string SQL_DELETE = @"
            DELETE FROM Notes
            WHERE Id = @Id AND UserId = @UserId";

        public NotesRepository(IDapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateAsync(Note note)
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QuerySingleAsync<int>(SQL_CREATE, note);
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            using var connection = _dbContext.CreateConnection();
            var updatedRow = await connection.ExecuteAsync(SQL_DELETE, new { Id = id, UserId = userId });
            return updatedRow > 0;
        }

        public async Task<IEnumerable<Note>> GetAllByUserIdAsync(int userId, string searchTerm = null, string sortField = null, string sortOrder = null)
        {
            using var connection = _dbContext.CreateConnection();
            
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);
            
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                parameters.Add("@SearchTerm", $"%{searchTerm}%");
            }
            
            var sql = BuildGetAllByUserQuery(searchTerm, sortField, sortOrder);
            return await connection.QueryAsync<Note>(sql, parameters);
        }

        public async Task<Note> GetByIdAsync(int id, int userId)
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Note>(SQL_GET_BY_ID, new { Id = id, UserId = userId });
        }

        public async Task<bool> UpdateAsync(Note note)
        {
            using var connection = _dbContext.CreateConnection();
            var updatedRow = await connection.ExecuteAsync(SQL_UPDATE, note);
            return updatedRow > 0;
        }
    }
}
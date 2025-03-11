using Dapper;
using NoteApi.Data;
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

        private const string SQL_GET_ALL_BY_USER = @"
            SELECT Id, UserId, Title, Content, CreatedAt, UpdatedAt 
            FROM Notes 
            WHERE UserId = @UserId";

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

        public async Task<IEnumerable<Note>> GetAllByUserIdAsync(int userId)
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryAsync<Note>(SQL_GET_ALL_BY_USER, new { UserId = userId });
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
using Dapper;
using NoteApi.Data;
using NoteApi.Models;
using NoteApi.Repositories.Interfaces;

namespace NoteApi.Repositories
{
    public class NotesRepository : INotesRepository
    {
        private readonly IDapperDbContext _dbContext;

        public NotesRepository(IDapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateAsync(Note note)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"
                INSERT INTO Notes (UserId, Title, Content, CreatedAt, UpdatedAt) 
                VALUES (@UserId, @Title, @Content, @CreatedAt, @UpdatedAt);
                SELECT CAST(SCOPE_IDENTITY() as int);";
            return await connection.QuerySingleAsync<int>(sql, note);
        }

        public Task<bool> DeleteAsync(int id, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Note>> GetAllByUserIdAsync(int userId)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"
                SELECT Id, UserId, Title, Content, CreatedAt, UpdatedAt 
                FROM Notes 
                WHERE UserId = @UserId";
            return await connection.QueryAsync<Note>(sql, new { UserId = userId });        
        }

        public Task<Note> GetByIdAsync(int id, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Note note)
        {
            throw new NotImplementedException();
        }
    }
}
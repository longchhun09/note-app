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

        public Task<int> CreateAsync(Note note)
        {
            throw new NotImplementedException();
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
using System.Threading.Tasks;
using Dapper;
using NoteApi.Data;
using NoteApi.Data.Constants;
using NoteApi.Models;
using NoteApi.Repositories.Base;
using NoteApi.Repositories.Interfaces;

namespace NoteApi.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private new readonly IDapperDbContext _dbContext;

        public UserRepository(IDapperDbContext dbContext)
            : base(
                dbContext,
                UserQueries.Create,
                UserQueries.GetById,
                UserQueries.Update,
                UserQueries.Delete
            )
        {
            _dbContext = dbContext;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await base.DeleteAsync(id, 0);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id, 0);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(
                UserQueries.GetByUsername,
                new { Username = username }
            );
        }
    }
}

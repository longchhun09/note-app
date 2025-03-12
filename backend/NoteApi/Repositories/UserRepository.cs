using Dapper;
using NoteApi.Models;

namespace NoteApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDapperDbContext _dbContext;

        public UserRepository(IDapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateAsync(User user)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"
                INSERT INTO Users (Username, Email, PasswordHash, PasswordSalt, CreatedAt, IsActive, RefreshToken) 
                VALUES (@Username, @Email, @PasswordHash, @PasswordSalt, @CreatedAt, @IsActive, @RefreshToken);
                SELECT CAST(SCOPE_IDENTITY() as int)";

            var newUserId = await connection.ExecuteScalarAsync<int>(sql, new
            {
                user.Username,
                user.Email,
                user.PasswordHash,
                user.PasswordSalt,
                user.CreatedAt,
                user.IsActive,
                user.RefreshToken
            });

            return newUserId;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"
                UPDATE Users 
                SET Username = @Username,
                    PasswordHash = @PasswordHash 
                WHERE Id = @Id";
            var updatedRow = await connection.ExecuteAsync(sql, user);
            return updatedRow > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = "DELETE FROM Users WHERE Id = @Id";
            var updatedRow = await connection.ExecuteAsync(sql, new { Id = id });
            return updatedRow > 0;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"
                SELECT Id, Username, PasswordHash, CreatedAt 
                FROM Users 
                WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"
                SELECT Id, Username, PasswordHash, CreatedAt 
                FROM Users 
                WHERE Username = @Username";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
        }

    }
}
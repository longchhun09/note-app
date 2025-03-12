using Dapper;
using NoteApi.Models;

namespace NoteApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDapperDbContext _dbContext;

        private const string CreateUserSql = @"
            INSERT INTO Users (Username, Email, PasswordHash, PasswordSalt, CreatedAt, IsActive, RefreshToken) 
            VALUES (@Username, @Email, @PasswordHash, @PasswordSalt, @CreatedAt, @IsActive, @RefreshToken);
            SELECT CAST(SCOPE_IDENTITY() as int)";

        private const string UpdateUserSql = @"
            UPDATE Users 
            SET Username = @Username,
                PasswordHash = @PasswordHash 
            WHERE Id = @Id";

        private const string DeleteUserSql = "DELETE FROM Users WHERE Id = @Id";

        private const string GetUserByIdSql = @"
            SELECT Id, Username, PasswordHash, CreatedAt 
            FROM Users 
            WHERE Id = @Id";

        private const string GetUserByUsernameSql = @"
            SELECT Id, Username, PasswordHash, CreatedAt 
            FROM Users 
            WHERE Username = @Username";

        public UserRepository(IDapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateAsync(User user)
        {
            using var connection = _dbContext.CreateConnection();
            var newUserId = await connection.ExecuteScalarAsync<int>(CreateUserSql, new
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
            var updatedRow = await connection.ExecuteAsync(UpdateUserSql, user);
            return updatedRow > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _dbContext.CreateConnection();
            var updatedRow = await connection.ExecuteAsync(DeleteUserSql, new { Id = id });
            return updatedRow > 0;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(GetUserByIdSql, new { Id = id });
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(GetUserByUsernameSql, new { Username = username });
        }
    }
}
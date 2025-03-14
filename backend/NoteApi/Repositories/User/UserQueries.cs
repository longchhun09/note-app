namespace NoteApi.Data.Constants
{
    public static class UserQueries
    {
        public const string Create =
            @"
            INSERT INTO Users (Username, Email, PasswordHash, PasswordSalt, CreatedAt, IsActive, RefreshToken) 
            VALUES (@Username, @Email, @PasswordHash, @PasswordSalt, @CreatedAt, @IsActive, @RefreshToken);
            SELECT CAST(SCOPE_IDENTITY() as int)";

        public const string GetById =
            @"
            SELECT Id, Username, PasswordHash, PasswordSalt, CreatedAt 
            FROM Users 
            WHERE Id = @Id";

        public const string Update =
            @"
            UPDATE Users
            SET Username = @Username,
                PasswordHash = @PasswordHash
            WHERE Id = @Id";

        public const string Delete =
            @"
            DELETE FROM Users
            WHERE Id = @Id";

        public const string GetByUsername =
            @"
            SELECT Id, Username, PasswordHash, PasswordSalt, CreatedAt 
            FROM Users 
            WHERE Username = @Username";
    }
}

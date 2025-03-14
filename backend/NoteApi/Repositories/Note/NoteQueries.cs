namespace NoteApi.Data.Constants
{
    public static class NoteQueries
    {
        public const string Create =
            @"
            INSERT INTO Notes (UserId, Title, Content, CreatedAt, UpdatedAt) 
            VALUES (@UserId, @Title, @Content, @CreatedAt, @UpdatedAt);
            SELECT CAST(SCOPE_IDENTITY() as int);";

        public const string GetById =
            @"
            SELECT Id, UserId, Title, Content, CreatedAt, UpdatedAt 
            FROM Notes 
            WHERE Id = @Id AND UserId = @UserId";

        public const string Update =
            @"
            UPDATE Notes
            SET Title = @Title,
                Content = @Content,
                UpdatedAt = @UpdatedAt
            WHERE Id = @Id AND UserId = @UserId";

        public const string Delete =
            @"
            DELETE FROM Notes
            WHERE Id = @Id AND UserId = @UserId";

        public const string GetAllBase =
            @"
            SELECT Id, UserId, Title, Content, CreatedAt, UpdatedAt 
            FROM Notes 
            WHERE UserId = @UserId";
    }
}

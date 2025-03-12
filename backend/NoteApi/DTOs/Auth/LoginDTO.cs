namespace NoteApi.DTOs.Auth
{
    public class LoginDTO
    {
        public required string Username { get; set; }
        public required byte[] PasswordHash { get; set; }
        public required string Password { get; set; }
    }
}
using NoteApi.Services.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace NoteApi.Services
{
    public class PasswordService : IPasswordService
    {
        public byte[] HashPassword(string password)
        {
            string hashString = BC.HashPassword(password);
            return System.Text.Encoding.UTF8.GetBytes(hashString);
        }

        public bool VerifyPassword(string password, byte[] passwordHash)
        {
            string hashString = System.Text.Encoding.UTF8.GetString(passwordHash);
            return BC.Verify(password, hashString);
        }
    }
}
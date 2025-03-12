namespace NoteApi.Services.Interfaces
{
    public interface IPasswordService
    {
        byte[] HashPassword(string password);
        bool VerifyPassword(string password, byte[] hashedPassword);
    }
}
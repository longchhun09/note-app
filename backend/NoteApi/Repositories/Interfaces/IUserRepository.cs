using System.Threading.Tasks;
using NoteApi.Models;

namespace NoteApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<int> CreateAsync(User user);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(User user);
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
    }
}

using NoteApi.Models;

namespace NoteApi.Repositories.Interfaces
{
    public interface INotesRepository
    {
        Task<IEnumerable<Note>> GetAllByUserIdAsync(int userId, string? searchTerm = null, string? sortField = null, string? sortOrder = null);
        Task<Note> GetByIdAsync(int id, int userId);
        Task<int> CreateAsync(Note note);
        Task<bool> UpdateAsync(Note note);
        Task<bool> DeleteAsync(int id, int userId);
    }
}
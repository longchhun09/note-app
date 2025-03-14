using NoteApi.DTOs.Notes;

namespace NoteApi.Services.Interfaces
{
    public interface INotesService
    {
        Task<IEnumerable<NoteDTO>> GetAllNotesAsync(int userId, string searchTerm = null, string sortField = null, string sortOrder = "asc");
        Task<NoteDTO> GetNoteByIdAsync(int id, int userId);
        Task<NoteDTO> CreateNoteAsync(CreateNoteDTO noteDto, int userId);
        Task<bool> UpdateNoteAsync(int id, UpdateNoteDTO noteDto, int userId);
        Task<bool> DeleteNoteAsync(int id, int userId);
    }
}
using NoteApi.Models;
using NoteApi.DTOs.Notes;
using NoteApi.Services.Interfaces;

namespace NoteApi.Services
{
    public class NotesService : INotesService
    {
        public Task<NoteDTO> CreateNoteAsync(CreateNoteDTO noteDto, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteNoteAsync(int id, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<NoteDTO>> GetAllNotesAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<NoteDTO> GetNoteByIdAsync(int id, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateNoteAsync(int id, UpdateNoteDTO noteDto, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
using NoteApi.Models;
using NoteApi.DTOs.Notes;
using NoteApi.Services.Interfaces;
using NoteApi.Repositories.Interfaces;

namespace NoteApi.Services
{
    public class NotesService : INotesService
    {
        private readonly INotesRepository _notesRepository;

        public NotesService(INotesRepository notesRepository)
        {
            _notesRepository = notesRepository;
        }

        public async Task<NoteDTO> CreateNoteAsync(CreateNoteDTO noteDto, int userId)
        {
            var note = new Note
            {
                Title = noteDto.Title,
                Content = noteDto.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UserId = userId,
            };

            note.Id = await _notesRepository.CreateAsync(note);
            return new NoteDTO
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            };
        }

        public async Task<bool> DeleteNoteAsync(int id, int userId)
        {
            return await _notesRepository.DeleteAsync(id, userId);
        }

        public async Task<IEnumerable<NoteDTO>> GetAllNotesAsync(int userId)
        {
            var notes = await _notesRepository.GetAllByUserIdAsync(userId);
            return notes.Select(n => new NoteDTO
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                CreatedAt = n.CreatedAt,
                UpdatedAt = n.UpdatedAt
            });
        }

        public async Task<NoteDTO> GetNoteByIdAsync(int id, int userId)
        {
            var note = await _notesRepository.GetByIdAsync(id, userId);
            if (note == null)
                throw new KeyNotFoundException("Note not found");

            return new NoteDTO
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            };        
        }

        public async Task<bool> UpdateNoteAsync(int id, UpdateNoteDTO noteDto, int userId)
        {
            var updateNote = await _notesRepository.GetByIdAsync(id, userId);
            if (updateNote == null) {
                throw new KeyNotFoundException("Note not found");
            }
            
            updateNote.Title = noteDto.Title;
            updateNote.Content = noteDto.Content;
            updateNote.UpdatedAt = DateTime.UtcNow;

            return await _notesRepository.UpdateAsync(updateNote);
        }
    }
}
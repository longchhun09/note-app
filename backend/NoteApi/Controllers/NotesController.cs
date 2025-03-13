using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteApi.Data;
using NoteApi.DTOs.Notes;
using NoteApi.Models;
using NoteApi.Services.Interfaces;
using System.Security.Claims;

namespace NoteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly NoteDbContext _context;
        private readonly ILogger<NotesController> _logger;

        private readonly INotesService _notesService;

        public NotesController(NoteDbContext context, ILogger<NotesController> logger, INotesService notesService)
        {
            _context = context;
            _logger = logger;
            _notesService = notesService;
        }

        private int GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID claim not found");
            }
            return int.Parse(userIdClaim.Value);
        }

        // GET: api/notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteDTO>>> GetNotes([FromQuery] string searchTerm = null, [FromQuery] string sortField = null, [FromQuery] string sortOrder = "asc")
        {
            try
            {
                var userId = GetUserIdFromClaims();
                var notes = await _notesService.GetAllNotesAsync(userId, searchTerm, sortField, sortOrder);
                return Ok(notes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all notes");
                return StatusCode(500, "An error occurred while retrieving notes. Please try again later.");
            }
        }

        // GET: api/notes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(int id)
        {
            try
            {
                var userId = GetUserIdFromClaims();
                var note = await _notesService.GetNoteByIdAsync(id, userId);

                if (note == null)
                {
                    return NotFound($"Note with ID {id} not found");
                }

                return Ok(note);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting note with ID {Id}", id);
                return StatusCode(500, $"An error occurred while retrieving note {id}. Please try again later.");
            }
        }

        // POST: api/notes
        [HttpPost]
        public async Task<ActionResult<NoteDTO>> CreateNote(CreateNoteDTO noteDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetUserIdFromClaims();
                var note = await _notesService.CreateNoteAsync(noteDTO, userId);
                return CreatedAtAction(nameof(GetNote), new { id = note.Id }, note);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new note");
                return StatusCode(500, "An error occurred while creating the note. Please try again later.");
            }
        }

        // PUT: api/notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, UpdateNoteDTO noteDTO)
        {
            try
            {
                var userId = GetUserIdFromClaims();

                if (id != noteDTO.Id)
                {
                    return BadRequest("Note ID in the URL does not match the ID in the request body");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingNote = await _notesService.UpdateNoteAsync(id, noteDTO, userId);

                if (existingNote == null)
                {
                    return NotFound($"Note with ID {id} not found");
                }
                return existingNote ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating note with ID {Id}", id);
                return StatusCode(500, $"An error occurred while updating note {id}. Please try again later.");
            }
        }

        // DELETE: api/notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            try
            {
                var userId = GetUserIdFromClaims();
                var result = await _notesService.DeleteNoteAsync(id, userId);
                return result ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting note with ID {Id}", id);
                return StatusCode(500, $"An error occurred while deleting note {id}. Please try again later.");
            }
        }
    }
}


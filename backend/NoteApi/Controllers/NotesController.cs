using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoteApi.Data;
using NoteApi.Models;

namespace NoteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly NoteDbContext _context;
        private readonly ILogger<NotesController> _logger;

        public NotesController(NoteDbContext context, ILogger<NotesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {
            try
            {
                return await _context.Notes.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all notes");
                return StatusCode(500, "An error occurred while retrieving notes. Please try again later.");
            }
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(int id)
        {
            try
            {
                var note = await _context.Notes.FindAsync(id);

                if (note == null)
                {
                    return NotFound($"Note with ID {id} not found");
                }

                return note;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting note with ID {Id}", id);
                return StatusCode(500, $"An error occurred while retrieving note {id}. Please try again later.");
            }
        }

        // POST: api/Notes
        [HttpPost]
        public async Task<ActionResult<Note>> CreateNote(Note note)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                note.CreatedAt = DateTime.UtcNow;
                note.UpdatedAt = DateTime.UtcNow;

                _context.Notes.Add(note);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetNote), new { id = note.Id }, note);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new note");
                return StatusCode(500, "An error occurred while creating the note. Please try again later.");
            }
        }

        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, Note note)
        {
            try
            {
                if (id != note.Id)
                {
                    return BadRequest("Note ID in the URL does not match the ID in the request body");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingNote = await _context.Notes.FindAsync(id);
                if (existingNote == null)
                {
                    return NotFound($"Note with ID {id} not found");
                }

                // Preserve the original creation date
                note.CreatedAt = existingNote.CreatedAt;
                note.UpdatedAt = DateTime.UtcNow;

                _context.Entry(existingNote).State = EntityState.Detached;
                _context.Entry(note).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(id))
                    {
                        return NotFound($"Note with ID {id} not found");
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating note with ID {Id}", id);
                return StatusCode(500, $"An error occurred while updating note {id}. Please try again later.");
            }
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            try
            {
                var note = await _context.Notes.FindAsync(id);
                if (note == null)
                {
                    return NotFound($"Note with ID {id} not found");
                }

                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting note with ID {Id}", id);
                return StatusCode(500, $"An error occurred while deleting note {id}. Please try again later.");
            }
        }

        private bool NoteExists(int id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }
    }
}


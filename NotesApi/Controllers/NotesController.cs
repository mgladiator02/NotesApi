using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApi.Data;
using NotesApi.Models;
using System.Security.Claims;

namespace NotesApi.Controllers
{
    [Route("notes")]
    [ApiController]
    [Authorize] 
    public class NotesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotesController(AppDbContext context)
        {
            _context = context;
        }

    
        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim != null && int.TryParse(idClaim.Value, out int userId))
            {
                return userId;
            }
            throw new Exception("User not authenticated properly");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {
            var userId = GetCurrentUserId();
            // Zwracamy tylko notatki tego użytkownika
            return await _context.Notes.Where(n => n.UserId == userId).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Note>> CreateNote([FromBody] CreateNoteDto dto)
        {
            var userId = GetCurrentUserId();
            var note = new Note
            {
                Content = dto.Content,
                UserId = userId
            };

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            // Zwracamy 201 Created
            return CreatedAtAction(nameof(GetNotes), new { id = note.Id }, note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, [FromBody] UpdateNoteDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var userId = GetCurrentUserId();
            var note = await _context.Notes.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            
            if (note.UserId != userId)
            {
                return Forbid();
            }

            note.Content = dto.Content;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var userId = GetCurrentUserId();
            var note = await _context.Notes.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }

         
            if (note.UserId != userId)
            {
                return Forbid();
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
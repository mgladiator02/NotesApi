namespace NotesApi.Models
{
    public class UserDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    // DTO do tworzenia notatki
    public class CreateNoteDto
    {
        public string Content { get; set; } = string.Empty;
    }

    // DTO do edycji
    public class UpdateNoteDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
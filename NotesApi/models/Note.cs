namespace NotesApi.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;

        // Relacja z użytkownijiem
        public int UserId { get; set; }
        
        [System.Text.Json.Serialization.JsonIgnore]
        public User? User { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoteApi.Models
{
    public class Note
    {
        public int Id { get; set; }
        
        public required string Title { get; set; }
        
        public string? Content { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
        
        public int? UserId { get; set; }
        
        public User User { get; set; }
    }
}

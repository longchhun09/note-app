using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NoteApi.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public required string Username { get; set; }
        
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public required string Email { get; set; }
        
        [JsonIgnore]
        public required byte[] PasswordHash { get; set; }
        
        [JsonIgnore]
        public required byte[] PasswordSalt { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastLoginDate { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        [JsonIgnore]
        public string RefreshToken { get; set; } = string.Empty;
        
        [JsonIgnore]
        public DateTime? RefreshTokenExpiry { get; set; }
        
        [JsonIgnore]
        public ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}

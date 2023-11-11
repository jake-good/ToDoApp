using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TodoApi.Models;

namespace ToDoApi.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Email { get; set; }

        [Required]
        [JsonIgnore]
        public required string HashedPassword { get; set; } // Hashed and Salted
        // Navigation property for tasks associated with the user
        public ICollection<TodoItem> TodoItems { get; set; }

        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; }
    }

}
using System.ComponentModel.DataAnnotations;
using TodoApi.Models;

namespace ToDoApi.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; } // Hashed and Salted
        // Navigation property for tasks associated with the user
        public ICollection<TodoItem> TodoItems { get; set; }
    }

}
using System.ComponentModel.DataAnnotations;
using ToDoApi.Models;

namespace TodoApi.Models;

public class TodoItem
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Title { get; set; }

    public required string Description { get; set; }

    public int UserId { get; set; } // Foreign key to associate the task with a user

    // Navigation property for the user who owns the task
}
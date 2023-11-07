using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models.DTO;

public class TodoItemDTO
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    public string Description { get; set; }

    public int UserId { get; set; } // Foreign key to associate the task with a user
}
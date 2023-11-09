using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Models.DTO;

namespace ToDoApi.Controllers;

[ApiController]
[Route("api/ToDoItems")]
public class ToDoItemsController : ControllerBase
{
    private readonly TodoContext _context;
    public ToDoItemsController(TodoContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        return todoItem;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<TodoItem[]>> GetTodoItems()
    {
        var user = User;

        // Access individual claims
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        var account = _context.Users.FirstOrDefault(user => user.Username == username);
        var items = await _context.TodoItems.Where(e => e.UserId == account.UserId).ToArrayAsync();
        return items;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoItemDTO)
    {
        var user = await _context.Users.FindAsync(todoItemDTO.UserId);
        if (user == null)
        {
            return NotFound("No User found for item");
        }

        var todoItem = new TodoItem
        {
            Id = todoItemDTO.Id,
            Description = todoItemDTO.Description,
            UserId = todoItemDTO.UserId,
            Title = todoItemDTO.Title,
        };

        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItemDTO);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<TodoItem>> DeleteTodoItem(int id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);

        if (todoItem == null)
        {
            return NotFound();
        }
        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();
        return CreatedAtAction("DeleteTodoItem", new { id = todoItem.Id }, todoItem);
    }
}
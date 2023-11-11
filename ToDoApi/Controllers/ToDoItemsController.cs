using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Models.DTO;
using ToDoApi.Models;
using ToDoApi.Services;
using WebApi.Authorization;

namespace ToDoApi.Controllers;

[Authorize]
[ApiController]
[Route("api/ToDoItems")]
public class ToDoItemsController : ControllerBase
{
    private readonly TodoContext _context;
    private readonly ITodoService _todoService;
    public ToDoItemsController(TodoContext context, ITodoService todoService)
    {
        _context = context;
        _todoService = todoService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
    {
        return await _todoService.GetTodoItem(id);
    }

    [HttpGet]
    public async Task<ActionResult<TodoItem[]>> GetTodoItems()
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null) return NotFound("No User validated");
        return await _todoService.GetTodoItems(user.UserId);
    }

    [HttpPost]
    public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoItemDTO)
    {
        var item = await _todoService.CreateItemAsync(todoItemDTO);
        return CreatedAtAction("GetTodoItem", new { id = item.Id }, todoItemDTO);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<TodoItem>> DeleteTodoItem(int id)
    {
        var item = await _todoService.DeleteItemAsync(id);
        return CreatedAtAction("DeleteTodoItem", new { id = item.Id }, item);
    }
}
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Models.DTO;
using ToDoApi.Models;

namespace ToDoApi.Services
{
    public class TodoService : ITodoService
    {
        private TodoContext _context;
        public TodoService(TodoContext context)
        {
            _context = context;
        }
        public async Task<TodoItem> CreateItemAsync(TodoItemDTO todoItemDTO)
        {
            var item = new TodoItem
            {
                Id = todoItemDTO.Id,
                Title = todoItemDTO.Title,
                Description = todoItemDTO.Description,
                UserId = todoItemDTO.UserId
            };

            await _context.TodoItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<TodoItem> DeleteItemAsync(int id)
        {
            var item = _context.TodoItems.Find(id) ?? throw new KeyNotFoundException("Item not found");
            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<TodoItem> GetTodoItem(int id)
        {
            var item = await _context.TodoItems.FindAsync(id) ?? throw new KeyNotFoundException("Item not found");
            return item;
        }

        public Task<TodoItem[]> GetTodoItems(int userId)
        {
            return _context.TodoItems.Where(item => item.UserId == userId).ToArrayAsync();
        }

        public async Task<TodoItem> UpdateItem(int id, TodoItemDTO todoItemDTO)
        {
            var item = _context.TodoItems.Find(id) ?? throw new KeyNotFoundException("Item not found");
            item.Title = todoItemDTO.Title;
            item.Description = todoItemDTO.Description;
            _context.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }
    }
}
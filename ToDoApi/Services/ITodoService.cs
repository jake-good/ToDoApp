using TodoApi.Models;
using TodoApi.Models.DTO;

namespace ToDoApi.Services
{
    public interface ITodoService
    {
        public Task<TodoItem> GetTodoItem(int id);
        public Task<TodoItem[]> GetTodoItems(int userId);
        public Task<TodoItem> UpdateItem(int id, TodoItemDTO todoItemDTO);
        public Task<TodoItem> CreateItemAsync(TodoItemDTO todoItemDTO);
        public Task<TodoItem> DeleteItemAsync(int id);

    }
}
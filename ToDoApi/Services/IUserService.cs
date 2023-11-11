using ToDoApi.Models;
using ToDoApi.Models.DTO;
using WebApi.Models.Users;

namespace ToDoApi.Services
{
    public interface IUserSerivce
    {
        public Task<User> DeleteUser(int id);
        public Task<User[]> GetAll();
        public Task AddUserAsync(UserRegistrationModel userRegistrationModel);
        public AuthenticateResponse Authenticate(UserRegistrationModel model);
        public Task<AuthenticateResponse> RefreshTokenAsync(string token);
        public Task<User> GetById(int id);
        public Task RevokeTokenAsync(string token);
    }
}
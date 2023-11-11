using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TodoApi.Models;
using ToDoApi.Auth;
using ToDoApi.Models;
using ToDoApi.Models.DTO;
using WebApi.Helpers;
using WebApi.Models.Users;

namespace ToDoApi.Services
{
    public class UserSerivce : IUserSerivce
    {
        private readonly TodoContext _context;
        private readonly AppSettings _appSettings;
        private readonly IJwtUtils _jwtUtils;
        public UserSerivce(TodoContext userContext, IOptions<AppSettings> appSettings, IJwtUtils jwtUtils)
        {
            _context = userContext;
            _appSettings = appSettings.Value;
            _jwtUtils = jwtUtils;
        }

        public async Task AddUserAsync(UserRegistrationModel userRegistrationModel)
        {
            var user = new User { Username = userRegistrationModel.Username, Email = "email", HashedPassword = HashPassword(userRegistrationModel.Password) };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public AuthenticateResponse Authenticate(UserRegistrationModel model)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == model.Username);

            // validate
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.HashedPassword))
                throw new AppException("Username or password is incorrect");


            var accessToken = _jwtUtils.GenerateJwtToken(user);
            var refreshToken = _jwtUtils.GenerateRefreshToken();
            user.RefreshTokens.Add(refreshToken);

            _context.Update(user);
            _context.SaveChanges();
            return new AuthenticateResponse(user, accessToken, refreshToken.Token);
        }

        public async Task<AuthenticateResponse> RefreshTokenAsync(string token)
        {
            var user = await getUserByRefreshTokenAsync(token);
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
            if (refreshToken.IsRevoked)
            {
                // revoke all descendant tokens in case this token has been compromised
                revokeDescendantRefreshTokens(refreshToken, user, $"Attempted reuse of revoked ancestor token: {token}");
                _context.Update(user);
                _context.SaveChanges();
            }

            if (!refreshToken.IsActive)
                throw new AppException("Invalid token");

            // replace old refresh token with a new one (rotate token)
            var newRefreshToken = rotateRefreshToken(refreshToken);
            user.RefreshTokens.Add(newRefreshToken);

            // remove old refresh tokens from user
            removeOldRefreshTokens(user);

            // save changes to db
            _context.Update(user);
            _context.SaveChanges();

            // generate new jwt
            var jwtToken = _jwtUtils.GenerateJwtToken(user);

            return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
        }

        public async Task RevokeTokenAsync(string token)
        {
            var user = await getUserByRefreshTokenAsync(token);
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
                throw new AppException("Invalid token");

            // revoke token and save
            revokeRefreshToken(refreshToken, "Revoked without replacement");
            _context.Update(user);
            _context.SaveChanges();
        }

        public Task<User[]> GetAll()
        {
            return _context.Users.ToArrayAsync();
        }

        public async Task<User> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id) ?? throw new KeyNotFoundException("User not found");
            return user;
        }

        public async Task<User> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id) ?? throw new KeyNotFoundException("User not found");
            _context.Users.Remove(user);
            _context.SaveChanges();
            return user;
        }

        // helper methods

        private async Task<User> getUserByRefreshTokenAsync(string token)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token)) ?? throw new AppException("Invalid token");
            return user;
        }

        private RefreshToken rotateRefreshToken(RefreshToken refreshToken)
        {
            var newRefreshToken = _jwtUtils.GenerateRefreshToken();
            revokeRefreshToken(refreshToken, "Replaced by new token", newRefreshToken.Token);
            return newRefreshToken;
        }

        private void removeOldRefreshTokens(User user)
        {
            // remove old inactive refresh tokens from user based on TTL in app settings
            user.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }

        private void revokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string reason)
        {
            // recursively traverse the refresh token chain and ensure all descendants are revoked
            if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
                if (childToken != null)
                {
                    if (childToken.IsActive)
                        revokeRefreshToken(childToken, reason);
                    else
                        revokeDescendantRefreshTokens(childToken, user, reason);
                }
            }
        }

        private void revokeRefreshToken(RefreshToken token, string? reason = null, string? replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
        }

        public string HashPassword(string plainTextPassword)
        {
            // Generate a salt and hash the password
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainTextPassword, salt);
            return hashedPassword;
        }
    }
}
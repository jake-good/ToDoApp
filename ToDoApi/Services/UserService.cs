using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Models;

public class UserSerivce : IUserSerivce
{
    private TodoContext _context;
    public UserSerivce(TodoContext userContext)
    {
        _context = userContext;
    }
    public string HashPassword(string plainTextPassword)
    {
        // Generate a salt and hash the password
        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainTextPassword, salt);
        return hashedPassword;
    }

    public bool IsUserValid(string username, string password)
    {
        // Replace this with your actual database query to retrieve the user's hashed password
        // You should also retrieve the user's salt to verify the password
        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        if (user != null)
        {
            // Verify the password using the hashed password from the database and the provided password
            return BCrypt.Net.BCrypt.Verify(password, user.HashedPassword);
        }

        return false;
    }

    public string GenerateJwtToken(string username)
    {
        var claims = new List<Claim>
            {
                new (ClaimTypes.Name, username)
                // Add any additional claims if needed
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKeyYourSecretKeyYourSecretKeyYourSecretKey"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddHours(1); // Set token expiration time

        var token = new JwtSecurityToken(
            "YourIssuer",
            "YourAudience",
            claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
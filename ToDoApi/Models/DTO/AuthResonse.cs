namespace WebApi.Models.Users;

using System.Text.Json.Serialization;
using ToDoApi.Models;

public class AuthenticateResponse
{
    public int UserId { get; set; }
    public string JwtToken { get; set; }

    [JsonIgnore] // refresh token is returned in http only cookie
    public string RefreshToken { get; set; }

    public AuthenticateResponse(User user, string jwtToken, string refreshToken)
    {
        UserId = user.UserId;
        JwtToken = jwtToken;
        RefreshToken = refreshToken;
    }
}
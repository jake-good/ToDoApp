public interface IUserSerivce
{
    public string HashPassword(string password);
    public bool IsUserValid(string username, string password);
    public string GenerateJwtToken(string username);
}
namespace Users.Api;

public class UserRegistrationRequest
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public int Type { get; set; }
}
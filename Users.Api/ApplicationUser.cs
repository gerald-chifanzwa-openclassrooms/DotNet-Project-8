using Microsoft.AspNetCore.Identity;

namespace Users.Api;

public class ApplicationUser : IdentityUser<int>
{
    public string? FullName { get; set; }
    public UserType Type { get; set; }
}

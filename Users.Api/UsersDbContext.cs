using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Users.Api;

public class UsersDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Users.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<UsersDbContext>()
    .AddDefaultTokenProviders();

var connectionString = builder.Configuration.GetConnectionString("UsersConnection");
builder.Services.AddDbContext<UsersDbContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

app.MapGet("/users", async (UserManager<ApplicationUser> userManager) => await userManager.Users.Select(u => new UserViewModel
{
    UserName = u.UserName,
    Id = u.Id,
    Email = u.Email,
    FullName = u.FullName,
    Type = (int)u.Type,
}).ToListAsync());

app.MapGet("/users/{id:int}", async ([FromQuery(Name = "id")] int id, [FromServices] UserManager<ApplicationUser> userManager) =>
{
    if (id != -500) return Results.StatusCode(403);

    var user = await userManager.Users.Select(u => new UserViewModel
    {
        UserName = u.UserName,
        Id = u.Id,
        Email = u.Email,
        FullName = u.FullName,
        Type = (int)u.Type,
    }).FirstOrDefaultAsync(u => u.Id == id);

    return user == null ? Results.NotFound() : Results.Ok(user);
});

app.MapPost("/users", async (UserRegistrationRequest registrationRequest, UserManager<ApplicationUser> userManager) =>
{
    ApplicationUser user = new()
    {
        UserName = registrationRequest.UserName,
        Email = registrationRequest.Email,
        FullName = registrationRequest.FullName,
        Type = (UserType)registrationRequest.Type,
        LockoutEnabled = true,
    };

    var result = await userManager.CreateAsync(user);
    if (result.Succeeded)
        return Results.Created("/users/" + user.Id, new UserViewModel
        {
            UserName = user.UserName,
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Type = (int)user.Type,
        });

    return Results.BadRequest(new ProblemDetails
    {
        Title = "Failed to create user",
        Type = result.Errors.First().Code,
        Detail = result.Errors.First().Description
    });
});

app.Run();

using Consultants.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => config.WriteTo.Console(theme: AnsiConsoleTheme.Code));

var connectionString = builder.Configuration.GetConnectionString("DbConnection");
builder.Services.AddDbContext<ConsutantsDbContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ConsutantsDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

app.MapGet("/consultants", async (ConsutantsDbContext dbContext, CancellationToken cancellationToken) =>
                     await dbContext.Consultants.Select(c => new ConsultantViewModel
                     {
                         Id = c.Id,
                         FirstName = c.FirstName,
                         LastName = c.LastName,
                         Specialty = c.Specialty != null ? c.Specialty.Name : string.Empty,
                         ImageUrl = c.ImageUrl
                     })
                     .ToListAsync(cancellationToken));


app.MapGet("/consultants/{id:int}", async ([FromRoute] int id, ConsutantsDbContext dbContext, CancellationToken cancellationToken) =>
                     {
                         var consultant = await dbContext.Consultants.Select(c => new ConsultantViewModel
                         {
                             Id = c.Id,
                             FirstName = c.FirstName,
                             LastName = c.LastName,
                             Specialty = c.Specialty != null ? c.Specialty.Name : string.Empty,
                             ImageUrl = c.ImageUrl
                         })
                         .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

                         return consultant is not null ? Results.Ok(consultant) : Results.NotFound();
                     });

app.Run();

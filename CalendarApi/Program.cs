using CalendarApi;
using CalendarApi.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DbConnection");
builder.Services.AddDbContext<CalendarDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddSingleton<RedLockFactory>(sp =>
{
    ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
    return RedLockFactory.Create(new List<RedLockMultiplexer> { redis });
});


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.MapGet("/appointments", async ([FromQuery] ListAppointmentsRequest request, IMediator mediator) =>
{
    var appointments = await mediator.Send(request);
    return appointments;
});
app.MapPost("/appointments", async ([FromBody] BookAppointmentRequest request, IMediator mediator) =>
{
    var appointment = await mediator.Send(request);
    return Results.Created($"/appointments/{appointment.Id}", appointment);
});

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
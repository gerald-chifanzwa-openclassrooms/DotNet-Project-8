using System.Net;
using System.Net.Http.Headers;
using CalendarApi;
using CalendarApi.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, config) => config.WriteTo.Console(theme: AnsiConsoleTheme.Code));

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("CalendarConnection");
builder.Services.AddDbContext<CalendarDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddSingleton<RedLockFactory>(sp =>
{

    var endPoints = new List<RedLockEndPoint>
    {
        new DnsEndPoint(builder.Configuration.GetValue<string>("RedisInstance"), 6379),
    };
    return RedLockFactory.Create(endPoints);
    // ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(builder.Configuration.GetValue<string>("RedisInstance"));
    // return RedLockFactory.Create(new List<RedLockMultiplexer> { redis });
});
builder.Services.AddSingleton<IConcurrencyManager, ConcurrencyManager>();
builder.Services.AddHttpClient("Consultants", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Services:Consultants"));
    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

var app = builder.Build();
using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CalendarDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}
// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();


app.MapGet("/appointments", async (int? patientId, int? consultantId, DateTime? date, IMediator mediator) =>
                                   await mediator.Send(new ListAppointmentsRequest()
                                   {
                                       PatientId = patientId,
                                       AppointmentDate = date,
                                       ConsultantId = consultantId
                                   }));
app.MapPost("/appointments", async ([FromBody] BookAppointmentRequest request, IMediator mediator) =>
{
    var result = await mediator.Send(request);
    return result switch
    {
        BookingSuccessResult successResult => Results.Created($"/appointments/{successResult.Appointment.Id}", successResult.Appointment),
        BookingErrorResult errorResult => Results.BadRequest(new ProblemDetails { Title = "Booking failed", Detail = errorResult.Reason }),
        _ => Results.StatusCode(500)
    };
});

app.Run();
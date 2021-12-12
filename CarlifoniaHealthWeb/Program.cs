using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => config.WriteTo.Console(theme: AnsiConsoleTheme.Code));
// Add services to the container.
builder.Services.AddRazorPages();

var consultantsServiceUrl = builder.Configuration.GetValue<string>("Services:Consultants");
var calendarServiceUrl = builder.Configuration.GetValue<string>("Services:Calendar");
builder.Services.AddHttpClient("ConsultantsService", options => options.BaseAddress = new Uri(consultantsServiceUrl!));
builder.Services.AddHttpClient("CalendarService", options => options.BaseAddress = new Uri(calendarServiceUrl!));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

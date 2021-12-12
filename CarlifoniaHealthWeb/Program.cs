using Auth0.AspNetCore.Authentication;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => config.WriteTo.Console(theme: AnsiConsoleTheme.Code));
// Add services to the container.

// Cookie configuration for HTTP to support cookies with SameSite=None
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    static void CheckSameSite(CookieOptions options)
    {
        if (options.SameSite == SameSiteMode.None && options.Secure == false)
        {
            options.SameSite = SameSiteMode.Unspecified;
        }
    }

    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
    options.OnAppendCookie = cookieContext => CheckSameSite(cookieContext.CookieOptions);
    options.OnDeleteCookie = cookieContext => CheckSameSite(cookieContext.CookieOptions);
});

// Cookie configuration for HTTPS
// services.Configure<CookiePolicyOptions>(options =>
// {
//    options.MinimumSameSitePolicy = SameSiteMode.None
// });


builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
});
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
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
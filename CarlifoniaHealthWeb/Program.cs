using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var consultantsServiceUrl = builder.Configuration.GetValue<string>("Services:Consultants");
builder.Services.AddHttpClient("ConsultantsService", options => options.BaseAddress = new Uri(consultantsServiceUrl!));


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

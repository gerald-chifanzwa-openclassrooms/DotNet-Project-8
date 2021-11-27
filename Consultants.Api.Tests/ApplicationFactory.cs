using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Consultants.Api.Tests;

public class ApplicationFactory : WebApplicationFactory<ConsultantViewModel>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(services =>
        {
            var dbOptionsService = services.FirstOrDefault(s => s.ServiceType == typeof(DbContextOptions<ConsutantsDbContext>));
            if (dbOptionsService != null)
                services.Remove(dbOptionsService);
            services.AddDbContext<ConsutantsDbContext>(dbOptions => dbOptions.UseInMemoryDatabase(nameof(ConsutantsDbContext)));
        });
    }
}

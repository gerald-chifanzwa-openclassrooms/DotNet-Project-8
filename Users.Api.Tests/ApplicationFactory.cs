using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Users.Api;

public class ApplicationFactory : WebApplicationFactory<UsersDbContext>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(services =>
        {
            var dbOptionsService = services.FirstOrDefault(s => s.ServiceType == typeof(DbContextOptions<UsersDbContext>));
            if (dbOptionsService != null)
                services.Remove(dbOptionsService);
            services.AddDbContext<UsersDbContext>(dbOptions => dbOptions.UseInMemoryDatabase(nameof(UsersDbContext)));
        });
    }
}


using Microsoft.EntityFrameworkCore;

namespace Consultants.Api;

public class ConsutantsDbContext : DbContext
{
    public ConsutantsDbContext(DbContextOptions<ConsutantsDbContext> options) : base(options) { }

    public DbSet<Consultant> Consultants => Set<Consultant>();
    public DbSet<Specialty> Specialties => Set<Specialty>();
}

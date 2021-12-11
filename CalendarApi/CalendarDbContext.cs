namespace CalendarApi;

using Microsoft.EntityFrameworkCore;

public class CalendarDbContext : DbContext
{
    public CalendarDbContext(DbContextOptions<CalendarDbContext> options) : base(options)
    {
    }

    public DbSet<Appointment> Appointments => Set<Appointment>();
}
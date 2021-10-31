using CalendarApi.EntityConfiguratoin;
using Microsoft.EntityFrameworkCore;

namespace CalendarApi;

[EntityTypeConfiguration(typeof(AppointmentStatusEntityConfiguration))]
public class AppointmentStatus
{
    public int Id { get; set; }
    public string? Status { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsRescheduled { get; set; }
}

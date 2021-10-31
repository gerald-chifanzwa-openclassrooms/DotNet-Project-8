using CalendarApi.EntityConfiguratoin;
using Microsoft.EntityFrameworkCore;

namespace CalendarApi;

[EntityTypeConfiguration(typeof(AppointmentEntityConfiguration))]
public class Appointment
{
    public int Id { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public int ConsultantId { get; set; }

    public int PatientId { get; set; }
    public int StatusId { get; set; }
    public AppointmentStatus? Status { get; set; }
}

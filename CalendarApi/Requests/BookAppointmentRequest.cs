using CalendarApi.ViewModels;
using MediatR;

namespace CalendarApi.Requests;

public class BookAppointmentRequest : IRequest<BookingResult>
{
    public DateTime AppointmentDate { get; set; }
    public int ConsultantId { get; set; }
    public int PatientId { get; set; }
}

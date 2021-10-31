using CalendarApi.ViewModels;
using MediatR;

namespace CalendarApi.Requests;

public class ListAppointmentsRequest : IRequest<IReadOnlyCollection<AppointmentViewModel>>
{
    public int? PatientId { get; set; }
    public int? ConsultantId { get; set; }
    public DateTime? AppointmentDate { get; set; }
}

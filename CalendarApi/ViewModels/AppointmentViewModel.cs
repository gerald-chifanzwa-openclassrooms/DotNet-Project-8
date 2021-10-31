namespace CalendarApi.ViewModels;

public class AppointmentViewModel
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Status { get; set; }
    public bool IsCompleted { get; set; }
    public int PatientId { get; set; }
    public int ConsultantId { get; set; }
}

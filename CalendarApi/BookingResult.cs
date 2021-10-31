using CalendarApi.ViewModels;

namespace CalendarApi;

public abstract class BookingResult
{
    public static BookingResult Success(AppointmentViewModel viewModel) => new BookingSuccessResult(viewModel);
    public static BookingResult Error(string reason) => new BookingErrorResult(reason);
}

public class BookingSuccessResult : BookingResult
{
    public BookingSuccessResult(AppointmentViewModel viewModel) => Appointment = viewModel;
    public AppointmentViewModel Appointment { get; }
}

public class BookingErrorResult : BookingResult
{
    public BookingErrorResult(string reason) => Reason = reason;
    public string Reason { get; }
}

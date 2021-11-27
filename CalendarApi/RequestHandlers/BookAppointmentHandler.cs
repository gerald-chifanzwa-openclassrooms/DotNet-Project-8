using System.Net;
using CalendarApi.Requests;
using CalendarApi.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CalendarApi.RequestHandlers;

public class BookAppointmentHandler : IRequestHandler<BookAppointmentRequest, BookingResult>
{
    private readonly CalendarDbContext _dbContext;
    private readonly IConcurrencyManager _concurrencyManager;
    private readonly IHttpClientFactory _httpClientFactory;

    public BookAppointmentHandler(CalendarDbContext dbContext,
        IConcurrencyManager concurrencyManager,
        IHttpClientFactory httpClientFactory)
    {
        _dbContext = dbContext;
        _concurrencyManager = concurrencyManager;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<BookingResult> Handle(BookAppointmentRequest request, CancellationToken cancellation)
    {
        var httpClient = _httpClientFactory.CreateClient("Consultants");
        ConsultantViewModel? consultant = null;
        try
        {
            consultant = await httpClient.GetFromJsonAsync<ConsultantViewModel>($"/consultants/{request.ConsultantId}");
        }
        catch (HttpRequestException httpException) when (httpException.StatusCode == HttpStatusCode.NotFound)
        {
            return BookingResult.Error("Invalid consultant selected");
        }

        try
        {
            return await _concurrencyManager.Execute("BOOK_APPOINTMENT", () => ProcessBooking(consultant!, request));
        }
        catch (ConcurrencyException)
        {
            return BookingResult.Error("Your booking clashed with another booking in progress. To avoid conflicts, please try again in a second or two");
        }
    }

    private async Task<BookingResult> ProcessBooking(ConsultantViewModel consultant, BookAppointmentRequest request)
    {

        var clashingAppointments = await _dbContext.Appointments.Include(a => a.Status)
        .Where(a => a.ConsultantId == consultant.Id &&
                a.StartDate >= request.AppointmentDate &&
                a.EndDate <= request.AppointmentDate &&
                a.Status!.IsCompleted == false &&
                a.Status.IsRescheduled == false)
        .ToListAsync();

        if (clashingAppointments.Any())
            return BookingResult.Error("Consultant has already been booked for the stated date");

        var appointment = new Appointment
        {
            ConsultantId = consultant!.Id,
            ConsultantName = string.Join(" ", consultant.FirstName, consultant.LastName),
            StartDate = request.AppointmentDate,
            EndDate = request.AppointmentDate,
            PatientId = request.PatientId,
            StatusId = 1
        };
        _dbContext.Appointments.Add(appointment);
        await _dbContext.SaveChangesAsync();

        return BookingResult.Success(new AppointmentViewModel()
        {
            Id = appointment.Id,
            ConsultantId = appointment.ConsultantId,
            IsCompleted = false,
            Status = "Booked",
            StartDate = appointment.StartDate,
            EndDate = request.AppointmentDate,
        });
    }
}

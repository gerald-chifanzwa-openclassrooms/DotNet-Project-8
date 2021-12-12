using System.Text.Json;
using CarlifoniaHealthWeb.ViewModels;
using CarliforniaHealthWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarlifoniaHealthWeb.Pages;

public class ConsultantCalendarViewModel : PageModel
{
    private readonly HttpClient _consultantsServiceClient;
    private readonly HttpClient _calendarServiceClient;
    private readonly ILogger<ConsultantCalendarViewModel> _logger;

    public ConsultantCalendarViewModel(IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory)
    {
        _consultantsServiceClient = httpClientFactory.CreateClient("ConsultantsService");
        _calendarServiceClient = httpClientFactory.CreateClient("CalendarService");

        _logger = loggerFactory.CreateLogger<ConsultantCalendarViewModel>();
    }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }
    public Consultant? SelectedConsultant { get; set; }

    public IReadOnlyCollection<AppointmentViewModel> ConsultantSchedule { get; set; } = new List<AppointmentViewModel>();
    public AppointmentViewModel? Result { get; set; }
    public string? Error { get; set; }


    public async Task OnGet()
    {
        await LoadConsultant();
        await LoadConsultantCalendar();
    }

    public async Task OnPostBookAppointment(DateTime appointmentDate)
    {
        _logger.LogInformation("Attempting to book an appointment  on {0:dd-mm-yyyy}", appointmentDate);

        var response = await _calendarServiceClient.PostAsJsonAsync("/appointments", new
        {
            AppointmentDate = appointmentDate,
            ConsultantId = Id,
            PatientId = User?.Identity?.Name?.GetHashCode() ?? 1// Request.HttpContext.TraceIdentifier.GetHashCode()
        });
        var responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            Result = JsonSerializer.Deserialize<AppointmentViewModel>(responseContent, options);

            _logger.LogInformation("Appointment booked {@Result}", Result);
        }
        else
        {
            _logger.LogWarning("Failed to secure appointment");
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(responseContent);
            Error = problemDetails?.Detail ?? "Failed to secure appointment, please try again later";
        }
    }

    private async Task LoadConsultantCalendar()
    {
        _logger.LogInformation("Fetching calendar for consultant {@Id}", Id);

        try
        {
            var results = await _calendarServiceClient.GetFromJsonAsync<IReadOnlyCollection<AppointmentViewModel>>($"/appointments?consultantId={Id}");
            _logger.LogInformation("Consultant Appointments: {@Results}", results);
            ConsultantSchedule = results!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load consultant calendar");
        }
    }

    private async Task LoadConsultant()
    {
        _logger.LogInformation("Fetching consultant details for ID {@Id}", Id);
        try
        {
            SelectedConsultant = await _consultantsServiceClient.GetFromJsonAsync<Consultant>($"/consultants/{Id}");
            _logger.LogInformation("Consultant: {@SelectedConsultant}", SelectedConsultant);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load consultant");
        }
    }
}

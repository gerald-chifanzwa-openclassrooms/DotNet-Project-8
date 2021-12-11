using CarlifoniaHealthWeb.ViewModels;
using CarliforniaHealthWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarlifoniaHealthWeb.Pages;

public class ConsultantCalendarViewModel : PageModel
{
    private readonly HttpClient _consultantsServiceClient;
    private readonly ILogger<ConsultantCalendarViewModel> _logger;

    public ConsultantCalendarViewModel(IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory)
    {
        _consultantsServiceClient = httpClientFactory.CreateClient("Consultants");
        _logger = loggerFactory.CreateLogger<ConsultantCalendarViewModel>();
    }

    public Consultant? SelectedConsultant { get; set; }


    public async Task OnGet([FromRoute] int id)
    {
        _logger.LogInformation("Fetching consultant details...");
        SelectedConsultant = await _consultantsServiceClient.GetFromJsonAsync<Consultant>("/consultant/");

    }
}

using System.Reflection.Metadata;
using System.Reflection;
using CarlifoniaHealthWeb.ViewModels;
using CarliforniaHealthWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarlifoniaHealthWeb.Pages;
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly HttpClient _httpClient;

    public IndexModel(IHttpClientFactory httpClientFactory, ILogger<IndexModel> logger)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("ConsultantsService");
    }

    public IReadOnlyCollection<Consultant> Consultants { get; set; } = new List<Consultant>();

    public async Task OnGet()
    {
        _logger.LogInformation("Fetching consultants...");
        try
        {
            var consultants = await _httpClient.GetFromJsonAsync<IReadOnlyCollection<Consultant>>("/consultants");
            _logger.LogInformation("Consultants: {@Consultants}", consultants);
            Consultants = consultants ?? new List<Consultant>();
        }
        catch (HttpRequestException ex) when (ex.StatusCode != System.Net.HttpStatusCode.InternalServerError)
        {
            _logger.LogWarning("Cannot retrieve consultants at the moment", ex);
        }
    }

    public IActionResult OnPostViewConsultant(int consultantId)
    {
        _logger.LogInformation("Redirecting to consultant view");
        return RedirectToPage("ConsultantCalendarView", new { id = consultantId });
    }
}

using CarliforniaHealthWeb.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarlifoniaHealthWeb.ViewModels;

public class ConsultantModelList
{
    public IReadOnlyCollection<ConsultantCalendar> ConsultantCalendars { get; set; } = new List<ConsultantCalendar>();
    public IReadOnlyCollection<Consultant> Consultants { get; set; } = new List<Consultant>();
    public int SelectedConsultantId { get; set; }
    public SelectList? ConsultantsList { get; set; }
}
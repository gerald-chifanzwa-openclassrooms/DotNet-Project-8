using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarlifoniaHealthWeb.ViewModels;

public class ConsultantModelList
{
    public IReadOnlyCollection<ConsultantCalendar> ConsultantCalendars { get; set; }
    public IReadOnlyCollection<Consultant> Consultants { get; set; }
    public int SelectedConsultantId { get; set; }
    public SelectList ConsultantsList { get; set; }
}

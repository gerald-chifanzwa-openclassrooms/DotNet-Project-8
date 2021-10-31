using System.Linq.Expressions;
using CalendarApi.Requests;
using CalendarApi.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;

namespace CalendarApi.RequestHandlers;

public class ListAppointmentsHandler : IRequestHandler<ListAppointmentsRequest, IReadOnlyCollection<AppointmentViewModel>>
{
    private readonly CalendarDbContext _dbContext;

    public ListAppointmentsHandler(CalendarDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<AppointmentViewModel>> Handle(ListAppointmentsRequest request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Appointments.Where(a =>
            (request.PatientId == null || request.PatientId == a.PatientId) &&
            (request.ConsultantId == null || request.ConsultantId == a.ConsultantId) &&
            (request.AppointmentDate == null || (a.StartDate >= request.AppointmentDate && a.EndDate <= request.AppointmentDate))
        )
            .Select(a => new AppointmentViewModel
            {
                ConsultantId = a.ConsultantId,
                EndDate = a.EndDate,
                StartDate = a.StartDate,
                Status = a.Status == null ? string.Empty : a.Status.Status,
                IsCompleted = a.Status != null && a.Status.IsCompleted,
                Id = a.Id,
                PatientId = a.PatientId,
            });

        var results = await query.ToListAsync(cancellationToken);
        return results.AsReadOnly();
    }
}

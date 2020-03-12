using CalifornianHealthMonolithic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalifornianHealthMonolithic.Controllers
{
    public class BookingController : Controller
    {
        // GET: Booking
        //TODO: Change this method to display the consultant calendar. Ensure that the user can have a real time view of 
        //the consultant's availability;
        public ActionResult GetConsultantCalendar()
        {
            ConsultantModelList conList = new ConsultantModelList();

            //For purposes of this example, assume that the FETCHCONSULTANTCALENDARS is a database call
            IEnumerable<ConsultantCalendarModel> cons = Repository.FetchConsultantCalendars();
            conList.ConsultantsList = new SelectList(cons, "id", "consultantName");
            conList.consultantCalendars = cons.ToList();

            return View(conList);
        }

        //TODO: Change this method to ensure that members do not have to wait endlessly. 
        public ActionResult ConfirmAppointment()
        {
            BookingModel model = new BookingModel();

            AppointmentDetails appointment = new AppointmentDetails();
            appointment.appointmentDate = DateTime.Now.Date;
            appointment.appointmentTime = DateTime.Now.ToLocalTime();
            appointment.appointmentId = Guid.NewGuid();

            model.appointment = appointment;
            ConsultantDetails consultant = new ConsultantDetails();
            consultant.consultantId = Convert.ToInt32(Request.Form["selectedConsultantId"]);
            model.consultant = consultant;

            //Code to create appointment in database

            //This needs to be reassessed. Before confirming the appointment, should we check if the consultant calendar is still available?
            var result = Repository.CreateAppointment(model);

            return View();
        }
    }
}
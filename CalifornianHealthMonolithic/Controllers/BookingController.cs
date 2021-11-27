using CalifornianHealthMonolithic.Code;
using CalifornianHealthMonolithic.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CalifornianHealthMonolithic.Controllers
{
    public class BookingController : Controller
    {
        // GET: Booking
        //TODO: Change this method to display the consultant calendar. Ensure that the user can have a real time view of 
        //the consultant's availability;
        public async Task<ActionResult> GetConsultantCalendar()
        {
            ConsultantModelList conList = new ConsultantModelList();
            var consultantsServiceUrl = ConfigurationManager.AppSettings.Get("ConsultantsServiceUrl");
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri(consultantsServiceUrl)
            };


            var response = await httpClient.GetAsync("/consultants");

            var responseContent = await response.Content.ReadAsStringAsync();
            var consultants = JsonConvert.DeserializeObject<List<Consultant>>(responseContent);


            //CHDBContext dbContext = new CHDBContext();
            //Repository repo = new Repository();

            //List<Consultant> cons = new List<Consultant>();
            //cons = repo.FetchConsultants(dbContext);

            conList.ConsultantsList = new SelectList(consultants, "Id", nameof(Consultant.FirstName));
            conList.consultants = consultants;

            return View(conList);
        }

        //TODO: Change this method to ensure that members do not have to wait endlessly. 
        public async Task<ActionResult> ConfirmAppointment(Appointment model)
        {
            //CHDBContext dbContext = new CHDBContext();

            ////Code to create appointment in database
            ////This needs to be reassessed. Before confirming the appointment, should we check if the consultant calendar is still available?
            //Repository repo = new Repository();
            //var result = repo.CreateAppointment(model, dbContext);
            var calenderServiceUrl = ConfigurationManager.AppSettings.Get("CalenderServiceUrl");
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri(calenderServiceUrl)
            };


            var response = await httpClient.PostAsync("/appointments", new StringContent(JObject.FromObject(model).ToString()));
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseJson = JObject.Parse(responseContent);

                ModelState.AddModelError("", responseJson.Value<string>("detail"));
            }
            return View();
        }
    }
}
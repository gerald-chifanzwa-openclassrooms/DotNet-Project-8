using CalifornianHealthMonolithic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CalifornianHealthMonolithic.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            ConsultantModelList conList = new ConsultantModelList();
            //CHDBContext dbContext = new CHDBContext();
            //Repository repo = new Repository();
            //List<Consultant> cons = new List<Consultant>();

            var consultantsServiceUrl = ConfigurationManager.AppSettings.Get("ConsultantsServiceUrl");
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri(consultantsServiceUrl)
            };


            var response = await httpClient.GetAsync("/consultants");
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ViewBag.Error = "failed to load consultants";
                return View();
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var consultants = JsonConvert.DeserializeObject<List<Consultant>>(responseContent);


            // var cons = repo.FetchConsultants(dbContext);
            conList.ConsultantsList = new SelectList(consultants, "Id", nameof(Consultant.FirstName));
            conList.consultants = consultants;
            return View(conList);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
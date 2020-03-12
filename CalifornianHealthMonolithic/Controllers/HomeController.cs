using CalifornianHealthMonolithic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalifornianHealthMonolithic.Controllers
{
    
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ConsultantModelList conList = new ConsultantModelList();
            List<ConsultantModel> consultantList = new List<ConsultantModel>();

            consultantList.Add(new ConsultantModel { id = 1, fname = "Jessica", lname = "Wally", speciality = "Cardiologist" });
            consultantList.Add(new ConsultantModel { id = 1, fname = "Iai", lname = "Donnas", speciality = "General Surgeon" });
            consultantList.Add(new ConsultantModel { id = 1, fname = "Amanda", lname = "Denyl", speciality = "Doctor" });
            consultantList.Add(new ConsultantModel { id = 1, fname = "Jason", lname = "Davis", speciality = "Cardiologist" });

            conList.selectedConsultantId = 0;
            conList.consultants = consultantList;

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
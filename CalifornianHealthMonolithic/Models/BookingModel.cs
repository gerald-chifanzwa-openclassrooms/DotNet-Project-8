using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalifornianHealthMonolithic.Models
{
    public class BookingModel
    {
        public PatientDetails patient { get; set; }
        public ConsultantDetails consultant { get; set; }
        public FacilityDetails facility { get; set; }
        public PaymentDetails payment { get; set; }
        public AppointmentDetails appointment { get; set; }
    }

    public class AppointmentDetails
    {
        public Guid appointmentId { get; set; }
        public DateTime appointmentDate { get; set; }
        public DateTime appointmentTime { get; set; }
    }

    public class ConsultantDetails
    {
        public int consultantId { get; set; }
        public string consultantName { get; set; }
        public string consultantSpeciality { get; set; }
        public int facilityId { get; set; }
    }

    public class FacilityDetails
    {
        public int facilityId { get; set; }
        public string facilityName { get; set; }
        public string facilityAddressLine1 { get; set; }
        public string facilityAddressLine2 { get; set; }
        public string facilityRegion { get; set; }
        public string facilityCity { get; set; }
        public string facilityPostcode { get; set; }
        public string facilityContactNumber { get; set; }
    }

    public class PatientDetails
    {
        public int patientId { get; set; }
        public string patientName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string contactNumber { get; set; }
    }

    public class PaymentDetails
    {
        public int paymentId { get; set; }
        public double payment { get; set; }
    }

    public static class Repository
    {
        public static IEnumerable<ConsultantModel> FetchConsultants()
        {
            return new List<ConsultantModel>()
            {
                new ConsultantModel(){ id = 1, fname = "Jessica", lname = "Wally", speciality = "Cardiologist"  },
                new ConsultantModel(){ id = 2, fname = "Iai", lname = "Donnas", speciality = "General Surgeon"  },
                new ConsultantModel(){  id = 3, fname = "Amanda", lname = "Denyl", speciality = "Doctor"  },
                new ConsultantModel(){  id = 4,fname = "Jason", lname = "Davis", speciality = "Cardiologist" }
            };
        }

        public static IEnumerable<ConsultantCalendarModel> FetchConsultantCalendars()
        {
            //Should the consultant detail and the calendar (available dates) be clubbed together?
            //Is this the reason the calendar is slow to load? Rethink how we can rewrite this?

            ConsultantCalendarModel model = new ConsultantCalendarModel();

            List<DateTime> consultantCalendarDates1 = new List<DateTime>();
            consultantCalendarDates1.Add(new DateTime(2020, 3, 4));
            consultantCalendarDates1.Add(new DateTime(2020, 3, 15));
            consultantCalendarDates1.Add(new DateTime(2020, 3, 22));
            consultantCalendarDates1.Add(new DateTime(2020, 3, 25));
            consultantCalendarDates1.Add(new DateTime(2020, 3, 28));

            List<DateTime> consultantCalendarDates2 = new List<DateTime>();
            consultantCalendarDates2.Add(new DateTime(2020, 3, 14));
            consultantCalendarDates2.Add(new DateTime(2020, 3, 29));
            consultantCalendarDates2.Add(new DateTime(2020, 3, 30));

            List<DateTime> consultantCalendarDates3 = new List<DateTime>();
            consultantCalendarDates3.Add(new DateTime(2020, 3, 1));
            consultantCalendarDates3.Add(new DateTime(2020, 3, 2));
            consultantCalendarDates3.Add(new DateTime(2020, 3, 3));
            consultantCalendarDates3.Add(new DateTime(2020, 3, 9));
            consultantCalendarDates3.Add(new DateTime(2020, 3, 10));

            List<DateTime> consultantCalendarDates4 = new List<DateTime>();
            consultantCalendarDates4.Add(new DateTime(2020, 3, 11));
            consultantCalendarDates4.Add(new DateTime(2020, 3, 21));


            return new List<ConsultantCalendarModel>()
            {
                new ConsultantCalendarModel() {id=1, consultantName="Jessica Wally", availableDates = consultantCalendarDates1},
                new ConsultantCalendarModel() {id=2, consultantName="Iai Donnas", availableDates = consultantCalendarDates2},
                new ConsultantCalendarModel() {id=3, consultantName="Amanda Denyl", availableDates = consultantCalendarDates3},
                new ConsultantCalendarModel() {id=4, consultantName="Jason Davis", availableDates = consultantCalendarDates4},
            };
        }

        public static bool CreateAppointment(BookingModel model)
        {
            //For purposes of illustration, this will create the appointment in the calendar
            //Should we double check here?
            return true;
        }
    }
}
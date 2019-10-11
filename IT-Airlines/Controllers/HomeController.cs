using IT_Airlines.DataContexts;
using IT_Airlines.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IT_Airlines.Controllers
{
    public class HomeController : Controller
    {
        private AirlineDbContext db = new AirlineDbContext();
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> selectListAirports = from s in db.Airports.ToList()
                                                             select new SelectListItem
                                                             {
                                                                 Value = s.Id.ToString(),
                                                                 Text = s.ToString()
                                                             };
            ViewBag.Airports = new SelectList(selectListAirports, "Value", "Text");


            IEnumerable<SelectListItem> selectListAirplanes = from s in db.Airplanes.ToList()
                                                              select new SelectListItem
                                                              {
                                                                  Value = s.Id.ToString(),
                                                                  Text = s.ToString()
                                                              };
            ViewBag.Airplanes = new SelectList(selectListAirplanes, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(BeginReservationsViewModel model)
        {
            Session["model"] = model;
            return RedirectToAction("Create", "Reservations");
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
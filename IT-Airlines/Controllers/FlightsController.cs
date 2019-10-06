using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IT_Airlines.DataContexts;
using IT_Airlines.Models.Entities;
using IT_Airlines.Models.ViewModels;

namespace IT_Airlines.Controllers
{
    public class FlightsController : Controller
    {
        private AirlineDbContext db = new AirlineDbContext();

        // GET: Flights
        public ActionResult Index()
        {
            return View(db.Flights.ToList());
        }

        // GET: Flights/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flight flight = db.Flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            return View(flight);
        }

        // GET: Flights/Create
        public ActionResult Create()
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

        // POST: Flights/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AirportFrom,AirportTo,Airplane,Departure,Landing,BasePrice")] FlightViewModel flightModel)
        {
            if (ModelState.IsValid)
            {
                
                Airplane airplane = db.Airplanes.Find(flightModel.Airplane);
                Flight flight = new Flight()
                {
                    Id = flightModel.Id,
                    AirportFrom = db.Airports.Find(flightModel.AirportFrom),
                    AirportTo = db.Airports.Find(flightModel.AirportTo),
                    Airplane = airplane,
                    Departure = flightModel.Departure,
                    Landing = flightModel.Landing,
                    NumOfSeats = airplane.NumOfSeats,
                    NumOfFreeSeats = airplane.NumOfSeats,
                    BasePrice = flightModel.BasePrice
                };

                db.Flights.Add(flight);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            IEnumerable<SelectListItem> selectListAirports = from s in db.Airports.ToList()
                                                             select new SelectListItem
                                                             {
                                                                 Value = s.Code,
                                                                 Text = s.ToString()
                                                             };
            ViewBag.Airports = new SelectList(selectListAirports, "Value", "Text");


            IEnumerable<SelectListItem> selectListAirplanes = from s in db.Airplanes.ToList()
                                                              select new SelectListItem
                                                              {
                                                                  Value = s.Code,
                                                                  Text = s.ToString()
                                                              };
            ViewBag.Airplanes = new SelectList(selectListAirplanes, "Value", "Text");

            return View(flightModel);
        }

        // GET: Flights/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flight flight = db.Flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            return View(flight);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Departure,Landing,NumOfSeats,NumOfFreeSeats,BasePrice")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flight).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(flight);
        }

        // GET: Flights/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flight flight = db.Flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Flight flight = db.Flights.Find(id);
            db.Flights.Remove(flight);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

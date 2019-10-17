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

            FlightViewModel flightToEdit = new FlightViewModel()
            {
                Id = flight.Id,
                AirportFrom = flight.AirportFrom.Id,
                AirportTo = flight.AirportTo.Id,
                Airplane = flight.Airplane.Id,
                Departure = flight.Departure,
                Landing = flight.Landing,
                BasePrice = flight.BasePrice
            };

            return View(flightToEdit);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AirportFrom,AirportTo,Airplane,Departure,Landing,BasePrice")] FlightViewModel editedFlight)
        {
            if (ModelState.IsValid)
            {
                Airplane airplane = db.Airplanes.Find(editedFlight.Airplane);

                Flight flight = db.Flights.Find(editedFlight.Id);
                flight.Id = editedFlight.Id;
                flight.AirportFrom = db.Airports.Find(editedFlight.AirportFrom);
                flight.AirportTo = db.Airports.Find(editedFlight.AirportTo);
                flight.Airplane = airplane;
                flight.Departure = editedFlight.Departure;
                flight.Landing = editedFlight.Landing;
                flight.NumOfSeats = airplane.NumOfSeats;
                flight.NumOfFreeSeats = airplane.NumOfSeats;
                flight.BasePrice = editedFlight.BasePrice;

                db.Entry(flight).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
            return View(editedFlight);
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
        public ActionResult DeleteConfirmed(int id)
        {
            Flight flight = db.Flights.Find(id);

            var reservations = db.Reservations.Where(r => r.FirstFlight.Id == flight.Id || r.SecondFlight.Id == flight.Id).ToList();

            for (int i = 0; i < reservations.Count; i++)
            {
                db.Reservations.Remove(reservations[i]);
                db.SaveChanges();
            }

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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IT_Airlines.DataContexts;
using IT_Airlines.Models.Entities;
using IT_Airlines.Models.UserRoles;
using IT_Airlines.Models.ViewModels;
using PagedList;
using PagedList.Mvc;

namespace IT_Airlines.Controllers
{
    public class FlightsController : Controller
    {
        private AirlineDbContext db = new AirlineDbContext();

        // GET: Flights
        public ActionResult Index(string searchBy, string search, int? page, string sortBy)
        {
            ViewBag.SortAirportFromParameter = sortBy == "Airport From desc" ? "Airport From desc":"Airport From asc";
            ViewBag.SortAirportToParameter = sortBy == "Airport To desc" ? "Airport To desc":"Airport To asc";
            ViewBag.SortDepartureParameter = sortBy == "Departure desc" ? "Departure desc" : "Departure asc";
            ViewBag.SortLandingParameter = sortBy == "Landing desc" ? "Landing desc" : "Landing asc";

            var flights = db.Flights.AsQueryable();
            if (string.IsNullOrEmpty(search))
            {
                flights = flights.OrderByDescending(d => d.Departure);
                switch (sortBy)
                {
                    case "Airport From desc":
                        flights = flights.OrderByDescending(x => x.AirportFrom.Name);
                        break;
                    case "Airport To desc":
                        flights = flights.OrderByDescending(x => x.AirportTo.Name);
                        break;
                    case "Landing asc":
                        flights = flights.OrderBy(x => x.Departure);
                        break;
                    case "Landing desc":
                        flights = flights.OrderBy(x => x.Departure);
                        break;
                    case "Departure asc":
                        flights = flights.OrderBy(x => x.Departure);
                        break;
                    default:
                        flights = flights.OrderByDescending(x => x.Departure);
                        break;
                }
            }
            else if (searchBy == "Name")
            {
                flights = flights.Where(x => x.AirportFrom.Name.ToLower().StartsWith(search.ToLower())).OrderByDescending(d => d.Departure);
                switch (sortBy)
                {
                    case "Airport From desc":
                        flights = flights.OrderByDescending(x => x.AirportFrom.Name);
                        break;
                    case "Airport To desc":
                        flights = flights.OrderByDescending(x => x.AirportTo.Name);
                        break;
                    case "Landing asc":
                        flights = flights.OrderBy(x => x.Departure);
                        break;
                    case "Landing desc":
                        flights = flights.OrderBy(x => x.Departure);
                        break;
                    case "Departure asc":
                        flights = flights.OrderBy(x => x.Departure);
                        break;
                    default:
                        flights = flights.OrderByDescending(x => x.Departure);
                        break;
                }
            }
            else
            {
                flights = flights.Where(x => x.AirportFrom.City.ToLower().StartsWith(search.ToLower())).OrderByDescending(d => d.Departure);
                switch (sortBy)
                {
                    case "Airport From desc":
                        flights = flights.OrderByDescending(x => x.AirportFrom.City);
                        break;
                    case "Airport To desc":
                        flights = flights.OrderByDescending(x => x.AirportTo.City);
                        break;
                    case "Landing asc":
                        flights = flights.OrderBy(x => x.Departure);
                        break;
                    case "Landing desc":
                        flights = flights.OrderBy(x => x.Departure);
                        break;
                    case "Departure asc":
                        flights = flights.OrderBy(x => x.Departure);
                        break;
                    default:
                        flights = flights.OrderByDescending(x => x.Departure);
                        break;
                }
            }

            return View(flights.ToPagedList(page ?? 1, 5));

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
        [Authorize(Roles = Roles.Administrator + ", " + Roles.Moderator)]
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
        [Authorize(Roles = Roles.Administrator + ", " + Roles.Moderator)]
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
                    Departure = DateTime.Parse(flightModel.Departure, CultureInfo.CreateSpecificCulture("en-US")),
                    Landing = DateTime.Parse(flightModel.Landing, CultureInfo.CreateSpecificCulture("en-US")),
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
        [Authorize(Roles = Roles.Administrator + ", " + Roles.Moderator)]
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
                Departure = flight.Departure.ToString("MM/dd/yyyy HH:mm"),
                Landing = flight.Landing.ToString("MM/dd/yyyy HH:mm"),
                BasePrice = flight.BasePrice
            };

            return View(flightToEdit);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Administrator + ", " + Roles.Moderator)]
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
                flight.Departure = DateTime.Parse(editedFlight.Departure, CultureInfo.CreateSpecificCulture("en-US"));
                flight.Landing = DateTime.Parse(editedFlight.Landing, CultureInfo.CreateSpecificCulture("en-US"));
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
        [Authorize(Roles = Roles.Administrator + ", " + Roles.Moderator)]
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
        [Authorize(Roles = Roles.Administrator + ", " + Roles.Moderator)]
        public ActionResult DeleteConfirmed(int id)
        {
            Flight flight = db.Flights.Find(id);

            if (flight == null)
            {
                Console.WriteLine("Flight with id={0} does not exist", id);
                return RedirectToAction("Index");
            }
            var reservations = db.Reservations.Where(r => r.FirstFlight.Id == flight.Id || (r.SecondFlight != null && r.SecondFlight.Id == flight.Id)).ToList();

            for (int i = 0; i < reservations.Count; i++)
            {
                db.Reservations.Remove(reservations[i]);
                db.SaveChanges();
            }

            db.Flights.Remove(flight);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult getPrice(int id)
        {
            Flight flight = db.Flights.Find(id);
            float price = flight != null ? flight.BasePrice : 0;
            return Json(price.ToString(), JsonRequestBehavior.AllowGet);
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

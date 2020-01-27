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
using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;

namespace IT_Airlines.Controllers
{

    [Authorize] 
    public class ReservationsController : Controller
    {
        private AirlineDbContext db = new AirlineDbContext();

        // GET: Reservations
        [Authorize(Roles= Roles.Administrator + "," + Roles.Moderator )]
        public ActionResult Index(int? page)
        {
            ViewBag.AllReservations = true;

            IPagedList<Reservation> reservations = db.Reservations.Include(r => r.Passenger).ToList().ToPagedList(page ?? 1, 5);

            return View(reservations);
        }

        public ActionResult MyReservations(int? page)
        {
            ViewBag.AllReservations = false;
            string email = User.Identity.GetUserName();

            IPagedList<Reservation> reservations = db.Reservations
                            .Include(r => r.Passenger)
                            .Where(x => x.AccountEmail.Equals(email))
                            .ToPagedList(page ?? 1, 5);

            return View("Index", reservations);
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Include(r => r.Passenger).SingleOrDefault(x => x.Id == id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        public ActionResult Create()
        {
            BeginReservationsViewModel model = (BeginReservationsViewModel) Session["model"];
            if(model == null)
            {
                return RedirectToAction("Index", "Home");
            }
            CreateReservationsViewModel newModel = new CreateReservationsViewModel();
            //newModel.PartialModel = model;
            newModel.RoundTrip = model.RoundTrip;
            //newModel.NumPassengers = model.Passengers;
            newModel.Reservations = new List<Reservation>(model.Passengers);
            newModel.Luggages = new List<int>(model.Passengers);
            newModel.ReturnLuggages = new List<int>(model.Passengers);

            for(int i=0;i<model.Passengers;++i)
            {
                Reservation res = new Reservation();
                res.RoundTrip = model.RoundTrip;
                newModel.Reservations.Add(res);

                newModel.Luggages.Add(0);
                newModel.ReturnLuggages.Add(0);
            }

            DateTime departure = DateTime.Parse(model.Departure, CultureInfo.CreateSpecificCulture("en-US"));
            DateTime returnDate = new DateTime(0);
            if (model.RoundTrip)
            {
                DateTime.Parse(model.Return, CultureInfo.CreateSpecificCulture("en-US"));
            }

            IEnumerable<Flight> flights = (from f in db.Flights.Where(x => x.AirportFrom.Id.Equals(model.AirportFrom))
                                    .Where(x => x.AirportTo.Id.Equals(model.AirportTo))
                                    .Where(x => DbFunctions.TruncateTime(x.Departure) == departure)
                                    .Where(x => x.NumOfFreeSeats >= model.Passengers)
                                    .ToList()
                                    select f);
            ViewBag.f = flights;

            IEnumerable<Flight> rflights = (from f in db.Flights.Where(x => x.AirportFrom.Id.Equals(model.AirportTo))
                                        .Where(x => x.AirportTo.Id.Equals(model.AirportFrom))
                                        .Where(x => DbFunctions.TruncateTime(x.Departure) == returnDate)
                                        .Where(x => x.NumOfFreeSeats >= model.Passengers)
                                        .ToList()
                                           select f);
            ViewBag.rf = rflights;

            IEnumerable<SelectListItem> selectListFlights = 
                from s in db.Flights.Where(x => x.AirportFrom.Id.Equals(model.AirportFrom))
                                    .Where(x => x.AirportTo.Id.Equals(model.AirportTo))
                                    .Where(x => DbFunctions.TruncateTime(x.Departure) == departure)
                                    .Where(x => x.NumOfFreeSeats >= model.Passengers)
                                    .ToList()
                                    select new SelectListItem
                                        {
                                            Value = s.Id.ToString(),
                                            Text = s.ToString()
                                        };
            ViewBag.Flights = new SelectList(selectListFlights, "Value", "Text");
            ViewBag.ReturnFlights = new SelectList(new List<string>());
            if (model.RoundTrip)
            { 
                IEnumerable<SelectListItem> selectListReturnFlights =
                    from s in db.Flights.Where(x => x.AirportFrom.Id.Equals(model.AirportTo))
                                        .Where(x => x.AirportTo.Id.Equals(model.AirportFrom))
                                        .Where(x => DbFunctions.TruncateTime(x.Departure) == returnDate)
                                        .Where(x => x.NumOfFreeSeats >= model.Passengers)
                                        .ToList()
                                        select new SelectListItem
                                        {
                                            Value = s.Id.ToString(),
                                            Text = s.ToString()
                                        };

                ViewBag.ReturnFlights = new SelectList(selectListReturnFlights, "Value", "Text");
            }

            IEnumerable<SelectListItem> selectListLuggages = from s in db.Luggages.ToList()
                                                             select new SelectListItem
                                                             {
                                                                 Value = s.Id.ToString(),
                                                                 Text = s.ToString()
                                                             };
            ViewBag.Luggages = new SelectList(selectListLuggages, "Value", "Text");

            return View(newModel);
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateReservationsViewModel model)
        {
            if (ModelState.IsValid)
            {
                Flight flight = db.Flights.Find(model.Flight);
                Flight returnFlight = null;
                if (model.RoundTrip)
                {
                     returnFlight = db.Flights.Find(model.ReturnFlight);
                }
                for(int i=0; i<model.Reservations.Count; ++i)
                {
                    Reservation reservation = model.Reservations.ElementAt(i);
                    reservation.RoundTrip = model.RoundTrip;
                    reservation.FirstFlight = flight;
                    reservation.SecondFlight = returnFlight;
                    reservation.AccountEmail = User.Identity.GetUserName();
                    reservation.FirstLuggage = db.Luggages.Find(model.Luggages.ElementAt(i));
                    flight.NumOfFreeSeats--;
                    if(model.RoundTrip)
                    {
                        reservation.SecondLuggage = db.Luggages.Find(model.ReturnLuggages.ElementAt(i));
                        returnFlight.NumOfFreeSeats--;
                        db.Entry(returnFlight).State = EntityState.Modified;
                    }
                    db.Entry(flight).State = EntityState.Modified;
                    db.Reservations.Add(reservation);
                    db.SaveChanges();
                }
                return RedirectToAction("MyReservations");
            }

            Session.Remove("model");
            return View(model);
        }

        // GET: Reservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Include(r => r.Passenger).SingleOrDefault(x => x.Id == id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            EditReservationViewModel editModel = new EditReservationViewModel()
            {
                Id = reservation.Id,
                RoundTrip = reservation.RoundTrip,
                FirstLuggage = reservation.FirstLuggage.Id,
                SecondLuggage = reservation.RoundTrip ? reservation.SecondLuggage.Id : -1,
                Passenger = reservation.Passenger
            };

            IEnumerable<SelectListItem> selectListLuggages = from s in db.Luggages.ToList()
                                                             select new SelectListItem
                                                             {
                                                                 Value = s.Id.ToString(),
                                                                 Text = s.ToString()
                                                             };

            ViewBag.Luggages = new SelectList(selectListLuggages, "Value", "Text");
            return View(editModel);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RoundTrip,FirstLuggage,SecondLuggage,Passenger")] EditReservationViewModel editReservation)
        {
            if (ModelState.IsValid)
            {
                Reservation reservation = db.Reservations.Find(editReservation.Id);
                reservation.FirstLuggage = db.Luggages.Find(editReservation.FirstLuggage);
                reservation.SecondLuggage = reservation.RoundTrip ? db.Luggages.Find(editReservation.SecondLuggage) : null;
                reservation.Passenger = editReservation.Passenger;
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyReservations");
            }

            IEnumerable<SelectListItem> selectListLuggages = from s in db.Luggages.ToList()
                                                             select new SelectListItem
                                                             {
                                                                 Value = s.Id.ToString(),
                                                                 Text = s.ToString()
                                                             };
            ViewBag.Luggages = new SelectList(selectListLuggages, "Value", "Text");
            return View(editReservation);
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            // da se implementira vrakjanje na slobodni sedista vo let!!
            Reservation reservation = db.Reservations.Find(id);
            reservation.FirstFlight.NumOfFreeSeats++;
            db.Entry(reservation.FirstFlight).State = EntityState.Modified;
            if (reservation.RoundTrip)
            {
                reservation.SecondFlight.NumOfFreeSeats++;
                db.Entry(reservation.SecondFlight).State = EntityState.Modified;
            }
         
            db.Reservations.Remove(reservation);
            db.SaveChanges();
            return RedirectToAction("MyReservations");
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

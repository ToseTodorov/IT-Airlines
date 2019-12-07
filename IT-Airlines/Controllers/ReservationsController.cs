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
using Microsoft.AspNet.Identity;

namespace IT_Airlines.Controllers
{
    public class ReservationsController : Controller
    {
        private AirlineDbContext db = new AirlineDbContext();

        // GET: Reservations
        public ActionResult Index()
        {
            return View(db.Reservations
                            .Include(r => r.Passenger)
                            .ToList());
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
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

        // GET: Reservations/Create
        public ActionResult Create()
        {
            BeginReservationsViewModel model = (BeginReservationsViewModel) Session["model"];
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


            IEnumerable<SelectListItem> selectListFlights = 
                from s in db.Flights.Where(x => x.AirportFrom.Id.Equals(model.AirportFrom))
                                    .Where(x => x.AirportTo.Id.Equals(model.AirportTo))
                                    .Where(x => DbFunctions.TruncateTime(x.Departure)==model.Departure.Date)
                                    .Where(x => x.NumOfFreeSeats >= model.Passengers)
                                    .ToList()
                                    select new SelectListItem
                                        {
                                            Value = s.Id.ToString(),
                                            Text = s.ToString()
                                        };
            ViewBag.Flights = new SelectList(selectListFlights, "Value", "Text");

            if (model.RoundTrip)
            { 
                IEnumerable<SelectListItem> selectListReturnFlights =
                    from s in db.Flights.Where(x => x.AirportFrom.Id.Equals(model.AirportTo))
                                        .Where(x => x.AirportTo.Id.Equals(model.AirportFrom))
                                        .Where(x => DbFunctions.TruncateTime(x.Departure) == model.Return.Date)
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
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Reservations/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountEmail,RoundTrip")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reservation);
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

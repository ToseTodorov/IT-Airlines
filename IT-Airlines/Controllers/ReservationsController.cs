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
    public class ReservationsController : Controller
    {
        private AirlineDbContext db = new AirlineDbContext();

        // GET: Reservations
        public ActionResult Index()
        {
            return View(db.Reservations.ToList());
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
            newModel.PartialReservation = model;
            newModel.Reservations = new List<Reservation>(model.Passengers);

            foreach(Reservation res in newModel.Reservations)
            {
                res.RoundTrip = model.RoundTrip;
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
                if (model.PartialReservation.RoundTrip)
                {
                     returnFlight = db.Flights.Find(model.Flight);
                }
                foreach (Reservation reservation in model.Reservations)
                {
                    reservation.FirstFlight = flight;
                    reservation.SecondFlight = returnFlight;
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
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
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

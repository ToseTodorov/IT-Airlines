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
using IT_Airlines.Models.UserRoles;
using PagedList;
using PagedList.Mvc;

namespace IT_Airlines.Controllers
{
    public class AirplanesController : Controller
    {
        private AirlineDbContext db = new AirlineDbContext();

        // GET: Airplanes
        public ActionResult Index(string search, int? page, string sortBy)
        {
            ViewBag.SortCodeParameter = sortBy == "asc" ? "desc" : "asc";

            var airplanes = db.Airplanes.AsQueryable();

            if (! string.IsNullOrEmpty(search))
            {
                airplanes = airplanes.Where(a => a.Code.ToLower().StartsWith(search.ToLower()));
            }

            switch (sortBy)
            {
                case "asc":
                    airplanes = airplanes.OrderBy(c => c.Code);
                    break;
                default:
                    airplanes = airplanes.OrderByDescending(c => c.Code);
                    break;
            }

            return View(airplanes.ToPagedList(page ?? 1, 5));
        }

        // GET: Airplanes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Airplane airplane = db.Airplanes.Find(id);
            if (airplane == null)
            {
                return HttpNotFound();
            }
            return View(airplane);
        }
        

        // GET: Airplanes/Create
        [Authorize(Roles = Roles.Administrator + ", " + Roles.Moderator)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Airplanes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Administrator + ", " + Roles.Moderator)]
        public ActionResult Create([Bind(Include = "Id,Code,NumOfSeats")] Airplane airplane)
        {
            if (ModelState.IsValid)
            {
                db.Airplanes.Add(airplane);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(airplane);
        }

        // GET: Airplanes/Edit/5
        [Authorize(Roles = Roles.Administrator + ", " + Roles.Moderator)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Airplane airplane = db.Airplanes.Find(id);
            if (airplane == null)
            {
                return HttpNotFound();
            }
            return View(airplane);
        }

        // POST: Airplanes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Administrator + ", " + Roles.Moderator)]
        public ActionResult Edit([Bind(Include = "Id,Code,NumOfSeats")] Airplane airplane)
        {
            if (ModelState.IsValid)
            {
                db.Entry(airplane).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(airplane);
        }

        // GET: Airplanes/Delete/5
        [Authorize(Roles = Roles.Administrator + ", " + Roles.Moderator)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Airplane airplane = db.Airplanes.Find(id);
            if (airplane == null)
            {
                return HttpNotFound();
            }
            return View(airplane);
        }

        // POST: Airplanes/Delete/5
        [Authorize(Roles = Roles.Administrator + ", " + Roles.Moderator)]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Airplane airplane = db.Airplanes.Find(id);

            var flights = db.Flights.Where(f => f.Airplane.Id == id).ToList();
            var flightsId = flights.Select(f => f.Id).ToList();
            var reservations = new List<Reservation>();
            reservations = db.Reservations.Where(r => flightsId.Contains(r.FirstFlight.Id) || flightsId.Contains(r.SecondFlight.Id)).ToList();

            for (int i = 0; i < reservations.Count; i++)
            {
                db.Reservations.Remove(reservations[i]);
                db.SaveChanges();
            }

            for (int i = 0; i < flights.Count; i++)
            {
                db.Flights.Remove(flights[i]);
                db.SaveChanges();
            }

            db.Airplanes.Remove(airplane);
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

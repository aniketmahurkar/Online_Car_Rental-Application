using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarService.Models;

namespace CarService.Controllers
{
    [Authorize (Roles ="Admin")]
    public class FrequenciesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Frequencies
        public ActionResult Index()
        {
            return View(db.Frequencies.ToList());
        }

        // GET: Frequencies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Frequency frequency = db.Frequencies.Find(id);
            if (frequency == null)
            {
                return HttpNotFound();
            }
            return View(frequency);
        }

        // GET: Frequencies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Frequencies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,description")] Frequency frequency)
        {
            if (ModelState.IsValid)
            {
                db.Frequencies.Add(frequency);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(frequency);
        }

        // GET: Frequencies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Frequency frequency = db.Frequencies.Find(id);
            if (frequency == null)
            {
                return HttpNotFound();
            }
            return View(frequency);
        }

        // POST: Frequencies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,description")] Frequency frequency)
        {
            if (ModelState.IsValid)
            {
                db.Entry(frequency).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(frequency);
        }

        // GET: Frequencies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Frequency frequency = db.Frequencies.Find(id);
            if (frequency == null)
            {
                return HttpNotFound();
            }
            return View(frequency);
        }

        // POST: Frequencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Frequency frequency = db.Frequencies.Find(id);
            db.Frequencies.Remove(frequency);
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

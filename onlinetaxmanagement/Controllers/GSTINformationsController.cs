using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using onlinetaxmanagement.Models;

namespace onlinetaxmanagement.Controllers
{
    public class GSTINformationsController : Controller
    {
        private TaxSystemEntities1 db = new TaxSystemEntities1();

        // GET: GSTINformations
        public ActionResult Index()
        {
            var gSTINformations = db.GSTINformations.Include(g => g.Registration);
            return View(gSTINformations.ToList());
        }

        // GET: GSTINformations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GSTINformation gSTINformation = db.GSTINformations.Find(id);
            if (gSTINformation == null)
            {
                return HttpNotFound();
            }
            return View(gSTINformation);
        }

        // GET: GSTINformations/Create
        public ActionResult Create()
        {
            ViewBag.Uid = new SelectList(db.Registrations, "Uid", "FirstName");
            return View();
        }

        // POST: GSTINformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Gid,MRP,Uid,GST,Bill,Flag")] GSTINformation gSTINformation)
        {
            if (ModelState.IsValid)
            {
                db.GSTINformations.Add(gSTINformation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Uid = new SelectList(db.Registrations, "Uid", "FirstName", gSTINformation.Uid);
            return View(gSTINformation);
        }

        // GET: GSTINformations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GSTINformation gSTINformation = db.GSTINformations.Find(id);
            if (gSTINformation == null)
            {
                return HttpNotFound();
            }
            ViewBag.Uid = new SelectList(db.Registrations, "Uid", "FirstName", gSTINformation.Uid);
            return View(gSTINformation);
        }

        // POST: GSTINformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Gid,MRP,Uid,GST,Bill,Flag")] GSTINformation gSTINformation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gSTINformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Uid = new SelectList(db.Registrations, "Uid", "FirstName", gSTINformation.Uid);
            return View(gSTINformation);
        }

        // GET: GSTINformations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GSTINformation gSTINformation = db.GSTINformations.Find(id);
            if (gSTINformation == null)
            {
                return HttpNotFound();
            }
            return View(gSTINformation);
        }

        // POST: GSTINformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GSTINformation gSTINformation = db.GSTINformations.Find(id);
            db.GSTINformations.Remove(gSTINformation);
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

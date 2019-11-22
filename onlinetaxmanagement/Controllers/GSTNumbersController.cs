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
    public class GSTNumbersController : Controller
    {
        private TaxSystemEntities1 db = new TaxSystemEntities1();

        // GET: GSTNumbers
        public ActionResult Index()
        {
            var gSTNumbers = db.GSTNumbers.Include(g => g.Registration);
            return View(gSTNumbers.ToList());
        }

        // GET: GSTNumbers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GSTNumber gSTNumber = db.GSTNumbers.Find(id);
            if (gSTNumber == null)
            {
                return HttpNotFound();
            }
            return View(gSTNumber);
        }

        // GET: GSTNumbers/Create
        public ActionResult Create()
        {
            ViewBag.Uid = new SelectList(db.Registrations, "Uid", "FirstName");
            return View();
        }

        // POST: GSTNumbers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Gnid,Uid,GSTNumber1")] GSTNumber gSTNumber)
        {
            if (ModelState.IsValid)
            {
                db.GSTNumbers.Add(gSTNumber);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Uid = new SelectList(db.Registrations, "Uid", "FirstName", gSTNumber.Uid);
            return View(gSTNumber);
        }

        // GET: GSTNumbers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GSTNumber gSTNumber = db.GSTNumbers.Find(id);
            if (gSTNumber == null)
            {
                return HttpNotFound();
            }
            ViewBag.Uid = new SelectList(db.Registrations, "Uid", "FirstName", gSTNumber.Uid);
            return View(gSTNumber);
        }

        // POST: GSTNumbers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Gnid,Uid,GSTNumber1")] GSTNumber gSTNumber)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gSTNumber).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Uid = new SelectList(db.Registrations, "Uid", "FirstName", gSTNumber.Uid);
            return View(gSTNumber);
        }

        // GET: GSTNumbers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GSTNumber gSTNumber = db.GSTNumbers.Find(id);
            if (gSTNumber == null)
            {
                return HttpNotFound();
            }
            return View(gSTNumber);
        }

        // POST: GSTNumbers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GSTNumber gSTNumber = db.GSTNumbers.Find(id);
            db.GSTNumbers.Remove(gSTNumber);
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
        public ActionResult Apply(GSTNumber gSTNumber)
        {
            var uid = Session["Uid"].ToString();
            gSTNumber.Uid = Convert.ToInt32(uid);
            if (ModelState.IsValid)
            {
                db.GSTNumbers.Add(gSTNumber);
                db.SaveChanges();
            }
            Session["message1"] = "Applied Successful for GstNumber";
            return RedirectToAction("Index","Home");
        }
    }
}

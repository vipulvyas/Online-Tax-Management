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
    public class TaxInformationsController : Controller
    {
        private TaxSystemEntities1 db = new TaxSystemEntities1();

        // GET: TaxInformations
        public ActionResult Index()
        {
           
            var taxInformations = db.TaxInformations.Include(t => t.Registration);
            return View(taxInformations.ToList());
        }

        // GET: TaxInformations/Details/5
        public ActionResult Details(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaxInformation taxInformation = db.TaxInformations.Find(id);
            if (taxInformation == null)
            {
                return HttpNotFound();
            }
            return View(taxInformation);
        }

        // GET: TaxInformations/Create
        public ActionResult Create()
        {
            ViewBag.Uid = new SelectList(db.Registrations, "Uid", "FirstName");
            return View();
        }

        // POST: TaxInformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Tid,Uid,Year,Status,TotalIncome,TotalTax")] TaxInformation taxInformation)
        {
            if (ModelState.IsValid)
            {
                db.TaxInformations.Add(taxInformation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Uid = new SelectList(db.Registrations, "Uid", "FirstName", taxInformation.Uid);
            return View(taxInformation);
        }

        // GET: TaxInformations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaxInformation taxInformation = db.TaxInformations.Find(id);
            if (taxInformation == null)
            {
                return HttpNotFound();
            }
            ViewBag.Uid = new SelectList(db.Registrations, "Uid", "FirstName", taxInformation.Uid);
            return View(taxInformation);
        }

        // POST: TaxInformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Tid,Uid,Year,Status,TotalIncome,TotalTax")] TaxInformation taxInformation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taxInformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Uid = new SelectList(db.Registrations, "Uid", "FirstName", taxInformation.Uid);
            return View(taxInformation);
        }

        // GET: TaxInformations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaxInformation taxInformation = db.TaxInformations.Find(id);
            if (taxInformation == null)
            {
                return HttpNotFound();
            }
            return View(taxInformation);
        }

        // POST: TaxInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TaxInformation taxInformation = db.TaxInformations.Find(id);
            db.TaxInformations.Remove(taxInformation);
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

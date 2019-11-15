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
    public class AdminInformationsController : Controller
    {
        private TaxSystemEntities1 db = new TaxSystemEntities1();

        // GET: AdminInformations
        public ActionResult Index()
        {
            return View(db.AdminInformations.ToList());
        }

        // GET: AdminInformations/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminInformation adminInformation = db.AdminInformations.Find(id);
            if (adminInformation == null)
            {
                return HttpNotFound();
            }
            return View(adminInformation);
        }

        // GET: AdminInformations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminInformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserName,Password,Email")] AdminInformation adminInformation)
        {
            if (ModelState.IsValid)
            {
                db.AdminInformations.Add(adminInformation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adminInformation);
        }

        // GET: AdminInformations/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminInformation adminInformation = db.AdminInformations.Find(id);
            if (adminInformation == null)
            {
                return HttpNotFound();
            }
            return View(adminInformation);
        }

        // POST: AdminInformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserName,Password,Email")] AdminInformation adminInformation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adminInformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(adminInformation);
        }

        // GET: AdminInformations/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminInformation adminInformation = db.AdminInformations.Find(id);
            if (adminInformation == null)
            {
                return HttpNotFound();
            }
            return View(adminInformation);
        }

        // POST: AdminInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AdminInformation adminInformation = db.AdminInformations.Find(id);
            db.AdminInformations.Remove(adminInformation);
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
            if (Session["PanNumber"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase billfile, [ Bind(Include = "MRP,GST,Flag")] GSTINformation gSTINformation)
        {
            try
            {
                if (billfile.ContentLength > 0)
                {
                    
                    string _FileName = Path.GetFileName(billfile.FileName);
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    billfile.SaveAs(_path);
                    gSTINformation.Bill = _FileName;
                    gSTINformation.Uid = Convert.ToInt32(Session["Uid"]);
                    if (ModelState.IsValid)
                    {
                        db.GSTINformations.Add(gSTINformation);
                        db.SaveChanges();
                    }
                }
                ViewBag.Message = "File Uploaded Successfully!!";
                return View("Gstrate");
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View("Gstrate");
            }
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
        public ActionResult Create([Bind(Include = "Gid,MRP,Uid,GST,Bill")] GSTINformation gSTINformation)
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

        public ActionResult EnterGST()
        {
            if (Session["PanNumber"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public ActionResult EnterGST(GSTNumber gstnumber)
        {

            if (Session["PanNumber"] != null)
            {
                using (TaxSystemEntities1 db = new TaxSystemEntities1())
                {
                    var x = Request["GSTNumber"].ToString();
                    var y = Convert.ToInt32(Session["Uid"]);
                    var data = db.GSTNumbers.Where(m => m.Uid.Equals(y)).FirstOrDefault();
                    if (data.GSTNumber1 == x)
                    {
                        return RedirectToAction("Gstrate", "GSTINformations");
                    }
                    ViewBag.Message = "Invalid GST Number";
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
           
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

        public ActionResult Gstrate()
        {
            if (Session["PanNumber"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}

using onlinetaxmanagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace onlinetaxmanagement.Controllers
{
    public class HomeController : Controller
    {
        private TaxSystemEntities1 db = new TaxSystemEntities1();
        public ActionResult Index()
        {
            if(Session["Uid"] != null || Session["Admin"] == null)
            {
                int uid = Convert.ToInt32(Session["Uid"]);
            var data = db.GSTNumbers.Where(m => m.Uid.Equals(uid)).FirstOrDefault();
            if(data != null)
            {
                ViewBag.temp = "1";
            }
            }
            return View();
        }

       

        public ActionResult Security()
        {

            return View();
        }
    }
}
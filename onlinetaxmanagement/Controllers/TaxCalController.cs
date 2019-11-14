using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaxApp.Controllers
{
    public class TaxCalController : Controller
    {
        // GET: TaxCal
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ComputeTax()
        {
            int year = Convert.ToInt32(Request["year"].ToString());
            int taxpayer = Convert.ToInt32(Request["taxpayer"].ToString());
            int status = Convert.ToInt32(Request["status"].ToString());
            int taxableincome = Convert.ToInt32(Request["taxableincome"].ToString());
            int arincometax = 0, surcharge = 0, educationcess = 0, totaltax = 0;
            if (taxpayer <= 1)
            {
                if (status <= 1)
                {
                    if (taxableincome <= 250000)
                    {
                        arincometax = 0;
                        surcharge = 0;
                        educationcess = 0;
                        totaltax = 0;
                    }
                    else if (taxableincome <= 500000)
                    {
                        arincometax = (taxableincome - 250000) * 5 / 100;
                        if (taxableincome <= 350000)
                        {
                            if (arincometax <= 2500)
                            {
                                arincometax = 0;
                            }
                            else
                            {
                                arincometax = arincometax - 2500;
                            }
                        }
                        surcharge = 0;

                    }
                    else if (taxableincome <= 1000000)
                    {
                        arincometax = 12500 + (taxableincome - 500000) * 20 / 100;
                        surcharge = 0;
                    }
                    else
                    {
                        arincometax = 112500 + (taxableincome - 1000000) * 30 / 100;
                        if (taxableincome <= 10000000 && taxableincome > 5000000)
                        {
                            surcharge = arincometax / 10;
                        }
                        else
                        {
                            surcharge = arincometax * 15 / 100;
                        }
                    }
                    educationcess = arincometax * 4 / 100;
                    totaltax = arincometax + surcharge + educationcess;
                }
                else if (status == 2)
                {
                    if (taxableincome <= 300000)
                    {
                        arincometax = 0;
                        surcharge = 0;
                        educationcess = 0;
                        totaltax = 0;
                    }
                    else if (taxableincome <= 500000)
                    {
                        arincometax = (taxableincome - 300000) * 5 / 100;
                        if (taxableincome <= 350000)
                        {
                            if (arincometax <= 2500)
                            {
                                arincometax = 0;
                            }
                            else
                            {
                                arincometax = arincometax - 2500;
                            }
                        }
                        surcharge = 0;

                    }
                    else if (taxableincome <= 1000000)
                    {
                        arincometax = 10000 + (taxableincome - 500000) * 20 / 100;
                        surcharge = 0;
                    }
                    else
                    {
                        arincometax = 110000 + (taxableincome - 1000000) * 30 / 100;
                        if (taxableincome <= 10000000 && taxableincome > 5000000)
                        {
                            surcharge = arincometax / 10;
                        }
                        else
                        {
                            surcharge = arincometax * 15 / 100;
                        }
                    }
                    educationcess = arincometax * 4 / 100;
                    totaltax = arincometax + surcharge + educationcess;
                }
                else
                {
                    if (taxableincome <= 500000)
                    {
                        arincometax = 0;
                        surcharge = 0;
                        educationcess = 0;
                        totaltax = 0;
                    }
                    else if (taxableincome <= 1000000)
                    {
                        arincometax = (taxableincome - 500000) * 20 / 100;
                        surcharge = 0;
                    }
                    else
                    {
                        arincometax = 100000 + (taxableincome - 1000000) * 30 / 100;
                        if (taxableincome <= 10000000 && taxableincome > 5000000)
                        {
                            surcharge = arincometax / 10;
                        }
                        else
                        {
                            surcharge = arincometax * 15 / 100;
                        }
                    }
                    educationcess = arincometax * 4 / 100;
                    totaltax = arincometax + surcharge + educationcess;
                }
            }
            else
            {
                if (taxableincome <= 10000)
                {
                    arincometax = (taxableincome - 0) * 10 / 100;
                    surcharge = 0;
                }
                else if (taxableincome <= 20000)
                {
                    arincometax = (taxableincome - 10000) * 20 / 100;
                    surcharge = 0;
                }
                else
                {
                    arincometax = (taxableincome - 20000) * 30 / 100;
                    surcharge = 0;
                }
                educationcess = arincometax * 4 / 100;
                totaltax = arincometax + surcharge + educationcess;
            }
            ViewBag.arincometax = arincometax;
            ViewBag.taxableincome = taxableincome;
            ViewBag.surcharge = surcharge;
            ViewBag.educationcess = educationcess;
            ViewBag.totaltax = totaltax;
            return View("Index");
        }

    }
}
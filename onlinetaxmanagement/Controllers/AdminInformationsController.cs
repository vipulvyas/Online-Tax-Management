using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using onlinetaxmanagement.Models;
using RegistrationAndLogin.Models;

namespace onlinetaxmanagement.Controllers
{
    public class AdminInformationsController : Controller
    {
        private TaxSystemEntities1 db = new TaxSystemEntities1();

        // GET: AdminInformations
        public ActionResult Index()
        {
            Session["adminuser"] = "1";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(AdminInformation reg)
        {
            if (ModelState.IsValid)
            {
                using (TaxSystemEntities1 db = new TaxSystemEntities1())
                {
                    var obj = db.AdminInformations.Where(a => a.UserName.Equals(reg.UserName) && a.Password.Equals(reg.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["Admin"] = obj.UserName.ToString();
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ViewBag.Message = "Invalid User Name or Password";
            // Response.Write("<script>alert(' Invalid PanNumber or Password')</script>");
            return View(reg);
        }

        public ActionResult Logout()
        {
            if (Session["Admin"] != null)
            {
                Session["Admin"] = null;
            }
            else
            {
                return RedirectToAction("Index", "AdminInformations");

            }
            return RedirectToAction("Index", "AdminInformations");
        }
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/AdminInformations/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("taxbazar001@gmail.com", "TaxBazar");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "@taxbazar.com"; // Replace with actual password

            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created!";
                body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                    " successfully created. Please click on the below link to verify your account" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi,<br/><br/>We got request for reset your account password. Please click on the below link to reset your password" +
                    "<br/><br/><a href=" + link + ">Reset Password link</a>";
            }


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            //Verify Email ID
            //Generate Reset password link 
            //Send Email 
            string message = "";

            using (TaxSystemEntities1 dc = new TaxSystemEntities1())
            {
                var account = dc.AdminInformations.Where(a => a.Email == EmailID).FirstOrDefault();
                if (account != null)
                {
                    //Send email for reset password
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.Email, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;
                    //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                    //in our model class in part 1
                    dc.Configuration.ValidateOnSaveEnabled = false;
                    dc.SaveChanges();
                    message = "Reset password link has been sent to your email id.";
                }
                else
                {
                    message = "Account not found";
                }
            }
            ViewBag.Message = message;
            return View();
        }
        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            using (TaxSystemEntities1 dc = new TaxSystemEntities1())
            {
                var user = dc.AdminInformations.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (TaxSystemEntities1 dc = new TaxSystemEntities1())
                {
                    var user = dc.AdminInformations.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = model.NewPassword;
                        user.ResetPasswordCode = "";
                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                        message = "New password updated successfully";
                    }
                }
            }
            else
            {
                message = "Something invalid";
            }
            ViewBag.Message = message;
            return View(model);
        }
        public ActionResult IncomeTaxInformation() {
            if (Session["Admin"] != null)
            {
                var taxInformations = db.TaxInformations.Include(t => t.Registration);
                return View(taxInformations.ToList());
            }
            else
            {
                return RedirectToAction("Index", "AdminInformations");
            }
           
        }
        public ActionResult Approve()
        {
            if (Session["Admin"] != null)
            {
                var taxInformations = db.GSTNumbers.Include(t => t.Registration).ToList();
                return View(taxInformations);
            }
            else
            {
                return RedirectToAction("Index", "AdminInformations");
            }

        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        [HttpPost]
        public ActionResult Approval()
        {
            if (Session["Admin"] != null)
            {
                var gst = RandomString(15);
                var uid =Convert.ToInt32(Request["item.Uid"]);
                var Email = Request["item.Registration.Email"].ToString();
                var taxInformations = db.GSTNumbers.Where(t => t.Registration.Uid.Equals(uid)).FirstOrDefault();
                taxInformations.GSTNumber1 = gst;
                db.SaveChanges();
                var fromEmail = new MailAddress("taxbazar001@gmail.com", "TaxBazar");
                var toEmail = new MailAddress(Email);
                var fromEmailPassword = "@taxbazar.com";
                string subject = "About GST Number";
                string body = "Your GST Number Request is Approved and Your GST Number is "+gst;
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
                };
                using (var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                    smtp.Send(message);

                return RedirectToAction("Approve", "AdminInformations");
            }
            else
            {
                return RedirectToAction("Index", "AdminInformations");
            }
        }

        public ActionResult Search()
        {
            if (Session["Admin"] != null)
            {

                using (TaxSystemEntities1 db = new TaxSystemEntities1())
                {
                    return View(db.Registrations.ToList());
                }
            }
            else
            {
                return RedirectToAction("Index", "AdminInformations");
            }

        }
        [HttpPost]
        public ActionResult DGSTbill(GSTINformation gst)
        {
            if (Session["Admin"] != null)
            {

                using (TaxSystemEntities1 db = new TaxSystemEntities1())
                {
                    var x =Convert.ToInt32(Request["item.Uid"]);
                    ViewBag.email =Request["item.Email"].ToString();
                    Session["Email"] = Request["item.Email"].ToString();
                    var data = db.GSTINformations.Where(m => m.Registration.Uid.Equals(x)).ToList();
                    if (data == null)
                        return View();
                    return View(data);
                }
            }
            else
            {
                return RedirectToAction("Index", "AdminInformations");
            }

        }
        public ActionResult Sendrefund()
        {
            if (Session["Admin"] != null)
            {
                var Email = Session["Email"].ToString();
                var total = Session["Total"].ToString();
                var fromEmail = new MailAddress("taxbazar001@gmail.com", "TaxBazar");
                var toEmail = new MailAddress(Email);
                var fromEmailPassword = "@taxbazar.com";
                string subject = "Refund Information";
                string body = "Your Refund is credited in your Bank Account.Amount is "+ total ;
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
                };
                using (var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                    smtp.Send(message);
            }
            else
            {
                return RedirectToAction("Index", "AdminInformations");
            }

            using (TaxSystemEntities1 db = new TaxSystemEntities1())
            {
                var x = Session["Email"].ToString();
                var data = db.Registrations.Where(m => m.Email.Equals(x)).SingleOrDefault();
                var data1 = db.GSTINformations.Where(q => q.Uid.Equals(data.Uid)).ToList();
                foreach (var model in data1)
                {
                    model.Flag = 0;
                }
                db.SaveChanges();
            }


            Session["Email"] = null;
            Session["Total"] = null;
            ViewBag.message = "SuccessFul";
            return RedirectToAction("Search", "AdminInformations");
        }
    }
}

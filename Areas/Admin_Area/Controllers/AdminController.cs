using Online_Exam_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Exam_Portal.Areas.Admin_Area.Controllers
{
    public class AdminController : Controller
    {
        Online_Exam_PortalEntities db;

        public AdminController()
        {
            db = new Online_Exam_PortalEntities();
        }

        // GET: Admin_Area/Admin
        public ActionResult Dashboard()
        {
            // Retrieve session data
            string username = Session["username"] as string;
            string email = Session["Email"] as string;

            // Check if session data is valid
            if (username != null && email != null)
            {
                // Pass session data to the view
                ViewBag.Username = username;
                ViewBag.Email = email;

                // Render the Dashboard view
                return View();
            }
            else
            {
                // If session data is not valid, redirect to the Login page
                return RedirectToAction("Login");
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin log)
        {
            var admin = db.Admins.FirstOrDefault(e => e.username == log.username && e.password == log.password);
            if (admin != null)
            {
                // Set session variables
                Session["username"] = admin.username;
                Session["Email"] = admin.email_address;

                // Redirect to Dashboard action
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.error = "Invalid Login";
                return View();
            }
        }

        public ActionResult Cource()
        {
            return View();
        }
    }
}

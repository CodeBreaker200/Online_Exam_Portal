using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Generator;
using Online_Exam_Portal.Models;
namespace Online_Exam_Portal.Areas.Users.Controllers
{
    public class UserController : Controller
    {
        // GET: Users/User
        Online_Exam_PortalEntities db;
        public UserController()
        {
            db = new Online_Exam_PortalEntities();
        }

        public ActionResult Index()
        {
            // Retrieve session data
            string Full_name = Session["fullname"] as string;
            
            // Check if session data is valid
            if (Full_name != null)
            {
                // Pass session data to the view
                ViewBag.Username = Full_name;
                

                // Render the Dashboard view
                return View();
            }
            else
            {
                // If session data is not valid, redirect to the Login page
                return RedirectToAction("Login");
            }

        }


        public ActionResult Exam()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Student e)
        {
            db.Students.Add(e);
            db.SaveChanges();
            ViewBag.msg = "Registration Successfull";
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Student log)
        {
            var user = db.Students.FirstOrDefault(x => x.username == log.username && x.password == log.password);


            if (user != null)
            {

                Session["UserEmail"] = user.email_address;
                Session["fullname"] = user.full_name;
                Session["student_id"] = user.student_id;

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.msg = "Invalid Login";
                return View();
            }
        }

        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }



        //Send Email
        public static void Send_Gmail_Email(string to, string subject, string full_name, string otp)
        {
            var fromAddress = new MailAddress("rohitkohakade200@gmail.com", "Rohit");
            var toAddress = new MailAddress(to, to);

            string body = $@"
            <p>Dear: {full_name},</p>
            <p>Your Exam Code: {otp}</p>
            <p>Regards,</p>
            <p>Rohit</p>";

            MailMessage message = new MailMessage(fromAddress, toAddress);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Timeout = 2000000,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("rohitkohakade200@gmail.com", "cnml inzi yyyv dwdw\r\n")
            };

            try
            {
                smtp.Send(message);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        //Generated Exam Code
        public string GeneratePassword(int size)
        {
            string msg = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string Password = "";
            Random r = new Random();
            for (int i = 1; i <= size; i++)
            {
                int P = r.Next(0, msg.Length - 1);
                Password += msg[P];
            }
            return Password;
        }

        public ActionResult Instruction()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendOTP(Student ot)
        {
            var pass = GeneratePassword(6);
            var userEmail = Session["UserEmail"] as string;


            Session["otp"] = pass;

            Send_Gmail_Email(userEmail, "OTP Verification", ot.full_name, pass);
            return View("Instruction");
        }

        [HttpPost]
        public ActionResult VerifyOTP(Student ot)
        {
            var passotp = Session["otp"] as string;

            if (ot.Exam_Code == passotp)
            {
                return RedirectToAction("Start_Exam");
            }
            else
            {
                ViewBag.ms = "Invalid Exam Code";
                return View("Instruction");
            }
        }


        public ActionResult Start_Exam()
        {

            var randomQuestions = db.ExamQuestions.OrderBy(q => Guid.NewGuid()).Take(5).ToList();
            return View(randomQuestions);

        }

        [HttpPost]
        public ActionResult Start_Exam(FormCollection form)
        {
            if (Session["student_id"] != null)
            {
                int studentId = (int)Session["student_id"];

                // Process each submitted option
                //foreach (var key in form.AllKeys)
                //{
                //    if (key.StartsWith("option_"))
                //    {
                //        int questionId = Convert.ToInt32(key.Replace("option_", ""));
                //        int submittedOptionNumber = Convert.ToInt32(form[key]);

                //        // Save the submission to the database
                //        SaveSubmissionToDatabase(studentId, questionId, submittedOptionNumber);
                //    }
                //}



                // Redirect to a success page or display a success message
                return RedirectToAction("Dashboard");
            }
            else
            {
                // Handle case when student ID is not found in session
                // You can redirect to a login page or display an error message
                return RedirectToAction("Login");
            }
        }

        private void SaveSubmissionToDatabase(int studentId, int questionId, int submittedOptionNumber)
        {
            // Create a new ExamSubmission object
            ExamSubmission submission = new ExamSubmission
            {
                student_id = studentId,
                question_id = questionId,
                submitted_option_number = submittedOptionNumber
            };

            // Save the submission to the database
            db.ExamSubmissions.Add(submission);
            db.SaveChanges();
        }





        public ActionResult Exam_Submited(int examid)
        {
            // Retrieve data related to the exam with the given examId
            // This could include fetching user responses, calculating scores, etc.

            // Example: Get exam details from the database
            var examDetails = db.ExamSubmissions.FirstOrDefault(e => e.exam_id == examid);

            // Pass the exam details to the view or perform other actions
            return View(examDetails);
        }

        public ActionResult Dashboard()
        {
            if (Session["UserEmail"] != null)
            {
                string useremail = Session["UserEmail"] as string;

                Student loggedemployee = db.Students.FirstOrDefault(x => x.email_address == useremail);

                return View(loggedemployee);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Edit(int id)
        {
            // Retrieve the student record by ID
            var student = db.Students.Find(id);

            if (student == null)
            {
                // If the student is not found, return a 404 Not Found error
                return HttpNotFound();
            }

            // Pass the student object to the Edit view for editing
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Student s)
        {
            if (ModelState.IsValid)
            {
               
                db.Entry(s).State = EntityState.Modified;

                // Save changes
                db.SaveChanges();

                return RedirectToAction("Dashboard");
            }

            return View(s);
        }


    }
}
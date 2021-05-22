using StoreFrontLab.Data.EF;
using StoreFrontLab.UI.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Linq;

namespace StoreFrontLab.UI.Controllers
{
    

    public class HomeController : Controller
    {
        StoreFrontEntities db = new StoreFrontEntities();

        //[HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //[Authorize]
        public ActionResult About()
        {
            

            return View();
        }

        //[HttpGet]
        public ActionResult Contact()
        {
            

            return View();
        }
        public ActionResult Products()
        {

            return View();
        }

        public ActionResult GetTreats()
        {
            var getTreats = db.Products.Where(x => x.ProductType.ProductTypeName.ToLower() == "treats").ToList();
            return View(getTreats);
        }

        public ActionResult GetToys()
        {
            var getToys = db.Products.Where(x => x.ProductType.ProductTypeName.ToLower() == "toys").ToList();
            return View(getToys);
        }

        public ActionResult GetAcc()
        {
            var getAcc = db.Products.Where(x => x.ProductType.ProductTypeName.ToLower() == "accessories").ToList();
            return View(getAcc);
        }

        [HttpPost]
        public ActionResult Contact(ContactViewModel cvm)
        {
            /*
             * For sending email, you typically need at leasst two namespaces available:
             * System.Net - Access to credential for SMTP server
             * System.Net.Mail - Access to Mail Message class and its methods.
             * 
             * Information required to send email:
             * SMTP server name - mail.yourDomain.ext
             * Email userName - created at SmarterASP
             * Email Password - by default, this is the same as your account, but you should change it.
             * Optional port numbers - only need to be defined if your attempt to send mail fails due to blacklisting
             */

            //First, check to see that the incoming ContactViewModel object meets validation requirements:
            if (!ModelState.IsValid)
            {
                //If the incoming object does not meet validation return the create view populated with the data the user just entered
                return View(cvm);
            }

            string message = $"You have recieved an email from {cvm.Name} with a subject of " +
                $"{cvm.Subject}. Respond to {cvm.EmailAddress}. Message: <br/>{cvm.Message}";

            MailMessage mm = new MailMessage("admin@tpgcode.com", "titleist315@outlook.com", cvm.Subject, message);

            //IsBodyHtml determines if the rendered message should be in HTML
            mm.IsBodyHtml = true;

            //Optional MailPriority that determines priority level
            //mm.Priority = MailPriority.High;

            //ReplyToList() updates the reply goto in the email to go to the end user instead of you admin account
            mm.ReplyToList.Add(cvm.EmailAddress);

            //Send the email:
            SmtpClient client = new SmtpClient("mail.tpgcode.com");

            client.Credentials = new NetworkCredential("admin@tpgcode.com", "P@ssw0rd");

            //Attempt to send the email but handle if it cant be sent.
            try
            {
                client.Send(mm);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Your message could not be sent at this time. Please try again later." +
                    $"<br/> Error message: <br/>{ex.Message}.";
                return View(cvm);
            }

            //Everything went well lets sendthe user to a confirmation view
            return View("EmailConfirmation", cvm);
        }

    }
}

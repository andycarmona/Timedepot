using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace TimelyDepotMVC.Controllers
{
    //[Authorize(Roles="Owner")]
    //[Authorize]
    public class HomeController : Controller
    {
        //
        // GET: /Home/Quit
        public ActionResult Quit()
        {
            return View(); 
        }

        //
        // GET: /Home/Development
        public ActionResult Development()
        {
            return View();
        }

        public ActionResult Index()
        {
            if (User.IsInRole("Owner"))
            {
                return View();
            }
            if (User.IsInRole("Admin"))
            {
                return View();
            }

            return RedirectToAction("LogOn", "Account");
            //return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /////////////////////////////////////////////////////////////////////
        // Name: Error
        // Version: 1.1.0
        // Summary: Send to the view the error to display the error
        // Date: dd/mm/yyyy
        // Author: Mario G Vernaza
        // Prerequisites: None
        // Change History:
        // Date of change (dd/mm/yyyy) [MGV] – Description of change
        // GET: /Home/Error
        /////////////////////////////////////////////////////////////////////
        public ActionResult Error(Exception error)
        {
            ViewData["ExceptionMessage"] = error.ToString();

            return View();
        }

    }
}

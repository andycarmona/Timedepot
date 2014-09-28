using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimelyDepotMVC.Controllers.Admin
{
    public class TablesAdminController : Controller
    {
        //
        // GET: /TablesAdmin/

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

    }
}

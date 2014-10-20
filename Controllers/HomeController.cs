using System;
using System.Web.Mvc;

namespace TimelyDepotMVC.Controllers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using TimelyDepotMVC.DAL;
    using TimelyDepotMVC.Models.Admin;
    using TimelyDepotMVC.ModelsView;

    //[Authorize(Roles="Owner")]
    //[Authorize]
    public class HomeController : Controller
    {
        private TimelyDepotContext db = new TimelyDepotContext();
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
            var environmentVariables = (from envParam in this.db.EnvironmentParameters
                                        select
                                            new EnvironmentParamViewModel
                                                {
                                                    ParameterId = envParam.ParameterId,
                                                    KeyParameter = envParam.KeyParameter,
                                                    KeyValue = envParam.KeyValue,
                                                    Description = envParam.Description,
                                                    Active = envParam.Active,
                                                    ServerUrl = envParam.ServerUrl,
                                                    TransactionUri = envParam.TransactionUri
                                                }).ToList();

            return View(environmentVariables);
        }

        public ActionResult Edit(int id)
        {
            EnvironmentParameters aEnvParam = this.db.EnvironmentParameters.SingleOrDefault(x => x.ParameterId == id);

            return View(aEnvParam);
        }

        [HttpPost]
        public ActionResult Edit(EnvironmentParameters aEnvParam)
        {
            var szError = string.Empty;
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Development");
            }

            try
            {
                string szWarning = null;
                PaymentController.DecodeInfo02(aEnvParam.Password, ref szWarning);

                if (!string.IsNullOrEmpty(szWarning))
                {
                    var encryptedPasswd = PaymentController.EncriptInfo02(aEnvParam.Password, ref szError);
                    aEnvParam.Password = encryptedPasswd;
                }

                this.db.Entry(aEnvParam).State = EntityState.Modified;
                    this.db.SaveChanges();
       
                if (aEnvParam.Active)
                {
                    var otherEnvParam =
                        this.db.EnvironmentParameters.SingleOrDefault(x => x.ParameterId != aEnvParam.ParameterId);
                    if (otherEnvParam != null)
                    {
                        otherEnvParam.Active = false;
                        this.db.Entry(otherEnvParam).State = EntityState.Modified;

                        this.db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError(string.Empty, e.Message);
                return this.View(aEnvParam);
            }

            if (string.IsNullOrEmpty(szError))
            {
                return RedirectToAction("Development");
            }

            this.ModelState.AddModelError(string.Empty, szError);
            return this.View(aEnvParam);
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

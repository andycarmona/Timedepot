using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimelyDepotMVC.Models.Admin;
using TimelyDepotMVC.Models;
using TimelyDepotMVC.DAL;

namespace TimelyDepotMVC.Controllers.Admin
{
    public class UserProfileAdminController : Controller
    {
        private TimelyDepotContext db = new TimelyDepotContext();

        //
        // GET: /UserProfileAdmin/

        public ActionResult Index()
        {
            return View(db.UserProfiles.ToList());
        }

        //
        // GET: /UserProfileAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        //
        // GET: /UserProfileAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /UserProfileAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserProfile userprofile)
        {
            if (ModelState.IsValid)
            {
                db.UserProfiles.Add(userprofile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userprofile);
        }

        //
        // GET: /UserProfileAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        //
        // POST: /UserProfileAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserProfile userprofile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userprofile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userprofile);
        }

        //
        // GET: /UserProfileAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        //
        // POST: /UserProfileAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
             webpages_Membership webmember = null;

            UserProfile userprofile = db.UserProfiles.Find(id);
            if (System.Web.Security.Roles.IsUserInRole(userprofile.UserName, "Owner"))
            {
                //Do not delete the user
            }
            else
            {
                if (userprofile != null)
                {
                    webmember = db.webpages_Membership.Find(id);
                    if (webmember != null)
                    {
                        db.webpages_Membership.Remove(webmember);
                    }
                }
                db.UserProfiles.Remove(userprofile);
                db.SaveChanges();                
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
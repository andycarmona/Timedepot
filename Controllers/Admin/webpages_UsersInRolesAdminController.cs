using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimelyDepotMVC.Models.Admin;
using TimelyDepotMVC.DAL;

namespace TimelyDepotMVC.Controllers.Admin
{
    public class webpages_UsersInRolesAdminController : Controller
    {
        private TimelyDepotContext db = new TimelyDepotContext();

        //
        // GET: /webpages_UsersInRolesAdmin/

        public ActionResult Index()
        {
            return View(db.webpages_UsersInRoles.ToList());
        }

        //
        // GET: /webpages_UsersInRolesAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            webpages_UsersInRoles webpages_usersinroles = db.webpages_UsersInRoles.Find(id);
            if (webpages_usersinroles == null)
            {
                return HttpNotFound();
            }
            return View(webpages_usersinroles);
        }

        //
        // GET: /webpages_UsersInRolesAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /webpages_UsersInRolesAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(webpages_UsersInRoles webpages_usersinroles)
        {
            if (ModelState.IsValid)
            {
                db.webpages_UsersInRoles.Add(webpages_usersinroles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(webpages_usersinroles);
        }

        //
        // GET: /webpages_UsersInRolesAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            webpages_UsersInRoles webpages_usersinroles = db.webpages_UsersInRoles.Find(id);
            if (webpages_usersinroles == null)
            {
                return HttpNotFound();
            }
            return View(webpages_usersinroles);
        }

        //
        // POST: /webpages_UsersInRolesAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(webpages_UsersInRoles webpages_usersinroles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(webpages_usersinroles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(webpages_usersinroles);
        }

        //
        // GET: /webpages_UsersInRolesAdmin/Delete/5

        public ActionResult Delete(int UserId = 0, int RoleId = 0)
        {
            //webpages_UsersInRoles webpages_usersinroles = db.webpages_UsersInRoles.Find(id);
            webpages_UsersInRoles webpages_usersinroles = db.webpages_UsersInRoles.Where(usrl => usrl.UserId == UserId && usrl.RoleId == RoleId).FirstOrDefault<webpages_UsersInRoles>();
            if (webpages_usersinroles == null)
            {
                return HttpNotFound();
            }
            return View(webpages_usersinroles);
        }

        //
        // POST: /webpages_UsersInRolesAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int UserId = 0, int RoleId = 0)
        {
            webpages_UsersInRoles webpages_usersinroles = db.webpages_UsersInRoles.Where(usrl => usrl.UserId == UserId && usrl.RoleId == RoleId).FirstOrDefault<webpages_UsersInRoles>();
            db.webpages_UsersInRoles.Remove(webpages_usersinroles);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
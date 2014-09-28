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
    public class webpages_MembershipAdminController : Controller
    {
        private TimelyDepotContext db = new TimelyDepotContext();

        //
        // GET: /webpages_MembershipAdmin/

        public ActionResult Index()
        {
            return View(db.webpages_Membership.ToList());
        }

        //
        // GET: /webpages_MembershipAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            webpages_Membership webpages_membership = db.webpages_Membership.Find(id);
            if (webpages_membership == null)
            {
                return HttpNotFound();
            }
            return View(webpages_membership);
        }

        //
        // GET: /webpages_MembershipAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /webpages_MembershipAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(webpages_Membership webpages_membership)
        {
            if (ModelState.IsValid)
            {
                db.webpages_Membership.Add(webpages_membership);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(webpages_membership);
        }

        //
        // GET: /webpages_MembershipAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            webpages_Membership webpages_membership = db.webpages_Membership.Find(id);
            if (webpages_membership == null)
            {
                return HttpNotFound();
            }
            return View(webpages_membership);
        }

        //
        // POST: /webpages_MembershipAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(webpages_Membership webpages_membership)
        {
            if (ModelState.IsValid)
            {
                db.Entry(webpages_membership).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(webpages_membership);
        }

        //
        // GET: /webpages_MembershipAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            webpages_Membership webpages_membership = db.webpages_Membership.Find(id);
            if (webpages_membership == null)
            {
                return HttpNotFound();
            }
            return View(webpages_membership);
        }

        //
        // POST: /webpages_MembershipAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            webpages_Membership webpages_membership = db.webpages_Membership.Find(id);
            db.webpages_Membership.Remove(webpages_membership);
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
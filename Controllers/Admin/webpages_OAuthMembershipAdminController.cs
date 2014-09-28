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
    public class webpages_OAuthMembershipAdminController : Controller
    {
        private TimelyDepotContext db = new TimelyDepotContext();

        //
        // GET: /webpages_OAuthMembershipAdmin/

        public ActionResult Index()
        {
            return View(db.webpages_OAuthMembership.ToList());
        }

        //
        // GET: /webpages_OAuthMembershipAdmin/Details/5

        public ActionResult Details(string ProviderUserId = null, int UserId = 0)
        {
            //int nId = 0;

            //webpages_OAuthMembership webpages_oauthmembership = db.webpages_OAuthMembership.Find(nId);
            webpages_OAuthMembership webpages_oauthmembership = db.webpages_OAuthMembership.Where(oau => oau.ProviderUserId == ProviderUserId && oau.UserId == UserId).FirstOrDefault<webpages_OAuthMembership>();
            if (webpages_oauthmembership == null)
            {
                return HttpNotFound();
            }
            return View(webpages_oauthmembership);
        }

        //
        // GET: /webpages_OAuthMembershipAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /webpages_OAuthMembershipAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(webpages_OAuthMembership webpages_oauthmembership)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(webpages_oauthmembership.Provider))
                {
                    ModelState.AddModelError("", "Provider can not be null.");
                    return View(webpages_oauthmembership);  
                }

                db.webpages_OAuthMembership.Add(webpages_oauthmembership);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(webpages_oauthmembership);
        }

        //
        // GET: /webpages_OAuthMembershipAdmin/Edit/5

        public ActionResult Edit(string ProviderUserId = null, int UserId = 0)
        {
            //webpages_OAuthMembership webpages_oauthmembership = db.webpages_OAuthMembership.Find(id);
            webpages_OAuthMembership webpages_oauthmembership = db.webpages_OAuthMembership.Where(oau => oau.ProviderUserId == ProviderUserId && oau.UserId == UserId).FirstOrDefault<webpages_OAuthMembership>();
            if (webpages_oauthmembership == null)
            {
                return HttpNotFound();
            }
            return View(webpages_oauthmembership);
        }

        //
        // POST: /webpages_OAuthMembershipAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(webpages_OAuthMembership webpages_oauthmembership)
        {
            if (ModelState.IsValid)
            {
                db.Entry(webpages_oauthmembership).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(webpages_oauthmembership);
        }

        //
        // GET: /webpages_OAuthMembershipAdmin/Delete/5

        public ActionResult Delete(string ProviderUserId = null, int UserId = 0)
        {
            //webpages_OAuthMembership webpages_oauthmembership = db.webpages_OAuthMembership.Find(id);
            webpages_OAuthMembership webpages_oauthmembership = db.webpages_OAuthMembership.Where(oau => oau.ProviderUserId == ProviderUserId && oau.UserId == UserId).FirstOrDefault<webpages_OAuthMembership>();
            if (webpages_oauthmembership == null)
            {
                return HttpNotFound();
            }
            return View(webpages_oauthmembership);
        }

        //
        // POST: /webpages_OAuthMembershipAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string ProviderUserId = null, int UserId = 0)
        {
            //webpages_OAuthMembership webpages_oauthmembership = db.webpages_OAuthMembership.Find(id);
            webpages_OAuthMembership webpages_oauthmembership = db.webpages_OAuthMembership.Where(oau => oau.ProviderUserId == ProviderUserId && oau.UserId == UserId).FirstOrDefault<webpages_OAuthMembership>();
            db.webpages_OAuthMembership.Remove(webpages_oauthmembership);
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
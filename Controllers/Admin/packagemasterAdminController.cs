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
    public class packagemasterAdminController : Controller
    {
        private TimelyDepotContext db = new TimelyDepotContext();

        //
        // GET: /packagemasterAdmin/

        public ActionResult Index()
        {
            return View(db.packagemasters.ToList());
        }

        //
        // GET: /packagemasterAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            packagemaster packagemaster = db.packagemasters.Find(id);
            if (packagemaster == null)
            {
                return HttpNotFound();
            }
            return View(packagemaster);
        }

        //
        // GET: /packagemasterAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /packagemasterAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(packagemaster packagemaster)
        {
            if (ModelState.IsValid)
            {
                db.packagemasters.Add(packagemaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(packagemaster);
        }

        //
        // GET: /packagemasterAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            packagemaster packagemaster = db.packagemasters.Find(id);
            if (packagemaster == null)
            {
                return HttpNotFound();
            }
            return View(packagemaster);
        }

        //
        // POST: /packagemasterAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(packagemaster packagemaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(packagemaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(packagemaster);
        }

        //
        // GET: /packagemasterAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            packagemaster packagemaster = db.packagemasters.Find(id);
            if (packagemaster == null)
            {
                return HttpNotFound();
            }
            return View(packagemaster);
        }

        //
        // POST: /packagemasterAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            packagemaster packagemaster = db.packagemasters.Find(id);
            db.packagemasters.Remove(packagemaster);
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
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
    public class InitialInfoAdminController : Controller
    {
        private TimelyDepotContext db = new TimelyDepotContext();

        //
        // GET: /InitialInfoAdmin/

        public ActionResult Index()
        {
            return View(db.InitialInfoes.ToList());
        }

        //
        // GET: /InitialInfoAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            InitialInfo initialinfo = db.InitialInfoes.Find(id);
            if (initialinfo == null)
            {
                return HttpNotFound();
            }
            return View(initialinfo);
        }

        //
        // GET: /InitialInfoAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /InitialInfoAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InitialInfo initialinfo)
        {
            if (ModelState.IsValid)
            {
                db.InitialInfoes.Add(initialinfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(initialinfo);
        }

        //
        // GET: /InitialInfoAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            InitialInfo initialinfo = db.InitialInfoes.Find(id);
            if (initialinfo == null)
            {
                return HttpNotFound();
            }
            return View(initialinfo);
        }

        //
        // POST: /InitialInfoAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InitialInfo initialinfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(initialinfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(initialinfo);
        }

        //
        // GET: /InitialInfoAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            InitialInfo initialinfo = db.InitialInfoes.Find(id);
            if (initialinfo == null)
            {
                return HttpNotFound();
            }
            return View(initialinfo);
        }

        //
        // POST: /InitialInfoAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InitialInfo initialinfo = db.InitialInfoes.Find(id);
            db.InitialInfoes.Remove(initialinfo);
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
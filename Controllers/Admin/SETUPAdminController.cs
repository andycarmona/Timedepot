using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimelyDepotMVC.Models.Admin;
using TimelyDepotMVC.DAL;
using PagedList;

namespace TimelyDepotMVC.Controllers.Admin
{
    public class SETUPAdminController : Controller
    {
        private TimelyDepotContext db = new TimelyDepotContext();

        int _pageIndex = 0;
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }

        int _pageSize = 25;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        //
        // GET: /SETUPAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<SETUP> qrySU = null;

            List<SETUP> setupList = new List<SETUP>();

            qrySU = db.SETUPs.OrderBy(stu => stu.SetupID);
            if (qrySU.Count() > 0)
            {
                foreach (var item in qrySU)
                {
                    setupList.Add(item);
                }
            }

            //Set the page
            if (page == null)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = Convert.ToInt32(page);
            }


            var onePageOfData = setupList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(setupList.ToPagedList(pageIndex, pageSize));
            //return View(db.SETUPs.ToList());
        }

        //
        // GET: /SETUPAdmin/Details/5

        public ActionResult Details(string id = null)
        {
            SETUP setup = db.SETUPs.Find(id);
            if (setup == null)
            {
                return HttpNotFound();
            }
            return View(setup);
        }

        //
        // GET: /SETUPAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SETUPAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SETUP setup)
        {
            if (ModelState.IsValid)
            {
                db.SETUPs.Add(setup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(setup);
        }

        //
        // GET: /SETUPAdmin/Edit/5

        public ActionResult Edit(string id = null)
        {
            SETUP setup = db.SETUPs.Find(id);
            if (setup == null)
            {
                return HttpNotFound();
            }
            return View(setup);
        }

        //
        // POST: /SETUPAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SETUP setup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(setup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(setup);
        }

        //
        // GET: /SETUPAdmin/Delete/5

        public ActionResult Delete(string id = null)
        {
            SETUP setup = db.SETUPs.Find(id);
            if (setup == null)
            {
                return HttpNotFound();
            }
            return View(setup);
        }

        //
        // POST: /SETUPAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SETUP setup = db.SETUPs.Find(id);
            db.SETUPs.Remove(setup);
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
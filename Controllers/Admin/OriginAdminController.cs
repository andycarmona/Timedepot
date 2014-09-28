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
    public class OriginAdminController : Controller
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
        // GET: /OriginAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<Origin> qryOrigin = null;

            List<Origin> OriginList = new List<Origin>();

            qryOrigin = db.Origins.OrderBy(org => org.Name);
            if (qryOrigin.Count() > 0)
            {
                foreach (var item in qryOrigin)
                {
                    OriginList.Add(item);
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


            var onePageOfData = OriginList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(OriginList.ToPagedList(pageIndex, pageSize));
            //return View(db.Origins.ToList());
        }

        //
        // GET: /OriginAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            Origin origin = db.Origins.Find(id);
            if (origin == null)
            {
                return HttpNotFound();
            }
            return View(origin);
        }

        //
        // GET: /OriginAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /OriginAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Origin origin)
        {
            if (ModelState.IsValid)
            {
                db.Origins.Add(origin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(origin);
        }

        //
        // GET: /OriginAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Origin origin = db.Origins.Find(id);
            if (origin == null)
            {
                return HttpNotFound();
            }
            return View(origin);
        }

        //
        // POST: /OriginAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Origin origin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(origin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(origin);
        }

        //
        // GET: /OriginAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Origin origin = db.Origins.Find(id);
            if (origin == null)
            {
                return HttpNotFound();
            }
            return View(origin);
        }

        //
        // POST: /OriginAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Origin origin = db.Origins.Find(id);
            db.Origins.Remove(origin);
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
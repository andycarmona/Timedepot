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
    public class CATEGORYAdminController : Controller
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
        // GET: /CATEGORYAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<CATEGORY> qryCat = null;

            List<CATEGORY> CATEGORYList = new List<CATEGORY>();

            qryCat = db.CATEGORies.OrderBy(ct => ct.CatID);
            if (qryCat.Count() > 0)
            {
                foreach (var item in qryCat)
                {
                    CATEGORYList.Add(item);
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


            var onePageOfData = CATEGORYList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(CATEGORYList.ToPagedList(pageIndex, pageSize));
            //return View(db.CATEGORies.ToList());
        }

        //
        // GET: /CATEGORYAdmin/Details/5

        public ActionResult Details(string id = null)
        {
            CATEGORY category = db.CATEGORies.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        //
        // GET: /CATEGORYAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CATEGORYAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CATEGORY category)
        {
            if (ModelState.IsValid)
            {
                db.CATEGORies.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        //
        // GET: /CATEGORYAdmin/Edit/5

        public ActionResult Edit(string id = null)
        {
            CATEGORY category = db.CATEGORies.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        //
        // POST: /CATEGORYAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CATEGORY category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        //
        // GET: /CATEGORYAdmin/Delete/5

        public ActionResult Delete(string id = null)
        {
            CATEGORY category = db.CATEGORies.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        //
        // POST: /CATEGORYAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CATEGORY category = db.CATEGORies.Find(id);
            db.CATEGORies.Remove(category);
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
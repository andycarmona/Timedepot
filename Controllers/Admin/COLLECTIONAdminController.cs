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
    public class COLLECTIONAdminController : Controller
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
        // GET: /COLLECTIONAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<COLLECTION> qryColl = null;

            List<COLLECTION> COLLECTIONList = new List<COLLECTION>();

            qryColl = db.COLLECTIONs.OrderBy(cll => cll.ColID);
            if (qryColl.Count() > 0)
            {
                foreach (var item in qryColl)
                {
                    COLLECTIONList.Add(item);
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


            var onePageOfData = COLLECTIONList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(COLLECTIONList.ToPagedList(pageIndex, pageSize));
            //return View(db.COLLECTIONs.ToList());
        }

        //
        // GET: /COLLECTIONAdmin/Details/5

        public ActionResult Details(string id = null)
        {
            COLLECTION collection = db.COLLECTIONs.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            return View(collection);
        }

        //
        // GET: /COLLECTIONAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /COLLECTIONAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(COLLECTION collection)
        {
            if (ModelState.IsValid)
            {
                db.COLLECTIONs.Add(collection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(collection);
        }

        //
        // GET: /COLLECTIONAdmin/Edit/5

        public ActionResult Edit(string id = null)
        {
            COLLECTION collection = db.COLLECTIONs.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            return View(collection);
        }

        //
        // POST: /COLLECTIONAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(COLLECTION collection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(collection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(collection);
        }

        //
        // GET: /COLLECTIONAdmin/Delete/5

        public ActionResult Delete(string id = null)
        {
            COLLECTION collection = db.COLLECTIONs.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            return View(collection);
        }

        //
        // POST: /COLLECTIONAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            COLLECTION collection = db.COLLECTIONs.Find(id);
            db.COLLECTIONs.Remove(collection);
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
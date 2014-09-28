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
    public class DELIVERYAdminController : Controller
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
        // GET: /DELIVERYAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<DELIVERY> qryDel = null;

            List<DELIVERY> DELIVERYList = new List<DELIVERY>();

            qryDel = db.DELIVERies.OrderBy(dlr => dlr.DeliveryID);
            if (qryDel.Count() > 0)
            {
                foreach (var item in qryDel)
                {
                    DELIVERYList.Add(item);
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


            var onePageOfData = DELIVERYList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(DELIVERYList.ToPagedList(pageIndex, pageSize));
            //return View(db.DELIVERies.ToList());
        }

        //
        // GET: /DELIVERYAdmin/Details/5

        public ActionResult Details(string id = null)
        {
            DELIVERY delivery = db.DELIVERies.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            return View(delivery);
        }

        //
        // GET: /DELIVERYAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /DELIVERYAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DELIVERY delivery)
        {
            if (ModelState.IsValid)
            {
                db.DELIVERies.Add(delivery);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(delivery);
        }

        //
        // GET: /DELIVERYAdmin/Edit/5

        public ActionResult Edit(string id = null)
        {
            DELIVERY delivery = db.DELIVERies.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            return View(delivery);
        }

        //
        // POST: /DELIVERYAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DELIVERY delivery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(delivery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(delivery);
        }

        //
        // GET: /DELIVERYAdmin/Delete/5

        public ActionResult Delete(string id = null)
        {
            DELIVERY delivery = db.DELIVERies.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            return View(delivery);
        }

        //
        // POST: /DELIVERYAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DELIVERY delivery = db.DELIVERies.Find(id);
            db.DELIVERies.Remove(delivery);
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
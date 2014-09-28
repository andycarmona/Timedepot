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
    public class VendorsHistoryAdminController : Controller
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
        // GET: /VendorsHistoryAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<VendorsHistory> qryVendorsHt = null;

            List<VendorsHistory> VendorsHistoryList = new List<VendorsHistory>();

            qryVendorsHt = db.VendorsHistories.OrderBy(vht => vht.VendorId);
            if (qryVendorsHt.Count() > 0)
            {
                foreach (var item in qryVendorsHt)
                {
                    VendorsHistoryList.Add(item);
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


            var onePageOfData = VendorsHistoryList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(VendorsHistoryList.ToPagedList(pageIndex, pageSize));
            //return View(db.VendorsHistories.ToList());
        }

        //
        // GET: /VendorsHistoryAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            VendorsHistory vendorshistory = db.VendorsHistories.Find(id);
            if (vendorshistory == null)
            {
                return HttpNotFound();
            }
            return View(vendorshistory);
        }

        //
        // GET: /VendorsHistoryAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /VendorsHistoryAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VendorsHistory vendorshistory)
        {
            if (ModelState.IsValid)
            {
                db.VendorsHistories.Add(vendorshistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vendorshistory);
        }

        //
        // GET: /VendorsHistoryAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            VendorsHistory vendorshistory = db.VendorsHistories.Find(id);
            if (vendorshistory == null)
            {
                return HttpNotFound();
            }
            return View(vendorshistory);
        }

        //
        // POST: /VendorsHistoryAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VendorsHistory vendorshistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendorshistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendorshistory);
        }

        //
        // GET: /VendorsHistoryAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            VendorsHistory vendorshistory = db.VendorsHistories.Find(id);
            if (vendorshistory == null)
            {
                return HttpNotFound();
            }
            return View(vendorshistory);
        }

        //
        // POST: /VendorsHistoryAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VendorsHistory vendorshistory = db.VendorsHistories.Find(id);
            db.VendorsHistories.Remove(vendorshistory);
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
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
    public class PurchaseOrdersAdminController : Controller
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
        // GET: /PurchaseOrdersAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<PurchaseOrders> qryVendors = null;

            List<PurchaseOrders> VendorsList = new List<PurchaseOrders>();

            qryVendors = db.PurchaseOrders.OrderBy(vd => vd.PurchaseOrderNo);
            if (qryVendors.Count() > 0)
            {
                foreach (var item in qryVendors)
                {
                    VendorsList.Add(item);
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


            var onePageOfData = VendorsList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(VendorsList.ToPagedList(pageIndex, pageSize));
            //return View(db.PurchaseOrders.ToList());
        }

        //
        // GET: /PurchaseOrdersAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            PurchaseOrders purchaseorders = db.PurchaseOrders.Find(id);
            if (purchaseorders == null)
            {
                return HttpNotFound();
            }
            return View(purchaseorders);
        }

        //
        // GET: /PurchaseOrdersAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PurchaseOrdersAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PurchaseOrders purchaseorders)
        {
            if (ModelState.IsValid)
            {
                db.PurchaseOrders.Add(purchaseorders);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(purchaseorders);
        }

        //
        // GET: /PurchaseOrdersAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PurchaseOrders purchaseorders = db.PurchaseOrders.Find(id);
            if (purchaseorders == null)
            {
                return HttpNotFound();
            }
            return View(purchaseorders);
        }

        //
        // POST: /PurchaseOrdersAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PurchaseOrders purchaseorders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchaseorders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(purchaseorders);
        }

        //
        // GET: /PurchaseOrdersAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PurchaseOrders purchaseorders = db.PurchaseOrders.Find(id);
            if (purchaseorders == null)
            {
                return HttpNotFound();
            }
            return View(purchaseorders);
        }

        //
        // POST: /PurchaseOrdersAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PurchaseOrders purchaseorders = db.PurchaseOrders.Find(id);
            db.PurchaseOrders.Remove(purchaseorders);
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
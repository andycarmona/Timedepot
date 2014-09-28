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
    public class VendorItemAdminController : Controller
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
        // GET: /VendorItemAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<VendorItem> qryVendorItem = null;

            List<VendorItem> VendorItemList = new List<VendorItem>();

            qryVendorItem = db.VendorItems.OrderBy(vi => vi.ItemId);
            if (qryVendorItem.Count() > 0)
            {
                foreach (var item in qryVendorItem)
                {
                    VendorItemList.Add(item);
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


            var onePageOfData = VendorItemList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(VendorItemList.ToPagedList(pageIndex, pageSize));
            //return View(db.VendorItems.ToList());
        }

        //
        // GET: /VendorItemAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            VendorItem vendoritem = db.VendorItems.Find(id);
            if (vendoritem == null)
            {
                return HttpNotFound();
            }
            return View(vendoritem);
        }

        //
        // GET: /VendorItemAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /VendorItemAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VendorItem vendoritem)
        {
            if (ModelState.IsValid)
            {
                db.VendorItems.Add(vendoritem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vendoritem);
        }

        //
        // GET: /VendorItemAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            VendorItem vendoritem = db.VendorItems.Find(id);
            if (vendoritem == null)
            {
                return HttpNotFound();
            }
            return View(vendoritem);
        }

        //
        // POST: /VendorItemAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VendorItem vendoritem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendoritem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendoritem);
        }

        //
        // GET: /VendorItemAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            VendorItem vendoritem = db.VendorItems.Find(id);
            if (vendoritem == null)
            {
                return HttpNotFound();
            }
            return View(vendoritem);
        }

        //
        // POST: /VendorItemAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VendorItem vendoritem = db.VendorItems.Find(id);
            db.VendorItems.Remove(vendoritem);
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
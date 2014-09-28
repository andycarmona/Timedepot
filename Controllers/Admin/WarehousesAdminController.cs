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
    public class WarehousesAdminController : Controller
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
        // GET: /WarehousesAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<Warehouses> qryVendors = null;

            List<Warehouses> VendorsList = new List<Warehouses>();

            qryVendors = db.Warehouses.OrderBy(vd => vd.Warehouse);
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
            //return View(db.Warehouses.ToList());
        }

        //
        // GET: /WarehousesAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            Warehouses warehouses = db.Warehouses.Find(id);
            if (warehouses == null)
            {
                return HttpNotFound();
            }
            return View(warehouses);
        }

        //
        // GET: /WarehousesAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /WarehousesAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Warehouses warehouses)
        {
            if (ModelState.IsValid)
            {
                db.Warehouses.Add(warehouses);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(warehouses);
        }

        //
        // GET: /WarehousesAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Warehouses warehouses = db.Warehouses.Find(id);
            if (warehouses == null)
            {
                return HttpNotFound();
            }
            return View(warehouses);
        }

        //
        // POST: /WarehousesAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Warehouses warehouses)
        {
            if (ModelState.IsValid)
            {
                db.Entry(warehouses).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(warehouses);
        }

        //
        // GET: /WarehousesAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Warehouses warehouses = db.Warehouses.Find(id);
            if (warehouses == null)
            {
                return HttpNotFound();
            }
            return View(warehouses);
        }

        //
        // POST: /WarehousesAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Warehouses warehouses = db.Warehouses.Find(id);
            db.Warehouses.Remove(warehouses);
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
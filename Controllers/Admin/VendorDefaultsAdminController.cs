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
    public class VendorDefaultsAdminController : Controller
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
        // GET: /VendorDefaultsAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<VendorDefaults> qryVendors = null;

            List<VendorDefaults> VendorsList = new List<VendorDefaults>();

            qryVendors = db.VendorDefaults.OrderBy(vd => vd.VendorId);
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
            //return View(db.VendorDefaults.ToList());
        }

        //
        // GET: /VendorDefaultsAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            VendorDefaults vendordefaults = db.VendorDefaults.Find(id);
            if (vendordefaults == null)
            {
                return HttpNotFound();
            }
            return View(vendordefaults);
        }

        //
        // GET: /VendorDefaultsAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /VendorDefaultsAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VendorDefaults vendordefaults)
        {
            if (ModelState.IsValid)
            {
                db.VendorDefaults.Add(vendordefaults);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vendordefaults);
        }

        //
        // GET: /VendorDefaultsAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            VendorDefaults vendordefaults = db.VendorDefaults.Find(id);
            if (vendordefaults == null)
            {
                return HttpNotFound();
            }
            return View(vendordefaults);
        }

        //
        // POST: /VendorDefaultsAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VendorDefaults vendordefaults)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendordefaults).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendordefaults);
        }

        //
        // GET: /VendorDefaultsAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            VendorDefaults vendordefaults = db.VendorDefaults.Find(id);
            if (vendordefaults == null)
            {
                return HttpNotFound();
            }
            return View(vendordefaults);
        }

        //
        // POST: /VendorDefaultsAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VendorDefaults vendordefaults = db.VendorDefaults.Find(id);
            db.VendorDefaults.Remove(vendordefaults);
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
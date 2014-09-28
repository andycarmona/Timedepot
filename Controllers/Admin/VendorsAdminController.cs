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
    public class VendorsAdminController : Controller
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
        // GET: /VendorsAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<Vendors> qryVendors = null;

            List<Vendors> VendorsList = new List<Vendors>();

            qryVendors = db.Vendors.OrderBy(vd => vd.VendorNo);
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
            //return View(db.Vendors.ToList());
        }

        //
        // GET: /VendorsAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            Vendors vendors = db.Vendors.Find(id);
            if (vendors == null)
            {
                return HttpNotFound();
            }
            return View(vendors);
        }

        //
        // GET: /VendorsAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /VendorsAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Vendors vendors)
        {
            if (ModelState.IsValid)
            {
                db.Vendors.Add(vendors);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vendors);
        }

        //
        // GET: /VendorsAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Vendors vendors = db.Vendors.Find(id);
            if (vendors == null)
            {
                return HttpNotFound();
            }
            return View(vendors);
        }

        //
        // POST: /VendorsAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vendors vendors)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendors).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendors);
        }

        //
        // GET: /VendorsAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Vendors vendors = db.Vendors.Find(id);
            if (vendors == null)
            {
                return HttpNotFound();
            }
            return View(vendors);
        }

        //
        // POST: /VendorsAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vendors vendors = db.Vendors.Find(id);
            db.Vendors.Remove(vendors);
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
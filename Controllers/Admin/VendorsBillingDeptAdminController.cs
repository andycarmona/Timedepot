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
    public class VendorsBillingDeptAdminController : Controller
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
        // GET: /VendorsBillingDeptAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<VendorsBillingDept> qryVendors = null;

            List<VendorsBillingDept> VendorsList = new List<VendorsBillingDept>();

            qryVendors = db.VendorsBillingDepts.OrderBy(vd => vd.Beneficiary);
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
            //return View(db.VendorsBillingDepts.ToList());
        }

        //
        // GET: /VendorsBillingDeptAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            VendorsBillingDept vendorsbillingdept = db.VendorsBillingDepts.Find(id);
            if (vendorsbillingdept == null)
            {
                return HttpNotFound();
            }
            return View(vendorsbillingdept);
        }

        //
        // GET: /VendorsBillingDeptAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /VendorsBillingDeptAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VendorsBillingDept vendorsbillingdept)
        {
            if (ModelState.IsValid)
            {
                db.VendorsBillingDepts.Add(vendorsbillingdept);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vendorsbillingdept);
        }

        //
        // GET: /VendorsBillingDeptAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            VendorsBillingDept vendorsbillingdept = db.VendorsBillingDepts.Find(id);
            if (vendorsbillingdept == null)
            {
                return HttpNotFound();
            }
            return View(vendorsbillingdept);
        }

        //
        // POST: /VendorsBillingDeptAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VendorsBillingDept vendorsbillingdept)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendorsbillingdept).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendorsbillingdept);
        }

        //
        // GET: /VendorsBillingDeptAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            VendorsBillingDept vendorsbillingdept = db.VendorsBillingDepts.Find(id);
            if (vendorsbillingdept == null)
            {
                return HttpNotFound();
            }
            return View(vendorsbillingdept);
        }

        //
        // POST: /VendorsBillingDeptAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VendorsBillingDept vendorsbillingdept = db.VendorsBillingDepts.Find(id);
            db.VendorsBillingDepts.Remove(vendorsbillingdept);
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
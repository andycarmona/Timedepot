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
    public class PackageRateLogAdminController : Controller
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
        // GET: /PackageRateLogAdmin/

        public ActionResult Index(int? page, string id)
        {
            int nId = 0;
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<PackageRateLog> qryVendors = null;

            List<PackageRateLog> VendorsList = new List<PackageRateLog>();

            qryVendors = db.PackageRateLogs.OrderBy(vd => vd.ItemId).OrderByDescending(vd => vd.DateSubmit);
            if (!string.IsNullOrEmpty(id))
            {
                nId = Convert.ToInt32(id);
                qryVendors = db.PackageRateLogs.Where(vd => vd.Id == nId).OrderBy(vd => vd.ItemId).OrderByDescending(vd => vd.DateSubmit);
            }
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
            //return View(db.PackageRateLogs.ToList());
        }

        //
        // GET: /PackageRateLogAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            PackageRateLog packageratelog = db.PackageRateLogs.Find(id);
            if (packageratelog == null)
            {
                return HttpNotFound();
            }
            return View(packageratelog);
        }

        //
        // GET: /PackageRateLogAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PackageRateLogAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PackageRateLog packageratelog)
        {
            if (ModelState.IsValid)
            {
                db.PackageRateLogs.Add(packageratelog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(packageratelog);
        }

        //
        // GET: /PackageRateLogAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PackageRateLog packageratelog = db.PackageRateLogs.Find(id);
            if (packageratelog == null)
            {
                return HttpNotFound();
            }
            return View(packageratelog);
        }

        //
        // POST: /PackageRateLogAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PackageRateLog packageratelog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(packageratelog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(packageratelog);
        }

        //
        // GET: /PackageRateLogAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PackageRateLog packageratelog = db.PackageRateLogs.Find(id);
            if (packageratelog == null)
            {
                return HttpNotFound();
            }
            return View(packageratelog);
        }

        //
        // POST: /PackageRateLogAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IQueryable<PackageRateLogDetails> qryLogDetails = null;
            IQueryable<PackageRateLogParameters> qryLogParameters = null;

            //Delete log details
            qryLogDetails = db.PackageRateLogDetails.Where(lgdt => lgdt.IdRateLog == id);
            if (qryLogDetails.Count() > 0)
            {
                foreach (var item in qryLogDetails)
                {
                    db.PackageRateLogDetails.Remove(item);
                }
            }

            //Delete log parameters
            qryLogParameters = db.PackageRateLogParameters.Where(lgdt => lgdt.IdRateLog == id);
            if (qryLogParameters.Count() > 0)
            {
                foreach (var item in qryLogParameters)
                {
                    db.PackageRateLogParameters.Remove(item);
                }
            }

            PackageRateLog packageratelog = db.PackageRateLogs.Find(id);
            db.PackageRateLogs.Remove(packageratelog);
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
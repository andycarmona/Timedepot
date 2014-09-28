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
    public class PackageRateLogDetailsAdminController : Controller
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
        // GET: /PackageRateLogDetailsAdmin/

        public ActionResult Index(int? page, int id = 0)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<PackageRateLogDetails> qryVendors = null;

            List<PackageRateLogDetails> VendorsList = new List<PackageRateLogDetails>();

            qryVendors = db.PackageRateLogDetails.OrderBy(vd => vd.IdRateLog);
            if (id != 0)
            {
                qryVendors = db.PackageRateLogDetails.Where(vd => vd.IdRateLog == id).OrderBy(vd => vd.IdRateLog);

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
            //return View(db.PackageRateLogDetails.ToList());
        }

        //
        // GET: /PackageRateLogDetailsAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            PackageRateLogDetails packageratelogdetails = db.PackageRateLogDetails.Find(id);
            if (packageratelogdetails == null)
            {
                return HttpNotFound();
            }
            return View(packageratelogdetails);
        }

        //
        // GET: /PackageRateLogDetailsAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PackageRateLogDetailsAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PackageRateLogDetails packageratelogdetails)
        {
            if (ModelState.IsValid)
            {
                db.PackageRateLogDetails.Add(packageratelogdetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(packageratelogdetails);
        }

        //
        // GET: /PackageRateLogDetailsAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PackageRateLogDetails packageratelogdetails = db.PackageRateLogDetails.Find(id);
            if (packageratelogdetails == null)
            {
                return HttpNotFound();
            }
            return View(packageratelogdetails);
        }

        //
        // POST: /PackageRateLogDetailsAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PackageRateLogDetails packageratelogdetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(packageratelogdetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(packageratelogdetails);
        }

        //
        // GET: /PackageRateLogDetailsAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PackageRateLogDetails packageratelogdetails = db.PackageRateLogDetails.Find(id);
            if (packageratelogdetails == null)
            {
                return HttpNotFound();
            }
            return View(packageratelogdetails);
        }

        //
        // POST: /PackageRateLogDetailsAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PackageRateLogDetails packageratelogdetails = db.PackageRateLogDetails.Find(id);
            db.PackageRateLogDetails.Remove(packageratelogdetails);
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
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
    public class PackageRateLogParametersAdminController : Controller
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
        // GET: /PackageRateLogParametersAdmin/

        public ActionResult Index(int? page, string id)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            int nRateLogId = 0;
            IQueryable<PackageRateLogParameters> qryVendors = null;

            List<PackageRateLogParameters> VendorsList = new List<PackageRateLogParameters>();

            qryVendors = db.PackageRateLogParameters.OrderBy(vd => vd.IdRateLog);

            if (!string.IsNullOrEmpty(id))
            {
                nRateLogId = Convert.ToInt32(id);
                qryVendors = db.PackageRateLogParameters.Where(vd => vd.IdRateLog == nRateLogId).OrderBy(vd => vd.IdRateLog);
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
            //return View(db.PackageRateLogParameters.ToList());
        }

        //
        // GET: /PackageRateLogParametersAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            PackageRateLogParameters packageratelogparameters = db.PackageRateLogParameters.Find(id);
            if (packageratelogparameters == null)
            {
                return HttpNotFound();
            }
            return View(packageratelogparameters);
        }

        //
        // GET: /PackageRateLogParametersAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PackageRateLogParametersAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PackageRateLogParameters packageratelogparameters)
        {
            if (ModelState.IsValid)
            {
                db.PackageRateLogParameters.Add(packageratelogparameters);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(packageratelogparameters);
        }

        //
        // GET: /PackageRateLogParametersAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PackageRateLogParameters packageratelogparameters = db.PackageRateLogParameters.Find(id);
            if (packageratelogparameters == null)
            {
                return HttpNotFound();
            }
            return View(packageratelogparameters);
        }

        //
        // POST: /PackageRateLogParametersAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PackageRateLogParameters packageratelogparameters)
        {
            if (ModelState.IsValid)
            {
                db.Entry(packageratelogparameters).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(packageratelogparameters);
        }

        //
        // GET: /PackageRateLogParametersAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PackageRateLogParameters packageratelogparameters = db.PackageRateLogParameters.Find(id);
            if (packageratelogparameters == null)
            {
                return HttpNotFound();
            }
            return View(packageratelogparameters);
        }

        //
        // POST: /PackageRateLogParametersAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PackageRateLogParameters packageratelogparameters = db.PackageRateLogParameters.Find(id);
            db.PackageRateLogParameters.Remove(packageratelogparameters);
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
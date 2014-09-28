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
    public class ApplicationsAdminController : Controller
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
        // GET: /ApplicationsAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<Applications> qryVendors = null;

            List<Applications> VendorsList = new List<Applications>();

            qryVendors = db.Applications.OrderBy(vd => vd.ApplicationName);
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
            //return View(db.Applications.ToList());
        }

        //
        // GET: /ApplicationsAdmin/Details/5

        public ActionResult Details(Guid? id)
        {
            Applications applications = db.Applications.Find(id);
            if (applications == null)
            {
                return HttpNotFound();
            }
            return View(applications);
        }

        //
        // GET: /ApplicationsAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ApplicationsAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Applications applications)
        {
            if (ModelState.IsValid)
            {
                applications.ApplicationId = Guid.NewGuid();
                db.Applications.Add(applications);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applications);
        }

        //
        // GET: /ApplicationsAdmin/Edit/5

        public ActionResult Edit(Guid? id)
        {
            Applications applications = db.Applications.Find(id);
            if (applications == null)
            {
                return HttpNotFound();
            }
            return View(applications);
        }

        //
        // POST: /ApplicationsAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Applications applications)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applications).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applications);
        }

        //
        // GET: /ApplicationsAdmin/Delete/5

        public ActionResult Delete(Guid? id)
        {
            Applications applications = db.Applications.Find(id);
            if (applications == null)
            {
                return HttpNotFound();
            }
            return View(applications);
        }

        //
        // POST: /ApplicationsAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Applications applications = db.Applications.Find(id);
            db.Applications.Remove(applications);
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
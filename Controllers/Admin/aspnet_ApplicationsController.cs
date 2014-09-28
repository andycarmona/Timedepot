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
    public class aspnet_ApplicationsAdminController : Controller
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
        // GET: /aspnet_Applications/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<aspnet_Applications> qryVendors = null;

            List<aspnet_Applications> VendorsList = new List<aspnet_Applications>();

            qryVendors = db.aspnet_Applications.OrderBy(vd => vd.ApplicationName);
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
            //return View(db.aspnet_Applications.ToList());
        }

        //
        // GET: /aspnet_Applications/Details/5

        public ActionResult Details(Guid? id)
        {
            aspnet_Applications aspnet_applications = db.aspnet_Applications.Find(id);
            if (aspnet_applications == null)
            {
                return HttpNotFound();
            }
            return View(aspnet_applications);
        }

        //
        // GET: /aspnet_Applications/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /aspnet_Applications/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(aspnet_Applications aspnet_applications)
        {
            if (ModelState.IsValid)
            {
                aspnet_applications.ApplicationId = Guid.NewGuid();
                db.aspnet_Applications.Add(aspnet_applications);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspnet_applications);
        }

        //
        // GET: /aspnet_Applications/Edit/5

        public ActionResult Edit(Guid? id)
        {
            aspnet_Applications aspnet_applications = db.aspnet_Applications.Find(id);
            if (aspnet_applications == null)
            {
                return HttpNotFound();
            }
            return View(aspnet_applications);
        }

        //
        // POST: /aspnet_Applications/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(aspnet_Applications aspnet_applications)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspnet_applications).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspnet_applications);
        }

        //
        // GET: /aspnet_Applications/Delete/5

        public ActionResult Delete(Guid? id)
        {
            aspnet_Applications aspnet_applications = db.aspnet_Applications.Find(id);
            if (aspnet_applications == null)
            {
                return HttpNotFound();
            }
            return View(aspnet_applications);
        }

        //
        // POST: /aspnet_Applications/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            aspnet_Applications aspnet_applications = db.aspnet_Applications.Find(id);
            db.aspnet_Applications.Remove(aspnet_applications);
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
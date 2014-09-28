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
    public class MembershipsAdminController : Controller
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
        // GET: /MembershipsAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<Memberships> qryVendors = null;

            List<Memberships> VendorsList = new List<Memberships>();

            qryVendors = db.Memberships.OrderBy(vd => vd.Email);
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
            //return View(db.Memberships.ToList());
        }

        //
        // GET: /MembershipsAdmin/Details/5

        public ActionResult Details(Guid? id)
        {
            Memberships memberships = db.Memberships.Find(id);
            if (memberships == null)
            {
                return HttpNotFound();
            }
            return View(memberships);
        }

        //
        // GET: /MembershipsAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /MembershipsAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Memberships memberships)
        {
            if (ModelState.IsValid)
            {
                memberships.UserId = Guid.NewGuid();
                db.Memberships.Add(memberships);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(memberships);
        }

        //
        // GET: /MembershipsAdmin/Edit/5

        public ActionResult Edit(Guid? id)
        {
            Memberships memberships = db.Memberships.Find(id);
            if (memberships == null)
            {
                return HttpNotFound();
            }
            return View(memberships);
        }

        //
        // POST: /MembershipsAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Memberships memberships)
        {
            if (ModelState.IsValid)
            {
                db.Entry(memberships).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(memberships);
        }

        //
        // GET: /MembershipsAdmin/Delete/5

        public ActionResult Delete(Guid? id)
        {
            Memberships memberships = db.Memberships.Find(id);
            if (memberships == null)
            {
                return HttpNotFound();
            }
            return View(memberships);
        }

        //
        // POST: /MembershipsAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Memberships memberships = db.Memberships.Find(id);
            db.Memberships.Remove(memberships);
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
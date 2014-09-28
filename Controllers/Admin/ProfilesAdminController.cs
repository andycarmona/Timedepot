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
    public class ProfilesAdminController : Controller
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
        // GET: /ProfilesAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<Profiles> qryVendors = null;

            List<Profiles> VendorsList = new List<Profiles>();

            qryVendors = db.Profiles.OrderBy(vd => vd.UserId);
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
            //return View(db.Profiles.ToList());
        }

        //
        // GET: /ProfilesAdmin/Details/5

        public ActionResult Details(Guid? id)
        {
            Profiles profiles = db.Profiles.Find(id);
            if (profiles == null)
            {
                return HttpNotFound();
            }
            return View(profiles);
        }

        //
        // GET: /ProfilesAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ProfilesAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Profiles profiles)
        {
            if (ModelState.IsValid)
            {
                profiles.UserId = Guid.NewGuid();
                db.Profiles.Add(profiles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(profiles);
        }

        //
        // GET: /ProfilesAdmin/Edit/5

        public ActionResult Edit(Guid? id)
        {
            Profiles profiles = db.Profiles.Find(id);
            if (profiles == null)
            {
                return HttpNotFound();
            }
            return View(profiles);
        }

        //
        // POST: /ProfilesAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Profiles profiles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(profiles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(profiles);
        }

        //
        // GET: /ProfilesAdmin/Delete/5

        public ActionResult Delete(Guid? id)
        {
            Profiles profiles = db.Profiles.Find(id);
            if (profiles == null)
            {
                return HttpNotFound();
            }
            return View(profiles);
        }

        //
        // POST: /ProfilesAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Profiles profiles = db.Profiles.Find(id);
            db.Profiles.Remove(profiles);
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
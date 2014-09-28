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
    public class PackageGroupDetailAdminController : Controller
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
        // GET: /PackageGroupDetailAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<PackageGroupDetail> qryPkGr = null;

            List<PackageGroupDetail> packagegroupList = new List<PackageGroupDetail>();

            qryPkGr = db.PackageGroupDetails.OrderBy(pkgr => pkgr.GroupName);
            if (qryPkGr.Count() > 0)
            {
                foreach (var item in qryPkGr)
                {
                    packagegroupList.Add(item);
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


            var onePageOfData = packagegroupList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(packagegroupList.ToPagedList(pageIndex, pageSize));
            //return View(db.PackageGroupDetails.ToList());
        }

        //
        // GET: /PackageGroupDetailAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            PackageGroupDetail packagegroupdetail = db.PackageGroupDetails.Find(id);
            if (packagegroupdetail == null)
            {
                return HttpNotFound();
            }
            return View(packagegroupdetail);
        }

        //
        // GET: /PackageGroupDetailAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PackageGroupDetailAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PackageGroupDetail packagegroupdetail)
        {
            if (ModelState.IsValid)
            {
                db.PackageGroupDetails.Add(packagegroupdetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(packagegroupdetail);
        }

        //
        // GET: /PackageGroupDetailAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PackageGroupDetail packagegroupdetail = db.PackageGroupDetails.Find(id);
            if (packagegroupdetail == null)
            {
                return HttpNotFound();
            }
            return View(packagegroupdetail);
        }

        //
        // POST: /PackageGroupDetailAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PackageGroupDetail packagegroupdetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(packagegroupdetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(packagegroupdetail);
        }

        //
        // GET: /PackageGroupDetailAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PackageGroupDetail packagegroupdetail = db.PackageGroupDetails.Find(id);
            if (packagegroupdetail == null)
            {
                return HttpNotFound();
            }
            return View(packagegroupdetail);
        }

        //
        // POST: /PackageGroupDetailAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PackageGroupDetail packagegroupdetail = db.PackageGroupDetails.Find(id);
            db.PackageGroupDetails.Remove(packagegroupdetail);
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
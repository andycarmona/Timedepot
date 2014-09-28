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
    public class PACKAGEAdminController : Controller
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
        // GET: /PACKAGEAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<PACKAGE> qryPk = null;

            List<PACKAGE> packageList = new List<PACKAGE>();

            qryPk = db.PACKAGEs.OrderBy(pk => pk.PackageID);
            if (qryPk.Count() > 0)
            {
                foreach (var item in qryPk)
                {
                    packageList.Add(item);
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


            var onePageOfData = packageList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(packageList.ToPagedList(pageIndex, pageSize));
            //return View(db.PACKAGEs.ToList());
        }

        //
        // GET: /PACKAGEAdmin/Details/5

        public ActionResult Details(string id = null)
        {
            PACKAGE package = db.PACKAGEs.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }

        //
        // GET: /PACKAGEAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PACKAGEAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PACKAGE package)
        {
            if (ModelState.IsValid)
            {
                db.PACKAGEs.Add(package);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(package);
        }

        //
        // GET: /PACKAGEAdmin/Edit/5

        public ActionResult Edit(string id = null)
        {
            PACKAGE package = db.PACKAGEs.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }

        //
        // POST: /PACKAGEAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PACKAGE package)
        {
            if (ModelState.IsValid)
            {
                db.Entry(package).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(package);
        }

        //
        // GET: /PACKAGEAdmin/Delete/5

        public ActionResult Delete(string id = null)
        {
            PACKAGE package = db.PACKAGEs.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }

        //
        // POST: /PACKAGEAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PACKAGE package = db.PACKAGEs.Find(id);
            db.PACKAGEs.Remove(package);
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
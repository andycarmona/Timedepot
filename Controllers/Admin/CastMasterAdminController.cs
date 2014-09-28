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
    public class CastMasterAdminController : Controller
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
        // GET: /CastMasterAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<CastMaster> qryCst = null;

            List<CastMaster> CastMasterList = new List<CastMaster>();

            qryCst = db.CastMasters.OrderBy(cst => cst.CastName);
            if (qryCst.Count() > 0)
            {
                foreach (var item in qryCst)
                {
                    CastMasterList.Add(item);
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


            var onePageOfData = CastMasterList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(CastMasterList.ToPagedList(pageIndex, pageSize));
            //return View(db.CastMasters.ToList());
        }

        //
        // GET: /CastMasterAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            CastMaster castmaster = db.CastMasters.Find(id);
            if (castmaster == null)
            {
                return HttpNotFound();
            }
            return View(castmaster);
        }

        //
        // GET: /CastMasterAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CastMasterAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CastMaster castmaster)
        {
            if (ModelState.IsValid)
            {
                db.CastMasters.Add(castmaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(castmaster);
        }

        //
        // GET: /CastMasterAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CastMaster castmaster = db.CastMasters.Find(id);
            if (castmaster == null)
            {
                return HttpNotFound();
            }
            return View(castmaster);
        }

        //
        // POST: /CastMasterAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CastMaster castmaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(castmaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(castmaster);
        }

        //
        // GET: /CastMasterAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CastMaster castmaster = db.CastMasters.Find(id);
            if (castmaster == null)
            {
                return HttpNotFound();
            }
            return View(castmaster);
        }

        //
        // POST: /CastMasterAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CastMaster castmaster = db.CastMasters.Find(id);
            db.CastMasters.Remove(castmaster);
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
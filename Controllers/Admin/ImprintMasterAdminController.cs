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
    public class ImprintMasterAdminController : Controller
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
        // GET: /ImprintMasterAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<ImprintMaster> qryIpMt = null;

            List<ImprintMaster> ImprintMasterList = new List<ImprintMaster>();

            qryIpMt = db.ImprintMasters.OrderBy(ipmt => ipmt.ImprintName);
            if (qryIpMt.Count() > 0)
            {
                foreach (var item in qryIpMt)
                {
                    ImprintMasterList.Add(item);
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


            var onePageOfData = ImprintMasterList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(ImprintMasterList.ToPagedList(pageIndex, pageSize));
            //return View(db.ImprintMasters.ToList());
        }

        //
        // GET: /ImprintMasterAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            ImprintMaster imprintmaster = db.ImprintMasters.Find(id);
            if (imprintmaster == null)
            {
                return HttpNotFound();
            }
            return View(imprintmaster);
        }

        //
        // GET: /ImprintMasterAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ImprintMasterAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ImprintMaster imprintmaster)
        {
            if (ModelState.IsValid)
            {
                db.ImprintMasters.Add(imprintmaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(imprintmaster);
        }

        //
        // GET: /ImprintMasterAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ImprintMaster imprintmaster = db.ImprintMasters.Find(id);
            if (imprintmaster == null)
            {
                return HttpNotFound();
            }
            return View(imprintmaster);
        }

        //
        // POST: /ImprintMasterAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ImprintMaster imprintmaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(imprintmaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(imprintmaster);
        }

        //
        // GET: /ImprintMasterAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ImprintMaster imprintmaster = db.ImprintMasters.Find(id);
            if (imprintmaster == null)
            {
                return HttpNotFound();
            }
            return View(imprintmaster);
        }

        //
        // POST: /ImprintMasterAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ImprintMaster imprintmaster = db.ImprintMasters.Find(id);
            db.ImprintMasters.Remove(imprintmaster);
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
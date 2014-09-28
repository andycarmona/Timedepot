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
    public class SetupChargeDetailAdminController : Controller
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
        // GET: /SetupChargeDetailAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<SetupChargeDetail> qryVendors = null;

            List<SetupChargeDetail> VendorsList = new List<SetupChargeDetail>();

            qryVendors = db.SetupChargeDetails.OrderBy(stcd => stcd.itemid);
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
            //return View(db.SetupChargeDetails.ToList());
        }

        //
        // GET: /SetupChargeDetailAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            SetupChargeDetail setupchargedetail = db.SetupChargeDetails.Find(id);
            if (setupchargedetail == null)
            {
                return HttpNotFound();
            }
            return View(setupchargedetail);
        }

        //
        // GET: /SetupChargeDetailAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SetupChargeDetailAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SetupChargeDetail setupchargedetail)
        {
            if (ModelState.IsValid)
            {
                db.SetupChargeDetails.Add(setupchargedetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(setupchargedetail);
        }

        //
        // GET: /SetupChargeDetailAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            SetupChargeDetail setupchargedetail = db.SetupChargeDetails.Find(id);
            if (setupchargedetail == null)
            {
                return HttpNotFound();
            }
            return View(setupchargedetail);
        }

        //
        // POST: /SetupChargeDetailAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SetupChargeDetail setupchargedetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(setupchargedetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(setupchargedetail);
        }

        //
        // GET: /SetupChargeDetailAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            SetupChargeDetail setupchargedetail = db.SetupChargeDetails.Find(id);
            if (setupchargedetail == null)
            {
                return HttpNotFound();
            }
            return View(setupchargedetail);
        }

        //
        // POST: /SetupChargeDetailAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SetupChargeDetail setupchargedetail = db.SetupChargeDetails.Find(id);
            db.SetupChargeDetails.Remove(setupchargedetail);
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
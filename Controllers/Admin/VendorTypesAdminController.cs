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
    public class VendorTypesAdminController : Controller
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
        // GET: /VendorTypesAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<VendorTypes> qryVendors = null;

            List<VendorTypes> VendorsList = new List<VendorTypes>();

            qryVendors = db.VendorTypes.OrderBy(vd => vd.VendorType);
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
            //return View(db.VendorTypes.ToList());
        }

        //
        // GET: /VendorTypesAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            VendorTypes vendortypes = db.VendorTypes.Find(id);
            if (vendortypes == null)
            {
                return HttpNotFound();
            }
            return View(vendortypes);
        }

        //
        // GET: /VendorTypesAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /VendorTypesAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VendorTypes vendortypes)
        {
            if (ModelState.IsValid)
            {
                db.VendorTypes.Add(vendortypes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vendortypes);
        }

        //
        // GET: /VendorTypesAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            VendorTypes vendortypes = db.VendorTypes.Find(id);
            if (vendortypes == null)
            {
                return HttpNotFound();
            }
            return View(vendortypes);
        }

        //
        // POST: /VendorTypesAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VendorTypes vendortypes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendortypes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendortypes);
        }

        //
        // GET: /VendorTypesAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            VendorTypes vendortypes = db.VendorTypes.Find(id);
            if (vendortypes == null)
            {
                return HttpNotFound();
            }
            return View(vendortypes);
        }

        //
        // POST: /VendorTypesAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VendorTypes vendortypes = db.VendorTypes.Find(id);
            db.VendorTypes.Remove(vendortypes);
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
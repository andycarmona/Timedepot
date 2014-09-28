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
    public class ParametersAdminController : Controller
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
        // GET: /ParametersAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<Parameters> qryVendors = null;

            List<Parameters> VendorsList = new List<Parameters>();

            qryVendors = db.Parameters.OrderBy(vd => vd.Parameter);
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
            //return View(db.Parameters.ToList());
        }

        //
        // GET: /ParametersAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            Parameters parameters = db.Parameters.Find(id);
            if (parameters == null)
            {
                return HttpNotFound();
            }
            return View(parameters);
        }

        //
        // GET: /ParametersAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ParametersAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Parameters parameters)
        {
            if (ModelState.IsValid)
            {
                db.Parameters.Add(parameters);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parameters);
        }

        //
        // GET: /ParametersAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Parameters parameters = db.Parameters.Find(id);
            if (parameters == null)
            {
                return HttpNotFound();
            }
            return View(parameters);
        }

        //
        // POST: /ParametersAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Parameters parameters)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parameters).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parameters);
        }

        //
        // GET: /ParametersAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Parameters parameters = db.Parameters.Find(id);
            if (parameters == null)
            {
                return HttpNotFound();
            }
            return View(parameters);
        }

        //
        // POST: /ParametersAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Parameters parameters = db.Parameters.Find(id);
            db.Parameters.Remove(parameters);
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
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
    public class CustomerDefaultsAdminController : Controller
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
        // GET: /CustomerDefaultsAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<CustomerDefaults> qryCustomerDefaults = null;

            List<CustomerDefaults> CustomerDefaultsList = new List<CustomerDefaults>();

            qryCustomerDefaults = db.CustomerDefaults.OrderBy(cudf => cudf.CustomerId);
            if (qryCustomerDefaults.Count() > 0)
            {
                foreach (var item in qryCustomerDefaults)
                {
                    CustomerDefaultsList.Add(item);
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


            var onePageOfData = CustomerDefaultsList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(CustomerDefaultsList.ToPagedList(pageIndex, pageSize));
            //return View(db.CustomerDefaults.ToList());
        }

        //
        // GET: /CustomerDefaultsAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            CustomerDefaults customerdefaults = db.CustomerDefaults.Find(id);
            if (customerdefaults == null)
            {
                return HttpNotFound();
            }
            return View(customerdefaults);
        }

        //
        // GET: /CustomerDefaultsAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CustomerDefaultsAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerDefaults customerdefaults)
        {
            if (ModelState.IsValid)
            {
                db.CustomerDefaults.Add(customerdefaults);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customerdefaults);
        }

        //
        // GET: /CustomerDefaultsAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CustomerDefaults customerdefaults = db.CustomerDefaults.Find(id);
            if (customerdefaults == null)
            {
                return HttpNotFound();
            }
            return View(customerdefaults);
        }

        //
        // POST: /CustomerDefaultsAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerDefaults customerdefaults)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerdefaults).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customerdefaults);
        }

        //
        // GET: /CustomerDefaultsAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CustomerDefaults customerdefaults = db.CustomerDefaults.Find(id);
            if (customerdefaults == null)
            {
                return HttpNotFound();
            }
            return View(customerdefaults);
        }

        //
        // POST: /CustomerDefaultsAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerDefaults customerdefaults = db.CustomerDefaults.Find(id);
            db.CustomerDefaults.Remove(customerdefaults);
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
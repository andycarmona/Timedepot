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
    public class CustomersHistoryAdminController : Controller
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
        // GET: /CustomersHistoryAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<CustomersHistory> qryCustomersHistory = null;

            List<CustomersHistory> CustomersHistoryList = new List<CustomersHistory>();

            qryCustomersHistory = db.CustomersHistories.OrderBy(cut => cut.CustomerId);
            if (qryCustomersHistory.Count() > 0)
            {
                foreach (var item in qryCustomersHistory)
                {
                    CustomersHistoryList.Add(item);
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


            var onePageOfData = CustomersHistoryList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(CustomersHistoryList.ToPagedList(pageIndex, pageSize));
            //return View(db.CustomersHistories.ToList());
        }

        //
        // GET: /CustomersHistoryAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            CustomersHistory customershistory = db.CustomersHistories.Find(id);
            if (customershistory == null)
            {
                return HttpNotFound();
            }
            return View(customershistory);
        }

        //
        // GET: /CustomersHistoryAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CustomersHistoryAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomersHistory customershistory)
        {
            if (ModelState.IsValid)
            {
                db.CustomersHistories.Add(customershistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customershistory);
        }

        //
        // GET: /CustomersHistoryAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CustomersHistory customershistory = db.CustomersHistories.Find(id);
            if (customershistory == null)
            {
                return HttpNotFound();
            }
            return View(customershistory);
        }

        //
        // POST: /CustomersHistoryAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomersHistory customershistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customershistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customershistory);
        }

        //
        // GET: /CustomersHistoryAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CustomersHistory customershistory = db.CustomersHistories.Find(id);
            if (customershistory == null)
            {
                return HttpNotFound();
            }
            return View(customershistory);
        }

        //
        // POST: /CustomersHistoryAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomersHistory customershistory = db.CustomersHistories.Find(id);
            db.CustomersHistories.Remove(customershistory);
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
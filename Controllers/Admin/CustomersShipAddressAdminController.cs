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
    public class CustomersShipAddressAdminController : Controller
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
        // GET: /CustomersShipAddressAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<CustomersShipAddress> qryCustomersShipAddress = null;

            List<CustomersShipAddress> CustomersShipAddressList = new List<CustomersShipAddress>();

            qryCustomersShipAddress = db.CustomersShipAddresses.OrderBy(vd => vd.CustomerId);
            if (qryCustomersShipAddress.Count() > 0)
            {
                foreach (var item in qryCustomersShipAddress)
                {
                    CustomersShipAddressList.Add(item);
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


            var onePageOfData = CustomersShipAddressList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(CustomersShipAddressList.ToPagedList(pageIndex, pageSize));
            //return View(db.CustomersShipAddresses.ToList());
        }

        //
        // GET: /CustomersShipAddressAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            CustomersShipAddress customersshipaddress = db.CustomersShipAddresses.Find(id);
            if (customersshipaddress == null)
            {
                return HttpNotFound();
            }
            return View(customersshipaddress);
        }

        //
        // GET: /CustomersShipAddressAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CustomersShipAddressAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomersShipAddress customersshipaddress)
        {
            if (ModelState.IsValid)
            {
                db.CustomersShipAddresses.Add(customersshipaddress);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customersshipaddress);
        }

        //
        // GET: /CustomersShipAddressAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CustomersShipAddress customersshipaddress = db.CustomersShipAddresses.Find(id);
            if (customersshipaddress == null)
            {
                return HttpNotFound();
            }
            return View(customersshipaddress);
        }

        //
        // POST: /CustomersShipAddressAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomersShipAddress customersshipaddress)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customersshipaddress).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customersshipaddress);
        }

        //
        // GET: /CustomersShipAddressAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CustomersShipAddress customersshipaddress = db.CustomersShipAddresses.Find(id);
            if (customersshipaddress == null)
            {
                return HttpNotFound();
            }
            return View(customersshipaddress);
        }

        //
        // POST: /CustomersShipAddressAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomersShipAddress customersshipaddress = db.CustomersShipAddresses.Find(id);
            db.CustomersShipAddresses.Remove(customersshipaddress);
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
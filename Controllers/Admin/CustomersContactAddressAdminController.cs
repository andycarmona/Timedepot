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
    public class CustomersContactAddressAdminController : Controller
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
        // GET: /UpdateCustomerBilling/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<CustomersContactAddress> qryCustomersContactAddress = null;

            List<CustomersContactAddress> CustomersContactAddressList = new List<CustomersContactAddress>();

            qryCustomersContactAddress = db.CustomersContactAddresses.OrderBy(cut => cut.CompanyName);
            if (qryCustomersContactAddress.Count() > 0)
            {
                foreach (var item in qryCustomersContactAddress)
                {
                    CustomersContactAddressList.Add(item);
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


            var onePageOfData = CustomersContactAddressList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(CustomersContactAddressList.ToPagedList(pageIndex, pageSize));
            //return View(db.CustomersContactAddresses.ToList());
        }

        //
        // GET: /CustomersContactAddressAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            CustomersContactAddress customerscontactaddress = db.CustomersContactAddresses.Find(id);
            if (customerscontactaddress == null)
            {
                return HttpNotFound();
            }
            return View(customerscontactaddress);
        }

        //
        // GET: /CustomersContactAddressAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CustomersContactAddressAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomersContactAddress customerscontactaddress)
        {
            if (ModelState.IsValid)
            {
                db.CustomersContactAddresses.Add(customerscontactaddress);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customerscontactaddress);
        }

        //
        // GET: /CustomersContactAddressAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CustomersContactAddress customerscontactaddress = db.CustomersContactAddresses.Find(id);
            if (customerscontactaddress == null)
            {
                return HttpNotFound();
            }
            return View(customerscontactaddress);
        }

        //
        // POST: /CustomersContactAddressAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomersContactAddress customerscontactaddress)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerscontactaddress).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customerscontactaddress);
        }

        //
        // GET: /CustomersContactAddressAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CustomersContactAddress customerscontactaddress = db.CustomersContactAddresses.Find(id);
            if (customerscontactaddress == null)
            {
                return HttpNotFound();
            }
            return View(customerscontactaddress);
        }

        //
        // POST: /CustomersContactAddressAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomersContactAddress customerscontactaddress = db.CustomersContactAddresses.Find(id);
            db.CustomersContactAddresses.Remove(customerscontactaddress);
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
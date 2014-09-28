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
    public class CustomersSubsidiaryAddressAdminController : Controller
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
        // GET: /CustomersSubsidiaryAddressAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<CustomersSubsidiaryAddress> qryVendors = null;

            List<CustomersSubsidiaryAddress> CustomersSubsidiaryAddressList = new List<CustomersSubsidiaryAddress>();

            qryVendors = db.CustomersSubsidiaryAddresses.OrderBy(cut => cut.CompanyName);
            if (qryVendors.Count() > 0)
            {
                foreach (var item in qryVendors)
                {
                    CustomersSubsidiaryAddressList.Add(item);
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


            var onePageOfData = CustomersSubsidiaryAddressList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(CustomersSubsidiaryAddressList.ToPagedList(pageIndex, pageSize));
            //return View(db.CustomersSubsidiaryAddresses.ToList());
        }

        //
        // GET: /CustomersSubsidiaryAddressAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            CustomersSubsidiaryAddress customerssubsidiaryaddress = db.CustomersSubsidiaryAddresses.Find(id);
            if (customerssubsidiaryaddress == null)
            {
                return HttpNotFound();
            }
            return View(customerssubsidiaryaddress);
        }

        //
        // GET: /CustomersSubsidiaryAddressAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CustomersSubsidiaryAddressAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomersSubsidiaryAddress customerssubsidiaryaddress)
        {
            if (ModelState.IsValid)
            {
                db.CustomersSubsidiaryAddresses.Add(customerssubsidiaryaddress);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customerssubsidiaryaddress);
        }

        //
        // GET: /CustomersSubsidiaryAddressAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CustomersSubsidiaryAddress customerssubsidiaryaddress = db.CustomersSubsidiaryAddresses.Find(id);
            if (customerssubsidiaryaddress == null)
            {
                return HttpNotFound();
            }
            return View(customerssubsidiaryaddress);
        }

        //
        // POST: /CustomersSubsidiaryAddressAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomersSubsidiaryAddress customerssubsidiaryaddress)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerssubsidiaryaddress).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customerssubsidiaryaddress);
        }

        //
        // GET: /CustomersSubsidiaryAddressAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CustomersSubsidiaryAddress customerssubsidiaryaddress = db.CustomersSubsidiaryAddresses.Find(id);
            if (customerssubsidiaryaddress == null)
            {
                return HttpNotFound();
            }
            return View(customerssubsidiaryaddress);
        }

        //
        // POST: /CustomersSubsidiaryAddressAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomersSubsidiaryAddress customerssubsidiaryaddress = db.CustomersSubsidiaryAddresses.Find(id);
            db.CustomersSubsidiaryAddresses.Remove(customerssubsidiaryaddress);
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
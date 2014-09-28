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
    public class CustomersSalesContactAdminController : Controller
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
        // GET: /CustomersSalesContactAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<CustomersSalesContact> qryCustomersSalesContact = null;

            List<CustomersSalesContact> CustomersSalesContactList = new List<CustomersSalesContact>();

            qryCustomersSalesContact = db.CustomersSalesContacts.OrderBy(cut => cut.LastName).ThenBy(cut => cut.FirstName);
            if (qryCustomersSalesContact.Count() > 0)
            {
                foreach (var item in qryCustomersSalesContact)
                {
                    CustomersSalesContactList.Add(item);
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


            var onePageOfData = CustomersSalesContactList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(CustomersSalesContactList.ToPagedList(pageIndex, pageSize));
            //return View(db.CustomersSalesContacts.ToList());
        }

        //
        // GET: /CustomersSalesContactAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            CustomersSalesContact customerssalescontact = db.CustomersSalesContacts.Find(id);
            if (customerssalescontact == null)
            {
                return HttpNotFound();
            }
            return View(customerssalescontact);
        }

        //
        // GET: /CustomersSalesContactAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CustomersSalesContactAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomersSalesContact customerssalescontact)
        {
            if (ModelState.IsValid)
            {
                db.CustomersSalesContacts.Add(customerssalescontact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customerssalescontact);
        }

        //
        // GET: /CustomersSalesContactAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CustomersSalesContact customerssalescontact = db.CustomersSalesContacts.Find(id);
            if (customerssalescontact == null)
            {
                return HttpNotFound();
            }
            return View(customerssalescontact);
        }

        //
        // POST: /CustomersSalesContactAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomersSalesContact customerssalescontact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerssalescontact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customerssalescontact);
        }

        //
        // GET: /CustomersSalesContactAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CustomersSalesContact customerssalescontact = db.CustomersSalesContacts.Find(id);
            if (customerssalescontact == null)
            {
                return HttpNotFound();
            }
            return View(customerssalescontact);
        }

        //
        // POST: /CustomersSalesContactAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomersSalesContact customerssalescontact = db.CustomersSalesContacts.Find(id);
            db.CustomersSalesContacts.Remove(customerssalescontact);
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
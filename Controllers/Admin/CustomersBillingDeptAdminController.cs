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
    public class CustomersBillingDeptAdminController : Controller
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
        // GET: /CustomersBillingDeptAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<CustomersBillingDept> qryCustomersBillingDept = null;

            List<CustomersBillingDept> CustomersBillingDeptList = new List<CustomersBillingDept>();

            qryCustomersBillingDept = db.CustomersBillingDepts.OrderBy(cut => cut.CustomerId);
            if (qryCustomersBillingDept.Count() > 0)
            {
                foreach (var item in qryCustomersBillingDept)
                {
                    CustomersBillingDeptList.Add(item);
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


            var onePageOfData = CustomersBillingDeptList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(CustomersBillingDeptList.ToPagedList(pageIndex, pageSize));
            //return View(db.CustomersBillingDepts.ToList());
        }

        //
        // GET: /CustomersBillingDeptAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            CustomersBillingDept customersbillingdept = db.CustomersBillingDepts.Find(id);
            if (customersbillingdept == null)
            {
                return HttpNotFound();
            }
            return View(customersbillingdept);
        }

        //
        // GET: /CustomersBillingDeptAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CustomersBillingDeptAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomersBillingDept customersbillingdept)
        {
            if (ModelState.IsValid)
            {
                db.CustomersBillingDepts.Add(customersbillingdept);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customersbillingdept);
        }

        //
        // GET: /CustomersBillingDeptAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CustomersBillingDept customersbillingdept = db.CustomersBillingDepts.Find(id);
            if (customersbillingdept == null)
            {
                return HttpNotFound();
            }
            return View(customersbillingdept);
        }

        //
        // POST: /CustomersBillingDeptAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomersBillingDept customersbillingdept)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customersbillingdept).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customersbillingdept);
        }

        //
        // GET: /CustomersBillingDeptAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CustomersBillingDept customersbillingdept = db.CustomersBillingDepts.Find(id);
            if (customersbillingdept == null)
            {
                return HttpNotFound();
            }
            return View(customersbillingdept);
        }

        //
        // POST: /CustomersBillingDeptAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomersBillingDept customersbillingdept = db.CustomersBillingDepts.Find(id);
            db.CustomersBillingDepts.Remove(customersbillingdept);
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
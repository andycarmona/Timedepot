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
    public class InvoiceDetailAdminController : Controller
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
        // GET: /InvoiceDetailAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<InvoiceDetail> qryVendors = null;

            List<InvoiceDetail> VendorsList = new List<InvoiceDetail>();

            qryVendors = db.InvoiceDetails.OrderBy(vd => vd.InvoiceId);
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
            //return View(db.InvoiceDetails.ToList());
        }

        //
        // GET: /InvoiceDetailAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            InvoiceDetail invoicedetail = db.InvoiceDetails.Find(id);
            if (invoicedetail == null)
            {
                return HttpNotFound();
            }
            return View(invoicedetail);
        }

        //
        // GET: /InvoiceDetailAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /InvoiceDetailAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InvoiceDetail invoicedetail)
        {
            if (ModelState.IsValid)
            {
                db.InvoiceDetails.Add(invoicedetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(invoicedetail);
        }

        //
        // GET: /InvoiceDetailAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            InvoiceDetail invoicedetail = db.InvoiceDetails.Find(id);
            if (invoicedetail == null)
            {
                return HttpNotFound();
            }
            return View(invoicedetail);
        }

        //
        // POST: /InvoiceDetailAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InvoiceDetail invoicedetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoicedetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invoicedetail);
        }

        //
        // GET: /InvoiceDetailAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            InvoiceDetail invoicedetail = db.InvoiceDetails.Find(id);
            if (invoicedetail == null)
            {
                return HttpNotFound();
            }
            return View(invoicedetail);
        }

        //
        // POST: /InvoiceDetailAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InvoiceDetail invoicedetail = db.InvoiceDetails.Find(id);
            db.InvoiceDetails.Remove(invoicedetail);
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
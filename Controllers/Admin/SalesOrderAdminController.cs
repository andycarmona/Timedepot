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
    public class SalesOrderAdminController : Controller
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
        // GET: /SalesOrderAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<SalesOrder> qrySalesOrder = null;

            List<SalesOrder> SalesOrderList = new List<SalesOrder>();

            qrySalesOrder = from slod in db.SalesOrders
                            join cust in db.CustomersContactAddresses on slod.CustomerId equals cust.CustomerId
                            orderby cust.CompanyName
                            select slod;
            if (qrySalesOrder.Count() > 0)
            {
                foreach (var item in qrySalesOrder)
                {
                    SalesOrderList.Add(item);
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


            var onePageOfData = SalesOrderList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(SalesOrderList.ToPagedList(pageIndex, pageSize));
            //return View(db.SalesOrders.ToList());
        }

        //
        // GET: /SalesOrderAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            SalesOrder salesorder = db.SalesOrders.Find(id);
            if (salesorder == null)
            {
                return HttpNotFound();
            }
            return View(salesorder);
        }

        //
        // GET: /SalesOrderAdmin/Create

        public ActionResult Create()
        {
            SalesOrder salesorder = new SalesOrder();
            salesorder.SODate = DateTime.Now;
            salesorder.ShipDate = DateTime.Now;
            salesorder.PaymentDate = DateTime.Now;

            return View(salesorder);
        }

        //
        // POST: /SalesOrderAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SalesOrder salesorder)
        {
            if (ModelState.IsValid)
            {
                db.SalesOrders.Add(salesorder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(salesorder);
        }

        //
        // GET: /SalesOrderAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            SalesOrder salesorder = db.SalesOrders.Find(id);
            if (salesorder == null)
            {
                return HttpNotFound();
            }
            return View(salesorder);
        }

        //
        // POST: /SalesOrderAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SalesOrder salesorder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salesorder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(salesorder);
        }

        //
        // GET: /SalesOrderAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            SalesOrder salesorder = db.SalesOrders.Find(id);
            if (salesorder == null)
            {
                return HttpNotFound();
            }
            return View(salesorder);
        }

        //
        // POST: /SalesOrderAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Delete Sales Order Details
            DeleteSalesOrderDetails(id);

            SalesOrder salesorder = db.SalesOrders.Find(id);
            db.SalesOrders.Remove(salesorder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void DeleteSalesOrderDetails(int id)
        {

            TimelyDepotContext db01 = new TimelyDepotContext();
            //SalesOrderDetail salesorderdetails = null;

            IQueryable<SalesOrderDetail> qryalesorderdetails = db.SalesOrderDetails.Where(sodt => sodt.SalesOrderId == id);
            if (qryalesorderdetails.Count() > 0)
            {
                foreach (var item in qryalesorderdetails)
                {
                    db.SalesOrderDetails.Remove(item); 
                }
                db01.SaveChanges();
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
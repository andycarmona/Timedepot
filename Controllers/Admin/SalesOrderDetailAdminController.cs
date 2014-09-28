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
    public class SalesOrderDetailAdminController : Controller
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
        // GET: /SalesOrderDetailAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<SalesOrderDetail> qrySalesOrderDetail = null;

            List<SalesOrderDetail> SalesOrderDetailList = new List<SalesOrderDetail>();

            qrySalesOrderDetail = db.SalesOrderDetails.OrderBy(sodt => sodt.SalesOrderId);
            if (qrySalesOrderDetail.Count() > 0)
            {
                foreach (var item in qrySalesOrderDetail)
                {
                    SalesOrderDetailList.Add(item);
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


            var onePageOfData = SalesOrderDetailList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(SalesOrderDetailList.ToPagedList(pageIndex, pageSize));
            //return View(db.SalesOrderDetails.ToList());
        }

        //
        // GET: /SalesOrderDetailAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            SalesOrderDetail salesorderdetail = db.SalesOrderDetails.Find(id);
            if (salesorderdetail == null)
            {
                return HttpNotFound();
            }
            return View(salesorderdetail);
        }

        //
        // GET: /SalesOrderDetailAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SalesOrderDetailAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SalesOrderDetail salesorderdetail)
        {
            if (ModelState.IsValid)
            {
                db.SalesOrderDetails.Add(salesorderdetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(salesorderdetail);
        }

        //
        // GET: /SalesOrderDetailAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            SalesOrderDetail salesorderdetail = db.SalesOrderDetails.Find(id);
            if (salesorderdetail == null)
            {
                return HttpNotFound();
            }
            return View(salesorderdetail);
        }

        //
        // POST: /SalesOrderDetailAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SalesOrderDetail salesorderdetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salesorderdetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(salesorderdetail);
        }

        //
        // GET: /SalesOrderDetailAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            SalesOrderDetail salesorderdetail = db.SalesOrderDetails.Find(id);
            if (salesorderdetail == null)
            {
                return HttpNotFound();
            }
            return View(salesorderdetail);
        }

        //
        // POST: /SalesOrderDetailAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SalesOrderDetail salesorderdetail = db.SalesOrderDetails.Find(id);
            db.SalesOrderDetails.Remove(salesorderdetail);
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
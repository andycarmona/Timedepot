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
    public class SalesOrderBlindShipAdminController : Controller
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
        // GET: /SalesOrderBlindShipAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<SalesOrderBlindShip> qrySalesOrderBlindShip = null;

            List<SalesOrderBlindShip> SalesOrderBlindShipList = new List<SalesOrderBlindShip>();

            qrySalesOrderBlindShip = db.SalesOrderBlindShips.OrderBy(bld => bld.SalesOrderId);
            if (qrySalesOrderBlindShip.Count() > 0)
            {
                foreach (var item in qrySalesOrderBlindShip)
                {
                    SalesOrderBlindShipList.Add(item);
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


            var onePageOfData = SalesOrderBlindShipList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(SalesOrderBlindShipList.ToPagedList(pageIndex, pageSize));
            //return View(db.SalesOrderBlindShips.ToList());
        }

        //
        // GET: /SalesOrderBlindShipAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            SalesOrderBlindShip salesorderblindship = db.SalesOrderBlindShips.Find(id);
            if (salesorderblindship == null)
            {
                return HttpNotFound();
            }
            return View(salesorderblindship);
        }

        //
        // GET: /SalesOrderBlindShipAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SalesOrderBlindShipAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SalesOrderBlindShip salesorderblindship)
        {
            if (ModelState.IsValid)
            {
                db.SalesOrderBlindShips.Add(salesorderblindship);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(salesorderblindship);
        }

        //
        // GET: /SalesOrderBlindShipAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            SalesOrderBlindShip salesorderblindship = db.SalesOrderBlindShips.Find(id);
            if (salesorderblindship == null)
            {
                return HttpNotFound();
            }
            return View(salesorderblindship);
        }

        //
        // POST: /SalesOrderBlindShipAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SalesOrderBlindShip salesorderblindship)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salesorderblindship).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(salesorderblindship);
        }

        //
        // GET: /SalesOrderBlindShipAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            SalesOrderBlindShip salesorderblindship = db.SalesOrderBlindShips.Find(id);
            if (salesorderblindship == null)
            {
                return HttpNotFound();
            }
            return View(salesorderblindship);
        }

        //
        // POST: /SalesOrderBlindShipAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SalesOrderBlindShip salesorderblindship = db.SalesOrderBlindShips.Find(id);
            db.SalesOrderBlindShips.Remove(salesorderblindship);
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
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
    public class ShipmentDetailsAdminController : Controller
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
        // GET: /ShipmentDetailsAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<ShipmentDetails> qryShipmentDetails = null;

            List<ShipmentDetails> ShipmentDetailsList = new List<ShipmentDetails>();

            qryShipmentDetails = db.ShipmentDetails.OrderBy(vd => vd.ShipmentId).ThenBy(vd => vd.Sub_ItemID);
            if (qryShipmentDetails.Count() > 0)
            {
                foreach (var item in qryShipmentDetails)
                {
                    ShipmentDetailsList.Add(item);
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


            var onePageOfData = ShipmentDetailsList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(ShipmentDetailsList.ToPagedList(pageIndex, pageSize));
            //return View(db.ShipmentDetails.ToList());
        }

        //
        // GET: /ShipmentDetailsAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            ShipmentDetails shipmentdetails = db.ShipmentDetails.Find(id);
            if (shipmentdetails == null)
            {
                return HttpNotFound();
            }
            return View(shipmentdetails);
        }

        //
        // GET: /ShipmentDetailsAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ShipmentDetailsAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ShipmentDetails shipmentdetails)
        {
            if (ModelState.IsValid)
            {
                db.ShipmentDetails.Add(shipmentdetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shipmentdetails);
        }

        //
        // GET: /ShipmentDetailsAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ShipmentDetails shipmentdetails = db.ShipmentDetails.Find(id);
            if (shipmentdetails == null)
            {
                return HttpNotFound();
            }
            return View(shipmentdetails);
        }

        //
        // POST: /ShipmentDetailsAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ShipmentDetails shipmentdetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shipmentdetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shipmentdetails);
        }

        //
        // GET: /ShipmentDetailsAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ShipmentDetails shipmentdetails = db.ShipmentDetails.Find(id);
            if (shipmentdetails == null)
            {
                return HttpNotFound();
            }
            return View(shipmentdetails);
        }

        //
        // POST: /ShipmentDetailsAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShipmentDetails shipmentdetails = db.ShipmentDetails.Find(id);
            db.ShipmentDetails.Remove(shipmentdetails);
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
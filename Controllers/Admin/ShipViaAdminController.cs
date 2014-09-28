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
    public class ShipViaAdminController : Controller
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
        // GET: /ShipViaAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<ShipVia> qryShipVia = null;

            List<ShipVia> ShipViaList = new List<ShipVia>();

            qryShipVia = db.ShipVias.OrderBy(vd => vd.Description);
            if (qryShipVia.Count() > 0)
            {
                foreach (var item in qryShipVia)
                {
                    ShipViaList.Add(item);
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


            var onePageOfData = ShipViaList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(ShipViaList.ToPagedList(pageIndex, pageSize));
            return View(db.ShipVias.ToList());
        }

        //
        // GET: /ShipViaAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            ShipVia shipvia = db.ShipVias.Find(id);
            if (shipvia == null)
            {
                return HttpNotFound();
            }
            return View(shipvia);
        }

        //
        // GET: /ShipViaAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ShipViaAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ShipVia shipvia)
        {
            if (ModelState.IsValid)
            {
                db.ShipVias.Add(shipvia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shipvia);
        }

        //
        // GET: /ShipViaAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ShipVia shipvia = db.ShipVias.Find(id);
            if (shipvia == null)
            {
                return HttpNotFound();
            }
            return View(shipvia);
        }

        //
        // POST: /ShipViaAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ShipVia shipvia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shipvia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shipvia);
        }

        //
        // GET: /ShipViaAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ShipVia shipvia = db.ShipVias.Find(id);
            if (shipvia == null)
            {
                return HttpNotFound();
            }
            return View(shipvia);
        }

        //
        // POST: /ShipViaAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShipVia shipvia = db.ShipVias.Find(id);
            db.ShipVias.Remove(shipvia);
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
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
    public class Setup_for_PriceAdminController : Controller
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
        // GET: /Setup_for_PriceAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<Setup_for_Price> qryStPr = null;

            List<Setup_for_Price> setupPriceList = new List<Setup_for_Price>();

            qryStPr = db.Setup_for_Price.OrderBy(stpr => stpr.Item).ThenBy(stpr => stpr.Qty);
            if (qryStPr.Count() > 0)
            {
                foreach (var item in qryStPr)
                {
                    setupPriceList.Add(item);
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


            var onePageOfData = setupPriceList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(setupPriceList.ToPagedList(pageIndex, pageSize));
            //return View(db.Setup_for_Price.ToList());
        }

        //
        // GET: /Setup_for_PriceAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            Setup_for_Price setup_for_price = db.Setup_for_Price.Find(id);
            if (setup_for_price == null)
            {
                return HttpNotFound();
            }
            return View(setup_for_price);
        }

        //
        // GET: /Setup_for_PriceAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Setup_for_PriceAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Setup_for_Price setup_for_price)
        {
            if (ModelState.IsValid)
            {
                db.Setup_for_Price.Add(setup_for_price);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(setup_for_price);
        }

        //
        // GET: /Setup_for_PriceAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Setup_for_Price setup_for_price = db.Setup_for_Price.Find(id);
            if (setup_for_price == null)
            {
                return HttpNotFound();
            }
            return View(setup_for_price);
        }

        //
        // POST: /Setup_for_PriceAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Setup_for_Price setup_for_price)
        {
            if (ModelState.IsValid)
            {
                db.Entry(setup_for_price).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(setup_for_price);
        }

        //
        // GET: /Setup_for_PriceAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Setup_for_Price setup_for_price = db.Setup_for_Price.Find(id);
            if (setup_for_price == null)
            {
                return HttpNotFound();
            }
            return View(setup_for_price);
        }

        //
        // POST: /Setup_for_PriceAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Setup_for_Price setup_for_price = db.Setup_for_Price.Find(id);
            db.Setup_for_Price.Remove(setup_for_price);
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
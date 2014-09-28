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
    public class YearProductsAdminController : Controller
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
        // GET: /YearProductsAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<YearProducts> qryYearProducts = null;

            List<YearProducts> VendorsList = new List<YearProducts>();

            qryYearProducts = db.YearProducts.OrderBy(vd => vd.YearofProducts);
            if (qryYearProducts.Count() > 0)
            {
                foreach (var item in qryYearProducts)
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
            //return View(db.YearProducts.ToList());
        }

        //
        // GET: /YearProductsAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            YearProducts yearproducts = db.YearProducts.Find(id);
            if (yearproducts == null)
            {
                return HttpNotFound();
            }
            return View(yearproducts);
        }

        //
        // GET: /YearProductsAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /YearProductsAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(YearProducts yearproducts)
        {
            if (ModelState.IsValid)
            {
                db.YearProducts.Add(yearproducts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(yearproducts);
        }

        //
        // GET: /YearProductsAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            YearProducts yearproducts = db.YearProducts.Find(id);
            if (yearproducts == null)
            {
                return HttpNotFound();
            }
            return View(yearproducts);
        }

        //
        // POST: /YearProductsAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(YearProducts yearproducts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(yearproducts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(yearproducts);
        }

        //
        // GET: /YearProductsAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            YearProducts yearproducts = db.YearProducts.Find(id);
            if (yearproducts == null)
            {
                return HttpNotFound();
            }
            return View(yearproducts);
        }

        //
        // POST: /YearProductsAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            YearProducts yearproducts = db.YearProducts.Find(id);
            db.YearProducts.Remove(yearproducts);
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
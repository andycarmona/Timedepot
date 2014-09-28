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
    public class CustomersCardTypeAdminController : Controller
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
        // GET: /CustomersCardTypeAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<CustomersCardType> qryVendors = null;

            List<CustomersCardType> VendorsList = new List<CustomersCardType>();

            qryVendors = db.CustomersCardTypes.OrderBy(vd => vd.CardType);
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
            //return View(db.CustomersCardTypes.ToList());
        }

        //
        // GET: /CustomersCardTypeAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            CustomersCardType customerscardtype = db.CustomersCardTypes.Find(id);
            if (customerscardtype == null)
            {
                return HttpNotFound();
            }
            return View(customerscardtype);
        }

        //
        // GET: /CustomersCardTypeAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CustomersCardTypeAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomersCardType customerscardtype)
        {
            if (ModelState.IsValid)
            {
                db.CustomersCardTypes.Add(customerscardtype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customerscardtype);
        }

        //
        // GET: /CustomersCardTypeAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CustomersCardType customerscardtype = db.CustomersCardTypes.Find(id);
            if (customerscardtype == null)
            {
                return HttpNotFound();
            }
            return View(customerscardtype);
        }

        //
        // POST: /CustomersCardTypeAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomersCardType customerscardtype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerscardtype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customerscardtype);
        }

        //
        // GET: /CustomersCardTypeAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CustomersCardType customerscardtype = db.CustomersCardTypes.Find(id);
            if (customerscardtype == null)
            {
                return HttpNotFound();
            }
            return View(customerscardtype);
        }

        //
        // POST: /CustomersCardTypeAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomersCardType customerscardtype = db.CustomersCardTypes.Find(id);
            db.CustomersCardTypes.Remove(customerscardtype);
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
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
    public class ITEM_PACKAGEAdminController : Controller
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
        // GET: /ITEM_PACKAGEAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<ITEM_PACKAGE> qryIP = null;

            List<ITEM_PACKAGE> itempackageList = new List<ITEM_PACKAGE>();

            qryIP = db.ITEM_PACKAGE.OrderBy(ipk => ipk.Item);
            if (qryIP.Count() > 0)
            {
                foreach (var item in qryIP)
                {
                    itempackageList.Add(item);
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


            var onePageOfData = itempackageList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(itempackageList.ToPagedList(pageIndex, pageSize));
            //return View(db.ITEM_PACKAGE.ToList());
        }

        //
        // GET: /ITEM_PACKAGEAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            ITEM_PACKAGE item_package = db.ITEM_PACKAGE.Find(id);
            if (item_package == null)
            {
                return HttpNotFound();
            }
            return View(item_package);
        }

        //
        // GET: /ITEM_PACKAGEAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ITEM_PACKAGEAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ITEM_PACKAGE item_package)
        {
            if (ModelState.IsValid)
            {
                db.ITEM_PACKAGE.Add(item_package);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(item_package);
        }

        //
        // GET: /ITEM_PACKAGEAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ITEM_PACKAGE item_package = db.ITEM_PACKAGE.Find(id);
            if (item_package == null)
            {
                return HttpNotFound();
            }
            return View(item_package);
        }

        //
        // POST: /ITEM_PACKAGEAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ITEM_PACKAGE item_package)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item_package).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item_package);
        }

        //
        // GET: /ITEM_PACKAGEAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ITEM_PACKAGE item_package = db.ITEM_PACKAGE.Find(id);
            if (item_package == null)
            {
                return HttpNotFound();
            }
            return View(item_package);
        }

        //
        // POST: /ITEM_PACKAGEAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ITEM_PACKAGE item_package = db.ITEM_PACKAGE.Find(id);
            db.ITEM_PACKAGE.Remove(item_package);
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
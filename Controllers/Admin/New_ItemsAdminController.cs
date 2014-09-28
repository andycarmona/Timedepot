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
    public class New_ItemsAdminController : Controller
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
        // GET: /New_ItemsAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<New_Items> qryNI = null;

            List<New_Items> newitemsList = new List<New_Items>();

            qryNI = db.New_Items.OrderBy(ni => ni.ITEMID);
            if (qryNI.Count() > 0)
            {
                foreach (var item in qryNI)
                {
                    newitemsList.Add(item);
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


            var onePageOfData = newitemsList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(newitemsList.ToPagedList(pageIndex, pageSize));
            //return View(db.New_Items.ToList());
        }

        //
        // GET: /New_ItemsAdmin/Details/5

        public ActionResult Details(string id = null)
        {
            New_Items new_items = db.New_Items.Find(id);
            if (new_items == null)
            {
                return HttpNotFound();
            }
            return View(new_items);
        }

        //
        // GET: /New_ItemsAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /New_ItemsAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(New_Items new_items)
        {
            if (ModelState.IsValid)
            {
                db.New_Items.Add(new_items);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(new_items);
        }

        //
        // GET: /New_ItemsAdmin/Edit/5

        public ActionResult Edit(string id = null)
        {
            New_Items new_items = db.New_Items.Find(id);
            if (new_items == null)
            {
                return HttpNotFound();
            }
            return View(new_items);
        }

        //
        // POST: /New_ItemsAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(New_Items new_items)
        {
            if (ModelState.IsValid)
            {
                db.Entry(new_items).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(new_items);
        }

        //
        // GET: /New_ItemsAdmin/Delete/5

        public ActionResult Delete(string id = null)
        {
            New_Items new_items = db.New_Items.Find(id);
            if (new_items == null)
            {
                return HttpNotFound();
            }
            return View(new_items);
        }

        //
        // POST: /New_ItemsAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            New_Items new_items = db.New_Items.Find(id);
            db.New_Items.Remove(new_items);
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
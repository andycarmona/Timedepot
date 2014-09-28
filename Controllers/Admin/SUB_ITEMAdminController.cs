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
    public class SUB_ITEMAdminController : Controller
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
        // GET: /SUB_ITEMAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<SUB_ITEM> qrySUB_ITEM = null;

            List<SUB_ITEM> SUB_ITEMList = new List<SUB_ITEM>();

            qrySUB_ITEM = db.SUB_ITEM.OrderBy(sbi => sbi.ItemID);
            if (qrySUB_ITEM.Count() > 0)
            {
                foreach (var item in qrySUB_ITEM)
                {
                    SUB_ITEMList.Add(item);
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


            var onePageOfData = SUB_ITEMList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(SUB_ITEMList.ToPagedList(pageIndex, pageSize));
            //return View(db.SUB_ITEM.ToList());
        }

        //
        // GET: /SUB_ITEMAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            SUB_ITEM sub_item = db.SUB_ITEM.Find(id);
            if (sub_item == null)
            {
                return HttpNotFound();
            }
            return View(sub_item);
        }

        //
        // GET: /SUB_ITEMAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SUB_ITEMAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SUB_ITEM sub_item)
        {
            if (ModelState.IsValid)
            {
                db.SUB_ITEM.Add(sub_item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sub_item);
        }

        //
        // GET: /SUB_ITEMAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            SUB_ITEM sub_item = db.SUB_ITEM.Find(id);
            if (sub_item == null)
            {
                return HttpNotFound();
            }
            return View(sub_item);
        }

        //
        // POST: /SUB_ITEMAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SUB_ITEM sub_item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sub_item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sub_item);
        }

        //
        // GET: /SUB_ITEMAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            SUB_ITEM sub_item = db.SUB_ITEM.Find(id);
            if (sub_item == null)
            {
                return HttpNotFound();
            }
            return View(sub_item);
        }

        //
        // POST: /SUB_ITEMAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SUB_ITEM sub_item = db.SUB_ITEM.Find(id);
            db.SUB_ITEM.Remove(sub_item);
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
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
    public class ITEMAdminController : Controller
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
        // GET: /ITEMAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<ITEM> qryITEM = null;

            List<ITEM> VITEMList = new List<ITEM>();

            qryITEM = db.ITEMs.OrderBy(it => it.ItemID);
            if (qryITEM.Count() > 0)
            {
                foreach (var item in qryITEM)
                {
                    VITEMList.Add(item);
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


            var onePageOfData = VITEMList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(VITEMList.ToPagedList(pageIndex, pageSize));
            //return View(db.ITEMs.ToList());
        }

        //
        // GET: /ITEMAdmin/Details/5

        public ActionResult Details(string id = null)
        {
            ITEM item = db.ITEMs.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        //
        // GET: /ITEMAdmin/Create

        public ActionResult Create()
        {
            ITEM item = new ITEM();
            item.Status = false;
            
            return View(item);
        }

        //
        // POST: /ITEMAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(ITEM item)
        {
            if (ModelState.IsValid)
            {
                db.ITEMs.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(item);
        }

        //
        // GET: /ITEMAdmin/Edit/5

        public ActionResult Edit(string id = null)
        {
            ITEM item = db.ITEMs.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        //
        // POST: /ITEMAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ITEM item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        //
        // GET: /ITEMAdmin/Delete/5

        public ActionResult Delete(string id = null)
        {
            ITEM item = db.ITEMs.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        //
        // POST: /ITEMAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ITEM item = db.ITEMs.Find(id);
            db.ITEMs.Remove(item);
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
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
    public class DFAdminController : Controller
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
        // GET: /DFAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<DF> qryDF = null;

            List<DF> DFList = new List<DF>();

            qryDF = db.DFs.OrderBy(df => df.ItemID);
            if (qryDF.Count() > 0)
            {
                foreach (var item in qryDF)
                {
                    DFList.Add(item);
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


            var onePageOfData = DFList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(DFList.ToPagedList(pageIndex, pageSize));
            //return View(db.DFs.ToList());
        }

        //
        // GET: /DFAdmin/Details/5

        public ActionResult Details(string id = null)
        {
            DF df = db.DFs.Find(id);
            if (df == null)
            {
                return HttpNotFound();
            }
            return View(df);
        }

        //
        // GET: /DFAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /DFAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DF df)
        {
            if (ModelState.IsValid)
            {
                db.DFs.Add(df);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(df);
        }

        //
        // GET: /DFAdmin/Edit/5

        public ActionResult Edit(string id = null)
        {
            DF df = db.DFs.Find(id);
            if (df == null)
            {
                return HttpNotFound();
            }
            return View(df);
        }

        //
        // POST: /DFAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DF df)
        {
            if (ModelState.IsValid)
            {
                db.Entry(df).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(df);
        }

        //
        // GET: /DFAdmin/Delete/5

        public ActionResult Delete(string id = null)
        {
            DF df = db.DFs.Find(id);
            if (df == null)
            {
                return HttpNotFound();
            }
            return View(df);
        }

        //
        // POST: /DFAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DF df = db.DFs.Find(id);
            db.DFs.Remove(df);
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
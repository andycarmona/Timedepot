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
    public class DIALAdminController : Controller
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
        // GET: /DIALAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<DIAL> qryDial = null;

            List<DIAL> DIALList = new List<DIAL>();

            qryDial = db.DIALs.OrderBy(dl => dl.DialID);
            if (qryDial.Count() > 0)
            {
                foreach (var item in qryDial)
                {
                    DIALList.Add(item);
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


            var onePageOfData = DIALList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(DIALList.ToPagedList(pageIndex, pageSize));
            //return View(db.DIALs.ToList());
        }

        //
        // GET: /DIALAdmin/Details/5

        public ActionResult Details(string id = null)
        {
            DIAL dial = db.DIALs.Find(id);
            if (dial == null)
            {
                return HttpNotFound();
            }
            return View(dial);
        }

        //
        // GET: /DIALAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /DIALAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DIAL dial)
        {
            if (ModelState.IsValid)
            {
                db.DIALs.Add(dial);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dial);
        }

        //
        // GET: /DIALAdmin/Edit/5

        public ActionResult Edit(string id = null)
        {
            DIAL dial = db.DIALs.Find(id);
            if (dial == null)
            {
                return HttpNotFound();
            }
            return View(dial);
        }

        //
        // POST: /DIALAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DIAL dial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dial);
        }

        //
        // GET: /DIALAdmin/Delete/5

        public ActionResult Delete(string id = null)
        {
            DIAL dial = db.DIALs.Find(id);
            if (dial == null)
            {
                return HttpNotFound();
            }
            return View(dial);
        }

        //
        // POST: /DIALAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DIAL dial = db.DIALs.Find(id);
            db.DIALs.Remove(dial);
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
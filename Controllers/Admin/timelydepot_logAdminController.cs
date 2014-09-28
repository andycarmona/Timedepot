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
    public class timelydepot_logAdminController : Controller
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
        // GET: /timelydepot_logAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<timelydepot_log> qryTime = null;

            List<timelydepot_log> timelydepotlogList = new List<timelydepot_log>();

            qryTime = db.timelydepot_log.OrderBy(tmp => tmp.logTime);
            if (qryTime.Count() > 0)
            {
                foreach (var item in qryTime)
                {
                    timelydepotlogList.Add(item);
                }
            }

            //Poner los datos aqui

            //Set the page
            if (page == null)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = Convert.ToInt32(page);
            }


            var onePageOfData = timelydepotlogList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(timelydepotlogList.ToPagedList(pageIndex, pageSize));
            //return View(db.timelydepot_log.ToList());
        }

        //
        // GET: /timelydepot_logAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            timelydepot_log timelydepot_log = db.timelydepot_log.Find(id);
            if (timelydepot_log == null)
            {
                return HttpNotFound();
            }
            return View(timelydepot_log);
        }

        //
        // GET: /timelydepot_logAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /timelydepot_logAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(timelydepot_log timelydepot_log)
        {
            if (ModelState.IsValid)
            {
                db.timelydepot_log.Add(timelydepot_log);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(timelydepot_log);
        }

        //
        // GET: /timelydepot_logAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            timelydepot_log timelydepot_log = db.timelydepot_log.Find(id);
            if (timelydepot_log == null)
            {
                return HttpNotFound();
            }
            return View(timelydepot_log);
        }

        //
        // POST: /timelydepot_logAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(timelydepot_log timelydepot_log)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timelydepot_log).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(timelydepot_log);
        }

        //
        // GET: /timelydepot_logAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            timelydepot_log timelydepot_log = db.timelydepot_log.Find(id);
            if (timelydepot_log == null)
            {
                return HttpNotFound();
            }
            return View(timelydepot_log);
        }

        //
        // POST: /timelydepot_logAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            timelydepot_log timelydepot_log = db.timelydepot_log.Find(id);
            db.timelydepot_log.Remove(timelydepot_log);
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
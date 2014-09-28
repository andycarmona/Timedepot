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
    public class EventLogAdminController : Controller
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
        // GET: /EventLogAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            DateTime dDate = new DateTime(2012, 12, 31, 0, 0, 0, 0);

            IQueryable<EventLog> qryEL = null;

            List<EventLog> EventLogList = new List<EventLog>();

            qryEL = db.EventLogs.Where(el => el.EventTime > dDate);

            if (qryEL.Count() > 0)
            {
                foreach (var item in qryEL)
                {
                    EventLogList.Add(item);
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


            var onePageOfData = EventLogList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(EventLogList.ToPagedList(pageIndex, pageSize));
            //return View(db.EventLogs.ToList());
        }

        //
        // GET: /EventLogAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            EventLog eventlog = db.EventLogs.Find(id);
            if (eventlog == null)
            {
                return HttpNotFound();
            }
            return View(eventlog);
        }

        //
        // GET: /EventLogAdmin/Create

        public ActionResult Create()
        {
            DateTime dDate = DateTime.Now;

            EventLog eventlog = new EventLog();
            eventlog.EventTime = dDate;

            return View(eventlog);
        }

        //
        // POST: /EventLogAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventLog eventlog)
        {
            if (ModelState.IsValid)
            {
                db.EventLogs.Add(eventlog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eventlog);
        }

        //
        // GET: /EventLogAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            EventLog eventlog = db.EventLogs.Find(id);
            if (eventlog == null)
            {
                return HttpNotFound();
            }
            return View(eventlog);
        }

        //
        // POST: /EventLogAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventLog eventlog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventlog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eventlog);
        }

        //
        // GET: /EventLogAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            EventLog eventlog = db.EventLogs.Find(id);
            if (eventlog == null)
            {
                return HttpNotFound();
            }
            return View(eventlog);
        }

        //
        // POST: /EventLogAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventLog eventlog = db.EventLogs.Find(id);
            db.EventLogs.Remove(eventlog);
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
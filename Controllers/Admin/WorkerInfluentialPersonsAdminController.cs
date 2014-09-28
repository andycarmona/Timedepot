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
    public class WorkerInfluentialPersonsAdminController : Controller
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
        // GET: /WorkerInfluentialPersonsAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;

            IQueryable<WorkerInfluentialPersons> qryWIP = null;
            List<WorkerInfluentialPersons> workerinfluentialpersonsList = new List<WorkerInfluentialPersons>();

            //Get the data
            qryWIP = db.WorkerInfluentialPersons.OrderBy(wip => wip.influentailPersonID);
            if (qryWIP.Count() > 0)
            {
                foreach (var item in qryWIP)
                {
                    workerinfluentialpersonsList.Add(item);
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


            var onePageOfData = workerinfluentialpersonsList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(workerinfluentialpersonsList.ToPagedList(pageIndex, pageSize));
            //return View(db.WorkerInfluentialPersons.ToList());
        }

        //
        // GET: /WorkerInfluentialPersonsAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            WorkerInfluentialPersons workerinfluentialpersons = db.WorkerInfluentialPersons.Find(id);
            if (workerinfluentialpersons == null)
            {
                return HttpNotFound();
            }
            return View(workerinfluentialpersons);
        }

        //
        // GET: /WorkerInfluentialPersonsAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /WorkerInfluentialPersonsAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WorkerInfluentialPersons workerinfluentialpersons)
        {
            if (ModelState.IsValid)
            {
                db.WorkerInfluentialPersons.Add(workerinfluentialpersons);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(workerinfluentialpersons);
        }

        //
        // GET: /WorkerInfluentialPersonsAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            WorkerInfluentialPersons workerinfluentialpersons = db.WorkerInfluentialPersons.Find(id);
            if (workerinfluentialpersons == null)
            {
                return HttpNotFound();
            }
            return View(workerinfluentialpersons);
        }

        //
        // POST: /WorkerInfluentialPersonsAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WorkerInfluentialPersons workerinfluentialpersons)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workerinfluentialpersons).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(workerinfluentialpersons);
        }

        //
        // GET: /WorkerInfluentialPersonsAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            WorkerInfluentialPersons workerinfluentialpersons = db.WorkerInfluentialPersons.Find(id);
            if (workerinfluentialpersons == null)
            {
                return HttpNotFound();
            }
            return View(workerinfluentialpersons);
        }

        //
        // POST: /WorkerInfluentialPersonsAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WorkerInfluentialPersons workerinfluentialpersons = db.WorkerInfluentialPersons.Find(id);
            db.WorkerInfluentialPersons.Remove(workerinfluentialpersons);
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
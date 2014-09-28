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
    public class BussinesAdminController : Controller
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
        // GET: /BussinesAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<Bussines> qryBussines = null;

            List<Bussines> BussinesList = new List<Bussines>();

            qryBussines = db.Bussines.OrderBy(bss => bss.BussinesType);
            if (qryBussines.Count() > 0)
            {
                foreach (var item in qryBussines)
                {
                    BussinesList.Add(item);
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


            var onePageOfData = BussinesList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(BussinesList.ToPagedList(pageIndex, pageSize));
            //return View(db.Bussines.ToList());
        }

        //
        // GET: /BussinesAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            Bussines bussines = db.Bussines.Find(id);
            if (bussines == null)
            {
                return HttpNotFound();
            }
            return View(bussines);
        }

        //
        // GET: /BussinesAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BussinesAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Bussines bussines)
        {
            if (ModelState.IsValid)
            {
                db.Bussines.Add(bussines);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bussines);
        }

        //
        // GET: /BussinesAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Bussines bussines = db.Bussines.Find(id);
            if (bussines == null)
            {
                return HttpNotFound();
            }
            return View(bussines);
        }

        //
        // POST: /BussinesAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Bussines bussines)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bussines).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bussines);
        }

        //
        // GET: /BussinesAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Bussines bussines = db.Bussines.Find(id);
            if (bussines == null)
            {
                return HttpNotFound();
            }
            return View(bussines);
        }

        //
        // POST: /BussinesAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bussines bussines = db.Bussines.Find(id);
            db.Bussines.Remove(bussines);
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
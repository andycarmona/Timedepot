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
    public class DeptosAdminController : Controller
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
        // GET: /DeptosAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<Deptos> qryDeptos = null;

            List<Deptos> DeptosList = new List<Deptos>();

            qryDeptos = db.Deptos.OrderBy(dpt => dpt.Name);
            if (qryDeptos.Count() > 0)
            {
                foreach (var item in qryDeptos)
                {
                    DeptosList.Add(item);
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


            var onePageOfData = DeptosList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(DeptosList.ToPagedList(pageIndex, pageSize));
            //return View(db.Deptos.ToList());
        }

        //
        // GET: /DeptosAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            Deptos deptos = db.Deptos.Find(id);
            if (deptos == null)
            {
                return HttpNotFound();
            }
            return View(deptos);
        }

        //
        // GET: /DeptosAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /DeptosAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Deptos deptos)
        {
            if (ModelState.IsValid)
            {
                db.Deptos.Add(deptos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(deptos);
        }

        //
        // GET: /DeptosAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Deptos deptos = db.Deptos.Find(id);
            if (deptos == null)
            {
                return HttpNotFound();
            }
            return View(deptos);
        }

        //
        // POST: /DeptosAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Deptos deptos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deptos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deptos);
        }

        //
        // GET: /DeptosAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Deptos deptos = db.Deptos.Find(id);
            if (deptos == null)
            {
                return HttpNotFound();
            }
            return View(deptos);
        }

        //
        // POST: /DeptosAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Deptos deptos = db.Deptos.Find(id);
            db.Deptos.Remove(deptos);
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
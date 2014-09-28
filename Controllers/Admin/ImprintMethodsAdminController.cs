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
    public class ImprintMethodsAdminController : Controller
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
        // GET: /ImprintMethodsAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<ImprintMethods> qryImprintMethods = null;

            List<ImprintMethods> ImprintMethodsList = new List<ImprintMethods>();

            qryImprintMethods = db.ImprintMethods.OrderBy(vd => vd.Description);
            if (qryImprintMethods.Count() > 0)
            {
                foreach (var item in qryImprintMethods)
                {
                    ImprintMethodsList.Add(item);
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


            var onePageOfData = ImprintMethodsList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(ImprintMethodsList.ToPagedList(pageIndex, pageSize));
            //return View(db.ImprintMethods.ToList());
        }

        //
        // GET: /ImprintMethodsAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            ImprintMethods imprintmethods = db.ImprintMethods.Find(id);
            if (imprintmethods == null)
            {
                return HttpNotFound();
            }
            return View(imprintmethods);
        }

        //
        // GET: /ImprintMethodsAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ImprintMethodsAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ImprintMethods imprintmethods)
        {
            if (ModelState.IsValid)
            {
                db.ImprintMethods.Add(imprintmethods);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(imprintmethods);
        }

        //
        // GET: /ImprintMethodsAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ImprintMethods imprintmethods = db.ImprintMethods.Find(id);
            if (imprintmethods == null)
            {
                return HttpNotFound();
            }
            return View(imprintmethods);
        }

        //
        // POST: /ImprintMethodsAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ImprintMethods imprintmethods)
        {
            if (ModelState.IsValid)
            {
                db.Entry(imprintmethods).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(imprintmethods);
        }

        //
        // GET: /ImprintMethodsAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ImprintMethods imprintmethods = db.ImprintMethods.Find(id);
            if (imprintmethods == null)
            {
                return HttpNotFound();
            }
            return View(imprintmethods);
        }

        //
        // POST: /ImprintMethodsAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ImprintMethods imprintmethods = db.ImprintMethods.Find(id);
            db.ImprintMethods.Remove(imprintmethods);
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
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
    public class UserQuotationAdminController : Controller
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
        // GET: /UserQuotationAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;

            IQueryable<UserQuotation> qryUsrQut = null;

            List<UserQuotation> userquotationList = new List<UserQuotation>();

            //Get the data
            qryUsrQut = db.UserQuotations;
            if (qryUsrQut.Count() > 0)
            {
                foreach (var item in qryUsrQut)
                {
                    userquotationList.Add(item);
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

            var onePageOfData = userquotationList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(userquotationList.ToPagedList(pageIndex, pageSize));
            //return View(db.UserQuotations.ToList());
        }

        //
        // GET: /UserQuotationAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            UserQuotation userquotation = db.UserQuotations.Find(id);
            if (userquotation == null)
            {
                return HttpNotFound();
            }
            return View(userquotation);
        }

        //
        // GET: /UserQuotationAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /UserQuotationAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserQuotation userquotation)
        {
            if (ModelState.IsValid)
            {
                db.UserQuotations.Add(userquotation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userquotation);
        }

        //
        // GET: /UserQuotationAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            UserQuotation userquotation = db.UserQuotations.Find(id);
            if (userquotation == null)
            {
                return HttpNotFound();
            }
            return View(userquotation);
        }

        //
        // POST: /UserQuotationAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserQuotation userquotation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userquotation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userquotation);
        }

        //
        // GET: /UserQuotationAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UserQuotation userquotation = db.UserQuotations.Find(id);
            if (userquotation == null)
            {
                return HttpNotFound();
            }
            return View(userquotation);
        }

        //
        // POST: /UserQuotationAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserQuotation userquotation = db.UserQuotations.Find(id);
            db.UserQuotations.Remove(userquotation);
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
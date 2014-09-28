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
    public class TermsAdminController : Controller
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
        // GET: /TermsAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<Terms> qryTerms = null;

            List<Terms> TermsList = new List<Terms>();

            qryTerms = db.Terms.OrderBy(trm => trm.Term);
            if (qryTerms.Count() > 0)
            {
                foreach (var item in qryTerms)
                {
                    TermsList.Add(item);
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


            var onePageOfData = TermsList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(TermsList.ToPagedList(pageIndex, pageSize));
            //return View(db.Terms.ToList());
        }

        //
        // GET: /TermsAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            Terms terms = db.Terms.Find(id);
            if (terms == null)
            {
                return HttpNotFound();
            }
            return View(terms);
        }

        //
        // GET: /TermsAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /TermsAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Terms terms)
        {
            if (ModelState.IsValid)
            {
                db.Terms.Add(terms);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(terms);
        }

        //
        // GET: /TermsAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Terms terms = db.Terms.Find(id);
            if (terms == null)
            {
                return HttpNotFound();
            }
            return View(terms);
        }

        //
        // POST: /TermsAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Terms terms)
        {
            if (ModelState.IsValid)
            {
                db.Entry(terms).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(terms);
        }

        //
        // GET: /TermsAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Terms terms = db.Terms.Find(id);
            if (terms == null)
            {
                return HttpNotFound();
            }
            return View(terms);
        }

        //
        // POST: /TermsAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Terms terms = db.Terms.Find(id);
            db.Terms.Remove(terms);
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
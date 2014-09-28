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
    public class ImprintitemdetailADMINController : Controller
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
        // GET: /ImprintitemdetailADMIN/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<Imprintitemdetail> qryIpDt = null;

            List<Imprintitemdetail> ImprintitemdetailList = new List<Imprintitemdetail>();

            qryIpDt = db.Imprintitemdetails.OrderBy(ipdt => ipdt.printId);
            if (qryIpDt.Count() > 0)
            {
                foreach (var item in qryIpDt)
                {
                    ImprintitemdetailList.Add(item);
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


            var onePageOfData = ImprintitemdetailList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(ImprintitemdetailList.ToPagedList(pageIndex, pageSize));
            //return View(db.Imprintitemdetails.ToList());
        }

        //
        // GET: /ImprintitemdetailADMIN/Details/5

        public ActionResult Details(int id = 0)
        {
            Imprintitemdetail imprintitemdetail = db.Imprintitemdetails.Find(id);
            if (imprintitemdetail == null)
            {
                return HttpNotFound();
            }
            return View(imprintitemdetail);
        }

        //
        // GET: /ImprintitemdetailADMIN/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ImprintitemdetailADMIN/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Imprintitemdetail imprintitemdetail)
        {
            if (ModelState.IsValid)
            {
                db.Imprintitemdetails.Add(imprintitemdetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(imprintitemdetail);
        }

        //
        // GET: /ImprintitemdetailADMIN/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Imprintitemdetail imprintitemdetail = db.Imprintitemdetails.Find(id);
            if (imprintitemdetail == null)
            {
                return HttpNotFound();
            }
            return View(imprintitemdetail);
        }

        //
        // POST: /ImprintitemdetailADMIN/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Imprintitemdetail imprintitemdetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(imprintitemdetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(imprintitemdetail);
        }

        //
        // GET: /ImprintitemdetailADMIN/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Imprintitemdetail imprintitemdetail = db.Imprintitemdetails.Find(id);
            if (imprintitemdetail == null)
            {
                return HttpNotFound();
            }
            return View(imprintitemdetail);
        }

        //
        // POST: /ImprintitemdetailADMIN/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Imprintitemdetail imprintitemdetail = db.Imprintitemdetails.Find(id);
            db.Imprintitemdetails.Remove(imprintitemdetail);
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
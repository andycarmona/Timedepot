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
    public class InformationDetailAdminController : Controller
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
        // GET: /InformationDetailAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<InformationDetail> qryIfDt = null;

            List<InformationDetail> InformationDetailList = new List<InformationDetail>();

            qryIfDt = db.InformationDetails.OrderBy(ifdt => ifdt.ItemId);
            if (qryIfDt.Count() > 0)
            {
                foreach (var item in qryIfDt)
                {
                    InformationDetailList.Add(item);
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


            var onePageOfData = InformationDetailList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(InformationDetailList.ToPagedList(pageIndex, pageSize));
            //return View(db.InformationDetails.ToList());
        }

        //
        // GET: /InformationDetailAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            InformationDetail informationdetail = db.InformationDetails.Find(id);
            if (informationdetail == null)
            {
                return HttpNotFound();
            }
            return View(informationdetail);
        }

        //
        // GET: /InformationDetailAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /InformationDetailAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InformationDetail informationdetail)
        {
            if (ModelState.IsValid)
            {
                db.InformationDetails.Add(informationdetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(informationdetail);
        }

        //
        // GET: /InformationDetailAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            InformationDetail informationdetail = db.InformationDetails.Find(id);
            if (informationdetail == null)
            {
                return HttpNotFound();
            }
            return View(informationdetail);
        }

        //
        // POST: /InformationDetailAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InformationDetail informationdetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(informationdetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(informationdetail);
        }

        //
        // GET: /InformationDetailAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            InformationDetail informationdetail = db.InformationDetails.Find(id);
            if (informationdetail == null)
            {
                return HttpNotFound();
            }
            return View(informationdetail);
        }

        //
        // POST: /InformationDetailAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InformationDetail informationdetail = db.InformationDetails.Find(id);
            db.InformationDetails.Remove(informationdetail);
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
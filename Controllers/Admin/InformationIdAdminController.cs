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
    public class InformationIdAdminController : Controller
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
        // GET: /InformationIdAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<InformationId> qryInf = null;

            List<InformationId> InformationIdList = new List<InformationId>();

            qryInf = db.InformationIds.OrderBy(ing => ing.InformationId1);
            if (qryInf.Count() > 0)
            {
                foreach (var item in qryInf)
                {
                    InformationIdList.Add(item);
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


            var onePageOfData = InformationIdList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(InformationIdList.ToPagedList(pageIndex, pageSize));
            //return View(db.InformationIds.ToList());
        }

        //
        // GET: /InformationIdAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            InformationId informationid = db.InformationIds.Find(id);
            if (informationid == null)
            {
                return HttpNotFound();
            }
            return View(informationid);
        }

        //
        // GET: /InformationIdAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /InformationIdAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(InformationId informationid)
        {
            if (ModelState.IsValid)
            {
                db.InformationIds.Add(informationid);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(informationid);
        }

        //
        // GET: /InformationIdAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            InformationId informationid = db.InformationIds.Find(id);
            if (informationid == null)
            {
                return HttpNotFound();
            }
            return View(informationid);
        }

        //
        // POST: /InformationIdAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(InformationId informationid)
        {
            if (ModelState.IsValid)
            {
                db.Entry(informationid).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(informationid);
        }

        //
        // GET: /InformationIdAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            InformationId informationid = db.InformationIds.Find(id);
            if (informationid == null)
            {
                return HttpNotFound();
            }
            return View(informationid);
        }

        //
        // POST: /InformationIdAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InformationId informationid = db.InformationIds.Find(id);
            db.InformationIds.Remove(informationid);
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
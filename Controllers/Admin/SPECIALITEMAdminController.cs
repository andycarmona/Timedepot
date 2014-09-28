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
    public class SPECIALITEMAdminController : Controller
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
        // GET: /SPECIALITEMAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<SPECIALITEM> qrySpecial = null;

            List<SPECIALITEM> specialitemList = new List<SPECIALITEM>();

            qrySpecial = db.SPECIALITEMs.OrderBy(spc => spc.ItemID);
            if (qrySpecial.Count() > 0)
            {
                foreach (var item in qrySpecial)
                {
                    specialitemList.Add(item);
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


            var onePageOfData = specialitemList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(specialitemList.ToPagedList(pageIndex, pageSize));
            //return View(db.SPECIALITEMs.ToList());
        }

        //
        // GET: /SPECIALITEMAdmin/Details/5

        public ActionResult Details(string id = null)
        {
            SPECIALITEM specialitem = db.SPECIALITEMs.Find(id);
            if (specialitem == null)
            {
                return HttpNotFound();
            }
            return View(specialitem);
        }

        //
        // GET: /SPECIALITEMAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SPECIALITEMAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SPECIALITEM specialitem)
        {
            if (ModelState.IsValid)
            {
                db.SPECIALITEMs.Add(specialitem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(specialitem);
        }

        //
        // GET: /SPECIALITEMAdmin/Edit/5

        public ActionResult Edit(string id = null)
        {
            SPECIALITEM specialitem = db.SPECIALITEMs.Find(id);
            if (specialitem == null)
            {
                return HttpNotFound();
            }
            return View(specialitem);
        }

        //
        // POST: /SPECIALITEMAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SPECIALITEM specialitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(specialitem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(specialitem);
        }

        //
        // GET: /SPECIALITEMAdmin/Delete/5

        public ActionResult Delete(string id = null)
        {
            SPECIALITEM specialitem = db.SPECIALITEMs.Find(id);
            if (specialitem == null)
            {
                return HttpNotFound();
            }
            return View(specialitem);
        }

        //
        // POST: /SPECIALITEMAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SPECIALITEM specialitem = db.SPECIALITEMs.Find(id);
            db.SPECIALITEMs.Remove(specialitem);
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
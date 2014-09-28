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
    public class packingitemdetailAdminController : Controller
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
        // GET: /packingitemdetailAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<packingitemdetail> qrPkd = null;

            List<packingitemdetail> packigdetaiList = new List<packingitemdetail>();

            qrPkd = db.packingitemdetails.OrderBy(pkd => pkd.itemId);
            if (qrPkd.Count() > 0)
            {
                foreach (var item in qrPkd)
                {
                    packigdetaiList.Add(item);
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


            var onePageOfData = packigdetaiList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(packigdetaiList.ToPagedList(pageIndex, pageSize));
            //return View(db.packingitemdetails.ToList());
        }

        //
        // GET: /packingitemdetailAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            packingitemdetail packingitemdetail = db.packingitemdetails.Find(id);
            if (packingitemdetail == null)
            {
                return HttpNotFound();
            }
            return View(packingitemdetail);
        }

        //
        // GET: /packingitemdetailAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /packingitemdetailAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(packingitemdetail packingitemdetail)
        {
            if (ModelState.IsValid)
            {
                db.packingitemdetails.Add(packingitemdetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(packingitemdetail);
        }

        //
        // GET: /packingitemdetailAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            packingitemdetail packingitemdetail = db.packingitemdetails.Find(id);
            if (packingitemdetail == null)
            {
                return HttpNotFound();
            }
            return View(packingitemdetail);
        }

        //
        // POST: /packingitemdetailAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(packingitemdetail packingitemdetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(packingitemdetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(packingitemdetail);
        }

        //
        // GET: /packingitemdetailAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            packingitemdetail packingitemdetail = db.packingitemdetails.Find(id);
            if (packingitemdetail == null)
            {
                return HttpNotFound();
            }
            return View(packingitemdetail);
        }

        //
        // POST: /packingitemdetailAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            packingitemdetail packingitemdetail = db.packingitemdetails.Find(id);
            db.packingitemdetails.Remove(packingitemdetail);
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
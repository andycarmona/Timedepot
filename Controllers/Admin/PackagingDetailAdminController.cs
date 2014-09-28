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
    public class PackagingDetailAdminController : Controller
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
        // GET: /PackagingDetailAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<PackagingDetail> qryPkDet = null;

            List<PackagingDetail> packingDetailList = new List<PackagingDetail>();

            qryPkDet = db.PackagingDetails.OrderBy(pkdet => pkdet.ItemNo);
            if (qryPkDet.Count() > 0)
            {
                foreach (var item in qryPkDet)
                {
                    packingDetailList.Add(item);
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


            var onePageOfData = packingDetailList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(packingDetailList.ToPagedList(pageIndex, pageSize));
            //return View(db.PackagingDetails.ToList());
        }

        //
        // GET: /PackagingDetailAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            PackagingDetail packagingdetail = db.PackagingDetails.Find(id);
            if (packagingdetail == null)
            {
                return HttpNotFound();
            }
            return View(packagingdetail);
        }

        //
        // GET: /PackagingDetailAdmin/Create

        public ActionResult Create()
        {
            PackagingDetail packagingdetail = new PackagingDetail();
            packagingdetail.Id = 0;

            return View(packagingdetail);
        }

        //
        // POST: /PackagingDetailAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PackagingDetail packagingdetail)
        {
            if (ModelState.IsValid)
            {
                db.PackagingDetails.Add(packagingdetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(packagingdetail);
        }

        //
        // GET: /PackagingDetailAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PackagingDetail packagingdetail = db.PackagingDetails.Find(id);
            if (packagingdetail == null)
            {
                return HttpNotFound();
            }
            return View(packagingdetail);
        }

        //
        // POST: /PackagingDetailAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PackagingDetail packagingdetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(packagingdetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(packagingdetail);
        }

        //
        // GET: /PackagingDetailAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PackagingDetail packagingdetail = db.PackagingDetails.Find(id);
            if (packagingdetail == null)
            {
                return HttpNotFound();
            }
            return View(packagingdetail);
        }

        //
        // POST: /PackagingDetailAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PackagingDetail packagingdetail = db.PackagingDetails.Find(id);
            db.PackagingDetails.Remove(packagingdetail);
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
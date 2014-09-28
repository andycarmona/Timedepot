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
    public class ClssNosAdminController : Controller
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
        // GET: /ClssNosAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<ClssNos> qryClssNos = null;

            List<ClssNos> VendorsList = new List<ClssNos>();

            qryClssNos = db.ClssNos.OrderBy(vd => vd.ClssNo);
            if (qryClssNos.Count() > 0)
            {
                foreach (var item in qryClssNos)
                {
                    VendorsList.Add(item);
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


            var onePageOfData = VendorsList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(VendorsList.ToPagedList(pageIndex, pageSize));
            //return View(db.ClssNos.ToList());
        }

        //
        // GET: /ClssNosAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            ClssNos clssnos = db.ClssNos.Find(id);
            if (clssnos == null)
            {
                return HttpNotFound();
            }
            return View(clssnos);
        }

        //
        // GET: /ClssNosAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ClssNosAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClssNos clssnos)
        {
            if (ModelState.IsValid)
            {
                db.ClssNos.Add(clssnos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(clssnos);
        }

        //
        // GET: /ClssNosAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ClssNos clssnos = db.ClssNos.Find(id);
            if (clssnos == null)
            {
                return HttpNotFound();
            }
            return View(clssnos);
        }

        //
        // POST: /ClssNosAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClssNos clssnos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clssnos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clssnos);
        }

        //
        // GET: /ClssNosAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ClssNos clssnos = db.ClssNos.Find(id);
            if (clssnos == null)
            {
                return HttpNotFound();
            }
            return View(clssnos);
        }

        //
        // POST: /ClssNosAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClssNos clssnos = db.ClssNos.Find(id);
            db.ClssNos.Remove(clssnos);
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
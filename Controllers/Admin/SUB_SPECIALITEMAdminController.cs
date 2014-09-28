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
    public class SUB_SPECIALITEMAdminController : Controller
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
        // GET: /SUB_SPECIALITEMAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<SUB_SPECIALITEM> qrySSI = null;

            List<SUB_SPECIALITEM> sub_specialitemList = new List<SUB_SPECIALITEM>();

            qrySSI = db.SUB_SPECIALITEM.OrderBy(ssi => ssi.ItemID);
            if (qrySSI.Count() > 0 )
            {
                foreach (var item in qrySSI)
                {
                    sub_specialitemList.Add(item);
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


            var onePageOfData = sub_specialitemList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(sub_specialitemList.ToPagedList(pageIndex, pageSize));
            //return View(db.SUB_SPECIALITEM.ToList());
        }

        //
        // GET: /SUB_SPECIALITEMAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            SUB_SPECIALITEM sub_specialitem = db.SUB_SPECIALITEM.Find(id);
            if (sub_specialitem == null)
            {
                return HttpNotFound();
            }
            return View(sub_specialitem);
        }

        //
        // GET: /SUB_SPECIALITEMAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SUB_SPECIALITEMAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SUB_SPECIALITEM sub_specialitem)
        {
            if (ModelState.IsValid)
            {
                db.SUB_SPECIALITEM.Add(sub_specialitem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sub_specialitem);
        }

        //
        // GET: /SUB_SPECIALITEMAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            SUB_SPECIALITEM sub_specialitem = db.SUB_SPECIALITEM.Find(id);
            if (sub_specialitem == null)
            {
                return HttpNotFound();
            }
            return View(sub_specialitem);
        }

        //
        // POST: /SUB_SPECIALITEMAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SUB_SPECIALITEM sub_specialitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sub_specialitem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sub_specialitem);
        }

        //
        // GET: /SUB_SPECIALITEMAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            SUB_SPECIALITEM sub_specialitem = db.SUB_SPECIALITEM.Find(id);
            if (sub_specialitem == null)
            {
                return HttpNotFound();
            }
            return View(sub_specialitem);
        }

        //
        // POST: /SUB_SPECIALITEMAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SUB_SPECIALITEM sub_specialitem = db.SUB_SPECIALITEM.Find(id);
            db.SUB_SPECIALITEM.Remove(sub_specialitem);
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
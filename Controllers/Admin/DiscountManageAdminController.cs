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
    public class DiscountManageAdminController : Controller
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
        // GET: /DiscountManageAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<DiscountManage> qryDsc = null;

            List<DiscountManage> DiscountManageList = new List<DiscountManage>();
            qryDsc = db.DiscountManages.OrderBy(dsc => dsc.DiscountName);
            if (qryDsc.Count() > 0)
            {
                foreach (var item in qryDsc)
                {
                    DiscountManageList.Add(item);
                }
            }
            //Poner los datos aqui

            //Set the page
            if (page == null)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = Convert.ToInt32(page);
            }


            var onePageOfData = DiscountManageList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(DiscountManageList.ToPagedList(pageIndex, pageSize));
            //return View(db.DiscountManages.ToList());
        }

        //
        // GET: /DiscountManageAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            DiscountManage discountmanage = db.DiscountManages.Find(id);
            if (discountmanage == null)
            {
                return HttpNotFound();
            }
            return View(discountmanage);
        }

        //
        // GET: /DiscountManageAdmin/Create

        public ActionResult Create()
        {
            DiscountManage discountmanage = new DiscountManage();
            discountmanage.id = 0;

            return View(discountmanage);
        }

        //
        // POST: /DiscountManageAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DiscountManage discountmanage)
        {
            if (ModelState.IsValid)
            {
                db.DiscountManages.Add(discountmanage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(discountmanage);
        }

        //
        // GET: /DiscountManageAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            DiscountManage discountmanage = db.DiscountManages.Find(id);
            if (discountmanage == null)
            {
                return HttpNotFound();
            }
            return View(discountmanage);
        }

        //
        // POST: /DiscountManageAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DiscountManage discountmanage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(discountmanage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(discountmanage);
        }

        //
        // GET: /DiscountManageAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            DiscountManage discountmanage = db.DiscountManages.Find(id);
            if (discountmanage == null)
            {
                return HttpNotFound();
            }
            return View(discountmanage);
        }

        //
        // POST: /DiscountManageAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DiscountManage discountmanage = db.DiscountManages.Find(id);
            db.DiscountManages.Remove(discountmanage);
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
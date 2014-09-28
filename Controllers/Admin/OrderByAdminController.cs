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
    public class OrderByAdminController : Controller
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
        // GET: /OrderByAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<OrderBy> qryOrderBy = null;

            List<OrderBy> OrderByList = new List<OrderBy>();

            qryOrderBy = db.OrderBies.OrderBy(vd => vd.Description);
            if (qryOrderBy.Count() > 0)
            {
                foreach (var item in qryOrderBy)
                {
                    OrderByList.Add(item);
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


            var onePageOfData = OrderByList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(OrderByList.ToPagedList(pageIndex, pageSize));
            //return View(db.OrderBies.ToList());
        }

        //
        // GET: /OrderByAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            OrderBy orderby = db.OrderBies.Find(id);
            if (orderby == null)
            {
                return HttpNotFound();
            }
            return View(orderby);
        }

        //
        // GET: /OrderByAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /OrderByAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderBy orderby)
        {
            if (ModelState.IsValid)
            {
                db.OrderBies.Add(orderby);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(orderby);
        }

        //
        // GET: /OrderByAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            OrderBy orderby = db.OrderBies.Find(id);
            if (orderby == null)
            {
                return HttpNotFound();
            }
            return View(orderby);
        }

        //
        // POST: /OrderByAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrderBy orderby)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderby).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(orderby);
        }

        //
        // GET: /OrderByAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            OrderBy orderby = db.OrderBies.Find(id);
            if (orderby == null)
            {
                return HttpNotFound();
            }
            return View(orderby);
        }

        //
        // POST: /OrderByAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderBy orderby = db.OrderBies.Find(id);
            db.OrderBies.Remove(orderby);
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
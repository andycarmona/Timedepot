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
    public class Group_MasterAdminController : Controller
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
        // GET: /Group_MasterAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<Group_Master> qryGMs = null;

            List<Group_Master> Group_MasterList = new List<Group_Master>();

            qryGMs = db.Group_Master.OrderBy(mst => mst.Group_Name);
            if (qryGMs.Count() > 0)
            {
                foreach (var item in qryGMs)
                {
                    Group_MasterList.Add(item);
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


            var onePageOfData = Group_MasterList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(Group_MasterList.ToPagedList(pageIndex, pageSize));
            //return View(db.Group_Master.ToList());
        }

        //
        // GET: /Group_MasterAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            Group_Master group_master = db.Group_Master.Find(id);
            if (group_master == null)
            {
                return HttpNotFound();
            }
            return View(group_master);
        }

        //
        // GET: /Group_MasterAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Group_MasterAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Group_Master group_master)
        {
            if (ModelState.IsValid)
            {
                db.Group_Master.Add(group_master);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group_master);
        }

        //
        // GET: /Group_MasterAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Group_Master group_master = db.Group_Master.Find(id);
            if (group_master == null)
            {
                return HttpNotFound();
            }
            return View(group_master);
        }

        //
        // POST: /Group_MasterAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Group_Master group_master)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group_master).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group_master);
        }

        //
        // GET: /Group_MasterAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Group_Master group_master = db.Group_Master.Find(id);
            if (group_master == null)
            {
                return HttpNotFound();
            }
            return View(group_master);
        }

        //
        // POST: /Group_MasterAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Group_Master group_master = db.Group_Master.Find(id);
            db.Group_Master.Remove(group_master);
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
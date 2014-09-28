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
    public class aspnet_UsersAdminController : Controller
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
        // GET: /aspnet_Users/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<aspnet_Users> qryVendors = null;

            List<aspnet_Users> VendorsList = new List<aspnet_Users>();

            qryVendors = db.aspnet_Users.OrderBy(vd => vd.UserName);
            if (qryVendors.Count() > 0)
            {
                foreach (var item in qryVendors)
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
            //return View(db.aspnet_Users.ToList());
        }

        //
        // GET: /aspnet_Users/Details/5

        public ActionResult Details(Guid? id)
        {
            aspnet_Users aspnet_users = db.aspnet_Users.Find(id);
            if (aspnet_users == null)
            {
                return HttpNotFound();
            }
            return View(aspnet_users);
        }

        //
        // GET: /aspnet_Users/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /aspnet_Users/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(aspnet_Users aspnet_users)
        {
            if (ModelState.IsValid)
            {
                aspnet_users.UserId = Guid.NewGuid();
                db.aspnet_Users.Add(aspnet_users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspnet_users);
        }

        //
        // GET: /aspnet_Users/Edit/5

        public ActionResult Edit(Guid? id)
        {
            aspnet_Users aspnet_users = db.aspnet_Users.Find(id);
            if (aspnet_users == null)
            {
                return HttpNotFound();
            }
            return View(aspnet_users);
        }

        //
        // POST: /aspnet_Users/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(aspnet_Users aspnet_users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspnet_users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspnet_users);
        }

        //
        // GET: /aspnet_Users/Delete/5

        public ActionResult Delete(Guid? id)
        {
            aspnet_Users aspnet_users = db.aspnet_Users.Find(id);
            if (aspnet_users == null)
            {
                return HttpNotFound();
            }
            return View(aspnet_users);
        }

        //
        // POST: /aspnet_Users/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            aspnet_Users aspnet_users = db.aspnet_Users.Find(id);
            db.aspnet_Users.Remove(aspnet_users);
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
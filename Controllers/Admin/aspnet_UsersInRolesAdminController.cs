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
    public class aspnet_UsersInRolesAdminController : Controller
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
        // GET: /aspnet_UsersInRolesAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<aspnet_UsersInRoles> qryVendors = null;

            List<aspnet_UsersInRoles> VendorsList = new List<aspnet_UsersInRoles>();

            qryVendors = db.aspnet_UsersInRoles.OrderBy(vd => vd.RoleId);
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
            return View(db.aspnet_UsersInRoles.ToList());
        }

        //
        // GET: /aspnet_UsersInRolesAdmin/Details/5

        public ActionResult Details(Guid? id)
        {
            aspnet_UsersInRoles aspnet_usersinroles = db.aspnet_UsersInRoles.Find(id);
            if (aspnet_usersinroles == null)
            {
                return HttpNotFound();
            }
            return View(aspnet_usersinroles);
        }

        //
        // GET: /aspnet_UsersInRolesAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /aspnet_UsersInRolesAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(aspnet_UsersInRoles aspnet_usersinroles)
        {
            if (ModelState.IsValid)
            {
                //aspnet_usersinroles.UserId = Guid.NewGuid();
                db.aspnet_UsersInRoles.Add(aspnet_usersinroles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspnet_usersinroles);
        }

        //
        // GET: /aspnet_UsersInRolesAdmin/Edit/5

        public ActionResult Edit(Guid? id)
        {
            aspnet_UsersInRoles aspnet_usersinroles = db.aspnet_UsersInRoles.Find(id);
            if (aspnet_usersinroles == null)
            {
                return HttpNotFound();
            }
            return View(aspnet_usersinroles);
        }

        //
        // POST: /aspnet_UsersInRolesAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(aspnet_UsersInRoles aspnet_usersinroles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspnet_usersinroles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspnet_usersinroles);
        }

        //
        // GET: /aspnet_UsersInRolesAdmin/Delete/5

        public ActionResult Delete(Guid? UserId, Guid? RoleId)
        {
            aspnet_UsersInRoles aspnet_usersinroles = db.aspnet_UsersInRoles.Where(usrl => usrl.UserId == UserId && usrl.RoleId == RoleId).FirstOrDefault<aspnet_UsersInRoles>();
            if (aspnet_usersinroles == null)
            {
                return HttpNotFound();
            }
            return View(aspnet_usersinroles);
        }

        //
        // POST: /aspnet_UsersInRolesAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid? UserId, Guid? RoleId)
        {
            aspnet_UsersInRoles aspnet_usersinroles = db.aspnet_UsersInRoles.Where(usrl => usrl.UserId == UserId && usrl.RoleId == RoleId).FirstOrDefault<aspnet_UsersInRoles>();
            db.aspnet_UsersInRoles.Remove(aspnet_usersinroles);
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
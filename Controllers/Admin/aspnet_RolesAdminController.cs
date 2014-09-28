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
    public class aspnet_RolesAdminController : Controller
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
        // GET: /aspnet_RolesAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<aspnet_Roles> qryVendors = null;

            List<aspnet_Roles> VendorsList = new List<aspnet_Roles>();

            qryVendors = db.aspnet_Roles.OrderBy(vd => vd.RoleName);
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
            //return View(db.aspnet_Roles.ToList());
        }

        //
        // GET: /aspnet_RolesAdmin/Details/5

        public ActionResult Details(Guid? id)
        {
            aspnet_Roles aspnet_roles = db.aspnet_Roles.Find(id);
            if (aspnet_roles == null)
            {
                return HttpNotFound();
            }
            return View(aspnet_roles);
        }

        //
        // GET: /aspnet_RolesAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /aspnet_RolesAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(aspnet_Roles aspnet_roles)
        {
            if (ModelState.IsValid)
            {
                aspnet_roles.RoleId = Guid.NewGuid();
                db.aspnet_Roles.Add(aspnet_roles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspnet_roles);
        }

        //
        // GET: /aspnet_RolesAdmin/Edit/5

        public ActionResult Edit(Guid? id)
        {
            aspnet_Roles aspnet_roles = db.aspnet_Roles.Find(id);
            if (aspnet_roles == null)
            {
                return HttpNotFound();
            }
            return View(aspnet_roles);
        }

        //
        // POST: /aspnet_RolesAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(aspnet_Roles aspnet_roles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspnet_roles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspnet_roles);
        }

        //
        // GET: /aspnet_RolesAdmin/Delete/5

        public ActionResult Delete(Guid? id)
        {
            aspnet_Roles aspnet_roles = db.aspnet_Roles.Find(id);
            if (aspnet_roles == null)
            {
                return HttpNotFound();
            }
            return View(aspnet_roles);
        }

        //
        // POST: /aspnet_RolesAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            aspnet_Roles aspnet_roles = db.aspnet_Roles.Find(id);
            db.aspnet_Roles.Remove(aspnet_roles);
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
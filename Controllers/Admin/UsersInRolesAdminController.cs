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
using TimelyDepotMVC.CommonCode;

namespace TimelyDepotMVC.Controllers.Admin
{
    public class UsersInRolesAdminController : Controller
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
        // GET: /_UsersInRolesAdmin/SeleccionarUsuario
        [NoCache]
        public PartialViewResult SeleccionarUsuario(int? page)
        {

            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<Users> qrUsers = null;

            List<Users> UsersList = new List<Users>();

            qrUsers = db.Users.OrderBy(vd => vd.UserName);
            if (qrUsers.Count() > 0)
            {
                foreach (var item in qrUsers)
                {
                    UsersList.Add(item);
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


            var onePageOfData = UsersList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return PartialView(UsersList.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /UsersInRolesAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<UsersInRoles> qryVendors = null;

            List<UsersInRoles> VendorsList = new List<UsersInRoles>();

            qryVendors = db.UsersInRoles.OrderBy(vd => vd.RoleId);
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
            //return View(db.UsersInRoles.ToList());
        }

        //
        // GET: /UsersInRolesAdmin/Details/5

        public ActionResult Details(Guid? id)
        {
            UsersInRoles usersinroles = db.UsersInRoles.Find(id);
            if (usersinroles == null)
            {
                return HttpNotFound();
            }
            return View(usersinroles);
        }

        //
        // GET: /UsersInRolesAdmin/Create

        public ActionResult Create()
        {
            IQueryable<Roles> qryRoles = null;

            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            //Get the dropdown data
            qryRoles = db.Roles.OrderBy(dpt => dpt.RoleName);
            if (qryRoles.Count() > 0)
            {
                foreach (var item in qryRoles)
                {
                    listSelector.Add(new KeyValuePair<string, string>(item.RoleId.ToString(), item.RoleName));
                }
            }
            SelectList deptoslist = new SelectList(listSelector, "Key", "Value");
            ViewBag.RolesList = deptoslist;
            
            return View();
        }

        //
        // POST: /UsersInRolesAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsersInRoles usersinroles)
        {
            if (ModelState.IsValid)
            {
                db.UsersInRoles.Add(usersinroles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usersinroles);
        }

        //
        // GET: /UsersInRolesAdmin/Edit/5

        public ActionResult Edit(Guid? id)
        {
            UsersInRoles usersinroles = db.UsersInRoles.Find(id);
            if (usersinroles == null)
            {
                return HttpNotFound();
            }
            return View(usersinroles);
        }

        //
        // POST: /UsersInRolesAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsersInRoles usersinroles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usersinroles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usersinroles);
        }

        //
        // GET: /UsersInRolesAdmin/Delete/5

        public ActionResult Delete(Guid? UserId, Guid? RoleId)
        {
            UsersInRoles usersinroles = db.UsersInRoles.Where(usrl => usrl.UserId == UserId && usrl.RoleId == RoleId).FirstOrDefault<UsersInRoles>();
            if (usersinroles == null)
            {
                return HttpNotFound();
            }
            return View(usersinroles);
        }

        //
        // POST: /UsersInRolesAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid? UserId, Guid? RoleId)
        {
            UsersInRoles usersinroles = db.UsersInRoles.Where(usrl => usrl.UserId == UserId && usrl.RoleId == RoleId).FirstOrDefault<UsersInRoles>();
            db.UsersInRoles.Remove(usersinroles);
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
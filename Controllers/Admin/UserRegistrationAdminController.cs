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
    public class UserRegistrationAdminController : Controller
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
        // GET: /UserRegistrationAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;

            IQueryable<UserRegistration> qryUsrReg = null;

            List<UserRegistration> userRegistrationList = new List<UserRegistration>();

            //Get the users
            qryUsrReg = db.UserRegistrations.OrderBy(usrreg => usrreg.UserName);

            if (qryUsrReg.Count() > 0)
            {
                foreach (var item in qryUsrReg)
                {
                    userRegistrationList.Add(item);
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

            var onePageOfData = userRegistrationList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;

            // return View(db.UserRegistrations.ToList());
            //return View(db.UserRegistrations.ToList().ToPagedList(pageIndex, pageSize));
            return View(userRegistrationList.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /UserRegistrationAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            UserRegistration userregistration = db.UserRegistrations.Find(id);
            if (userregistration == null)
            {
                return HttpNotFound();
            }
            return View(userregistration);
        }

        //
        // GET: /UserRegistrationAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /UserRegistrationAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserRegistration userregistration)
        {
            if (ModelState.IsValid)
            {
                db.UserRegistrations.Add(userregistration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userregistration);
        }

        //
        // GET: /UserRegistrationAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            UserRegistration userregistration = db.UserRegistrations.Find(id);
            if (userregistration == null)
            {
                return HttpNotFound();
            }
            return View(userregistration);
        }

        //
        // POST: /UserRegistrationAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserRegistration userregistration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userregistration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userregistration);
        }

        //
        // GET: /UserRegistrationAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UserRegistration userregistration = db.UserRegistrations.Find(id);
            if (userregistration == null)
            {
                return HttpNotFound();
            }
            return View(userregistration);
        }

        //
        // POST: /UserRegistrationAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserRegistration userregistration = db.UserRegistrations.Find(id);
            db.UserRegistrations.Remove(userregistration);
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
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
    public class UsersAdminController : Controller
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
        // GET: /UsersAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<Users> qryVendors = null;

            List<Users> VendorsList = new List<Users>();

            qryVendors = db.Users.OrderBy(vd => vd.UserName);
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
            //return View(db.Users.ToList());
        }

        //
        // GET: /UsersAdmin/Details/5

        public ActionResult Details(Guid? id)
        {
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        //
        // GET: /UsersAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /UsersAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Users users)
        {
            if (ModelState.IsValid)
            {
                users.UserId = Guid.NewGuid();
                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(users);
        }

        //
        // GET: /UsersAdmin/Edit/5

        public ActionResult Edit(Guid? id)
        {
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        //
        // POST: /UsersAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(users);
        }

        //
        // GET: /UsersAdmin/Delete/5

        public ActionResult Delete(Guid? id)
        {
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        //
        // POST: /UsersAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            string szError = "";
            Users users = null;
            Memberships membership = null;
            IQueryable<UsersInRoles> qryUsersinRole = null;


            try
            {


                users = db.Users.Find(id);

                if (users.UserName == User.Identity.Name)
                {
                    return RedirectToAction("Index");
                }

                //Remove usersrinroles
                qryUsersinRole = db.UsersInRoles.Where(usrl => usrl.UserId == users.UserId);
                if (qryUsersinRole.Count() > 0)
                {
                    foreach (var item in qryUsersinRole)
                    {
                        db.UsersInRoles.Remove(item);
                    }
                }

                //Remove membership
                membership = db.Memberships.Where(mbr => mbr.UserId == users.UserId).FirstOrDefault<Memberships>();
                if (membership != null)
                {
                    db.Memberships.Remove(membership);
                }


                db.Users.Remove(users);


                db.SaveChanges();

            }
            catch (Exception err)
            {
                szError = err.Message;
                //throw;
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
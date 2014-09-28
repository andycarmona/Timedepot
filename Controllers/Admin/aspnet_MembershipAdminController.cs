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
    public class aspnet_MembershipAdminController : Controller
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
        // GET: /aspnet_MembershipAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<aspnet_Membership> qryVendors = null;

            List<aspnet_Membership> VendorsList = new List<aspnet_Membership>();

            qryVendors = db.aspnet_Membership.OrderBy(vd => vd.Email);
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
            //return View(db.aspnet_Membership.ToList());
        }

        //
        // GET: /aspnet_MembershipAdmin/Details/5

        public ActionResult Details(Guid? id)
        {
            aspnet_Membership aspnet_membership = db.aspnet_Membership.Find(id);
            if (aspnet_membership == null)
            {
                return HttpNotFound();
            }
            return View(aspnet_membership);
        }

        //
        // GET: /aspnet_MembershipAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /aspnet_MembershipAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(aspnet_Membership aspnet_membership)
        {
            if (ModelState.IsValid)
            {
                aspnet_membership.UserId = Guid.NewGuid();
                db.aspnet_Membership.Add(aspnet_membership);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspnet_membership);
        }

        //
        // GET: /aspnet_MembershipAdmin/Edit/5

        public ActionResult Edit(Guid? id)
        {
            aspnet_Membership aspnet_membership = db.aspnet_Membership.Find(id);
            if (aspnet_membership == null)
            {
                return HttpNotFound();
            }
            return View(aspnet_membership);
        }

        //
        // POST: /aspnet_MembershipAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(aspnet_Membership aspnet_membership)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspnet_membership).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspnet_membership);
        }

        //
        // GET: /aspnet_MembershipAdmin/Delete/5

        public ActionResult Delete(Guid? id)
        {
            aspnet_Membership aspnet_membership = db.aspnet_Membership.Find(id);
            if (aspnet_membership == null)
            {
                return HttpNotFound();
            }
            return View(aspnet_membership);
        }

        //
        // POST: /aspnet_MembershipAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            aspnet_Membership aspnet_membership = db.aspnet_Membership.Find(id);
            db.aspnet_Membership.Remove(aspnet_membership);
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
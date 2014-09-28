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
    public class UserQuotationDetailAdminController : Controller
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
        // GET: /UserQuotationDetailAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<UserQuotationDetail> qryUserQuotationDetail = null;

            List<UserQuotationDetail> UserQuotationDetailList = new List<UserQuotationDetail>();

            qryUserQuotationDetail = db.UserQuotationDetails.OrderBy(vd => vd.ItemID).ThenBy(vd => vd.Id).ThenBy(vd => vd.ProductType);
            if (qryUserQuotationDetail.Count() > 0)
            {
                foreach (var item in qryUserQuotationDetail)
                {
                    UserQuotationDetailList.Add(item);
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


            var onePageOfData = UserQuotationDetailList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(UserQuotationDetailList.ToPagedList(pageIndex, pageSize));
            //return View(db.UserQuotationDetails.ToList());
        }

        //
        // GET: /UserQuotationDetailAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            UserQuotationDetail userquotationdetail = db.UserQuotationDetails.Find(id);
            if (userquotationdetail == null)
            {
                return HttpNotFound();
            }
            return View(userquotationdetail);
        }

        //
        // GET: /UserQuotationDetailAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /UserQuotationDetailAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserQuotationDetail userquotationdetail)
        {
            if (ModelState.IsValid)
            {
                db.UserQuotationDetails.Add(userquotationdetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userquotationdetail);
        }

        //
        // GET: /UserQuotationDetailAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            UserQuotationDetail userquotationdetail = db.UserQuotationDetails.Find(id);
            if (userquotationdetail == null)
            {
                return HttpNotFound();
            }
            return View(userquotationdetail);
        }

        //
        // POST: /UserQuotationDetailAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserQuotationDetail userquotationdetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userquotationdetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userquotationdetail);
        }

        //
        // GET: /UserQuotationDetailAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UserQuotationDetail userquotationdetail = db.UserQuotationDetails.Find(id);
            if (userquotationdetail == null)
            {
                return HttpNotFound();
            }
            return View(userquotationdetail);
        }

        //
        // POST: /UserQuotationDetailAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserQuotationDetail userquotationdetail = db.UserQuotationDetails.Find(id);
            db.UserQuotationDetails.Remove(userquotationdetail);
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
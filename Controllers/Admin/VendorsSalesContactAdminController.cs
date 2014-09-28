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
    public class VendorsSalesContactAdminController : Controller
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
        // GET: /VendorsSalesContactAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<VendorsSalesContact> qryVendorsSC = null;

            List<VendorsSalesContact> VendorsSalesContactList = new List<VendorsSalesContact>();

            qryVendorsSC = db.VendorsSalesContacts.OrderBy(vsc => vsc.FirstName).ThenBy(vsc => vsc.LastName);
            if (qryVendorsSC.Count() > 0)
            {
                foreach (var item in qryVendorsSC)
                {
                    VendorsSalesContactList.Add(item);
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


            var onePageOfData = VendorsSalesContactList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(VendorsSalesContactList.ToPagedList(pageIndex, pageSize));
            //return View(db.VendorsSalesContacts.ToList());
        }

        //
        // GET: /VendorsSalesContactAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            VendorsSalesContact vendorssalescontact = db.VendorsSalesContacts.Find(id);
            if (vendorssalescontact == null)
            {
                return HttpNotFound();
            }
            return View(vendorssalescontact);
        }

        //
        // GET: /VendorsSalesContactAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /VendorsSalesContactAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VendorsSalesContact vendorssalescontact)
        {
            if (ModelState.IsValid)
            {
                db.VendorsSalesContacts.Add(vendorssalescontact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vendorssalescontact);
        }

        //
        // GET: /VendorsSalesContactAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            VendorsSalesContact vendorssalescontact = db.VendorsSalesContacts.Find(id);
            if (vendorssalescontact == null)
            {
                return HttpNotFound();
            }
            return View(vendorssalescontact);
        }

        //
        // POST: /VendorsSalesContactAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VendorsSalesContact vendorssalescontact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendorssalescontact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendorssalescontact);
        }

        //
        // GET: /VendorsSalesContactAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            VendorsSalesContact vendorssalescontact = db.VendorsSalesContacts.Find(id);
            if (vendorssalescontact == null)
            {
                return HttpNotFound();
            }
            return View(vendorssalescontact);
        }

        //
        // POST: /VendorsSalesContactAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VendorsSalesContact vendorssalescontact = db.VendorsSalesContacts.Find(id);
            db.VendorsSalesContacts.Remove(vendorssalescontact);
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
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
    public class VendorsContactAddressAdminController : Controller
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
        // GET: /VendorsContactAddressAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<VendorsContactAddress> qryVendorsCA = null;

            List<VendorsContactAddress> VendorsContactAddressList = new List<VendorsContactAddress>();

            qryVendorsCA = db.VendorsContactAddresses.OrderBy(vca => vca.CompanyName);
            if (qryVendorsCA.Count() > 0)
            {
                foreach (var item in qryVendorsCA)
                {
                    VendorsContactAddressList.Add(item);
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


            var onePageOfData = VendorsContactAddressList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(VendorsContactAddressList.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /VendorsContactAddressAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            VendorsContactAddress vendorscontactaddress = db.VendorsContactAddresses.Find(id);
            if (vendorscontactaddress == null)
            {
                return HttpNotFound();
            }
            return View(vendorscontactaddress);
        }

        //
        // GET: /VendorsContactAddressAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /VendorsContactAddressAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VendorsContactAddress vendorscontactaddress)
        {
            if (ModelState.IsValid)
            {
                db.VendorsContactAddresses.Add(vendorscontactaddress);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vendorscontactaddress);
        }

        //
        // GET: /VendorsContactAddressAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            VendorsContactAddress vendorscontactaddress = db.VendorsContactAddresses.Find(id);
            if (vendorscontactaddress == null)
            {
                return HttpNotFound();
            }
            return View(vendorscontactaddress);
        }

        //
        // POST: /VendorsContactAddressAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VendorsContactAddress vendorscontactaddress)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendorscontactaddress).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendorscontactaddress);
        }

        //
        // GET: /VendorsContactAddressAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            VendorsContactAddress vendorscontactaddress = db.VendorsContactAddresses.Find(id);
            if (vendorscontactaddress == null)
            {
                return HttpNotFound();
            }
            return View(vendorscontactaddress);
        }

        //
        // POST: /VendorsContactAddressAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VendorsContactAddress vendorscontactaddress = db.VendorsContactAddresses.Find(id);
            db.VendorsContactAddresses.Remove(vendorscontactaddress);
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
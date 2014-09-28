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
    public class CustomersCreditCardShippingAdminController : Controller
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
        // GET: /CustomersCreditCardShippingAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<CustomersCreditCardShipping> qryVendors = null;

            List<CustomersCreditCardShipping> VendorsList = new List<CustomersCreditCardShipping>();

            qryVendors = db.CustomersCreditCardShippings.OrderBy(vd => vd.CustomerId);
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
            //return View(db.CustomersCreditCardShippings.ToList());
        }

        //
        // GET: /CustomersCreditCardShippingAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            CustomersCreditCardShipping customerscreditcardshipping = db.CustomersCreditCardShippings.Find(id);
            if (customerscreditcardshipping == null)
            {
                return HttpNotFound();
            }
            return View(customerscreditcardshipping);
        }

        //
        // GET: /CustomersCreditCardShippingAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CustomersCreditCardShippingAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomersCreditCardShipping customerscreditcardshipping)
        {
            string szError = "";
            string szEncriptedData = "";

            if (ModelState.IsValid)
            {
                //Encode the credit card info
                if (!string.IsNullOrEmpty(customerscreditcardshipping.CreditNumber))
                {
                    szEncriptedData = TimelyDepotMVC.Controllers.PaymentController.EncriptInfo02(customerscreditcardshipping.CreditNumber, ref szError);
                    customerscreditcardshipping.CreditNumber = szEncriptedData;
                }

                //Encode the secure code
                if (!string.IsNullOrEmpty(customerscreditcardshipping.SecureCode))
                {
                    szEncriptedData = TimelyDepotMVC.Controllers.PaymentController.EncriptInfo02(customerscreditcardshipping.SecureCode, ref szError);
                    customerscreditcardshipping.SecureCode = szEncriptedData;
                }

                db.CustomersCreditCardShippings.Add(customerscreditcardshipping);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customerscreditcardshipping);
        }

        //
        // GET: /CustomersCreditCardShippingAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CustomersCreditCardShipping customerscreditcardshipping = db.CustomersCreditCardShippings.Find(id);
            if (customerscreditcardshipping == null)
            {
                return HttpNotFound();
            }
            return View(customerscreditcardshipping);
        }

        //
        // POST: /CustomersCreditCardShippingAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomersCreditCardShipping customerscreditcardshipping, string CreditNumber01, string SecureCode01)
        {
            int nPos = -1;
            string szError = "";
            string szEncriptedData = "";

            if (ModelState.IsValid)
            {
                //use the user supplied data
                if (!string.IsNullOrEmpty(CreditNumber01))
                {
                    nPos = CreditNumber01.IndexOf("*");
                    if (nPos == -1)
                    {
                        customerscreditcardshipping.CreditNumber = CreditNumber01; 

                        //Encode the credit card info
                        if (!string.IsNullOrEmpty(customerscreditcardshipping.CreditNumber))
                        {
                            szEncriptedData = TimelyDepotMVC.Controllers.PaymentController.EncriptInfo02(customerscreditcardshipping.CreditNumber, ref szError);
                            customerscreditcardshipping.CreditNumber = szEncriptedData;
                        }
                    }
                    else
                    {
                        //Do not replace the credit card number
                    }
                }

                if (!string.IsNullOrEmpty(SecureCode01))
                {
                    nPos = -1;
                    nPos = SecureCode01.IndexOf("*");
                    if (nPos == -1)
                    {
                        customerscreditcardshipping.SecureCode = SecureCode01;

                        //Encode the credit card info
                        if (!string.IsNullOrEmpty(customerscreditcardshipping.SecureCode))
                        {
                            szEncriptedData = TimelyDepotMVC.Controllers.PaymentController.EncriptInfo02(customerscreditcardshipping.SecureCode, ref szError);
                            customerscreditcardshipping.SecureCode = szEncriptedData;
                        }
                    }
                    else
                    {
                        //Do not replace the credit card number
                    }
                }


                db.Entry(customerscreditcardshipping).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customerscreditcardshipping);
        }

        //
        // GET: /CustomersCreditCardShippingAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CustomersCreditCardShipping customerscreditcardshipping = db.CustomersCreditCardShippings.Find(id);
            if (customerscreditcardshipping == null)
            {
                return HttpNotFound();
            }
            return View(customerscreditcardshipping);
        }

        //
        // POST: /CustomersCreditCardShippingAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomersCreditCardShipping customerscreditcardshipping = db.CustomersCreditCardShippings.Find(id);
            db.CustomersCreditCardShippings.Remove(customerscreditcardshipping);
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
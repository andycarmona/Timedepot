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
    public class PaymentsAdminController : Controller
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
        // GET: /PaymentsAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<Payments> qryVendors = null;

            List<Payments> VendorsList = new List<Payments>();

            qryVendors = db.Payments.OrderBy(vd => vd.PaymentNo);
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
            //return View(db.Payments.ToList());
        }

        //
        // GET: /PaymentsAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            Payments payments = db.Payments.Find(id);
            if (payments == null)
            {
                return HttpNotFound();
            }
            return View(payments);
        }

        //
        // GET: /PaymentsAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PaymentsAdmin/Create

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Payments payments)
        {

            string szError = "";
            string szEncriptedData = "";

            if (ModelState.IsValid)
            {
                //Encode the credit card info
                if (!string.IsNullOrEmpty(payments.CreditCardNumber))
                {
                    szEncriptedData = TimelyDepotMVC.Controllers.PaymentController.EncriptInfo02(payments.CreditCardNumber, ref szError);
                    payments.CreditCardNumber = szEncriptedData;
                }

                ////Encode the secure code
                //if (!string.IsNullOrEmpty(customerscreditcardshipping.SecureCode))
                //{
                //    szEncriptedData = TimelyDepotMVC.Controllers.PaymentController.EncriptInfo02(customerscreditcardshipping.SecureCode, ref szError);
                //    customerscreditcardshipping.SecureCode = szEncriptedData;
                //}



                db.Payments.Add(payments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(payments);
        }

        //
        // GET: /PaymentsAdmin/Edit/5
        public ActionResult Edit(int id = 0)
        {
            Payments payments = db.Payments.Find(id);
            if (payments == null)
            {
                return HttpNotFound();
            }
            return View(payments);
        }

        //
        // POST: /PaymentsAdmin/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Payments payments, string CreditNumber01)
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
                        payments.CreditCardNumber = CreditNumber01;

                        //Encode the credit card info
                        if (!string.IsNullOrEmpty(payments.CreditCardNumber))
                        {
                            szEncriptedData = TimelyDepotMVC.Controllers.PaymentController.EncriptInfo02(payments.CreditCardNumber, ref szError);
                            payments.CreditCardNumber = szEncriptedData;
                        }
                    }
                    else
                    {
                        //Do not replace the credit card number
                    }
                }

                db.Entry(payments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(payments);
        }

        //
        // GET: /PaymentsAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Payments payments = db.Payments.Find(id);
            if (payments == null)
            {
                return HttpNotFound();
            }
            return View(payments);
        }

        //
        // POST: /PaymentsAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payments payments = db.Payments.Find(id);
            db.Payments.Remove(payments);
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
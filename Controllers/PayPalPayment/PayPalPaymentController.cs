using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TimelyDepotMVC.DAL;
using TimelyDepotMVC.Models.Admin;
using TimelyDepotMVC.CommonCode;
using System.Data;

namespace TimelyDepotMVC.Controllers.PayPalPayment
{
    using System.Data.Entity;

    public class PayPalPaymentController : Controller
    {
        private TimelyDepotContext db = new TimelyDepotContext();

        //
        // GET: /PayPalPayment/OrderSimple
        public ActionResult OrderSimple(Payments payment)
        {
            string cancelUrl = "";
            string returnUrl = "";
            string szMsg = "";

            payment = db.Payments.Find(payment.Id);
            if (payment != null)
            {


                string customerEmail = Request["orderEmail"];

                string hostUrl = Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped) + Request.ApplicationPath;
                //string cancelUrl = hostUrl + "Payment";
                //string returnUrl = hostUrl + "PayPalPayment/PaymentSuccess";

                szMsg = hostUrl.Substring(hostUrl.Length - 1);
                if (szMsg == "/")
                {
                    cancelUrl = string.Format("{0}Payment?id={1}", hostUrl, payment.Id);
                    returnUrl = string.Format("{0}PayPalPayment/PaymentSuccess?paymentid={1}", hostUrl, payment.Id);
                }
                else
                {
                    cancelUrl = string.Format("{0}/Payment?id={1}", hostUrl, payment.Id);
                    returnUrl = string.Format("{0}/PayPalPayment/PaymentSuccess?paymentid={1}", hostUrl, payment.Id);
                }


                // Enter the email of the receiver 
                string receiver = "mariog484-facilitator@hotmail.com";


                var amount = Convert.ToDecimal(payment.Amount);
                var sender = customerEmail;
                var response = PayPal.AdaptivePayments.SimplePay.Execute
                    (receiver, amount, sender, "WebMatrix Integration", "127.0.0.1", "Test Device", cancelUrl, returnUrl, returnUrl);
                response.Redirect();
            }

            return RedirectToAction("Index", "Payment");
        }

        //
        // GET: /PayPalPayment/PaymentSuccess
        public ActionResult PaymentSuccess(string paymentid)
        {
            int nPaymentId = 0;
            string szSalesOrderNo = "";
            Payments payment = null;
            SalesOrder salesorder = null;
            Invoice invoice = null;

            if (!string.IsNullOrEmpty(paymentid))
            {
                //Get the payment
                nPaymentId = Convert.ToInt32(paymentid);
                payment = db.Payments.Find(nPaymentId);
                if (payment != null)
                {
                    //Update the Sales order
                    salesorder = db.SalesOrders.Where(slod => slod.SalesOrderNo == payment.SalesOrderNo).FirstOrDefault<SalesOrder>();
                    if (salesorder != null)
                    {
                        szSalesOrderNo = salesorder.SalesOrderNo;
                        salesorder.PaymentAmount = Convert.ToDecimal(salesorder.PaymentAmount) + Convert.ToDecimal(payment.Amount);
                        salesorder.PaymentDate = Convert.ToDateTime(payment.PaymentDate);
                        db.Entry(salesorder).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    //Update the invoice
                    invoice = db.Invoices.Where(invc => invc.SalesOrderNo == szSalesOrderNo).FirstOrDefault<Invoice>();
                    if (invoice != null)
                    {
                        invoice.PaymentAmount = Convert.ToDecimal(invoice.PaymentAmount) + Convert.ToDecimal(payment.Amount);
                        invoice.PaymentDate = Convert.ToDateTime(payment.PaymentDate);
                        db.Entry(invoice).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }

            return View();
        }

        //
        // GET: /PayPalPayment/
        public ActionResult Index(int id = 0)
        {
            Payments payment = db.Payments.Find(id);
            SalesOrder salesorder = null;
            CustomersShipAddress shipto = null;
            CustomersContactAddress customeraddress = null;

            //Display Payment data
            if (payment != null)
            {
                //Get the sales order
                salesorder = db.SalesOrders.Where(slor => slor.SalesOrderNo == payment.SalesOrderNo).FirstOrDefault<SalesOrder>();
                if (salesorder != null)
                {
                    //Get the customer
                    customeraddress = db.CustomersContactAddresses.Where(ctad => ctad.CustomerId == salesorder.CustomerId).FirstOrDefault<CustomersContactAddress>();
                    if (customeraddress != null)
                    {
                        ViewBag.Company = customeraddress.CompanyName;

                    }

                    //Get the ship to address
                    shipto = db.CustomersShipAddresses.Where(ctst => ctst.CustomerId == salesorder.CustomerId).FirstOrDefault<CustomersShipAddress>();
                    if (shipto != null)
                    {
                        ViewBag.FirstName = shipto.FirstName;
                        ViewBag.LastName = shipto.LastName;
                        ViewBag.AddressHlp4 = string.Format("{0}", shipto.Address1);
                        ViewBag.AddressHlp5 = string.Format("{0} {1} {2}", shipto.City, shipto.State, shipto.Zip);

                    }

                }

            }
            else
            {
                return RedirectToAction("Index", "Payment");
            }

            return View(payment);
        }

    }
}

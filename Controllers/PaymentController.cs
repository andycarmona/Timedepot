using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimelyDepotMVC.Models.Admin;
using TimelyDepotMVC.DAL;
using TimelyDepotMVC.CommonCode;
using PagedList;

using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using System.Xml;
using System.Net;

namespace TimelyDepotMVC.Controllers
{
    using System.Collections;
    using System.Data.Entity.Core;
    using System.Globalization;
    using System.Net.Mime;

    using PdfReportSamples.Models;

    using PdfRpt.Core.Helper;

    using TimelyDepotMVC.ModelsView;
    using System.Text.RegularExpressions;

    using WebGrease.Css;

    public class PaymentController : Controller
    {
        private TimelyDepotContext db = new TimelyDepotContext();

        //private static byte[] _salt = Encoding.ASCII.GetBytes("o6806642kbM7c5");
        private static string _salt = "o6806642kbT7e5";
        internal const string szKey = "560A18CD-6346-4CF0-A2E8-671F9B6B9EA9";

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

        // GET: //Refund
        public ActionResult Refund(int id = 0)
        {
            //Get the payment
            Payments payment = db.Payments.Find(id);
            if (payment != null)
            {
                return RedirectToAction("FDZPayment", new { id = payment.Id, invoicepayment = payment.InvoicePayment, refund = "refund" });
            }

            return RedirectToAction("Index");
        }


        //
        // POST: //InvoicePayment
        [HttpPost]
        public ActionResult InvoicePayment(Payments payment, string CreditCardNumberhlp)
        {
            int nCardId = 0;
            int nPos = 0;
            int nHas = 0;
            string szError = string.Empty;
            string szDecriptedData = string.Empty;
            string szMsg = string.Empty;
            string szEncriptedData = string.Empty;

            CustomersCreditCardShipping card = null;

            //Set the card numbrer
            if (string.IsNullOrEmpty(CreditCardNumberhlp))
            {
                nCardId = Convert.ToInt32(payment.CreditCardNumber);
                card = db.CustomersCreditCardShippings.Find(nCardId);
                if (card != null)
                {
                    //Decode card number
                    szError = string.Empty;
                    szDecriptedData = TimelyDepotMVC.Controllers.PaymentController.DecodeInfo02(card.CreditNumber, ref szError);
                    if (!string.IsNullOrEmpty(szError))
                    {
                        nPos = szError.IndexOf("data to decode");
                        if (nPos != -1)
                        {
                            szDecriptedData = string.Empty;
                        }
                        else
                        {
                            szDecriptedData = string.Format("******");
                        }
                    }
                    else
                    {
                        //Mask the card number
                        nHas = szDecriptedData.Length;
                        if (nHas > 4)
                        {
                            szMsg = szDecriptedData.Substring(nHas - 4, 4);
                            szDecriptedData = string.Format("******{0}", szMsg);
                        }
                        else
                        {
                            szDecriptedData = string.Format("******");
                        }
                    }
                    //payment.CreditCardNumber = szDecriptedData;
                    payment.CreditCardNumber = card.CreditNumber;
                }
                else
                {
                    payment.CreditCardNumber = string.Empty;
                }
            }
            else
            {
                if (CreditCardNumberhlp.Contains("*"))
                {
                    nCardId = Convert.ToInt32(payment.CreditCardNumber);
                    card = db.CustomersCreditCardShippings.Find(nCardId);
                    if (card != null)
                    {
                        payment.CreditCardNumber = card.CreditNumber;
                    }
                    //payment.CreditCardNumber = CreditCardNumberhlp;
                }
                else
                {
                    //Encript the credit card numbre
                    szEncriptedData = EncriptInfo02(CreditCardNumberhlp, ref szError);
                    if (!string.IsNullOrEmpty(szEncriptedData))
                    {
                        szError = string.Empty;
                        szDecriptedData = TimelyDepotMVC.Controllers.PaymentController.DecodeInfo02(szEncriptedData, ref szError);
                        if (!string.IsNullOrEmpty(szError))
                        {
                            nPos = szError.IndexOf("data to decode");
                            if (nPos != -1)
                            {
                                szDecriptedData = string.Empty;
                            }
                            else
                            {
                                szDecriptedData = string.Format("******");
                            }
                        }
                        else
                        {
                            //Mask the card number
                            nHas = szDecriptedData.Length;
                            if (nHas > 4)
                            {
                                szMsg = szDecriptedData.Substring(nHas - 4, 4);
                                szDecriptedData = string.Format("******{0}", szMsg);
                            }
                            else
                            {
                                szDecriptedData = string.Format("******");
                            }
                        }
                        //payment.CreditCardNumber = szDecriptedData;
                        payment.CreditCardNumber = szEncriptedData;
                    }
                }
            }

            if (ModelState.IsValid)
            {

                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("FDZPayment", "Payment", new { id = payment.Id, invoicepayment = "true" });
            }


            return RedirectToAction("Index");
        }

        //
        // GET: //GetCards
        public PartialViewResult GetCards(string customerNo, string PaymentType)
        {
            int nCustomerId = 0;
            int nPos = 0;
            int nHas = 0;
            string szMsg = string.Empty;
            string szError = string.Empty;
            string szDecriptedData = string.Empty;

            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();


            //Get the Credit Card Number
            listSelector = new List<KeyValuePair<string, string>>();
            if (string.IsNullOrEmpty(PaymentType))
            {
                var qryPayment =
                    db.CustomersCreditCardShippings.Join(
                        db.Customers,
                        ctcc => ctcc.CustomerId,
                        cstm => cstm.Id,
                        (ctcc, cstm) => new { ctcc, cstm })
                        .OrderBy(Nctad => Nctad.ctcc.CardType)
                        .ThenBy(Nctad => Nctad.ctcc.CreditNumber)
                        .Where(Nctad => Nctad.cstm.CustomerNo == customerNo);

                if (qryPayment.Count() > 0)
                {
                    foreach (var item in qryPayment)
                    {
                        if (nCustomerId == 0)
                        {
                            nCustomerId = item.cstm.Id;
                        }

                        //Decode card number
                        szError = string.Empty;
                        szDecriptedData = DecodeInfo02(item.ctcc.CreditNumber, ref szError);
                        if (!string.IsNullOrEmpty(szError))
                        {
                            nPos = szError.IndexOf("data to decode");
                            if (nPos != -1)
                            {
                                szDecriptedData = string.Empty;
                            }
                            else
                            {
                                szDecriptedData = string.Format("******");
                            }
                        }
                        else
                        {
                            //Mask the card number
                            nHas = szDecriptedData.Length;
                            if (nHas > 4)
                            {
                                szMsg = szDecriptedData.Substring(nHas - 4, 4);
                                szDecriptedData = string.Format("******{0}", szMsg);
                            }
                            else
                            {
                                szDecriptedData = string.Format("******");
                            }
                        }


                        szMsg = string.Format("{0} - {1}", item.ctcc.CardType, szDecriptedData);
                        szMsg = item.ctcc.Id.ToString();
                        listSelector.Add(new KeyValuePair<string, string>(szMsg, szDecriptedData));
                    }
                }
            }
            else
            {
                var qryPayment = db.CustomersCreditCardShippings.Join(db.Customers, ctcc => ctcc.CustomerId, cstm => cstm.Id, (ctcc, cstm)
                    => new { ctcc, cstm }).OrderBy(Nctad => Nctad.ctcc.CardType).ThenBy(Nctad => Nctad.ctcc.CreditNumber).Where(Nctad => Nctad.cstm.CustomerNo == customerNo && Nctad.ctcc.CardType == PaymentType);

                if (qryPayment.Count() > 0)
                {
                    foreach (var item in qryPayment)
                    {
                        if (nCustomerId == 0)
                        {
                            nCustomerId = item.cstm.Id;
                        }

                        //Decode card number
                        szError = string.Empty;
                        szDecriptedData = DecodeInfo02(item.ctcc.CreditNumber, ref szError);
                        if (!string.IsNullOrEmpty(szError))
                        {
                            nPos = szError.IndexOf("data to decode");
                            if (nPos != -1)
                            {
                                szDecriptedData = string.Empty;
                            }
                            else
                            {
                                szDecriptedData = string.Format("******");
                            }
                        }
                        else
                        {
                            //Mask the card number
                            nHas = szDecriptedData.Length;
                            if (nHas > 4)
                            {
                                szMsg = szDecriptedData.Substring(nHas - 4, 4);
                                szDecriptedData = string.Format("******{0}", szMsg);
                            }
                            else
                            {
                                szDecriptedData = string.Format("******");
                            }
                        }


                        szMsg = string.Format("{0} - {1}", item.ctcc.CardType, szDecriptedData);
                        szMsg = item.ctcc.Id.ToString();
                        listSelector.Add(new KeyValuePair<string, string>(szMsg, szDecriptedData));
                    }
                }

            }
            SelectList paymentselectorlist = new SelectList(listSelector, "Key", "Value");
            ViewBag.PaymentList = paymentselectorlist;

            return PartialView();
        }

        //
        // GET //SelectInvoicePayment
        [NoCache]
        public ActionResult SelectInvoicePayment(int id = 0)
        {
            double dSalesAmount = 0;
            double dTax = 0;
            double dTotalTax = 0;
            double dTotalAmount = 0;
            double dBalanceDue = 0;

            Invoice invoice = null;
            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            Payments payment = db.Payments.Find(id);
            if (payment != null)
            {
                invoice = db.Invoices.Where(inv => inv.SalesOrderNo == payment.SalesOrderNo).FirstOrDefault<Invoice>();
                if (invoice != null)
                {
                    if (string.IsNullOrEmpty(invoice.InvoiceNo))
                    {
                        return RedirectToAction("Index");
                    }
                    ViewBag.InvoiceNo = invoice.InvoiceNo;

                    //Get the totals
                    InvoiceController invoiceCtrl = new InvoiceController();
                    invoiceCtrl.GetInvoiceTotals(invoice.InvoiceId, ref dSalesAmount, ref dTotalTax, ref dTax, ref dTotalAmount, ref dBalanceDue);
                    payment.Amount = Convert.ToDecimal(dTotalAmount);


                    //Get Payment Type
                    listSelector = new List<KeyValuePair<string, string>>();
                    var qryPaymentType = db.CustomersCardTypes.OrderBy(cdty => cdty.CardType);
                    if (qryPaymentType.Count() > 0)
                    {
                        foreach (var item in qryPaymentType)
                        {
                            listSelector.Add(new KeyValuePair<string, string>(item.CardType, item.CardType));
                        }
                    }
                    SelectList paymentTypeselectorlist = new SelectList(listSelector, "Key", "Value");
                    ViewBag.PaymentType = paymentTypeselectorlist;

                }

                return View(payment);
            }

            return RedirectToAction("Index");

        }

        //
        // GET://PayInvoice
        public ActionResult PayInvoice(int id = 0)
        {
            int nPaymentNo = 0;
            int nCustomerId = 0;
            int nPos = -1;
            int nHas = 0;
            double dSalesAmount = 0;
            double dTax = 0;
            double dTotalTax = 0;
            double dTotalAmount = 0;
            double dBalanceDue = 0;
            string szError = string.Empty;
            string szDecriptedData = string.Empty;
            string szMsg = string.Empty;
            string szCustomerNo = string.Empty;

            Payments payment = null;

            InitialInfo initialinfo = null;


            Customers customer = null;
            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            //Get sales order
            Invoice salesorder = db.Invoices.Find(id);
            if (salesorder != null)
            {
                //Get the totals
                GetSalesOrderTotals(salesorder.InvoiceId, ref dSalesAmount, ref dTotalTax, ref dTax, ref dTotalAmount, ref dBalanceDue);

                customer = db.Customers.Where(cst => cst.Id == salesorder.CustomerId).FirstOrDefault<Customers>();
                if (customer != null)
                {
                    //Get the next payment No
                    initialinfo = db.InitialInfoes.FirstOrDefault<InitialInfo>();
                    if (initialinfo == null)
                    {
                        initialinfo = new InitialInfo();
                        initialinfo.InvoiceNo = 0;
                        initialinfo.PaymentNo = 1;
                        initialinfo.PurchaseOrderNo = 0;
                        initialinfo.SalesOrderNo = 0;
                        initialinfo.TaxRate = 0;
                        db.InitialInfoes.Add(initialinfo);
                    }
                    else
                    {
                        nPaymentNo = initialinfo.PaymentNo;
                        nPaymentNo++;
                        initialinfo.PaymentNo = nPaymentNo;
                        db.Entry(initialinfo).State = EntityState.Modified;
                    }
                    db.SaveChanges();


                    //Get Payment Type
                    listSelector = new List<KeyValuePair<string, string>>();
                    var qryPaymentType = db.TransactionCodes.OrderBy(cdty => cdty.CodeDescription);
                    if (qryPaymentType.Count() > 0)
                    {
                        foreach (var item in qryPaymentType)
                        {
                            if (item.TransactionCode != 3)
                                listSelector.Add(new KeyValuePair<string, string>(item.CodeDescription, item.CodeDescription));
                        }
                    }
                    SelectList paymentTypeselectorlist = new SelectList(listSelector, "Key", "Value");

                    //Get the Credit Card Number for each credit card
                    listSelector = new List<KeyValuePair<string, string>>();
                    if (paymentTypeselectorlist.Count() > 0)
                    {
                        foreach (var item in paymentTypeselectorlist)
                        {
                            var qryPayment = db.CustomersCreditCardShippings.Join(db.Customers, ctcc => ctcc.CustomerId, cstm => cstm.Id, (ctcc, cstm)
                                 => new { ctcc, cstm }).OrderBy(Nctad => Nctad.ctcc.CardType).ThenBy(Nctad => Nctad.ctcc.CreditNumber).Where(Nctad => Nctad.ctcc.CustomerId == customer.Id && Nctad.ctcc.CardType == item.Value);
                            if (qryPayment.Count() > 0)
                            {
                                foreach (var itemCard in qryPayment)
                                {
                                    if (nCustomerId == 0)
                                    {
                                        nCustomerId = itemCard.cstm.Id;
                                    }

                                    //Decode card number
                                    szError = string.Empty;
                                    szDecriptedData = TimelyDepotMVC.Controllers.PaymentController.DecodeInfo02(itemCard.ctcc.CreditNumber, ref szError);
                                    if (!string.IsNullOrEmpty(szError))
                                    {
                                        nPos = szError.IndexOf("data to decode");
                                        if (nPos != -1)
                                        {
                                            szDecriptedData = string.Empty;
                                        }
                                        else
                                        {
                                            szDecriptedData = string.Format("******");
                                        }
                                    }
                                    else
                                    {
                                        //Mask the card number
                                        nHas = szDecriptedData.Length;
                                        if (nHas > 4)
                                        {
                                            szMsg = szDecriptedData.Substring(nHas - 4, 4);
                                            szDecriptedData = string.Format("******{0}", szMsg);
                                        }
                                        else
                                        {
                                            szDecriptedData = string.Format("******");
                                        }
                                    }


                                    szMsg = string.Format("{0} - {1}", itemCard.ctcc.CardType, szDecriptedData);
                                    listSelector.Add(new KeyValuePair<string, string>(itemCard.ctcc.CreditNumber, szMsg));
                                }
                            }

                        }
                    }

                    SelectList cardsselectorlist = new SelectList(listSelector, "Key", "Value");
                    ViewBag.CardsList = cardsselectorlist;

                    payment = new Payments();
                    payment.PaymentNo = nPaymentNo.ToString();
                    payment.PaymentDate = DateTime.Now;
                    payment.CustomerNo = customer.CustomerNo;
                    payment.SalesOrderNo = salesorder.SalesOrderNo;
                    payment.Amount = Convert.ToDecimal(dBalanceDue);
                    db.Payments.Add(payment);
                    db.SaveChanges();

                }
            }

            return RedirectToAction("SelectInvoicePayment", "Payment", new { id = payment.Id });
        }

        // 
        // GET: //PaySalesOrder
        public ActionResult PaySalesOrder(int id = 0)
        {
            int nPaymentNo = 0;
            int nCustomerId = 0;
            int nPos = -1;
            int nHas = 0;
            double dSalesAmount = 0;
            double dTax = 0;
            double dTotalTax = 0;
            double dTotalAmount = 0;
            double dBalanceDue = 0;
            string szError = string.Empty;
            string szDecriptedData = string.Empty;
            string szMsg = string.Empty;
            string szCustomerNo = string.Empty;

            Payments payment = null;

            InitialInfo initialinfo = null;


            Customers customer = null;
            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            //Get sales order
            SalesOrder salesorder = db.SalesOrders.Find(id);
            if (salesorder != null)
            {
                //Get the totals
                GetSalesOrderTotals(salesorder.SalesOrderId, ref dSalesAmount, ref dTotalTax, ref dTax, ref dTotalAmount, ref dBalanceDue);

                customer = db.Customers.Where(cst => cst.Id == salesorder.CustomerId).FirstOrDefault<Customers>();
                if (customer != null)
                {
                    //Get the next payment No
                    initialinfo = db.InitialInfoes.FirstOrDefault<InitialInfo>();
                    if (initialinfo == null)
                    {
                        initialinfo = new InitialInfo();
                        initialinfo.InvoiceNo = 0;
                        initialinfo.PaymentNo = 1;
                        initialinfo.PurchaseOrderNo = 0;
                        initialinfo.SalesOrderNo = 0;
                        initialinfo.TaxRate = 0;
                        db.InitialInfoes.Add(initialinfo);
                    }
                    else
                    {
                        nPaymentNo = initialinfo.PaymentNo;
                        nPaymentNo++;
                        initialinfo.PaymentNo = nPaymentNo;
                        db.Entry(initialinfo).State = EntityState.Modified;
                    }
                    db.SaveChanges();

                    //Get Payment Type
                    listSelector = new List<KeyValuePair<string, string>>();
                    var qryPaymentType = db.CustomersCardTypes.OrderBy(cdty => cdty.CardType);
                    if (qryPaymentType.Count() > 0)
                    {
                        foreach (var item in qryPaymentType)
                        {
                            listSelector.Add(new KeyValuePair<string, string>(item.CardType, item.CardType));
                        }
                    }
                    SelectList paymentTypeselectorlist = new SelectList(listSelector, "Key", "Value");

                    //Get the Credit Card Number for each credit card
                    listSelector = new List<KeyValuePair<string, string>>();
                    if (paymentTypeselectorlist.Count() > 0)
                    {
                        foreach (var item in paymentTypeselectorlist)
                        {
                            var qryPayment = db.CustomersCreditCardShippings.Join(db.Customers, ctcc => ctcc.CustomerId, cstm => cstm.Id, (ctcc, cstm)
                                 => new { ctcc, cstm }).OrderBy(Nctad => Nctad.ctcc.CardType).ThenBy(Nctad => Nctad.ctcc.CreditNumber).Where(Nctad => Nctad.ctcc.CustomerId == customer.Id && Nctad.ctcc.CardType == item.Value);
                            if (qryPayment.Count() > 0)
                            {
                                foreach (var itemCard in qryPayment)
                                {
                                    if (nCustomerId == 0)
                                    {
                                        nCustomerId = itemCard.cstm.Id;
                                    }

                                    //Decode card number
                                    szError = string.Empty;
                                    szDecriptedData = TimelyDepotMVC.Controllers.PaymentController.DecodeInfo02(itemCard.ctcc.CreditNumber, ref szError);
                                    if (!string.IsNullOrEmpty(szError))
                                    {
                                        nPos = szError.IndexOf("data to decode");
                                        if (nPos != -1)
                                        {
                                            szDecriptedData = string.Empty;
                                        }
                                        else
                                        {
                                            szDecriptedData = string.Format("******");
                                        }
                                    }
                                    else
                                    {
                                        //Mask the card number
                                        nHas = szDecriptedData.Length;
                                        if (nHas > 4)
                                        {
                                            szMsg = szDecriptedData.Substring(nHas - 4, 4);
                                            szDecriptedData = string.Format("******{0}", szMsg);
                                        }
                                        else
                                        {
                                            szDecriptedData = string.Format("******");
                                        }
                                    }


                                    szMsg = string.Format("{0} - {1}", itemCard.ctcc.CardType, szDecriptedData);
                                    listSelector.Add(new KeyValuePair<string, string>(itemCard.ctcc.CreditNumber, szMsg));
                                }
                            }

                        }
                    }

                    SelectList cardsselectorlist = new SelectList(listSelector, "Key", "Value");
                    ViewBag.CardsList = cardsselectorlist;

                    payment = new Payments();
                    payment.PaymentNo = nPaymentNo.ToString();
                    payment.PaymentDate = DateTime.Now;
                    payment.CustomerNo = customer.CustomerNo;
                    payment.SalesOrderNo = salesorder.SalesOrderNo;
                    payment.Amount = Convert.ToDecimal(dBalanceDue);
                    db.Payments.Add(payment);
                    db.SaveChanges();

                    return RedirectToAction("SelectPayment", "Payment", new { id = payment.Id });
                }
            }
            return RedirectToAction("Index");
        }

        //
        // GET: //ViewLog
        [NoCache]
        public PartialViewResult ViewLog(string id)
        {
            int nId = 0;
            int nPos = 0;
            int nHas = 0;
            string szMsg = string.Empty;
            string szError = string.Empty;
            string szDecriptedData = string.Empty;

            Payments payment = null;
            Invoice invoice = null;

            XmlDocument xmlDoc = null;

            if (!string.IsNullOrEmpty(id))
            {
                nId = Convert.ToInt32(id);
            }

            payment = db.Payments.Find(nId);

            //Get the required data
            if (payment != null)
            {
                szDecriptedData = TimelyDepotMVC.Controllers.PaymentController.DecodeInfo02(payment.CreditCardNumber, ref szError);
                if (!string.IsNullOrEmpty(szError))
                {
                    nPos = szError.IndexOf("data to decode");
                    if (nPos != -1)
                    {
                        szDecriptedData = string.Empty;
                    }
                    else
                    {
                        szDecriptedData = string.Format("******");
                    }
                }
                else
                {
                    //Mask the card number
                    nHas = szDecriptedData.Length;
                    if (nHas > 4)
                    {
                        szMsg = szDecriptedData.Substring(nHas - 4, 4);
                        szDecriptedData = string.Format("******{0}", szMsg);
                    }
                    else
                    {
                        szDecriptedData = string.Format("******");
                    }
                }
                payment.CreditCardNumber = szDecriptedData;

                //Get the invoice
                invoice = db.Invoices.Where(invc => invc.SalesOrderNo == payment.SalesOrderNo).FirstOrDefault<Invoice>();
                if (invoice != null)
                {
                    ViewBag.InvoiceNo = invoice.InvoiceNo;
                }

                //Get the xml data form the payment log string
                if (!string.IsNullOrEmpty(payment.PayLog))
                {
                    try
                    {
                        xmlDoc = new XmlDocument();
                        //xmlDoc.LoadXml("<item><name>wrench</name></item>");

                        XmlElement xmlElement = xmlDoc.CreateElement("gge4item");

                        xmlElement.InnerXml = payment.PayLog;

                        xmlDoc.InnerXml = xmlElement.OuterXml;

                        //xmlDoc.DocumentElement.AppendChild(xmlElement);

                        //XmlNodeList xmlNodeList = xmlDoc.DocumentElement.GetElementsByTagName("gge4item");
                        XmlNode root = xmlDoc.FirstChild;

                        XmlNodeList xmlNodeList = root.ChildNodes;
                        if (xmlNodeList != null)
                        {
                            ViewBag.ElChildsList = xmlNodeList;

                        }
                    }
                    catch (XmlException errxml)
                    {
                        szError = errxml.Message;
                    }
                    catch (Exception err)
                    {
                        szError = err.Message;
                    }
                }
            }

            return PartialView(payment);
        }

        // 
        // GET: //FDZPayment
        public ActionResult FDZPayment(string id, string invoicepayment, decimal paymentAmount, string refund)
        {
            int nHas = 0;
            int nPos = -1;
            double dSalesAmount = 0;
            double dTax = 0;
            double dTotalTax = 0;
            double dTotalAmount = 0;
            double dBalanceDue = 0;
            string szError = string.Empty;
            string szDecriptedData = string.Empty;
            string szDecriptedData02 = string.Empty;
            string szMsg = string.Empty;

            int nId = 0;
            string szYear = string.Empty;
            decimal dTotalSalesOrder = 0;
            DateTime dDate = DateTime.Now;


            TimelyDepotContext db01 = new TimelyDepotContext();

            Payments payment = null;
            Invoice invoice = null;
            SalesOrder salesOrder = null;
            Customers customer = null;
            CustomersCreditCardShipping creditcard = null;
            IQueryable<CustomersCreditCardShipping> qryCard = null;

            if (!string.IsNullOrEmpty(id))
            {
                nId = Convert.ToInt32(id);
            }
            payment = db.Payments.Find(nId);

            //There is no more payment wizard process
            TempData["Status"] = null;

            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            //Set the available transactions
            listSelector.Add(new KeyValuePair<string, string>("00", "Purchase"));
            listSelector.Add(new KeyValuePair<string, string>("04", "Refund"));
            listSelector.Add(new KeyValuePair<string, string>("13", "Void"));
            SelectList transactionlist = new SelectList(listSelector, "Key", "Value");
            ViewBag.TransactionList = transactionlist;

            //Get the required data
            if (payment != null)
            {
                if (string.IsNullOrEmpty(invoicepayment))
                {
                    salesOrder = db.SalesOrders.Where(slor => slor.SalesOrderNo == payment.SalesOrderNo).FirstOrDefault<SalesOrder>();
                    if (salesOrder != null)
                    {
                        SalesOrderController salesctrl = new SalesOrderController();
                        dTotalSalesOrder = salesctrl.GetSalesOrderAmount(db01, salesOrder.SalesOrderId);

                        if (!string.IsNullOrEmpty(refund))
                        {
                            if (refund.ToUpper() == "REFUND")
                            {
                                dTotalSalesOrder = Convert.ToDecimal(payment.Amount);
                            }
                        }
                    }
                }
                else
                {
                    salesOrder = db.SalesOrders.Where(slor => slor.SalesOrderNo == payment.SalesOrderNo).FirstOrDefault<SalesOrder>();
                    if (salesOrder != null)
                    {
                        invoice = db.Invoices.Where(inv => inv.SalesOrderNo == salesOrder.SalesOrderNo).FirstOrDefault<Invoice>();
                        if (invoice != null)
                        {
                            //Get the totals
                            InvoiceController invoiceCtrl = new InvoiceController();
                            invoiceCtrl.GetInvoiceTotals(invoice.InvoiceId, ref dSalesAmount, ref dTotalTax, ref dTax, ref dTotalAmount, ref dBalanceDue);
                            dTotalSalesOrder = Convert.ToDecimal(dTotalAmount);

                            if (!string.IsNullOrEmpty(refund))
                            {
                                if (refund.ToUpper() == "REFUND")
                                {
                                    dTotalSalesOrder = Convert.ToDecimal(payment.Amount);
                                }
                            }
                        }
                    }
                }

                customer = db.Customers.Where(cst => cst.CustomerNo == payment.CustomerNo).FirstOrDefault<Customers>();
                if (customer != null)
                {
                    qryCard = db.CustomersCreditCardShippings.Where(cstcrd => cstcrd.CustomerId == customer.Id && cstcrd.CardType == payment.PaymentType);
                    if (qryCard.Count() > 0)
                    {
                        foreach (var item in qryCard)
                        {
                            if (item.CreditNumber == payment.CreditCardNumber)
                            {
                                creditcard = item;
                                ViewBag.CardType = item.CardType;
                                ViewBag.CardAddress = string.Format("{0} {1}", item.Address1, item.Address2);
                                ViewBag.ZipAddress = item.Zip;
                                ViewBag.ExpireDateAddress = string.Format("{0}", Convert.ToDateTime(item.ExpirationDate).ToString("MM/dd/yyyy"));
                                break;
                            }
                        }
                    }
                }

                if (salesOrder != null && creditcard != null)
                {
                    szDecriptedData = DecodeInfo02(creditcard.CreditNumber, ref szError);
                    szDecriptedData02 = szDecriptedData;
                    if (!string.IsNullOrEmpty(szError))
                    {
                        nPos = szError.IndexOf("data to decode");
                        if (nPos != -1)
                        {
                            szDecriptedData = string.Empty;
                        }
                        else
                        {
                            szDecriptedData = string.Format("******");
                        }
                    }
                    else
                    {
                        //Mask the card number
                        nHas = szDecriptedData.Length;
                        if (nHas > 4)
                        {
                            szMsg = szDecriptedData.Substring(nHas - 4, 4);
                            szDecriptedData = string.Format("******{0}", szMsg);
                        }
                        else
                        {
                            szDecriptedData = string.Format("******");
                        }
                    }

                    if (creditcard.ExpirationDate != null)
                    {
                        dDate = Convert.ToDateTime(creditcard.ExpirationDate);
                    }

                }

                if (!string.IsNullOrEmpty(refund))
                {
                    if (refund.ToUpper() == "REFUND")
                    {
                        ViewBag.Transaction = "Refund";
                    }
                }
                else
                {
                    ViewBag.Transaction = "Purchase";
                }

                ViewBag.Amount = paymentAmount.ToString(CultureInfo.InvariantCulture);
                szYear = dDate.Year.ToString();
                ViewBag.expiryDate = string.Format("{0}{1}", dDate.Month.ToString("00"), szYear.Substring(2, 2));
                ViewBag.CardHolderName = creditcard.Name;
                ViewBag.CardNumber = szDecriptedData;
                ViewBag.CardNumber02 = creditcard.Id;
                ViewBag.PaymentId = payment.Id;
                ViewBag.SalesOrderNo = payment.SalesOrderNo;

                ViewBag.InvoicePayment = invoicepayment;

                if (string.IsNullOrEmpty(invoicepayment))
                {
                    ViewBag.PaymentTitle = string.Format("Sales Order No: {0}", payment.SalesOrderNo);
                }
                else
                {
                    ViewBag.PaymentTitle = string.Format("Invoice No: {0}", invoice.InvoiceNo);

                }

            }

            return View();
        }

        //
        // GET: //GetRestData02
        [NoCache]
        [ValidateInput(false)]
        public PartialViewResult GetRestData02()
        {
            bool bApproved = true;
            int nId = 0;
            decimal dAmount = 0;
            string szMsg = string.Empty;
            string szInvoicePayment = string.Empty;
            string Authorization_Num = string.Empty;
            string szTransaction_Approved = string.Empty;
            string szTransaction_Type = string.Empty;
            string szSalesOrderNo = string.Empty;
            string szPaymentid = string.Empty;
            string szAmount = string.Empty;
            string szExpiryDate = string.Empty;
            string szCardHoldersName = string.Empty;
            string szCardNumber = string.Empty;
            string szCardNumber02 = string.Empty;
            string szAuthorizationNumber = string.Empty;
            string szTransaction = string.Empty;
            string szError = string.Empty;
            string szRequest = string.Empty;
            string szResponse = string.Empty;
            string szNodeList = string.Empty;
            XmlNodeList xmlchilds = null;

            Payments payment = null;
            SalesOrder salesorder = null;
            Invoice invoice = null;

            szError = "Loading data...";

            if (Request.QueryString["amount"] != null)
            {
                szAmount = Request.QueryString["amount"];
            }
            if (Request.QueryString["expiryDate"] != null)
            {
                szExpiryDate = Request.QueryString["expiryDate"];
            }
            if (Request.QueryString["cardHoldersName"] != null)
            {
                szCardHoldersName = Request.QueryString["cardHoldersName"];
            }
            if (Request.QueryString["cardNumber"] != null)
            {
                szCardNumber = Request.QueryString["cardNumber"];
            }
            if (Request.QueryString["cardNumber02"] != null)
            {
                szCardNumber02 = Request.QueryString["cardNumber02"];
            }
            if (Request.QueryString["authorizationNumber"] != null)
            {
                szAuthorizationNumber = Request.QueryString["authorizationNumber"];
            }
            if (Request.QueryString["transaction"] != null)
            {
                szTransaction = Request.QueryString["transaction"];
            }
            if (Request.QueryString["paymentid"] != null)
            {
                szPaymentid = Request.QueryString["paymentid"];
            }
            if (Request.QueryString["invoicepayment"] != null)
            {
                szInvoicePayment = Request.QueryString["invoicepayment"];
            }

            //Get transaction response
            GetRestData02(szTransaction, szAmount, szExpiryDate, szCardHoldersName, szCardNumber, szCardNumber02, szAuthorizationNumber, szPaymentid, ref szError, ref szRequest, ref szResponse, ref xmlchilds);

            ViewBag.ElError = szError;


            ViewBag.ElRequest = szRequest;

            ViewBag.ElResponse = szResponse;
            ViewBag.ElChildsList = xmlchilds;

            //Get the transaction REsults data
            XmlNodeList xmlNodeList = xmlchilds;

            if (xmlNodeList != null)
            {
                foreach (XmlNode item in xmlNodeList)
                {
                    if (item.Name == "Authorization_Num")
                    {
                        szAuthorizationNumber = item.InnerText;
                    }
                    if (item.Name == "Transaction_Approved")
                    {
                        szTransaction_Approved = item.InnerText;
                    }
                    if (item.Name == "Transaction_Type")
                    {
                        szTransaction_Type = item.InnerText;
                    }

                    if (string.IsNullOrEmpty(szMsg))
                    {
                        szMsg = item.OuterXml;
                    }
                    else
                    {
                        szMsg = string.Format("{0}{1}", szMsg, item.OuterXml);
                    }
                }
            }
            else
            {
                szAuthorizationNumber = string.Format("Approved: {0}", "false");
                szMsg = szError;
                szError = string.Empty;
            }


            //Update payment
            if (string.IsNullOrEmpty(szError))
            {
                if (szAuthorizationNumber.Contains("false"))
                {
                    bApproved = false;
                }

                if (!string.IsNullOrEmpty(szPaymentid))
                {
                    nId = Convert.ToInt32(szPaymentid);
                    payment = db.Payments.Find(nId);
                    if (payment != null)
                    {
                        //Update payment
                        if (!string.IsNullOrEmpty(szAmount))
                        {
                            if (bApproved)
                            {
                                szAmount = szAmount.Replace('.', ',');
                                dAmount = Convert.ToDecimal(szAmount);
                                payment.Amount = dAmount;
                                payment.ReferenceNo = szAuthorizationNumber;
                                if (!string.IsNullOrEmpty(szInvoicePayment))
                                {
                                    payment.InvoicePayment = "True";
                                }
                            }
                            payment.PayLog = szMsg;
                            db.Entry(payment).State = EntityState.Modified;
                        }


                        if (bApproved)
                        {
                            szSalesOrderNo = payment.SalesOrderNo;

                            //Update the Sales order
                            if (string.IsNullOrEmpty(szInvoicePayment))
                            {
                                salesorder = db.SalesOrders.Where(slod => slod.SalesOrderNo == payment.SalesOrderNo).FirstOrDefault<SalesOrder>();
                                if (salesorder != null)
                                {
                                    if (szTransaction_Type == "00")
                                    {
                                        salesorder.PaymentAmount = Convert.ToDecimal(salesorder.PaymentAmount) + Convert.ToDecimal(payment.Amount);
                                        salesorder.PaymentDate = Convert.ToDateTime(payment.PaymentDate);
                                        db.Entry(salesorder).State = EntityState.Modified;

                                    }
                                    //db.SaveChanges();
                                }

                            }
                            else
                            {
                                if (szInvoicePayment.ToUpper() == "TRUE")
                                {
                                    //Update the invoice
                                    invoice = db.Invoices.Where(invc => invc.SalesOrderNo == szSalesOrderNo).FirstOrDefault<Invoice>();
                                    if (invoice != null)
                                    {
                                        if (szTransaction_Type == "00")
                                        {
                                            invoice.PaymentAmount = Convert.ToDecimal(invoice.PaymentAmount) + Convert.ToDecimal(payment.Amount);
                                            invoice.PaymentDate = Convert.ToDateTime(payment.PaymentDate);
                                            db.Entry(invoice).State = EntityState.Modified;

                                        }
                                        //db.SaveChanges();
                                    }

                                }
                            }
                        }

                        db.SaveChanges();

                        //Delete the payment when refund
                        if (szTransaction_Type == "04")
                        {
                   
                            var aRefund = new Refunds()
                                              {
                                                  RefundAmount = (decimal)payment.Amount,
                                                  Refunddate = DateTime.Now,
                                                  TransactionId = payment.Id,
                                                  SalesOrderNo = payment.SalesOrderNo,
                                                  CustomerNo = payment.CustomerNo
                                              };
                            this.db.Refunds.Add(aRefund);
                            db.SaveChanges();
                        }
                    }
                }
            }
            else
            {
                payment.Amount = null;
                payment.ReferenceNo = szAuthorizationNumber;
                if (!string.IsNullOrEmpty(szInvoicePayment))
                {
                    payment.InvoicePayment = "True";
                }
                if (string.IsNullOrEmpty(szError))
                {
                    payment.PayLog = szMsg;
                }
                else
                {
                    payment.PayLog = szError;
                }
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();

            }

            return PartialView();
        }

        private void GetRestData02(string szTransaction, string szAmount, string szExpiryDate, string szCardHoldersName, string szCardNumber, string szCardNumber02, string szAuthorizationNumber, string szPaymentid, ref string szError, ref string szRequest, ref string szResponse, ref XmlNodeList xmlchilds)
        {
            int nId = 0;
            int nHas = 0;
            decimal dHlp = 0;
            string szCreditCard = string.Empty;
            string response_string = string.Empty;


            if (!string.IsNullOrEmpty(szCardNumber02))
            {
                nId = Convert.ToInt32(szCardNumber02);
            }
            int paymentId = int.Parse(szPaymentid);
            var paymentData = this.db.Payments.SingleOrDefault(x => x.Id == paymentId);
            
            var environmentParam = this.db.EnvironmentParameters.SingleOrDefault(x => x.Active);
            CustomersCreditCardShipping creditcard = db.CustomersCreditCardShippings.Find(nId);
          
            if (creditcard != null)
            {
                szCreditCard = DecodeInfo02(creditcard.CreditNumber, ref szError); ;
            }

            if (szCardNumber.Contains("*"))
            {
                szCardNumber = szCreditCard;
            }

            szError = string.Empty;
            //XmlNodeList xmlchilds = null;

            szResponse = string.Empty;
            szResponse = string.Empty;

            try
            {
                szAmount = szAmount.Replace(",", ".");
                //dHlp = Convert.ToDecimal(szAmount);
                //szAmount = dHlp.ToString("N2");
            }
            catch (Exception err)
            {
                szError = err.Message;
                return;
            }

            if (szExpiryDate.Length > 4)
            {
                szExpiryDate = szExpiryDate.Substring(0, 4);
            }

            StringBuilder string_builder = new StringBuilder();
            using (StringWriter string_writer = new StringWriter(string_builder))
            {
                using (XmlTextWriter xml_writer = new XmlTextWriter(string_writer))
                {     //build XML string 
                    xml_writer.Formatting = Formatting.Indented;
                    xml_writer.WriteStartElement("Transaction");
                    xml_writer.WriteElementString("ExactID", "AF2940-05");//Gateway ID
                    xml_writer.WriteElementString("Password", "q515t487");//Password

                    xml_writer.WriteElementString("Transaction_Type", szTransaction);
                    //xml_writer.WriteElementString("Transaction_Type", "55");
                    xml_writer.WriteElementString("DollarAmount", szAmount);
                    xml_writer.WriteElementString("Expiry_Date", szExpiryDate);
                    xml_writer.WriteElementString("CardHoldersName", szCardHoldersName);
                    xml_writer.WriteElementString("Card_Number", szCardNumber);
                    xml_writer.WriteEndElement();
                }
            }
            string xml_string = string_builder.ToString();

            //SHA1 hash on XML string
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] xml_byte = encoder.GetBytes(xml_string);
            SHA1CryptoServiceProvider sha1_crypto = new SHA1CryptoServiceProvider();
            string hash = BitConverter.ToString(sha1_crypto.ComputeHash(xml_byte)).Replace("-", string.Empty);
            string hashed_content = hash.ToLower();

            //assign values to hashing and header variables
            string keyID = "143876";//key ID
            string key = "d8H7UNVW_nFrLP3uuPwPSg2B2y1JhXu1";//Hmac key
            string method = "POST\n";
            string type = "application/xml";//REST XML
            string time = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string uri = "/transaction/v13";
            string hash_data = method + type + "\n" + hashed_content + "\n" + time + "\n" + uri;
            //hmac sha1 hash with key + hash_data
            HMAC hmac_sha1 = new HMACSHA1(Encoding.UTF8.GetBytes(key)); //key
            byte[] hmac_data = hmac_sha1.ComputeHash(Encoding.UTF8.GetBytes(hash_data)); //data
            //base64 encode on hmac_data
            string base64_hash = Convert.ToBase64String(hmac_data);

            //uri = "/transaction/v131";

            string url = "https://api.demo.globalgatewaye4.firstdata.com" + uri; //DEMO Endpoint

            //url = "invalid site";

            //get response and read into string
            try
            {

                //begin HttpWebRequest 
                HttpWebRequest web_request = (HttpWebRequest)WebRequest.Create(url);
                web_request.Method = "POST";
                web_request.ContentType = type;
                web_request.Accept = "*/*";
                web_request.Headers.Add("x-gge4-date", time);
                web_request.Headers.Add("x-gge4-content-sha1", hashed_content);
                web_request.Headers.Add("Authorization", "GGE4_API " + keyID + ":" + base64_hash);
                web_request.ContentLength = xml_string.Length;

                // write and send request data 
                using (StreamWriter stream_writer = new StreamWriter(web_request.GetRequestStream()))
                {
                    stream_writer.Write(xml_string);
                }

                using (HttpWebResponse web_response = (HttpWebResponse)web_request.GetResponse())
                {
                    using (StreamReader response_stream = new StreamReader(web_response.GetResponseStream()))
                    {
                        response_string = response_stream.ReadToEnd();

                    }

                    //load xml
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(response_string);

                    foreach (XmlElement child in xmldoc.SelectNodes("TransactionResult"))
                    {
                        var attributesHlp = child.Attributes;
                        var childnodesHlp = child.ChildNodes;

                        xmlchilds = child.ChildNodes;

                        //Console.WriteLine("child with id {0} has {1} child element(s).", child.GetAttribute("ID"), child.SelectNodes("*").Count);
                    }


                    XmlNodeList nodelist = xmldoc.SelectNodes("TransactionResult");

                    //szNodeList = nodelist.ToString();

                    //bind XML source DataList control
                    //DataList1.DataSource = nodelist;
                    //DataList1.DataBind();

                    //output raw XML for debugging
                    //request_label.Text = "<b>Request</b><br />" + web_request.Headers.ToString() + System.Web.HttpUtility.HtmlEncode(xml_string);
                    //response_label.Text = "<b>Response</b><br />" + web_response.Headers.ToString() + System.Web.HttpUtility.HtmlEncode(response_string);
                    szRequest = "<b>Request</b><br />" + web_request.Headers.ToString() + System.Web.HttpUtility.HtmlEncode(xml_string);
                    szResponse = "<b>Response</b><br />" + web_response.Headers.ToString() + System.Web.HttpUtility.HtmlEncode(response_string);

                }
            }
            //read stream for remote error response
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (HttpWebResponse error_response = (HttpWebResponse)ex.Response)
                    {
                        using (StreamReader reader = new StreamReader(error_response.GetResponseStream()))
                        {
                            string remote_ex = reader.ReadToEnd();
                            //error.Text = remote_ex;
                            szError = remote_ex;
                        }
                    }
                }

                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    //Console.WriteLine("Status Code : {0}", ((HttpWebResponse)ex.Response).StatusCode);
                    //Console.WriteLine("Status Description : {0}", ((HttpWebResponse)ex.Response).StatusDescription);
                    //szError = string.Format("{0} Status Code : {1} Status Description : {2}", szError, ((HttpWebResponse)ex.Response).StatusCode, ((HttpWebResponse)ex.Response).StatusDescription);

                    szError = string.Format("{0} - Message: {1} - StackTrace: {2}.", ex.Status, ex.Message, ex.StackTrace);
                }
                this.db.Payments.Remove(paymentData);
                this.db.SaveChanges();
            }
            catch (Exception err)
            {
                szError = err.Message;
                this.db.Payments.Remove(paymentData);
                this.db.SaveChanges();
            }
        }

        //
        // GET: //TestRest02
        public ActionResult TestRest02()
        {
            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            //qryTipoGasto = db.TipoGasto.OrderBy(tpgt => tpgt.Nombre);
            //if (qryTipoGasto.Count() > 0)
            //{
            //    foreach (var item in qryTipoGasto)
            //    {
            //        listSelector.Add(new KeyValuePair<string, string>(item.Nombre, item.Nombre));
            //    }
            //}

            //Set the available transactions
            listSelector.Add(new KeyValuePair<string, string>("00", "Purchase"));
            listSelector.Add(new KeyValuePair<string, string>("04", "Refund"));
            listSelector.Add(new KeyValuePair<string, string>("13", "Void"));
            SelectList transactionlist = new SelectList(listSelector, "Key", "Value");
            ViewBag.TransactionList = transactionlist;

            return View();
        }

        //
        // GET: //TestRest
        public ActionResult TestRest01()
        {
            string szError = string.Empty;
            string szRequest = string.Empty;
            string szResponse = string.Empty;
            string szNodeList = string.Empty;
            XmlNodeList xmlchilds = null;

            szError = "En proceso de obtener la respuesta";

            GetRestData01(ref szError, ref szRequest, ref szResponse, ref xmlchilds);

            if (!string.IsNullOrEmpty(szRequest))
            {
                TempData["ElRequest"] = szRequest;
            }
            if (!string.IsNullOrEmpty(szResponse))
            {
                TempData["ElResponse"] = szResponse;
            }

            if (xmlchilds != null)
            {
                TempData["ElChildsList"] = xmlchilds;
            }

            if (!string.IsNullOrEmpty(szNodeList))
            {
                TempData["ElNodeList"] = szNodeList;
            }

            if (!string.IsNullOrEmpty(szError))
            {
                TempData["ElError"] = szError;
            }

            return RedirectToAction("FDZTest");
        }

        private void GetRestData01(ref string szError, ref string szRequest, ref string szResponse, ref XmlNodeList xmlchilds)
        {
            string response_string = string.Empty;
            var environmentParam = this.db.EnvironmentParameters.SingleOrDefault(x => x.Active);
            szError = string.Empty;
            //XmlNodeList xmlchilds = null;

            szResponse = string.Empty;
            szResponse = string.Empty;

            StringBuilder string_builder = new StringBuilder();
            using (StringWriter string_writer = new StringWriter(string_builder))
            {
                using (XmlTextWriter xml_writer = new XmlTextWriter(string_writer))
                {     //build XML string 
                    xml_writer.Formatting = Formatting.Indented;
                    xml_writer.WriteStartElement("Transaction");
                    xml_writer.WriteElementString("ExactID", "AF2940-05");//Gateway ID
                    xml_writer.WriteElementString("Password", "q515t487");//Password
                    xml_writer.WriteElementString("Transaction_Type", "00");
                    //xml_writer.WriteElementString("Transaction_Type", "55");
                    xml_writer.WriteElementString("DollarAmount", "1.66");
                    xml_writer.WriteElementString("Expiry_Date", "1214");
                    xml_writer.WriteElementString("CardHoldersName", "C# REST Client");
                    xml_writer.WriteElementString("Card_Number", "4111111111111111");
                    xml_writer.WriteEndElement();
                }
            }
            string xml_string = string_builder.ToString();

            //SHA1 hash on XML string
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] xml_byte = encoder.GetBytes(xml_string);
            SHA1CryptoServiceProvider sha1_crypto = new SHA1CryptoServiceProvider();
            string hash = BitConverter.ToString(sha1_crypto.ComputeHash(xml_byte)).Replace("-", string.Empty);
            string hashed_content = hash.ToLower();

            //assign values to hashing and header variables
            string keyID = "143876";//key ID
            string key = "d8H7UNVW_nFrLP3uuPwPSg2B2y1JhXu1";//Hmac key
            string method = "POST\n";
            string type = "application/xml";//REST XML
            string time = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string uri = "/transaction/v13";
            string hash_data = method + type + "\n" + hashed_content + "\n" + time + "\n" + uri;
            //hmac sha1 hash with key + hash_data
            HMAC hmac_sha1 = new HMACSHA1(Encoding.UTF8.GetBytes(key)); //key
            byte[] hmac_data = hmac_sha1.ComputeHash(Encoding.UTF8.GetBytes(hash_data)); //data
            //base64 encode on hmac_data
            string base64_hash = Convert.ToBase64String(hmac_data);

            //uri = "/transaction/v131";

            string url = "https://api.demo.globalgatewaye4.firstdata.com" + uri; //DEMO Endpoint

            //url = "invalid site";

            //get response and read into string
            try
            {

                //begin HttpWebRequest 
                HttpWebRequest web_request = (HttpWebRequest)WebRequest.Create(url);
                web_request.Method = "POST";
                web_request.ContentType = type;
                web_request.Accept = "*/*";
                web_request.Headers.Add("x-gge4-date", time);
                web_request.Headers.Add("x-gge4-content-sha1", hashed_content);
                web_request.Headers.Add("Authorization", "GGE4_API " + keyID + ":" + base64_hash);
                web_request.ContentLength = xml_string.Length;

                // write and send request data 
                using (StreamWriter stream_writer = new StreamWriter(web_request.GetRequestStream()))
                {
                    stream_writer.Write(xml_string);
                }

                using (HttpWebResponse web_response = (HttpWebResponse)web_request.GetResponse())
                {
                    using (StreamReader response_stream = new StreamReader(web_response.GetResponseStream()))
                    {
                        response_string = response_stream.ReadToEnd();
                    }

                    //load xml
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(response_string);

                    foreach (XmlElement child in xmldoc.SelectNodes("TransactionResult"))
                    {
                        var attributesHlp = child.Attributes;
                        var childnodesHlp = child.ChildNodes;

                        xmlchilds = child.ChildNodes;

                        //Console.WriteLine("child with id {0} has {1} child element(s).", child.GetAttribute("ID"), child.SelectNodes("*").Count);
                    }


                    XmlNodeList nodelist = xmldoc.SelectNodes("TransactionResult");

                    //szNodeList = nodelist.ToString();

                    //bind XML source DataList control
                    //DataList1.DataSource = nodelist;
                    //DataList1.DataBind();

                    //output raw XML for debugging
                    //request_label.Text = "<b>Request</b><br />" + web_request.Headers.ToString() + System.Web.HttpUtility.HtmlEncode(xml_string);
                    //response_label.Text = "<b>Response</b><br />" + web_response.Headers.ToString() + System.Web.HttpUtility.HtmlEncode(response_string);
                    szRequest = "<b>Request</b><br />" + web_request.Headers.ToString() + System.Web.HttpUtility.HtmlEncode(xml_string);
                    szResponse = "<b>Response</b><br />" + web_response.Headers.ToString() + System.Web.HttpUtility.HtmlEncode(response_string);

                }
            }
            //read stream for remote error response
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (HttpWebResponse error_response = (HttpWebResponse)ex.Response)
                    {
                        using (StreamReader reader = new StreamReader(error_response.GetResponseStream()))
                        {
                            string remote_ex = reader.ReadToEnd();
                            //error.Text = remote_ex;
                            szError = remote_ex;
                            reader.Close();
                        }
                    }
                }

                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    //Console.WriteLine("Status Code : {0}", ((HttpWebResponse)ex.Response).StatusCode);
                    //Console.WriteLine("Status Description : {0}", ((HttpWebResponse)ex.Response).StatusDescription);
                    //szError = string.Format("{0} Status Code : {1} Status Description : {2}", szError, ((HttpWebResponse)ex.Response).StatusCode, ((HttpWebResponse)ex.Response).StatusDescription);

                    szError = string.Format("{0} - Message: {1} - StackTrace: {2}.", ex.Status, ex.Message, ex.StackTrace);
                }
            }
            catch (Exception err)
            {
                szError = err.Message;
            }
        }

        //
        // GET; /Payment/FDZTest
        public ActionResult FDZTest()
        {

            if (TempData["ElError"] != null)
            {
                ViewBag.ElError = TempData["ElError"].ToString();
            }

            if (TempData["ElRequest"] != null)
            {
                ViewBag.ElRequest = TempData["ElRequest"].ToString();
            }

            if (TempData["ElResponse"] != null)
            {
                ViewBag.ElResponse = TempData["ElResponse"].ToString();
            }

            if (TempData["ElChildsList"] != null)
            {
                ViewBag.ElChildsList = TempData["ElChildsList"];
            }

            if (TempData["ElNodeList"] != null)
            {
                ViewBag.ElNodeList = TempData["ElNodeList"].ToString();
            }
            return View();
        }

        //
        // GET: /Payment/DeletePayment
        public ActionResult DeletePayment(int id = 0)
        {
            string szSalesOrderNo = string.Empty;

            SalesOrder salesorder = null;
            Invoice invoice = null;

            Payments payment = db.Payments.Find(id);
            if (payment != null)
            {
                //Update the Sales order
                salesorder = db.SalesOrders.Where(slod => slod.SalesOrderNo == payment.SalesOrderNo).FirstOrDefault<SalesOrder>();
                if (salesorder != null)
                {
                    szSalesOrderNo = salesorder.SalesOrderNo;

                    if (string.IsNullOrEmpty(payment.InvoicePayment))
                    {
                        salesorder.PaymentAmount = Convert.ToDecimal(salesorder.PaymentAmount) - Convert.ToDecimal(payment.Amount);
                        salesorder.PaymentDate = Convert.ToDateTime(payment.PaymentDate);
                        if (Convert.ToDecimal(salesorder.PaymentAmount) < 0)
                        {
                            salesorder.PaymentAmount = null;
                        }

                        db.Entry(salesorder).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                }

                //Update the invoice
                invoice = db.Invoices.Where(invc => invc.SalesOrderNo == szSalesOrderNo).FirstOrDefault<Invoice>();
                if (invoice != null)
                {
                    if (!string.IsNullOrEmpty(payment.InvoicePayment))
                    {
                        if (payment.InvoicePayment.ToUpper() == "TRUE")
                        {
                            invoice.PaymentAmount = Convert.ToDecimal(invoice.PaymentAmount) - Convert.ToDecimal(payment.Amount);
                            invoice.PaymentDate = Convert.ToDateTime(payment.PaymentDate);
                            if (Convert.ToDecimal(invoice.PaymentAmount) < 0)
                            {
                                invoice.PaymentAmount = null;
                            }

                            db.Entry(invoice).State = EntityState.Modified;
                            db.SaveChanges();

                        }

                    }
                }

                db.Payments.Remove(payment);
                db.SaveChanges();



            }

            if (payment != null)
            {
                return this.RedirectToAction("PaymentTransactionList", new { salesOrderNo = payment.SalesOrderNo, invoiceId = -1 });
            }

            return RedirectToAction("PaymentIndex");
        }

        //
        // GET: /Payment/ResetPayment
        public ActionResult ResetPayment(int id = 0)
        {
            TempData["Status"] = "Initial";

            Payments payment = db.Payments.Find(id);
            if (payment != null)
            {
                payment.Amount = null;
                payment.CreditCardNumber = null;
                payment.CustomerNo = null;
                payment.PaymentDate = DateTime.Now;
                payment.PaymentType = null;
                payment.ReferenceNo = null;
                payment.SalesOrderNo = null;
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();

            }

            return RedirectToAction("Index", new { id = id });
        }

        //
        // GET: /Payment/DecodeInfo
        public ActionResult DecodeInfo()
        {
            byte[] btKey = null;
            byte[] btIV = null;
            byte[] encrypted = null;
            string szError = string.Empty;
            string szMsg = string.Empty;
            string szEncriptedData = string.Empty;
            string szDecriptedData = string.Empty;

            UnicodeEncoding unicode = new UnicodeEncoding();

            Parameters parameter = null;

            try
            {

                //Get the data from the database
                parameter = db.Parameters.Where(pmt => pmt.Parameter == "ViosParameter").FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szEncriptedData = parameter.ParameterValue;

                    //Decode the data
                    //szDecriptedData = DecodeInfo01(szEncriptedData, ref szError);
                    szDecriptedData = DecodeInfo02(szEncriptedData, ref szError);


                    TempData["DecodedData"] = szDecriptedData;
                }
                else
                {
                    TempData["EncriptionError"] = "Can not read the database.";
                }


            }
            catch (Exception err)
            {
                szError = err.Message;
            }

            if (!string.IsNullOrEmpty(szError))
            {
                TempData["EncriptionError"] = szError;
            }

            return RedirectToAction("Test02Encription");
        }



        /// <summary>
        /// Decrypt the given string.  Assumes the string was encrypted using
        /// EncryptStringAES(), using an identical sharedSecret.
        /// </summary>
        public static string DecodeInfo02(string szEncriptedData, ref string szError)
        {
            string szDecriptedData = string.Empty;

            try
            {
                if (!IsBase64String(szEncriptedData))
                {
                    throw new Exception("The cipherText input parameter is not base64 encoded");
                }

                if (string.IsNullOrEmpty(szEncriptedData))
                {
                    throw new ArgumentNullException("data to encode");
                }

                if (string.IsNullOrEmpty(szKey))
                {
                    throw new ArgumentNullException("shared key");
                }

                var saltBytes = Encoding.ASCII.GetBytes(_salt);
                var key = new Rfc2898DeriveBytes(szKey, saltBytes);

                var aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);
                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                var cipher = Convert.FromBase64String(szEncriptedData);

                using (var msDecrypt = new MemoryStream(cipher))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            szDecriptedData = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                szError = err.Message;
            }

            return szDecriptedData;
        }

        public static bool IsBase64String(string base64String)
        {
            base64String = base64String.Trim();
            return (base64String.Length % 4 == 0) &&
                   Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

        }
        public static byte[] ReadByteArray(MemoryStream msDecrypt, ref string szError)
        {
            byte[] buffer = null;
            try
            {
                byte[] rawLength = new byte[sizeof(int)];

                if (msDecrypt.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
                {
                    throw new SystemException("Stream did not contain properly formatted byte array");
                }

                buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
                if (msDecrypt.Read(buffer, 0, buffer.Length) != buffer.Length)
                {
                    throw new SystemException("Did not read byte array properly");
                }

            }
            catch (Exception err)
            {
                szError = err.Message;
            }
            msDecrypt.Close();
            return buffer;
        }

        public static string DecodeInfo01(string szEncriptedData, ref string szError)
        {
            byte[] btKey = null;
            byte[] btIV = null;
            byte[] encrypted = null;
            string szMsg = string.Empty;
            string szDecriptedData = string.Empty;

            UnicodeEncoding unicode = new UnicodeEncoding();

            try
            {
                //Get the Encription keys
                szMsg = ConfigurationManager.AppSettings["EncodeKey"];
                btKey = unicode.GetBytes(szMsg);
                szMsg = ConfigurationManager.AppSettings["EncodeIV"];
                btIV = unicode.GetBytes(szMsg);

                RijndaelManaged myRijndael = new RijndaelManaged();
                //myRijndael.Key = btKey;
                //myRijndael.IV = btIV;

                if (!string.IsNullOrEmpty(szEncriptedData))
                {
                    encrypted = unicode.GetBytes(szEncriptedData);

                    // Decrypt the bytes to a string. 
                    szDecriptedData = DecryptStringFromBytes(encrypted, myRijndael.Key, myRijndael.IV);
                }
                else
                {
                    szError = string.Format("Data is null.");
                }

            }
            catch (Exception err)
            {
                szError = err.Message;
            }
            return szDecriptedData;
        }

        //
        // POST: /Payment/EncriptInfo
        [HttpPost]
        public ActionResult EncriptInfo(string originaldata)
        {
            //byte[] btKey = null;
            //byte[] btIV = null;
            //byte[] encrypted = null;
            //string szMsg = "";
            //string szDecriptedData = "";
            string szError = string.Empty;
            string szEncriptedData = string.Empty;

            UnicodeEncoding unicode = new UnicodeEncoding();

            Parameters parameter = null;

            try
            {

                //szEncriptedData = EncriptInfo01(originaldata, ref szError);
                szEncriptedData = EncriptInfo02(originaldata, ref szError);

                //Save encripted data
                parameter = db.Parameters.Where(pmt => pmt.Parameter == "ViosParameter").FirstOrDefault<Parameters>();
                if (parameter == null)
                {
                    parameter = new Parameters();
                    parameter.Parameter = "ViosParameter";
                    db.Parameters.Add(parameter);
                    db.SaveChanges();
                }

                parameter.ParameterValue = szEncriptedData;
                db.Entry(parameter).State = EntityState.Modified;
                db.SaveChanges();

                TempData["EncriptedData"] = szEncriptedData;

            }
            catch (Exception err)
            {
                szError = err.Message;
            }

            if (!string.IsNullOrEmpty(szError))
            {
                TempData["EncriptionError"] = szError;
            }
            return RedirectToAction("Test02Encription");
        }

        public static string EncriptInfo02(string szOriginalData, ref string szError)
        {
            string szEncriptedData = string.Empty;

            try
            {
                //if (!IsBase64String(szOriginalData))
                //{
                //    throw new Exception("The cipherText input parameter is not base64 encoded");
                //}

                if (string.IsNullOrEmpty(szOriginalData))
                {
                    throw new ArgumentNullException("data to encode");
                }

                if (string.IsNullOrEmpty(szKey))
                {
                    throw new ArgumentNullException("shared key");
                }

                var saltBytes = Encoding.ASCII.GetBytes(_salt);
                var key = new Rfc2898DeriveBytes(szKey, saltBytes);

                var aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);
                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);


                var msEncrypt = new MemoryStream();

                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var srEncrypt = new StreamWriter(csEncrypt))
                    {
                        srEncrypt.Write(szOriginalData);
                    }
                    szEncriptedData = Convert.ToBase64String(msEncrypt.ToArray());
                }

            }
            catch (Exception err)
            {
                szError = err.Message;
            }

            return szEncriptedData;
        }

        public static string EncriptInfo01(string originaldata, ref string szError)
        {
            byte[] btKey = null;
            byte[] btIV = null;
            byte[] encrypted = null;
            string szMsg = string.Empty;
            string szEncriptedData = string.Empty;

            UnicodeEncoding unicode = new UnicodeEncoding();

            try
            {
                //Get the Encription keys
                szMsg = ConfigurationManager.AppSettings["EncodeKey"];
                btKey = unicode.GetBytes(szMsg);
                szMsg = ConfigurationManager.AppSettings["EncodeIV"];
                btIV = unicode.GetBytes(szMsg);

                RijndaelManaged myRijndael = new RijndaelManaged();
                //myRijndael.Key = btKey;
                //myRijndael.IV = btIV;

                //Encript the data and save it
                // Encrypt the string to an array of bytes. 
                encrypted = EncryptStringToBytes(originaldata, myRijndael.Key, myRijndael.IV);

                szEncriptedData = unicode.GetString(encrypted);

            }
            catch (Exception err)
            {
                szError = err.Message;
            }

            return szEncriptedData;
        }

        //
        // GET: /Payment/Test02Encription
        public ActionResult Test02Encription()
        {

            if (TempData["EncriptionError"] != null)
            {
                ViewBag.EncriptionError = TempData["EncriptionError"].ToString();
            }
            if (TempData["EncriptedData"] != null)
            {
                ViewBag.EncriptedData = string.Format("Encripted data: {0}", TempData["EncriptedData"].ToString());
            }
            if (TempData["DecodedData"] != null)
            {
                ViewBag.DecodedData = string.Format(" = {0}", TempData["DecodedData"].ToString());
            }

            return View();
        }



        public void AyudaEncDec01()
        {
            byte[] btKey = null;
            byte[] btIV = null;
            byte[] encrypted = null;
            string szError = string.Empty;
            string szMsg = string.Empty;
            string szEncriptedData = string.Empty;
            string szDecriptedData = string.Empty;

            UnicodeEncoding unicode = new UnicodeEncoding();

            try
            {
                //Get the Encription keys
                szMsg = ConfigurationManager.AppSettings["EncodeKey"];
                btKey = unicode.GetBytes(szMsg);
                szMsg = ConfigurationManager.AppSettings["EncodeIV"];
                btIV = unicode.GetBytes(szMsg);

                RijndaelManaged myRijndael = new RijndaelManaged();
                myRijndael.Key = btKey;
                myRijndael.IV = btIV;

                //Get encripted data
                szEncriptedData = GetEncriptedData(ref szError);
                encrypted = unicode.GetBytes(szEncriptedData);

                // Decrypt the bytes to a string. 
                szDecriptedData = DecryptStringFromBytes(encrypted, myRijndael.Key, myRijndael.IV);

            }
            catch (Exception err)
            {
                szError = err.Message;
            }
        }
        //
        // GET: /Payment/TestEncription
        public ActionResult TestEncription()
        {
            byte[] btKey = null;
            byte[] btIV = null;

            string szError = string.Empty;
            string szMsg = string.Empty;
            string szMsg01 = string.Empty;
            string szOriginalData = "HolaVios VIOS";
            string szEncriptedData = string.Empty;
            string szDecriptedData = string.Empty;

            UnicodeEncoding unicode = new UnicodeEncoding();


            // Create a new instance of the RijndaelManaged 
            // class.  This generates a new key and initialization  
            // vector (IV). 
            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {

                myRijndael.GenerateKey();
                myRijndael.GenerateIV();

                btKey = myRijndael.Key;
                btIV = myRijndael.IV;

                szMsg = unicode.GetString(btKey);
                szMsg01 = unicode.GetString(btIV);


                // Encrypt the string to an array of bytes. 
                byte[] encrypted = EncryptStringToBytes(szOriginalData, myRijndael.Key, myRijndael.IV);

                szEncriptedData = unicode.GetString(encrypted);

                //Save encripted data
                SaveEncriptedData(szEncriptedData, ref szError);

                //Get encripted data
                szEncriptedData = GetEncriptedData(ref szError);

                encrypted = unicode.GetBytes(szEncriptedData);

                // Decrypt the bytes to a string. 
                szDecriptedData = DecryptStringFromBytes(encrypted, myRijndael.Key, myRijndael.IV);

            }

            return RedirectToAction("Index", "TablesAdmin");
        }

        private string GetEncriptedData(ref string szError)
        {
            string szEncriptedData = string.Empty;
            string szDataPath = "~/Content/Data.dat";

            StreamReader sreader = null;

            try
            {
                szDataPath = Server.MapPath(szDataPath);

                sreader = new StreamReader(szDataPath);

                szEncriptedData = sreader.ReadLine();

            }
            catch (Exception err)
            {
                szError = err.Message;
            }
            finally
            {
                sreader.Close();
            }

            return szEncriptedData;
        }

        private void SaveEncriptedData(string szEncriptedData, ref string szError)
        {
            string szDataPath = "~/Content/Data.dat";

            StreamWriter swriter = null;

            try
            {
                szDataPath = Server.MapPath(szDataPath);

                swriter = new StreamWriter(szDataPath);

                swriter.WriteLine(szEncriptedData);

            }
            catch (Exception err)
            {
                szError = err.Message;
            }
            finally
            {
                swriter.Close();
            }
        }

        private void AyudaEncDec()
        {
            //string szDataPath = "~/Content/Data.dat";
            //string szOriginalData = "";

            //szDataPath = Server.MapPath(szDataPath);

            //// Create the original data to be encrypted (The data length should be a multiple of 16). 
            //byte[] toEncrypt = UnicodeEncoding.ASCII.GetBytes("This is some data of any length.");
            //szOriginalData = UnicodeEncoding.ASCII.GetString(toEncrypt);

            //// Create some random entropy. 
            //byte[] entropy = CreateRandomEntropy();
            ////szOriginalData = UnicodeEncoding.ASCII.GetString(entropy);

            //// Create a file.
            //FileStream fStream = new FileStream(szDataPath, FileMode.OpenOrCreate);

            // Encrypt a copy of the data to the stream. 
            //int bytesWritten = EncryptDataToStream(toEncrypt, entropy, DataProtectionScope.CurrentUser, fStream);


            //fStream.Close();

            //Lo pone en el archivo, pero no lo lee !!
            //szEncriptedData = EncriptData(szOriginalData);

            //szDecriptedData = DecriptData(szEncriptedData);

            //szEncriptedData = EncriptString(szOriginalData);

            //szDecriptedData = DecriptString(szEncriptedData);

        }

        public static string DecryptStringFromBytes(byte[] encrypted, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (encrypted == null || encrypted.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (Key == null || Key.Length <= 0)
            {
                throw new ArgumentNullException("Key");
            }
            if (IV == null || IV.Length <= 0)
            {
                throw new ArgumentNullException("Key");
            }

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(encrypted))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                            srDecrypt.Close();
                        }
                    }
                }

            }

            return plaintext;
        }

        public static byte[] EncryptStringToBytes(string szOriginalData, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (szOriginalData == null || szOriginalData.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }

            if (Key == null || Key.Length <= 0)
            {
                throw new ArgumentNullException("Key");
            }
            if (IV == null || IV.Length <= 0)
            {
                throw new ArgumentNullException("Key");
            }

            byte[] encrypted;

            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(szOriginalData);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream. 
            return encrypted;

        }

        private string DecriptData(string szEncriptedData)
        {
            string szDecriptedData = string.Empty;
            string szError = string.Empty;
            string szDataPath = "~/Content/Data.dat";

            FileStream fStream = null;
            CryptoStream CryptStream = null;
            StreamReader SRead = null;

            try
            {
                szDataPath = Server.MapPath(szDataPath);

                // Create a file.
                fStream = new FileStream(szDataPath, FileMode.Open);

                //Create a new instance of the RijndaelManaged class
                // and decrypt the stream.
                RijndaelManaged RMCrypto = new RijndaelManaged();
                CryptStream = new CryptoStream(fStream, RMCrypto.CreateEncryptor(), CryptoStreamMode.Read);


                SRead = new StreamReader(CryptStream);

                szDecriptedData = SRead.ReadToEnd();
            }
            catch (Exception err)
            {
                szError = err.Message;
            }
            finally
            {
                SRead.Close();
                fStream.Close();
            }

            return szDecriptedData;
        }

        private string EncriptData(string szOriginalData)
        {
            byte[] plainBytes = null;
            string szEncriptedData = string.Empty;
            string szError = string.Empty;
            string szDataPath = "~/Content/Data.dat";

            FileStream fStream = null;
            CryptoStream CryptStream = null;
            StreamWriter SWriter = null;

            try
            {
                szDataPath = Server.MapPath(szDataPath);

                // Create a file.
                fStream = new FileStream(szDataPath, FileMode.OpenOrCreate);

                //Create a new instance of the RijndaelManaged class
                // and encrypt the stream.
                RijndaelManaged RMCrypto = new RijndaelManaged();
                CryptStream = new CryptoStream(fStream, RMCrypto.CreateEncryptor(), CryptoStreamMode.Write);

                //Create a StreamWriter for easy writing to the 
                //network stream.
                SWriter = new StreamWriter(CryptStream);

                //Write to the stream.
                SWriter.WriteLine(szOriginalData);


                //plainBytes = Encoding.ASCII.GetBytes(szOriginalData);
                //fStream.Write(plainBytes, 0, plainBytes.Length);


            }
            catch (Exception err)
            {
                szError = err.Message;
            }
            finally
            {
                SWriter.Close();
                CryptStream.Close();
                fStream.Close();
            }

            return szEncriptedData;
        }

        private string DecriptString(string szEncriptedData)
        {
            // Instantiate a new RijndaelManaged object to perform string symmetric encryption
            RijndaelManaged rijndaelCipher = new RijndaelManaged();

            // Set key and IV
            //rijndaelCipher.Key = Convert.FromBase64String("TimelydepotS1234567890123456789g");
            //rijndaelCipher.IV = Convert.FromBase64String("123");

            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our RijndaelManaged object
            ICryptoTransform rijndaelDecryptor = rijndaelCipher.CreateDecryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelDecryptor, CryptoStreamMode.Write);

            // Will contain decrypted plaintext
            string plainText = String.Empty;

            try
            {
                // Convert the ciphertext string into a byte array
                byte[] cipherBytes = Convert.FromBase64String(szEncriptedData);

                // Decrypt the input ciphertext string
                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

                // Complete the decryption process
                cryptoStream.FlushFinalBlock();

                // Convert the decrypted data from a MemoryStream to a byte array
                byte[] plainBytes = memoryStream.ToArray();

                // Convert the encrypted byte array to a base64 encoded string
                plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
            }
            finally
            {
                // Close both the MemoryStream and the CryptoStream
                memoryStream.Close();
                cryptoStream.Close();
            }

            return plainText;
        }

        private string EncriptString(string szOriginalData)
        {
            byte[] btKey = null;
            byte[] btIV = null;
            string szEncriptedData = string.Empty;
            string szMsg = string.Empty;

            // Instantiate a new RijndaelManaged object to perform string symmetric encryption
            RijndaelManaged rijndaelCipher = new RijndaelManaged();

            // Set key and IV
            //btKey = rijndaelCipher.Key;
            //btIV = rijndaelCipher.IV;
            //szMsg = UnicodeEncoding.ASCII.GetString(btKey);
            //szMsg = UnicodeEncoding.ASCII.GetString(btIV);

            //"uiwyeroiugfyqcajkds897945234==";
            btKey = Convert.FromBase64String("TimelydepotS1234567890123456789g");
            szMsg = UnicodeEncoding.ASCII.GetString(btKey);

            //rijndaelCipher.Key = Convert.FromBase64String("TimelydepotS1234567890123456789g");
            //rijndaelCipher.IV = Convert.FromBase64String("123Essenoftimelydept57759889");

            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our RijndaelManaged object
            ICryptoTransform rijndaelEncryptor = rijndaelCipher.CreateEncryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelEncryptor, CryptoStreamMode.Write);

            // Convert the plainText string into a byte array
            byte[] plainBytes = Encoding.ASCII.GetBytes(szOriginalData);

            // Encrypt the input plaintext string
            cryptoStream.Write(plainBytes, 0, plainBytes.Length);

            // Complete the encryption process
            cryptoStream.FlushFinalBlock();


            // Convert the encrypted data from a MemoryStream to a byte array
            byte[] cipherBytes = memoryStream.ToArray();

            // Close both the MemoryStream and the CryptoStream
            memoryStream.Close();
            cryptoStream.Close();

            // Convert the encrypted byte array to a base64 encoded string
            szEncriptedData = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);

            return szEncriptedData;
        }

        public static byte[] CreateRandomEntropy()
        {
            // Create a byte array to hold the random value. 
            byte[] entropy = new byte[16];

            // Create a new instance of the RNGCryptoServiceProvider. 
            // Fill the array with a random value. 
            new RNGCryptoServiceProvider().GetBytes(entropy);


            // Return the array. 
            return entropy;
        }


        //
        // GET: /Payment/SelectPayment
        public ActionResult SelectPayment(int id = 0)
        {
            TempData["Status"] = "Add";

            Payments payment = db.Payments.Find(id);
            if (payment != null)
            {

                if (string.IsNullOrEmpty(payment.CustomerNo))
                {
                    TempData["Status"] = "Add";
                    return RedirectToAction("Index", new { id = payment.Id });
                }

                if (string.IsNullOrEmpty(payment.SalesOrderNo))
                {
                    TempData["Status"] = "Add01";
                    return RedirectToAction("Index", new { id = payment.Id });
                }

                if (string.IsNullOrEmpty(payment.PaymentType))
                {
                    TempData["Status"] = "Add01";
                    return RedirectToAction("Index", new { id = payment.Id });
                }

                if (string.IsNullOrEmpty(payment.ReferenceNo))
                {
                    TempData["Status"] = "Add02";
                    return RedirectToAction("Index", new { id = payment.Id });
                }

                if (!string.IsNullOrEmpty(payment.ReferenceNo))
                {
                    TempData["Status"] = "Add02";
                    return RedirectToAction("Index", new { id = payment.Id });
                }
            }

            return RedirectToAction("Index", new { id = id });
        }

        //
        // GET: /Payment/EditPay
        [NoCache]
        public PartialViewResult EditPay(int? page, string searchPaymentNo)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<Payments> qryVendors = null;

            List<Payments> paymentList = new List<Payments>();

            qryVendors = db.Payments.OrderBy(vd => vd.SalesOrderNo);
            if (!string.IsNullOrEmpty(searchPaymentNo))
            {
                ViewBag.SerchPaymentNo = searchPaymentNo;
                qryVendors = db.Payments.Where(vd => vd.PaymentNo == searchPaymentNo);
            }

            if (qryVendors.Count() > 0)
            {
                foreach (var item in qryVendors)
                {
                    paymentList.Add(item);
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


            var onePageOfData = paymentList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return PartialView(paymentList.ToPagedList(pageIndex, pageSize));
        }

        public static string GetCompanyName(TimelyDepotMVC.DAL.TimelyDepotContext db01, string szCustomerNo)
        {
            string szCompanyName = string.Empty;

            var qryCust = db01.CustomersContactAddresses.Join(db01.Customers, ctad => ctad.CustomerId, ctmr => ctmr.Id, (ctad, ctmr)
                => new { ctad, ctmr }).Where(Ncact => Ncact.ctmr.CustomerNo == szCustomerNo);
            if (qryCust.Count() > 0)
            {
                foreach (var item in qryCust)
                {
                    szCompanyName = item.ctad.CompanyName;
                    break;
                }
            }

            return szCompanyName;
        }

        //
        // GET: /Payment/Pay
        public ActionResult Pay(int paymentid = 0)
        {
            Payments payment = db.Payments.Find(paymentid);
            if (payment != null)
            {

            }
            return RedirectToAction("Index");
        }

        //
        // POST: /Payment/UpdatePay
        [NoCache]
        [HttpPost]
        public ActionResult UpdatePay(Payments payment, string PaymentDateHlp, string Status, string CreditCardNumberhlp)
        {

            int nPos = -1;
            int nHas = 0;
            double dSalesAmount = 0;
            double dTax = 0;
            double dTotalTax = 0;
            double dTotalAmount = 0;
            double dBalanceDue = 0;
            DateTime dDate = DateTime.Now;
            CultureInfo provider = CultureInfo.InvariantCulture;

            string szError = string.Empty;
            string szDecriptedData = string.Empty;
            string szEncriptedData = string.Empty;
            string szMsg = string.Empty;

            dDate = DateTime.ParseExact(PaymentDateHlp, "MM-dd-yyyy", provider);

            if (ModelState.IsValid)
            {
                if (payment.PaymentDate != dDate)
                {
                    payment.PaymentDate = dDate;
                }

                if (Status == "Add01")
                {
                    //Manage the Card Number mask
                    szDecriptedData = DecodeInfo02(payment.CreditCardNumber, ref szError);
                    if (!string.IsNullOrEmpty(szError))
                    {
                        nPos = szError.IndexOf("data to decode");
                        if (nPos != -1)
                        {
                            szDecriptedData = string.Empty;
                        }
                        else
                        {
                            szDecriptedData = string.Format("******");
                        }
                    }
                    else
                    {
                        //Mask the card number
                        nHas = szDecriptedData.Length;
                        if (nHas > 4)
                        {
                            szMsg = szDecriptedData.Substring(nHas - 4, 4);
                            szDecriptedData = string.Format("******{0}", szMsg);
                        }
                        else
                        {
                            szDecriptedData = string.Format("******");
                        }
                    }
                }

                //Set the Card Number
                if (Status == "Add02")
                {
                    nPos = -1;
                    if (!string.IsNullOrEmpty(CreditCardNumberhlp))
                    {
                        nPos = CreditCardNumberhlp.IndexOf("*");
                        if (nPos == -1)
                        {
                            //Encode the Card Number
                            if (!string.IsNullOrEmpty(CreditCardNumberhlp))
                            {
                                szEncriptedData = EncriptInfo02(CreditCardNumberhlp, ref szError);
                                payment.CreditCardNumber = szEncriptedData;
                            }

                            //Add the card number to the customer cards
                            AddCardNumber(payment, CreditCardNumberhlp);
                        }
                    }
                }

                if (Status == "Add03")
                {
                    //Verify that the payment is less than the balance due
                    SalesOrder salesorder = db.SalesOrders.Where(slod => slod.SalesOrderNo == payment.SalesOrderNo).FirstOrDefault<SalesOrder>();
                    if (salesorder != null)
                    {
                        GetSalesOrderTotals(salesorder.SalesOrderId, ref dSalesAmount, ref dTotalTax, ref dTax, ref dTotalAmount, ref dBalanceDue);
                        if (Convert.ToDouble(payment.Amount) > dBalanceDue)
                        {
                            //Status = "Add02";
                            TempData["PayError"] = string.Format("Sales Oder No. {0} has a balance due of {1}. Can not apply {2}", payment.SalesOrderNo, dBalanceDue.ToString("C"), Convert.ToDouble(payment.Amount).ToString("C"));
                            payment.Amount = Convert.ToDecimal(dBalanceDue);
                        }
                    }
                }

                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
            }

            if (!string.IsNullOrEmpty(Status))
            {
                if (Status == "Add")
                {
                    TempData["Status"] = "Add01";
                }
                if (Status == "Add01")
                {
                    TempData["Status"] = "Add02";
                }
                if (Status == "Add02")
                {
                    TempData["Status"] = "Add03";
                }
                if (Status == "Add03")
                {
                    TempData["Status"] = "Pay";
                }
            }

            return RedirectToAction("Index", new { id = payment.Id });
        }

        private void AddCardNumber(Payments payment, string CreditCardNumberhlp)
        {
            bool bAddCard = true;
            int nPos = -1;
            int nHas = -1;
            string szDecriptedData = string.Empty;
            string szError = string.Empty;
            string szMsg = string.Empty;

            Customers customer = null;
            CustomersCreditCardShipping creditcard = null;
            IQueryable<CustomersCreditCardShipping> qryCard = null;

            // Get CustomerId
            customer = db.Customers.Where(cst => cst.CustomerNo == payment.CustomerNo).FirstOrDefault<Customers>();
            if (customer != null)
            {
                qryCard = db.CustomersCreditCardShippings.Where(cstcrd => cstcrd.CustomerId == customer.Id && cstcrd.CardType == payment.PaymentType);
                if (qryCard.Count() > 0)
                {
                    foreach (var item in qryCard)
                    {
                        //Manage the Card Number mask
                        nPos = -1;
                        szDecriptedData = DecodeInfo02(item.CreditNumber, ref szError);
                        //if (!string.IsNullOrEmpty(szError))
                        //{
                        //    nPos = szError.IndexOf("data to decode");
                        //    if (nPos != -1)
                        //    {
                        //        szDecriptedData = string.Empty;
                        //    }
                        //    else
                        //    {
                        //        szDecriptedData = string.Format("******");
                        //    }
                        //}
                        //else
                        //{
                        //    //Mask the card number
                        //    nHas = szDecriptedData.Length;
                        //    if (nHas > 4)
                        //    {
                        //        szMsg = szDecriptedData.Substring(nHas - 4, 4);
                        //        szDecriptedData = string.Format("******{0}", szMsg);
                        //    }
                        //    else
                        //    {
                        //        szDecriptedData = string.Format("******");
                        //    }
                        //}

                        if (szDecriptedData == CreditCardNumberhlp)
                        {
                            bAddCard = false;
                            break;
                        }
                    }
                }

                if (bAddCard)
                {
                    creditcard = new CustomersCreditCardShipping();
                    creditcard.CreditNumber = payment.CreditCardNumber;
                    creditcard.CustomerId = customer.Id;
                    creditcard.CardType = payment.PaymentType;
                    db.CustomersCreditCardShippings.Add(creditcard);
                    db.SaveChanges();
                }
            }

        }
        //
        // GET: /Payment/UpdatePay
        [NoCache]
        public ActionResult UpdatePay(string paystate, string customerno, int id = 0)
        {

            Payments payment = new Payments();
            payment = db.Payments.Find(id);
            if (payment != null)
            {
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
            }


            return RedirectToAction("Index");
        }

        //
        // GET: /Payment/Add
        [NoCache]
        public ActionResult Add()
        {
            int nPaymentNo = 0;
            string CustomerNo = string.Empty;
            string CompanyName = string.Empty;
            InitialInfo initialinfo = null;


            //Get the next payment No
            initialinfo = db.InitialInfoes.FirstOrDefault<InitialInfo>();
            if (initialinfo == null)
            {
                initialinfo = new InitialInfo();
                initialinfo.InvoiceNo = 0;
                initialinfo.PaymentNo = 1;
                initialinfo.PurchaseOrderNo = 0;
                initialinfo.SalesOrderNo = 0;
                initialinfo.TaxRate = 0;
                db.InitialInfoes.Add(initialinfo);
            }
            else
            {
                nPaymentNo = initialinfo.PaymentNo;
                nPaymentNo++;
                initialinfo.PaymentNo = nPaymentNo;
                db.Entry(initialinfo).State = EntityState.Modified;
            }

            Payments payment = new Payments();
            payment.PaymentNo = nPaymentNo.ToString();
            payment.PaymentDate = DateTime.Now;
            db.Payments.Add(payment);
            db.SaveChanges();

            TempData["Status"] = "Add";

            return RedirectToAction("Index", new { id = payment.Id });
        }

        [NoCache]
        public ActionResult PaymentIndex(int? page, string searchItem, string ckActive, string ckCriteria)
        {
            bool bCustomerStatus = false;
            int pageIndex = 0;
            int pageSize = PageSize;

            Customers customer = null;
            IQueryable<Customers> qryCustomers = null;
            //IQueryable<CustomersContactAddress> qryMainContact = null;

            List<Customers> CustomersList = new List<Customers>();

            if (string.IsNullOrEmpty(searchItem) || searchItem == "0")
            {
                //qryItem = db.ITEMs.OrderBy(it => it.ItemID);
                ViewBag.ckActiveHlp = "true";
                ViewBag.ckCriteriaHlp = "customer";

                if (searchItem == "0")
                {
                    ViewBag.SearchItem = searchItem;

                    if (ckCriteria == "customer")
                    {
                        if (ckActive == "true")
                        {
                            qryCustomers = db.Customers.Where(cut => cut.Status == true).OrderBy(cut => cut.CustomerNo);
                        }
                        else
                        {
                            qryCustomers = db.Customers.Where(cut => cut.Status == false).OrderBy(cut => cut.CustomerNo);
                        }

                        //Display the data
                        if (qryCustomers != null)
                        {
                            if (qryCustomers.Count() > 0)
                            {
                                foreach (var item in qryCustomers)
                                {
                                    CustomersList.Add(item);
                                }
                            }
                        }
                    }

                }
            }
            else
            {
                ViewBag.SearchItem = searchItem;
                ViewBag.ckActiveHlp = ckActive;
                ViewBag.ckCriteriaHlp = ckCriteria;

                if (ckCriteria == "customer")
                {
                    if (ckActive == "true")
                    {
                        qryCustomers = db.Customers.Where(cut => cut.CustomerNo.StartsWith(searchItem) && cut.Status == true).OrderBy(cut => cut.CustomerNo);
                    }
                    else
                    {
                        qryCustomers = db.Customers.Where(cut => cut.CustomerNo.StartsWith(searchItem) && cut.Status == false).OrderBy(cut => cut.CustomerNo);
                    }

                    //Display the data
                    if (qryCustomers != null)
                    {
                        if (qryCustomers.Count() > 0)
                        {
                            foreach (var item in qryCustomers)
                            {
                                CustomersList.Add(item);
                            }
                        }
                    }
                }

                if (ckCriteria == "company")
                {
                    if (ckActive == "true")
                    {
                        var qryMainContact = db.Customers.Join(db.CustomersContactAddresses, ctc => ctc.Id, cus => cus.CustomerId, (ctc, cus)
                             => new { ctc, cus }).Where(Nctcs => Nctcs.cus.CompanyName.StartsWith(searchItem) && Nctcs.ctc.Status == true).OrderBy(Nctcs => Nctcs.cus.CompanyName);
                        if (qryMainContact.Count() > 0)
                        {
                            foreach (var item in qryMainContact)
                            {
                                CustomersList.Add(item.ctc);
                            }
                        }
                    }
                    else
                    {
                        var qryMainContact = db.Customers.Join(db.CustomersContactAddresses, ctc => ctc.Id, cus => cus.CustomerId, (ctc, cus)
                             => new { ctc, cus }).Where(Nctcs => Nctcs.cus.CompanyName.StartsWith(searchItem) && Nctcs.ctc.Status == false).OrderBy(Nctcs => Nctcs.cus.CompanyName);
                        if (qryMainContact.Count() > 0)
                        {
                            foreach (var item in qryMainContact)
                            {
                                CustomersList.Add(item.ctc);
                            }
                        }
                    }
                }

                if (ckCriteria == "phone")
                {
                    if (ckActive == "true")
                    {
                        var qryMainContact = db.Customers.Join(db.CustomersContactAddresses, ctc => ctc.Id, cus => cus.CustomerId, (ctc, cus)
                             => new { ctc, cus }).Where(Nctcs => Nctcs.cus.Tel.StartsWith(searchItem) && Nctcs.ctc.Status == true).OrderBy(Nctcs => Nctcs.cus.Tel);
                        if (qryMainContact.Count() > 0)
                        {
                            foreach (var item in qryMainContact)
                            {
                                CustomersList.Add(item.ctc);
                            }
                        }
                    }
                    else
                    {
                        var qryMainContact = db.Customers.Join(db.CustomersContactAddresses, ctc => ctc.Id, cus => cus.CustomerId, (ctc, cus)
                             => new { ctc, cus }).Where(Nctcs => Nctcs.cus.Tel.StartsWith(searchItem) && Nctcs.ctc.Status == false).OrderBy(Nctcs => Nctcs.cus.Tel);
                        if (qryMainContact.Count() > 0)
                        {
                            foreach (var item in qryMainContact)
                            {
                                CustomersList.Add(item.ctc);
                            }
                        }
                    }
                }

                if (ckCriteria == "email")
                {
                    if (ckActive == "true")
                    {
                        var qryMainContact = db.Customers.Join(db.CustomersContactAddresses, ctc => ctc.Id, cus => cus.CustomerId, (ctc, cus)
                             => new { ctc, cus }).Where(Nctcs => Nctcs.cus.Email.StartsWith(searchItem) && Nctcs.ctc.Status == true).OrderBy(Nctcs => Nctcs.cus.Email);
                        if (qryMainContact.Count() > 0)
                        {
                            foreach (var item in qryMainContact)
                            {
                                CustomersList.Add(item.ctc);
                            }
                        }
                    }
                    else
                    {
                        var qryMainContact = db.Customers.Join(db.CustomersContactAddresses, ctc => ctc.Id, cus => cus.CustomerId, (ctc, cus)
                             => new { ctc, cus }).Where(Nctcs => Nctcs.cus.Email.StartsWith(searchItem) && Nctcs.ctc.Status == false).OrderBy(Nctcs => Nctcs.cus.Email);
                        if (qryMainContact.Count() > 0)
                        {
                            foreach (var item in qryMainContact)
                            {
                                CustomersList.Add(item.ctc);
                            }
                        }
                    }
                }

                if (ckCriteria == "areacode")
                {
                    if (ckActive == "true")
                    {
                        var qryMainContact = db.Customers.Join(db.CustomersContactAddresses, ctc => ctc.Id, cus => cus.CustomerId, (ctc, cus)
                             => new { ctc, cus }).Where(Nctcs => Nctcs.cus.Zip.StartsWith(searchItem) && Nctcs.ctc.Status == true).OrderBy(Nctcs => Nctcs.cus.Zip);
                        if (qryMainContact.Count() > 0)
                        {
                            foreach (var item in qryMainContact)
                            {
                                CustomersList.Add(item.ctc);
                            }
                        }
                    }
                    else
                    {
                        var qryMainContact = db.Customers.Join(db.CustomersContactAddresses, ctc => ctc.Id, cus => cus.CustomerId, (ctc, cus)
                             => new { ctc, cus }).Where(Nctcs => Nctcs.cus.Zip.StartsWith(searchItem) && Nctcs.ctc.Status == false).OrderBy(Nctcs => Nctcs.cus.Zip);
                        if (qryMainContact.Count() > 0)
                        {
                            foreach (var item in qryMainContact)
                            {
                                CustomersList.Add(item.ctc);
                            }
                        }
                    }
                }

                if (ckCriteria == "state")
                {
                    if (ckActive == "true")
                    {
                        var qryMainContact = db.Customers.Join(db.CustomersContactAddresses, ctc => ctc.Id, cus => cus.CustomerId, (ctc, cus)
                             => new { ctc, cus }).Where(Nctcs => Nctcs.cus.State.StartsWith(searchItem) && Nctcs.ctc.Status == true).OrderBy(Nctcs => Nctcs.cus.State);
                        if (qryMainContact.Count() > 0)
                        {
                            foreach (var item in qryMainContact)
                            {
                                CustomersList.Add(item.ctc);
                            }
                        }
                    }
                    else
                    {
                        var qryMainContact = db.Customers.Join(db.CustomersContactAddresses, ctc => ctc.Id, cus => cus.CustomerId, (ctc, cus)
                             => new { ctc, cus }).Where(Nctcs => Nctcs.cus.State.StartsWith(searchItem) && Nctcs.ctc.Status == false).OrderBy(Nctcs => Nctcs.cus.State);
                        if (qryMainContact.Count() > 0)
                        {
                            foreach (var item in qryMainContact)
                            {
                                CustomersList.Add(item.ctc);
                            }
                        }
                    }
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


            var onePageOfData = CustomersList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;


            //Authorize user
            if (User.IsInRole("Owner"))
            {
                return View(CustomersList.ToPagedList(pageIndex, pageSize));
            }
            if (User.IsInRole("Admin"))
            {
                return View(CustomersList.ToPagedList(pageIndex, pageSize));
            }


            //return View(CustomersList.ToPagedList(pageIndex, pageSize));
            return RedirectToAction("LogOn", "Account");
        }
        public ActionResult EditCreditCardPayment()
        {
            return this.View();
        }

        public ActionResult AddCashPayment(string salesOrderNumber, int transactionCode, decimal? totalBalance, int invoiceId)
        {
            var latestPayments = this.db.Payments.OrderByDescending(x => x.Id).First();
            Invoice anInvoice = null;
         

            if (latestPayments == null)
            {
                return this.View();
            }

            if (invoiceId > 0)
            {
                anInvoice = this.db.Invoices.SingleOrDefault(x => x.InvoiceId == invoiceId);
            }

            var parsedPaymentNo = int.Parse(latestPayments.PaymentNo);

            parsedPaymentNo++;
            var actualPaymentNo = parsedPaymentNo.ToString(CultureInfo.InvariantCulture);
            double dBalanceDue = 0;
            double dTotalAmount = 0;
            double dTax = 0;
            double dTotalTax = 0;
            double dSalesAmount = 0;



            var paymentCash = (from salesorders in this.db.SalesOrders
                               where (salesorders.SalesOrderNo == salesOrderNumber)
                               select new CashPayment()
                                          {
                                              TransactionCode = transactionCode,
                                              SalesOrderId = salesorders.SalesOrderId,
                                              CustomerId = salesorders.CustomerId,
                                              SalesOrderNo = salesorders.SalesOrderNo,
                                              SalesAmount = (decimal)dTotalAmount,
                                              PaymentNo = actualPaymentNo,
                                              PaymentDate = DateTime.Now
                                          }).FirstOrDefault();

            if (anInvoice != null)
            {
                if (paymentCash != null)
                {
                    paymentCash.InvoiceDate = anInvoice.InvoiceDate;
                    paymentCash.InvoiceNo = anInvoice.InvoiceNo;
                    paymentCash.InvoiceId = anInvoice.InvoiceId;
                }
            }

            if (paymentCash != null)
            {
                this.GetSalesOrderTotals(
                    paymentCash.SalesOrderId,
                    ref dSalesAmount,
                    ref dTotalTax,
                    ref dTax,
                    ref dTotalAmount,
                    ref dBalanceDue);

                paymentCash.SalesAmount = (decimal)dSalesAmount;
                paymentCash.BalanceDue = totalBalance;
                ViewBag.PaymentType = SelectPayTypeListItems();
                return this.View(paymentCash);
            }

            ViewBag.ErrorMessage("Error getting payment data!");
            return this.View();
        }

        [HttpPost]
        public ActionResult AddCashPayment(CashPayment aPayment)
        {
            if (ModelState.IsValid)
            {
                var customerData = this.db.Customers.SingleOrDefault(x => x.Id == aPayment.CustomerId);
                var aCustomerNo = string.Empty;
                if (customerData != null)
                {
                    aCustomerNo = customerData.CustomerNo;
                }

                var aTransactionCode = Convert.ToInt32(aPayment.PaymentType);
                var paymentDescription = string.Empty;
                var transactioncodeQuery = this.db.TransactionCodes.SingleOrDefault(z => z.TransactionCode == aTransactionCode);
                if (transactioncodeQuery != null)
                {
                    paymentDescription = transactioncodeQuery.CodeDescription;
                }

                var aNewPayment = new Payments()
                                           {
                                               CheckNo = aPayment.CheckNumber,
                                               PaymentNo = aPayment.PaymentNo,
                                               CustomerNo = string.IsNullOrEmpty(aCustomerNo) ? string.Empty : aCustomerNo,
                                               SalesOrderNo = aPayment.SalesOrderNo,
                                               PaymentType = string.IsNullOrEmpty(paymentDescription) ? "None" : paymentDescription,
                                               Amount = (decimal?)aPayment.PaymentAmount,
                                               PaymentDate = aPayment.PaymentDate,
                                               TransactionCode = 1,

                                           };


                db.Payments.Add(aNewPayment);
                db.SaveChanges();
                return RedirectToAction("PaymentTransactionList", new { salesOrderNo = aPayment.SalesOrderNo, invoiceId = -1 });
            }
           
            ViewBag.PaymentType = SelectPayTypeListItems();
            return View(aPayment);
        }

        private static List<SelectListItem> SelectPayTypeListItems()
        {
            var paymentType = new List<SelectListItem>()
                                  {
                                      new SelectListItem() { Text = "Cash", Value = "1" },
                                      new SelectListItem() { Text = "Check", Value = "4" }
                                  };
            return paymentType;
        }

        public ActionResult AddCreditCardPayment(string salesOrderNumber, decimal totalBalance, int invoiceId, string paymentTypeSelected = "")
        {
            var latestPayments = this.db.Payments.OrderByDescending(x => x.Id).First();
            var environmentParam = this.db.EnvironmentParameters.SingleOrDefault(x => x.Active);
            Invoice anInvoice = null;
            if (invoiceId > 0)
            {
              anInvoice = this.db.Invoices.SingleOrDefault(x => x.InvoiceId == invoiceId);
            }

            if (latestPayments == null)
            {
                return this.View();
            }
            var parsedPaymentNo = int.Parse(latestPayments.PaymentNo);

            parsedPaymentNo++;
            var actualPaymentNo = parsedPaymentNo.ToString(CultureInfo.InvariantCulture);
            double dBalanceDue = 0;
            double dTotalAmount = 0;
            double dTax = 0;
            double dTotalTax = 0;
            double dSalesAmount = 0;

            var paymentCash = (from salesorders in this.db.SalesOrders
                               where (salesorders.SalesOrderNo == salesOrderNumber)
                               select new CashPayment()
                               {
                                   SalesOrderId = salesorders.SalesOrderId,
                                   CustomerId = salesorders.CustomerId,
                                   SalesOrderNo = salesorders.SalesOrderNo,
                                   SalesAmount = (decimal)dTotalAmount,
                                   PaymentNo = actualPaymentNo,
                                   PaymentType = "CreditCard",
                                   PaymentDate = DateTime.Now,
                                   ActualEnvironment = environmentParam.Description
                               }).FirstOrDefault();

            this.GetSalesOrderTotals(
                paymentCash.SalesOrderId,
                ref dSalesAmount,
                ref dTotalTax,
                ref dTax,
                ref dTotalAmount,
                ref dBalanceDue);

            if (anInvoice != null)
            {
                paymentCash.InvoiceDate = anInvoice.InvoiceDate;
                paymentCash.InvoiceNo = anInvoice.InvoiceNo;
                paymentCash.InvoiceId = anInvoice.InvoiceId;
            }

            paymentCash.SalesAmount = (decimal)dSalesAmount;
            paymentCash.BalanceDue = totalBalance;

            var listSelector = new List<KeyValuePair<string, string>>();
            var qryPaymentType = db.CustomersCardTypes.OrderBy(cdty => cdty.CardType);
            if (qryPaymentType.Count() > 0)
            {
                foreach (var item in qryPaymentType)
                {
                    listSelector.Add(new KeyValuePair<string, string>(item.CardType, item.CardType));
                }
            }
            var paymentTypeselectorlist = new SelectList(listSelector, "Key", "Value");
            var aCustomer = this.db.Customers.SingleOrDefault(x => x.Id == paymentCash.CustomerId);
            if (aCustomer != null)
            {
                var listOfCards = this.GetListOfCardSelectItems(aCustomer);
                ViewBag.CreditCardNumber = listOfCards;
            }

            ViewBag.PaymentType = paymentTypeselectorlist;

            return this.View(paymentCash);
        }

        private List<SelectListItem> GetListOfCardSelectItems(Customers aCustomer)
        {
            var emptyListOfCards = new List<SelectListItem>()
                                  {
                                      new SelectListItem() { Text = "", Value = "" }
                                  };

            var listOfCards = this.GetListOfCardsByUsers(aCustomer.CustomerNo) ?? emptyListOfCards;
            return listOfCards;
        }

        [HttpPost]
        public ActionResult AddCreditCardPayment(CashPayment aPayment)
        {
            if (ModelState.IsValid)
            {
                var creditCardNumber = "No credit card";
                var aCreditCardType = "CreditCard";
                var creditcardId = int.Parse(aPayment.CreditCardNumber);
                var creditcardType = this.db.CustomersCreditCardShippings.SingleOrDefault(x => x.Id == creditcardId);
                if (creditcardType != null)
                {
                  aCreditCardType = creditcardType.CardType;
                }
                var customersCreditCardShipping =
                    this.db.CustomersCreditCardShippings.SingleOrDefault(
                        x => x.Id == creditcardId );
                if (customersCreditCardShipping != null)
                {
                creditCardNumber =
                        customersCreditCardShipping.CreditNumber;
                }
          

                var customerData = this.db.Customers.SingleOrDefault(x => x.Id == aPayment.CustomerId);
                var aNewPayment = new Payments()
                {
                    PaymentNo = aPayment.PaymentNo,
                    CheckNo = aPayment.CheckNumber,
                    CustomerNo = String.IsNullOrEmpty(customerData.CustomerNo) ? string.Empty : customerData.CustomerNo,
                    SalesOrderNo = aPayment.SalesOrderNo,
                    PaymentType = aCreditCardType,
                    Amount = (decimal?)aPayment.PaymentAmount,
                    PaymentDate = aPayment.PaymentDate,
                    TransactionCode = 2,
                    CreditCardNumber = creditCardNumber};

                if (!string.IsNullOrEmpty(aPayment.InvoiceNo) || (aPayment.InvoiceId != -1))
                {
                    aNewPayment.InvoicePayment = "True";
                }

                db.Payments.Add(aNewPayment);
                db.SaveChanges();
                 ViewBag.Environment = aPayment.ActualEnvironment;
                return RedirectToAction("FDZPayment", new { id = aNewPayment.Id, invoiceId = aNewPayment.InvoicePayment, paymentAmount = aNewPayment.Amount });
            }

            var aCustomer = this.db.Customers.SingleOrDefault(x => x.Id == aPayment.CustomerId);
            var listOfCards = this.GetListOfCardSelectItems(aCustomer);
            this.ViewBag.CreditCardNumber = listOfCards;


            return View(aPayment);
        }

      

        private List<SelectListItem> GetListOfCardsByUsers(string customerNo)
        {
            int nCustomerId = 0;
            int nPos = 0;
            int nHas = 0;
            string szMsg = string.Empty;
            string szError = string.Empty;
            string szDecriptedData = string.Empty;




            //Get the Credit Card Number
            List<SelectListItem> listSelector = new List<SelectListItem>();

            var qryPayment =
                    db.CustomersCreditCardShippings.Join(
                        db.Customers,
                        ctcc => ctcc.CustomerId,
                        cstm => cstm.Id,
                        (ctcc, cstm) => new { ctcc, cstm })
                        .OrderBy(Nctad => Nctad.ctcc.CardType)
                        .ThenBy(Nctad => Nctad.ctcc.CreditNumber)
                        .Where(Nctad => Nctad.cstm.CustomerNo == customerNo);

            if (qryPayment.Count() > 0)
            {
                foreach (var item in qryPayment)
                {
                    if (nCustomerId == 0)
                    {
                        nCustomerId = item.cstm.Id;
                    }

                    //Decode card number
                    szError = string.Empty;
                    szDecriptedData = DecodeInfo02(item.ctcc.CreditNumber, ref szError);

                    //Mask the card number
                    nHas = szDecriptedData.Length;
                    if (nHas > 4)
                    {
                        szMsg = szDecriptedData.Substring(nHas - 4, 4);
                        szDecriptedData = string.Format("******{0}", szMsg);
                    }
                    else
                    {
                        szDecriptedData = string.Format("******");
                    }
                    //szMsg = string.Format("{0} - {1}", item.ctcc.CardType, szDecriptedData);
                    szDecriptedData = item.ctcc.CardType + " - " + szDecriptedData;
                    szMsg = item.ctcc.Id.ToString();
                    listSelector.Add(new SelectListItem { Text = szDecriptedData, Value = szMsg });
                }
            }

            return listSelector;
        }

        public ActionResult ViewCashPayment(string customerNo, string paymentId)
        {

            var query = (from paymentlist in this.db.Payments
                         join customerslist in this.db.Customers on paymentlist.CustomerNo equals
                             customerslist.CustomerNo
                         select
                             new PaymentTransactionList()
                             {
                                 TransactionId = paymentlist.Id.ToString(),
                                 CustomerNo = customerslist.CustomerNo,
                                 SalesOrderNo = paymentlist.SalesOrderNo,
                                 PaymentType = paymentlist.PaymentType,
                                 PaymentDate = paymentlist.PaymentDate,
                                 PaymentAmount = paymentlist.Amount,
                                 PaymentNo = paymentlist.PaymentNo
                             }).ToList();

            if (!string.IsNullOrEmpty(customerNo) && !string.IsNullOrEmpty(paymentId))
            {
                var salesOrderNoForCustomer = query.SingleOrDefault(x => x.TransactionId == paymentId);

                if (salesOrderNoForCustomer != null)
                {
                    return this.View(salesOrderNoForCustomer);
                }
            }

            return this.View();
        }

        public ActionResult ViewCreditCardPayment(string customerNo, string paymentId)
        {

            var query = (from paymentlist in this.db.Payments
                         join customerslist in this.db.Customers on paymentlist.CustomerNo equals
                             customerslist.CustomerNo
                         select
                             new PaymentTransactionList()
                             {
                                 TransactionId = paymentlist.Id.ToString(),
                                 CustomerNo = customerslist.CustomerNo,
                                 SalesOrderNo = paymentlist.SalesOrderNo,
                                 PaymentType = paymentlist.PaymentType,
                                 CreditCardNumber = paymentlist.CreditCardNumber,
                                 PaymentDate = paymentlist.PaymentDate,
                                 PaymentAmount = paymentlist.Amount,
                                 PaymentNo = paymentlist.PaymentNo
                             }).ToList();

            if (!string.IsNullOrEmpty(customerNo) && !string.IsNullOrEmpty(paymentId))
            {
                var salesOrderNoForCustomer = query.SingleOrDefault(x => x.TransactionId == paymentId);

                if (salesOrderNoForCustomer != null)
                {
                    salesOrderNoForCustomer.CreditCardNumber = DecriptCreditCardNumber(salesOrderNoForCustomer.CreditCardNumber);
                    return this.View(salesOrderNoForCustomer);
                }
            }

            return this.View();
        }

        private static string DecriptCreditCardNumber(string creditNumber)
        {
            var nPos = 0;
            var szError = string.Empty;
            var szDecriptedData = DecodeInfo02(creditNumber, ref szError);
            var nHas = szDecriptedData.Length;

            //Decode card number


            if (!string.IsNullOrEmpty(szError))
            {
                nPos = szError.IndexOf("data to decode");
                if (nPos != -1)
                {
                    szDecriptedData = string.Empty;
                }
                else
                {
                    szDecriptedData = string.Format("******");
                }
            }
            else
            {
                //Mask the card number

                if (nHas > 4)
                {
                    var szMsg = szDecriptedData.Substring(nHas - 4, 4);
                    szDecriptedData = string.Format("******{0}", szMsg);
                }
                else
                {
                    szDecriptedData = string.Format("******");
                }
            }
            return szDecriptedData;
        }

        public ActionResult TransactionDetail()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PaymentComponent()
        {
            return this.View();
        }

        [HttpGet]
        public ActionResult SalesOrderByCustomer(int nCustomerId)
        {
            double dSalesAmount = 0;
            double dTax = 0;
            double dTotalTax = 0;
            double dTotalAmount = 0;
            double dBalanceDue = 0;

            if (nCustomerId <= 0)
            {
                return this.RedirectToAction("PaymentIndex");
            }

            var listOfSalesOrder = (from salesorder in this.db.SalesOrders
                                    where salesorder.CustomerId == nCustomerId
                                    select
                                        new PurchaseOrderList()
                                            {
                                                CustomerId = nCustomerId,
                                                SalesOrderId = salesorder.SalesOrderId,
                                                SalesOrderNo = salesorder.SalesOrderNo,
                                                SODate = salesorder.SODate,

                                            }).OrderBy(x => x.SODate).ToList();

            var listOfInvoices = (from invoices in this.db.Invoices
                                  where invoices.CustomerId == nCustomerId
                                  select
                                      new PurchaseOrderList()
                                          {
                                              SalesOrderNo = invoices.SalesOrderNo,
                                              InvoiceDate = invoices.InvoiceDate,
                                              InvoiceNo = invoices.InvoiceNo
                                          }).ToList();


            var totalSalesOrderWithInvoice = new List<PurchaseOrderList>();

            foreach (var salesorder in listOfSalesOrder)
            {
                var aPurchaseList = salesorder;

                try
                {
                    var aInvoice = listOfInvoices.Single(x => x.SalesOrderNo == salesorder.SalesOrderNo);
                    var sumPayment = (from paymentslist in this.db.Payments
                                      where paymentslist.SalesOrderNo == salesorder.SalesOrderNo
                                      select paymentslist.Amount).Sum();
                    if (sumPayment == null)
                    {
                        sumPayment = 0;
                    }

                    if (aInvoice != null)
                    {
                        aPurchaseList.InvoiceDate = aInvoice.InvoiceDate;
                        aPurchaseList.InvoiceNo = aInvoice.InvoiceNo;
                        aPurchaseList.InvoiceId = aInvoice.InvoiceId;
                    }

                    this.GetSalesOrderTotals(
                        salesorder.SalesOrderId,
                        ref dSalesAmount,
                        ref dTotalTax,
                        ref dTax,
                        ref dTotalAmount,
                        ref dBalanceDue);

                    aPurchaseList.SalesAmount = (decimal)dTotalAmount;
                    aPurchaseList.BalanceDue = (decimal)dTotalAmount - sumPayment;
                    aPurchaseList.PaymentAmount = sumPayment;

                    totalSalesOrderWithInvoice.Add(aPurchaseList);
                }
                catch (Exception e)
                {
                    ViewBag.ErrorMssg = e.Message;
                    return this.View(totalSalesOrderWithInvoice);
                }
            }

            return this.View(totalSalesOrderWithInvoice);
        }

        public ActionResult PaymentTransactionList(string salesOrderNo, int invoiceId)
        {
            var salesorderElement = this.db.SalesOrders.FirstOrDefault(x => x.SalesOrderNo == salesOrderNo);
            double dSalesAmount = 0;
            double dTotalTax = 0;
            double dTax = 0;
            double dTotalAmount = 0;
            double dBalanceDue = 0;
            var companyName = "No company name";

            var customersContactAddress = this.db.CustomersContactAddresses.SingleOrDefault(x => x.CustomerId == salesorderElement.CustomerId);
            if (customersContactAddress != null)
            {
                var customerData =customersContactAddress.CompanyName;
                if (!string.IsNullOrEmpty(customerData))
                {
                    companyName = customerData;
                }
            }

            if (salesorderElement != null)
            {
                this.GetSalesOrderTotals(
                    salesorderElement.SalesOrderId,
                    ref dSalesAmount,
                    ref dTotalTax,
                    ref dTax,
                    ref dTotalAmount,
                    ref dBalanceDue);


                var salesorderData = new PaymentTransactionList()
                                         {
                                             TransactionId = salesorderElement.SalesOrderId.ToString(),
                                             TransactionCode = 0,
                                             TransactionDate = salesorderElement.SODate,
                                             SalesAmount = (decimal)dTotalAmount,
                                             CompanyName = companyName,
                                             CustomerId = salesorderElement.CustomerId.ToString(),
                                             SalesOrderNo = salesorderElement.SalesOrderNo,
                                         };

                var queryPaymentList = (from payments in this.db.Payments
                                        where payments.SalesOrderNo == salesOrderNo
                                        select new PaymentTransactionList()
                                                   {
                                                       TransactionId = payments.Id.ToString(),
                                                       TransactionCode = payments.TransactionCode,
                                                       CustomerNo = payments.CustomerNo,
                                                       SalesOrderNo = payments.SalesOrderNo,
                                                       TransactionDate = payments.PaymentDate,
                                                       PaymentType = payments.PaymentType,
                                                       PaymentAmount = payments.Amount ?? 0,
                                                       RefundAmount = null,
                                                   }).ToList();
                if (queryPaymentList.Any())
                {
                    salesorderData.CustomerNo = queryPaymentList[0].CustomerNo;
                    salesorderData.BalanceDue = (decimal?)dBalanceDue;
                }

                queryPaymentList.Insert(0, salesorderData);

                var queryRefundList = (from refunds in this.db.Refunds
                                       where refunds.SalesOrderNo == salesOrderNo
                                       select
                                           new PaymentTransactionList()
                                               {
                                                   TransactionId = refunds.RefundId.ToString(),
                                                   CustomerNo = refunds.CustomerNo,
                                                   SalesOrderNo = refunds.SalesOrderNo,
                                                   TransactionCode = 3,
                                                   TransactionDate = refunds.Refunddate,
                                                   PaymentType = null,
                                                   PaymentAmount = 0,
                                                   RefundAmount = refunds.RefundAmount
                                               }).ToList();

                var joinedRefundsAndPayments = queryPaymentList.Concat(queryRefundList);

                var paymentTransactionLists = joinedRefundsAndPayments as IList<PaymentTransactionList> ?? joinedRefundsAndPayments.ToList();

                if (paymentTransactionLists.Any())
                {
                    var sumPayment = (from paymentslist in paymentTransactionLists
                                      where paymentslist.TransactionCode == 2 || paymentslist.TransactionCode == 1 || paymentslist.TransactionCode == 3
                                      select paymentslist.PaymentAmount).Sum();

                    var sumRefunds = (from refundlist in paymentTransactionLists
                                      where refundlist.TransactionCode == 3
                                      select refundlist.RefundAmount).Sum();

                    this.ViewBag.CustomerData = salesorderData;

                    if ((sumPayment != null) && (sumRefunds != null))
                    {
                        this.ViewBag.DueBalance = dTotalAmount - (double)sumPayment + (double)sumRefunds;
                    }

                    ViewBag.InvoiceId = invoiceId;
                    return this.View(joinedRefundsAndPayments);
                }
            }

            ViewBag.ErrorMessage = "There is no payments for this salesorder.";

            return this.View();
        }

        [HttpGet]
        public ActionResult SalesOrderForInvoice()
        {
            var listOfSalesOrder = (from salesorder in this.db.SalesOrders
                                    where !(from invoices in this.db.Invoices select invoices.SalesOrderId)
                                    .Contains(salesorder.SalesOrderId)
                                    select
                                        new PurchaseOrderList()
                                        {
                                            SalesOrderId = salesorder.SalesOrderId,
                                            SalesOrderNo = salesorder.SalesOrderNo,
                                            SODate = salesorder.SODate
                                        }).OrderBy(x => x.SODate).ToList();
            if (listOfSalesOrder.Any())
            {
                return this.View(listOfSalesOrder);
            }

            ViewBag.ErrorMessage = "All sales orders has got Invoice already. Please press 'Back' button to edit the existing ones.";
            return this.View();
        }

        [HttpGet]
        public ActionResult PaymentComponentUpdate(string id)
        {
            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();
            Payments payment = new Payments();
            ViewBag.PaymentId = id;
            payment = db.Payments.Find(id);

            //Get the dropdown data
            var qryCust = db.CustomersContactAddresses.Join(db.Customers, ctad => ctad.CustomerId, cstm => cstm.Id, (ctad, cstm)
                => new { ctad, cstm }).OrderBy(Nctad => Nctad.ctad.CompanyName);
            if (qryCust.Count() > 0)
            {
                foreach (var item in qryCust)
                {
                    listSelector.Add(new KeyValuePair<string, string>(item.cstm.CustomerNo, item.ctad.CompanyName));
                }
            }
            SelectList tradeselectorlist = new SelectList(listSelector, "Key", "Value");
            ViewBag.CustomerList = tradeselectorlist;
            return View();
        }
        //
        // GET: /Payment/
        [NoCache]
        public ActionResult Index(string searchPaymentNo, int id = 0)
        {
            //int nCustomerId = 0;
            //int nPos = -1;
            //int nHas = 0;
            //double dSalesAmount = 0;
            //double dTax = 0;
            //double dTotalTax = 0;
            //double dTotalAmount = 0;
            //double dBalanceDue = 0;
            //string szError = string.Empty;
            //string szDecriptedData = string.Empty;
            //string szMsg = string.Empty;
            //string szCustomerNo = string.Empty;
            //string szSalesOrderNo = string.Empty;
            //string szCardType = string.Empty;

            //TimelyDepotContext db02 = new TimelyDepotContext();
            //TimelyDepotContext db03 = new TimelyDepotContext();
            //TimelyDepotContext db04 = new TimelyDepotContext();
            //TimelyDepotContext db05 = new TimelyDepotContext();

            //SalesOrder salesorder = null;

            //List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();
            //IQueryable<SalesOrder> qrySalesOrder = null;

            //Payments payment = new Payments();

            ////Set the status of the page
            //ViewBag.Status = "Initial";
            //if (TempData["Status"] != null)
            //{
            //    ViewBag.Status = TempData["Status"].ToString();
            //}

            //if (ViewBag.Status == "Initial")
            //{
            //    if (!string.IsNullOrEmpty(searchPaymentNo))
            //    {
            //        ViewBag.SearchPaymentNo = searchPaymentNo;
            //    }
            //}

            ////Add payment
            //if (ViewBag.Status == "Add")
            //{
            //    ViewBag.PaymentId = id;
            //    payment = db.Payments.Find(id);

            //    //Get the dropdown data
            //    var qryCust = db.CustomersContactAddresses.Join(db.Customers, ctad => ctad.CustomerId, cstm => cstm.Id, (ctad, cstm)
            //        => new { ctad, cstm }).OrderBy(Nctad => Nctad.ctad.CompanyName);
            //    if (qryCust.Count() > 0)
            //    {
            //        foreach (var item in qryCust)
            //        {
            //            listSelector.Add(new KeyValuePair<string, string>(item.cstm.CustomerNo, item.ctad.CompanyName));
            //        }
            //    }
            //    SelectList tradeselectorlist = new SelectList(listSelector, "Key", "Value");
            //    ViewBag.CustomerList = tradeselectorlist;
            //}

            ////Add payment, get sales orders and payment type for this customer
            //if (ViewBag.Status == "Add01")
            //{
            //    ViewBag.PaymentId = id;
            //    payment = db.Payments.Find(id);
            //    if (payment != null)
            //    {
            //        szCustomerNo = payment.CustomerNo;
            //    }

            //    //Get the dropdown data with all the customers
            //    var qryCust = db.CustomersContactAddresses.Join(db.Customers, ctad => ctad.CustomerId, cstm => cstm.Id, (ctad, cstm)
            //        => new { ctad, cstm }).OrderBy(Nctad => Nctad.ctad.CompanyName);
            //    if (qryCust.Count() > 0)
            //    {
            //        foreach (var item in qryCust)
            //        {
            //            listSelector.Add(new KeyValuePair<string, string>(item.cstm.CustomerNo, item.ctad.CompanyName));
            //        }
            //    }
            //    SelectList tradeselectorlist = new SelectList(listSelector, "Key", "Value");
            //    ViewBag.CustomerList = tradeselectorlist;

            //    //Get Payment Type
            //    listSelector = new List<KeyValuePair<string, string>>();
            //    var qryPaymentType = db05.CustomersCardTypes.OrderBy(cdty => cdty.CardType);
            //    if (qryPaymentType.Count() > 0)
            //    {
            //        foreach (var item in qryPaymentType)
            //        {
            //            listSelector.Add(new KeyValuePair<string, string>(item.CardType, item.CardType));
            //    }
            //    SelectList paymentTypeselectorlist = new SelectList(listSelector, "Key", "Value");
            //    ViewBag.PaymentType = paymentTypeselectorlist;


            //    //Get the Credit Card Number
            //    listSelector = new List<KeyValuePair<string, string>>();
            //    var qryPayment = db02.CustomersCreditCardShippings.Join(db02.Customers, ctcc => ctcc.CustomerId, cstm => cstm.Id, (ctcc, cstm)
            //        => new { ctcc, cstm }).OrderBy(Nctad => Nctad.ctcc.CardType).ThenBy(Nctad => Nctad.ctcc.CreditNumber).Where(Nctad => Nctad.cstm.CustomerNo == szCustomerNo);
            //    if (qryPayment.Count() > 0)
            //    {
            //        foreach (var item in qryPayment)
            //        {
            //            if (nCustomerId == 0)
            //            {
            //                nCustomerId = item.cstm.Id;
            //            }

            //            //Decode card number
            //            szError = string.Empty;
            //            szDecriptedData = TimelyDepotMVC.Controllers.PaymentController.DecodeInfo02(item.ctcc.CreditNumber, ref szError);
            //            if (!string.IsNullOrEmpty(szError))
            //            {
            //                nPos = szError.IndexOf("data to decode");
            //                if (nPos != -1)
            //                {
            //                    szDecriptedData = string.Empty;
            //                }
            //                else
            //                {
            //                    szDecriptedData = string.Format("******");
            //                }
            //            }
            //            else
            //            {
            //                //Mask the card number
            //                nHas = szDecriptedData.Length;
            //                if (nHas > 4)
            //                {
            //                    szMsg = szDecriptedData.Substring(nHas - 4, 4);
            //                    szDecriptedData = string.Format("******{0}", szMsg);
            //                }
            //                else
            //                {
            //                    szDecriptedData = string.Format("******");
            //                }
            //            }


            //            szMsg = string.Format("{0} - {1}", item.ctcc.CardType, szDecriptedData);
            //            listSelector.Add(new KeyValuePair<string, string>(item.ctcc.CreditNumber, szMsg));
            //        }
            //    }
            //    SelectList paymentselectorlist = new SelectList(listSelector, "Key", "Value");
            //    ViewBag.PaymentList = paymentselectorlist;

            //    //Get the sales orders with a due amount
            //    listSelector = new List<KeyValuePair<string, string>>();
            //    qrySalesOrder = db03.SalesOrders.Where(slor => slor.CustomerId == nCustomerId).OrderBy(slor => slor.SalesOrderNo);
            //    if (qrySalesOrder.Count() > 0)
            //    {
            //        foreach (var item in qrySalesOrder)
            //        {
            //            //Get the balance due
            //            GetSalesOrderTotals(item.SalesOrderId, ref dSalesAmount, ref dTotalTax, ref dTax, ref dTotalAmount, ref dBalanceDue);
            //            if (dBalanceDue > 0)
            //            {
            //                szMsg = string.Format("{0} - {1}", item.SalesOrderNo, dBalanceDue.ToString("C"));
            //                listSelector.Add(new KeyValuePair<string, string>(item.SalesOrderNo, szMsg));
            //            }
            //        }
            //    }
            //    SelectList salesorderselectorlist = new SelectList(listSelector, "Key", "Value");
            //    ViewBag.SalesOrderList = salesorderselectorlist;
            //}

            ////Add payment, get sales orders and payment type for this customer
            //if (ViewBag.Status == "Add02")
            //{
            //    ViewBag.PaymentId = id;
            //    payment = db.Payments.Find(id);
            //    if (payment != null)
            //    {
            //        szCustomerNo = payment.CustomerNo;
            //        szCardType = payment.PaymentType;
            //        ViewBag.PaymentTypeSelected = payment.PaymentType;
            //    }

            //    //Get the dropdown data
            //    var qryCust = db.CustomersContactAddresses.Join(db.Customers, ctad => ctad.CustomerId, cstm => cstm.Id, (ctad, cstm)
            //        => new { ctad, cstm }).OrderBy(Nctad => Nctad.ctad.CompanyName);
            //    if (qryCust.Count() > 0)
            //    {
            //        foreach (var item in qryCust)
            //        {
            //            listSelector.Add(new KeyValuePair<string, string>(item.cstm.CustomerNo, item.ctad.CompanyName));
            //        }
            //    }
            //    SelectList tradeselectorlist = new SelectList(listSelector, "Key", "Value");
            //    ViewBag.CustomerList = tradeselectorlist;

            //    //Get Payment Type
            //    listSelector = new List<KeyValuePair<string, string>>();
            //    var qryPaymentType = db05.CustomersCardTypes.OrderBy(cdty => cdty.CardType);
            //    if (qryPaymentType.Count() > 0)
            //    {
            //        foreach (var item in qryPaymentType)
            //        {
            //            listSelector.Add(new KeyValuePair<string, string>(item.CardType, item.CardType));
            //        }
            //    }
            //    SelectList paymentTypeselectorlist = new SelectList(listSelector, "Key", "Value");
            //    ViewBag.PaymentType = paymentTypeselectorlist;


            //    //Get the Credit Card Number for the selected Payment Type
            //    listSelector = new List<KeyValuePair<string, string>>();
            //    var qryPayment = db02.CustomersCreditCardShippings.Join(db02.Customers, ctcc => ctcc.CustomerId, cstm => cstm.Id, (ctcc, cstm)
            //        => new { ctcc, cstm }).Where(Nctad => Nctad.ctcc.CardType == szCardType).OrderBy(Nctad => Nctad.ctcc.CardType).ThenBy(Nctad => Nctad.ctcc.CreditNumber).Where(Nctad => Nctad.cstm.CustomerNo == szCustomerNo);
            //    if (qryPayment.Count() > 0)
            //    {
            //        foreach (var item in qryPayment)
            //        {
            //            if (nCustomerId == 0)
            //            {
            //                nCustomerId = item.cstm.Id;
            //            }

            //            //Decode card number
            //            szError = string.Empty;
            //            szDecriptedData = TimelyDepotMVC.Controllers.PaymentController.DecodeInfo02(item.ctcc.CreditNumber, ref szError);
            //            if (!string.IsNullOrEmpty(szError))
            //            {
            //                nPos = szError.IndexOf("data to decode");
            //                if (nPos != -1)
            //                {
            //                    szDecriptedData = string.Empty;
            //                }
            //                else
            //                {
            //                    szDecriptedData = string.Format("******");
            //                }
            //            }
            //            else
            //            {
            //                //Mask the card number
            //                nHas = szDecriptedData.Length;
            //                if (nHas > 4)
            //                {
            //                    szMsg = szDecriptedData.Substring(nHas - 4, 4);
            //                    szDecriptedData = string.Format("******{0}", szMsg);
            //                }
            //                else
            //                {
            //                    szDecriptedData = string.Format("******");
            //                }
            //            }


            //            szMsg = string.Format("{0} - {1}", item.ctcc.CardType, szDecriptedData);
            //            listSelector.Add(new KeyValuePair<string, string>(item.ctcc.CreditNumber, szMsg));
            //        }
            //    }
            //    SelectList paymentselectorlist = new SelectList(listSelector, "Key", "Value");
            //    ViewBag.PaymentList = paymentselectorlist;

            //    //Get the sales orders with a due amount
            //    listSelector = new List<KeyValuePair<string, string>>();
            //    qrySalesOrder = db03.SalesOrders.Where(slor => slor.CustomerId == nCustomerId).OrderBy(slor => slor.SalesOrderNo);
            //    if (qrySalesOrder.Count() > 0)
            //    {
            //        foreach (var item in qrySalesOrder)
            //        {
            //            //Get the balance due
            //            GetSalesOrderTotals(item.SalesOrderId, ref dSalesAmount, ref dTotalTax, ref dTax, ref dTotalAmount, ref dBalanceDue);
            //            if (dBalanceDue > 0)
            //            {
            //                szMsg = string.Format("{0} - {1}", item.SalesOrderNo, dBalanceDue.ToString("C"));
            //                listSelector.Add(new KeyValuePair<string, string>(item.SalesOrderNo, szMsg));
            //            }
            //        }
            //    }
            //    SelectList salesorderselectorlist = new SelectList(listSelector, "Key", "Value");
            //    ViewBag.SalesOrderList = salesorderselectorlist;
            //}

            ////Add payment, get sales orders and payment type for this customer, dispay due amount, enable Pay link
            //// display selected card number
            //if (ViewBag.Status == "Add03")
            //{
            //    ViewBag.PaymentId = id;
            //    payment = db.Payments.Find(id);
            //    if (payment != null)
            //    {
            //        szCustomerNo = payment.CustomerNo;
            //        szSalesOrderNo = payment.SalesOrderNo;
            //        szCardType = payment.PaymentType;
            //        ViewBag.PaymentTypeSelected = payment.PaymentType;


            //        //Decode card number
            //        szError = string.Empty;
            //        szDecriptedData = TimelyDepotMVC.Controllers.PaymentController.DecodeInfo02(payment.CreditCardNumber, ref szError);
            //        if (!string.IsNullOrEmpty(szError))
            //        {
            //            nPos = szError.IndexOf("data to decode");
            //            if (nPos != -1)
            //            {
            //                szDecriptedData = string.Empty;
            //            }
            //            else
            //            {
            //                szDecriptedData = string.Format("******");
            //            }
            //        }
            //        else
            //        {
            //            //Mask the card number
            //            nHas = szDecriptedData.Length;
            //            if (nHas > 4)
            //            {
            //                szMsg = szDecriptedData.Substring(nHas - 4, 4);
            //                szDecriptedData = string.Format("******{0}", szMsg);
            //            }
            //            else
            //            {
            //                szDecriptedData = string.Format("******");
            //            }
            //        }
            //        ViewBag.SelectedCardNumber = szDecriptedData;

            //        salesorder = db.SalesOrders.Where(slor => slor.SalesOrderNo == szSalesOrderNo).FirstOrDefault<SalesOrder>();
            //        if (salesorder != null)
            //        {
            //            //Get the balance due
            //            GetSalesOrderTotals(salesorder.SalesOrderId, ref dSalesAmount, ref dTotalTax, ref dTax, ref dTotalAmount, ref dBalanceDue);
            //            payment.Amount = Convert.ToDecimal(dBalanceDue);
            //        }
            //    }

            //    //Get the dropdown data
            //    var qryCust = db02.CustomersContactAddresses.Join(db02.Customers, ctad => ctad.CustomerId, cstm => cstm.Id, (ctad, cstm)
            //        => new { ctad, cstm }).OrderBy(Nctad => Nctad.ctad.CompanyName);
            //    if (qryCust.Count() > 0)
            //    {
            //        foreach (var item in qryCust)
            //        {
            //            listSelector.Add(new KeyValuePair<string, string>(item.cstm.CustomerNo, item.ctad.CompanyName));
            //        }
            //    }
            //    SelectList tradeselectorlist = new SelectList(listSelector, "Key", "Value");
            //    ViewBag.CustomerList = tradeselectorlist;


            //    //Get Payment Type
            //    listSelector = new List<KeyValuePair<string, string>>();
            //    var qryPaymentType = db05.CustomersCardTypes.OrderBy(cdty => cdty.CardType);
            //    if (qryPaymentType.Count() > 0)
            //    {
            //        foreach (var item in qryPaymentType)
            //        {
            //            listSelector.Add(new KeyValuePair<string, string>(item.CardType, item.CardType));
            //        }
            //    }
            //    SelectList paymentTypeselectorlist = new SelectList(listSelector, "Key", "Value");
            //    ViewBag.PaymentType = paymentTypeselectorlist;


            //    //Get the Credit Card Number for the selected Payment Type
            //    listSelector = new List<KeyValuePair<string, string>>();
            //    var qryPayment = db02.CustomersCreditCardShippings.Join(db02.Customers, ctcc => ctcc.CustomerId, cstm => cstm.Id, (ctcc, cstm)
            //        => new { ctcc, cstm }).Where(Nctad => Nctad.ctcc.CardType == szCardType).OrderBy(Nctad => Nctad.ctcc.CardType).ThenBy(Nctad => Nctad.ctcc.CreditNumber).Where(Nctad => Nctad.cstm.CustomerNo == szCustomerNo);
            //    if (qryPayment.Count() > 0)
            //    {
            //        foreach (var item in qryPayment)
            //        {
            //            if (nCustomerId == 0)
            //            {
            //                nCustomerId = item.cstm.Id;
            //            }

            //            //Decode card number
            //            szError = string.Empty;
            //            szDecriptedData = TimelyDepotMVC.Controllers.PaymentController.DecodeInfo02(item.ctcc.CreditNumber, ref szError);
            //            if (!string.IsNullOrEmpty(szError))
            //            {
            //                nPos = szError.IndexOf("data to decode");
            //                if (nPos != -1)
            //                {
            //                    szDecriptedData = string.Empty;
            //                }
            //                else
            //                {
            //                    szDecriptedData = string.Format("******");
            //                }
            //            }
            //            else
            //            {
            //                //Mask the card number
            //                nHas = szDecriptedData.Length;
            //                if (nHas > 4)
            //                {
            //                    szMsg = szDecriptedData.Substring(nHas - 4, 4);
            //                    szDecriptedData = string.Format("******{0}", szMsg);
            //                }
            //                else
            //                {
            //                    szDecriptedData = string.Format("******");
            //                }
            //            }

            //            szMsg = string.Format("{0} - {1}", item.ctcc.CardType, szDecriptedData);
            //            listSelector.Add(new KeyValuePair<string, string>(item.ctcc.CreditNumber, szMsg));
            //        }
            //    }
            //    SelectList paymentselectorlist = new SelectList(listSelector, "Key", "Value");
            //    ViewBag.PaymentList = paymentselectorlist;

            //    //Get the sales orders with a due amount
            //    listSelector = new List<KeyValuePair<string, string>>();
            //    qrySalesOrder = db04.SalesOrders.Where(slor => slor.CustomerId == nCustomerId).OrderBy(slor => slor.SalesOrderNo);
            //    if (qrySalesOrder.Count() > 0)
            //    {
            //        foreach (var item in qrySalesOrder)
            //        {
            //            //Get the balance due
            //            GetSalesOrderTotals(item.SalesOrderId, ref dSalesAmount, ref dTotalTax, ref dTax, ref dTotalAmount, ref dBalanceDue);
            //            if (dBalanceDue > 0)
            //            {
            //                szMsg = string.Format("{0} - {1}", item.SalesOrderNo, dBalanceDue.ToString("C"));
            //                listSelector.Add(new KeyValuePair<string, string>(item.SalesOrderNo, szMsg));
            //            }
            //        }
            //    }
            //    SelectList salesorderselectorlist = new SelectList(listSelector, "Key", "Value");
            //    ViewBag.SalesOrderList = salesorderselectorlist;

            //    return RedirectToAction("FDZPayment", "Payment", new { id = ViewBag.PaymentId });
            //}

            ////Pay the selected Sales Order
            //if (ViewBag.Status == "Pay")
            //{
            //    ViewBag.PaymentId = id;
            //    payment = db.Payments.Find(id);
            //    if (payment != null)
            //    {
            //        //Display any pay error
            //        if (TempData["PayError"] != null)
            //        {
            //            ViewBag.PayError = TempData["PayError"].ToString();
            //        }

            //        //Display paypal payment page
            //        return RedirectToAction("Index", "PayPalPayment", new { id = payment.Id });

            //        //issue2.docx
            //        //2.	After “pay” that’s, no need go to PayPal page. 
            //        //return RedirectToAction("PaymentSuccess", "PayPalPayment", new { paymentid = payment.Id });
            //    }

            //    //Start again the secuence
            //    TempData["Status"] = null;
            //    TempData["PayError"] = null;
            //    return RedirectToAction("Index");

            //}

            ////return View(db.Payments.ToList());
            //return View(payment);
            return this.RedirectToAction("PaymentIndex");
        }

        private void GetSalesOrderTotals02(int nSalesOrderId, double dPayment, ref double dSalesAmount, ref double dTotalTax, ref double dTax, ref double dTotalAmount, ref double dBalanceDue)
        {
            double dShipping = 0;
            //double dPayment = 0;
            double dSOTax = 0;

            IQueryable<SalesOrderDetail> qryDetails = null;
            InitialInfo initialinfo = null;

            dSalesAmount = 0;
            dTax = 0;
            dTotalAmount = 0;
            dBalanceDue = 0;
            dTotalTax = 0;

            initialinfo = db.InitialInfoes.FirstOrDefault<InitialInfo>();
            if (initialinfo == null)
            {
                initialinfo = new InitialInfo();
                initialinfo.InvoiceNo = 0;
                initialinfo.PaymentNo = 0;
                initialinfo.PurchaseOrderNo = 0;
                initialinfo.SalesOrderNo = 1;
                initialinfo.TaxRate = 0;
                db.InitialInfoes.Add(initialinfo);
                dTax = initialinfo.TaxRate;
            }
            else
            {
                dTax = initialinfo.TaxRate;
            }


            SalesOrder salesorder = db.SalesOrders.Find(nSalesOrderId);
            if (salesorder != null)
            {
                dShipping = Convert.ToDouble(salesorder.ShippingHandling);
                //dPayment = Convert.ToDouble(salesorder.PaymentAmount);

                //Use the sales order tax information
                if (salesorder.Tax_rate != null)
                {
                    if (Convert.ToDecimal(salesorder.Tax_rate) >= 0)
                    {
                        dTax = Convert.ToDouble(salesorder.Tax_rate);
                    }
                }
                dSOTax = dTax;

                qryDetails = db.SalesOrderDetails.Where(sldt => sldt.SalesOrderId == salesorder.SalesOrderId);
                if (qryDetails.Count() > 0)
                {
                    foreach (var item in qryDetails)
                    {
                        ////use the tax on product
                        //if (item.Tax != null)
                        //{
                        //    if (Convert.ToDecimal(item.Tax) >= 0)
                        //    {
                        //        dTax = Convert.ToDouble(item.Tax);
                        //    }
                        //}

                        dSalesAmount = dSalesAmount + (Convert.ToDouble(item.Quantity) * Convert.ToDouble(item.UnitPrice));
                        //use the tax on product
                        if (!string.IsNullOrEmpty(item.Sub_ItemID))
                        {
                            dTotalTax = dTotalTax + (Convert.ToDouble(item.Quantity) * Convert.ToDouble(item.UnitPrice) * (dTax / 100));
                        }
                    }
                }

                dTotalAmount = dSalesAmount + dTotalTax + dShipping;
                dBalanceDue = dTotalAmount - dPayment;

                //Set the sales order tax again
                dTax = dSOTax;
            }
        }


        private void GetSalesOrderTotals(int nSalesOrderId, ref double dSalesAmount, ref double dTotalTax, ref double dTax, ref double dTotalAmount, ref double dBalanceDue)
        {
            double dShipping = 0;
            double dPayment = 0;
            double dSOTax = 0;

            IQueryable<SalesOrderDetail> qryDetails = null;
            InitialInfo initialinfo = null;

            dSalesAmount = 0;
            dTax = 0;
            dTotalAmount = 0;
            dBalanceDue = 0;
            dTotalTax = 0;
            try
            {
                initialinfo = db.InitialInfoes.FirstOrDefault<InitialInfo>();


                if (initialinfo == null)
                {
                    initialinfo = new InitialInfo();
                    initialinfo.InvoiceNo = 0;
                    initialinfo.PaymentNo = 0;
                    initialinfo.PurchaseOrderNo = 0;
                    initialinfo.SalesOrderNo = 1;
                    initialinfo.TaxRate = 0;
                    db.InitialInfoes.Add(initialinfo);
                    dTax = initialinfo.TaxRate;
                }
                else
                {
                    dTax = initialinfo.TaxRate;
                }



                SalesOrder salesorder = db.SalesOrders.Find(nSalesOrderId);
                if (salesorder != null)
                {
                    dShipping = Convert.ToDouble(salesorder.ShippingHandling);
                    dPayment = Convert.ToDouble(salesorder.PaymentAmount);

                    //Use the sales order tax information
                    if (salesorder.Tax_rate != null)
                    {
                        if (Convert.ToDecimal(salesorder.Tax_rate) >= 0)
                        {
                            dTax = Convert.ToDouble(salesorder.Tax_rate);
                        }
                    }
                    dSOTax = dTax;

                    qryDetails = db.SalesOrderDetails.Where(sldt => sldt.SalesOrderId == salesorder.SalesOrderId);
                    if (qryDetails.Count() > 0)
                    {
                        foreach (var item in qryDetails)
                        {
                            ////use the tax on product
                            //if (item.Tax != null)
                            //{
                            //    if (Convert.ToDecimal(item.Tax) >= 0)
                            //    {
                            //        dTax = Convert.ToDouble(item.Tax);
                            //    }
                            //}

                            dSalesAmount = dSalesAmount + (Convert.ToDouble(item.Quantity) * Convert.ToDouble(item.UnitPrice));
                            //use the tax on product
                            if (!string.IsNullOrEmpty(item.Sub_ItemID))
                            {
                                dTotalTax = dTotalTax + (Convert.ToDouble(item.Quantity) * Convert.ToDouble(item.UnitPrice) * (dTax / 100));
                            }
                        }
                    }

                    dTotalAmount = dSalesAmount + dTotalTax + dShipping;
                    dBalanceDue = dTotalAmount - dPayment;

                    //Set the sales order tax again
                    dTax = dSOTax;
                }
            }
            catch (EntityCommandExecutionException e)
            {

            }

        }

        //
        // GET: /Payment/Details/5

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
        // GET: /Payment/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Payment/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Payments payments)
        {
            if (ModelState.IsValid)
            {
                db.Payments.Add(payments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(payments);
        }

  
        // GET: /Payment/Edit/5
        [NoCache]
        public ActionResult Edit(int TransCode, int paymentId = 0)
        {
            Payments payments = db.Payments.Find(paymentId);
            if (payments == null)
            {
                return HttpNotFound();
            }

            var refundPayment = this.db.Refunds.Where(x => x.TransactionId == payments.Id);
            ViewBag.Refunded = refundPayment.Any();

            payments.PaymentDate = Convert.ToDateTime(payments.PaymentDate);
   
            return View(payments);
        }

     
        // POST: /Payment/Edit/5

        [HttpPost]
        public ActionResult Edit(Payments payments, string CreditCardNumberHlp)
        {
            int nPos = -1;
            decimal dPayments = 0;
            string szSalesOrderNo = string.Empty;
            string szEncriptedData = string.Empty;
            string szError = string.Empty;

            Invoice invoice = null;

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(CreditCardNumberHlp))
                {
                    nPos = CreditCardNumberHlp.IndexOf("*");
                    if (nPos == -1)
                    {
                        //Encode the Card Number
                        if (!string.IsNullOrEmpty(CreditCardNumberHlp))
                        {
                            szEncriptedData = EncriptInfo02(CreditCardNumberHlp, ref szError);
                            payments.CreditCardNumber = szEncriptedData;
                        }

                        //Add the card number to the customer cards
                        AddCardNumber(payments, CreditCardNumberHlp);
                    }
                }


                //Verify that the payment is less than the balance due
                dPayments = GetSalesOrderPayments(payments.SalesOrderNo, payments);
                //dPayments = dPayments + Convert.ToDecimal(payments.Amount);
                SalesOrder salesorder = db.SalesOrders.Where(slod => slod.SalesOrderNo == payments.SalesOrderNo).FirstOrDefault<SalesOrder>();
                if (salesorder != null)
                {
                        //Update the Sales order
                        salesorder = db.SalesOrders.Where(slod => slod.SalesOrderNo == payments.SalesOrderNo).FirstOrDefault<SalesOrder>();
                        if (salesorder != null)
                        {
                            szSalesOrderNo = salesorder.SalesOrderNo;
                            //salesorder.PaymentAmount = Convert.ToDecimal(salesorder.PaymentAmount) - Convert.ToDecimal(payments.Amount);
                            salesorder.PaymentAmount = dPayments + Convert.ToDecimal(payments.Amount);
                            salesorder.PaymentDate = Convert.ToDateTime(payments.PaymentDate);
                            if (Convert.ToDecimal(salesorder.PaymentAmount) < 0)
                            {
                                salesorder.PaymentAmount = null;
                            }

                            db.Entry(salesorder).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        //Update the invoice
                        invoice = db.Invoices.Where(invc => invc.SalesOrderNo == szSalesOrderNo).FirstOrDefault<Invoice>();
                        if (invoice != null)
                        {
                            //invoice.PaymentAmount = Convert.ToDecimal(invoice.PaymentAmount) - Convert.ToDecimal(payments.Amount);
                            invoice.PaymentAmount = dPayments + Convert.ToDecimal(payments.Amount);
                            invoice.PaymentDate = Convert.ToDateTime(payments.PaymentDate);
                            if (Convert.ToDecimal(invoice.PaymentAmount) < 0)
                            {
                                invoice.PaymentAmount = null;
                            }
                            db.Entry(invoice).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                }

                db.Entry(payments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PaymentTransactionList", new { salesOrderNo = payments.SalesOrderNo,invoiceId=-1 });
            }
            return View(payments);
        }


        private decimal GetSalesOrderPayments(string SalesOrderNo, Payments payments)
        {
            decimal dPayments = 0;

            TimelyDepotContext db01 = new TimelyDepotContext();

            IQueryable<Payments> qryPayment = db01.Payments.Where(pmt => pmt.SalesOrderNo == SalesOrderNo);
            if (qryPayment.Count() > 0)
            {
                foreach (var item in qryPayment)
                {
                    if (item.Id != payments.Id)
                    {
                        dPayments = dPayments + Convert.ToDecimal(item.Amount);
                    }
                }
            }

            return dPayments;
        }

        //
        // GET: /Payment/Delete/5

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
        // POST: /Payment/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payments payments = db.Payments.Find(id);
            db.Payments.Remove(payments);
            db.SaveChanges();
            return RedirectToAction("PaymentTransactionList", new { salesOrderNo = payments.SalesOrderNo ,invoiceId=-1});
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
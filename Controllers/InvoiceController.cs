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
using TimelyDepotMVC.CommonCode;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using System.IO;
//using PdfReportSamples.InMemory;
using TimelyDepotMVC.ModelsView;
using TimelyDepotMVC.PDFReporting;

namespace TimelyDepotMVC.Controllers
{
    public class InvoiceController : Controller
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
        // GET: //Payment
        [NoCache]
        public PartialViewResult Payment(int id = 0)
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
            string szError = "";
            string szDecriptedData = "";
            string szMsg = "";
            string szCustomerNo = "";

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
                                    szError = "";
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
                    //db.Payments.Add(payment);
                    //db.SaveChanges();

                }
            }

            return PartialView(payment);
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
            catch (Exception e)
            {

            }

        }

        //
        // POST: /SalesOrder/Payment
        [HttpPost]
        public ActionResult Payment(Payments payments)
        {
            int nPos = 0;
            string CreditNumber01 = payments.CreditCardNumber;
            string szEncriptedData = "";
            string szError = "";

            Payments paymentsHlp = null;
            SalesOrder salesorder = null;
            CustomersContactAddress customeraddress = null;
            CustomersShipAddress shipto = null;

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

            //Get the Payment and update paymen type
            paymentsHlp = new Payments();
            if (paymentsHlp != null)
            {
                paymentsHlp.Amount = payments.Amount;
                paymentsHlp.CustomerNo = payments.CustomerNo;
                paymentsHlp.PaymentDate = payments.PaymentDate;
                paymentsHlp.PaymentNo = payments.PaymentNo;
                paymentsHlp.SalesOrderNo = payments.SalesOrderNo;

                paymentsHlp.PaymentType = payments.PaymentType;
                paymentsHlp.CreditCardNumber = payments.CreditCardNumber;

                db.Payments.Add(paymentsHlp);
                db.SaveChanges();

                //Get the sales order
                salesorder = db.SalesOrders.Where(slor => slor.SalesOrderNo == paymentsHlp.SalesOrderNo).FirstOrDefault<SalesOrder>();
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

                    return RedirectToAction("Index", "PayPalPayment", new { id = paymentsHlp.Id });
                }

            }

            return RedirectToAction("Index");
        }


        //
        // /Invoice/GetOrderContact
        public static string GetOrderContact(int CustomerID)
        {
            string szOrderContact = "";

            TimelyDepotContext db01 = new TimelyDepotContext();
            CustomersSalesContact salescontact = db01.CustomersSalesContacts.Where(slct => slct.CustomerId == CustomerID).FirstOrDefault<CustomersSalesContact>();
            if (salescontact != null)
            {
                szOrderContact = string.Format("{0} {1}", salescontact.FirstName, salescontact.LastName);
            }


            return szOrderContact;
        }
        public static string GetOrderContact01(int CustomerID)
        {
            string szOrderContact = "";

            TimelyDepotContext db01 = new TimelyDepotContext();
            CustomersSalesContact salescontact = db01.CustomersSalesContacts.Where(slct => slct.CustomerId == CustomerID).FirstOrDefault<CustomersSalesContact>();
            if (salescontact != null)
            {
                szOrderContact = salescontact.Id.ToString();
            }


            return szOrderContact;
        }

        //
        // POST: /Invoice/UpdateBlindShip
        public ActionResult UpdateBlindShip(SalesOrderBlindShip salesorderblindship, string invoiceid)
        {
            int nInvoiceid = 0;
            SalesOrder salesorder = null;
            Invoice invoice = null;
            if (ModelState.IsValid)
            {
                //Update salesorder blindship field
                salesorder = db.SalesOrders.Find(salesorderblindship.SalesOrderId);
                if (salesorder != null)
                {
                    salesorder.IsBlindShip = true;
                    db.Entry(salesorder).State = EntityState.Modified;
                }


                db.Entry(salesorderblindship).State = EntityState.Modified;

                if (!string.IsNullOrEmpty(invoiceid))
                {
                    nInvoiceid = Convert.ToInt32(invoiceid);
                    invoice = db.Invoices.Find(nInvoiceid);
                    if (invoice != null)
                    {
                        invoice.IsBlindShip = true;
                        db.Entry(invoice).State = EntityState.Modified;
                    }
                }

                db.SaveChanges();
            }

            return RedirectToAction("Edit", new { id = invoiceid });
        }

        //
        // GET: /Invoice/InvoiceListExcel
        public ActionResult InvoiceListExcel()
        {
            //DataTable hlpTbl = GetCustomerListTable();

            ExportCSV(GetInvoiceTable(), "InvoiceList");

            return RedirectToAction("Index", "ReportsExcel");
        }

        private void ExportCSV(DataTable data, string szFileName)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + szFileName + ".csv");

            //Write column heads names
            for (int i = 0; i < data.Columns.Count; i++)
            {
                if (i > 0)
                {
                    Response.Write(",");
                }
                Response.Write(data.Columns[i].ColumnName);
            }
            Response.Write(Environment.NewLine);

            //Write data
            foreach (DataRow row in data.Rows)
            {
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    if (i > 0)
                    {
                        Response.Write(",");
                    }
                    Response.Write(row[i].ToString());
                }
                Response.Write(Environment.NewLine);
            }

            Response.End();
        }

        private DataTable GetInvoiceTable()
        {
            long telHlp = 0;
            long faxHlp = 0;
            string telfmt = "000-000-0000";
            string szTel = "";

            TimelyDepotContext db01 = new TimelyDepotContext();

            DataTable table = null;
            DataRow row = null;

            InvoiceList theinvoicelist = null;
            List<InvoiceList> invoiceList = new List<InvoiceList>();

            var qryInvoice = db.CustomersContactAddresses.Join(db.Invoices, ctad => ctad.CustomerId, slod => slod.CustomerId, (ctad, slod)
                => new { ctad, slod }).OrderBy(cact => cact.slod.InvoiceId);
            if (qryInvoice.Count() > 0)
            {
                foreach (var item in qryInvoice)
                {
                    if (string.IsNullOrEmpty(item.ctad.Tel))
                    {
                        szTel = "0";
                    }
                    else
                    {
                        szTel = item.ctad.Tel;
                    }
                    telHlp = Convert.ToInt64(szTel);
                    szTel = string.Format("{0}", telHlp.ToString(telfmt));

                    theinvoicelist = new InvoiceList();
                    theinvoicelist.InvoiceId = item.slod.InvoiceId;
                    theinvoicelist.InvoiceNo = item.slod.InvoiceNo;
                    theinvoicelist.SODate = item.slod.ShipDate;
                    theinvoicelist.CustomerNo = GetCustomerDataInvoice(db01, item.ctad.CustomerId.ToString());
                    theinvoicelist.CompanyName = item.ctad.CompanyName;
                    theinvoicelist.PurchaseOrderNo = item.slod.PurchaseOrderNo;
                    theinvoicelist.SalesOrderNo = item.slod.SalesOrderNo;
                    theinvoicelist.PaymentAmount = GetInvoiceAmount(db01, item.slod.InvoiceId);

                    invoiceList.Add(theinvoicelist);
                }
            }

            table = new DataTable("InvoiceList");

            // Set the header
            DataColumn col01 = new DataColumn("InvoiceNo", System.Type.GetType("System.String"));
            DataColumn col02 = new DataColumn("ShipDate", System.Type.GetType("System.String"));
            DataColumn col03 = new DataColumn("CustomerNo", System.Type.GetType("System.String"));
            DataColumn col04 = new DataColumn("CompanyName", System.Type.GetType("System.String"));
            DataColumn col05 = new DataColumn("SalesOrderNo", System.Type.GetType("System.String"));
            DataColumn col06 = new DataColumn("PurchaseOrderNo", System.Type.GetType("System.String"));
            DataColumn col07 = new DataColumn("Amount", System.Type.GetType("System.String"));
            table.Columns.Add(col01);
            table.Columns.Add(col02);
            table.Columns.Add(col03);
            table.Columns.Add(col04);
            table.Columns.Add(col05);
            table.Columns.Add(col06);
            table.Columns.Add(col07);

            //Set the data row
            foreach (var item in invoiceList)
            {
                row = table.NewRow();
                row["InvoiceNo"] = item.InvoiceNo;
                row["ShipDate"] = item.SODate;
                row["CustomerNo"] = item.CustomerNo;
                row["CompanyName"] = item.CompanyName;
                row["SalesOrderNo"] = item.SalesOrderNo;
                row["PurchaseOrderNo"] = item.PurchaseOrderNo;
                row["Amount"] = item.PaymentAmount;
                table.Rows.Add(row);
            }

            return table;
        }

        //
        // GET: /Invoice/InvoiceList
        [NoCache]
        public PartialViewResult InvoiceList(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;

            TimelyDepotContext db01 = new TimelyDepotContext();

            InvoiceList theinvoicelist = null;
            List<InvoiceList> invoiceList = new List<InvoiceList>();

            var qrySalesOrder = db.CustomersContactAddresses.Join(db.Invoices, ctad => ctad.CustomerId, slod => slod.CustomerId, (ctad, slod)
                => new { ctad, slod }).OrderBy(cact => cact.slod.SalesOrderId);
            if (qrySalesOrder.Count() > 0)
            {
                foreach (var item in qrySalesOrder)
                {
                    theinvoicelist = new InvoiceList();
                    theinvoicelist.InvoiceId = item.slod.InvoiceId;
                    theinvoicelist.InvoiceNo = item.slod.InvoiceNo;
                    theinvoicelist.SODate = item.slod.ShipDate;
                    theinvoicelist.CustomerNo = GetCustomerDataInvoice(db01, item.ctad.CustomerId.ToString());
                    theinvoicelist.CompanyName = item.ctad.CompanyName;
                    theinvoicelist.PurchaseOrderNo = item.slod.PurchaseOrderNo;
                    theinvoicelist.SalesOrderNo = item.slod.SalesOrderNo;
                    theinvoicelist.PaymentAmount = GetInvoiceAmount(db01, item.slod.InvoiceId);

                    invoiceList.Add(theinvoicelist);
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


            var onePageOfData = invoiceList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return PartialView(invoiceList.ToPagedList(pageIndex, pageSize));
        }

        private decimal GetInvoiceAmount(TimelyDepotContext db01, int nInvoiceId)
        {
            double dAmount = 0;
            double dSalesAmount = 0;
            double dTotalTax = 0;
            double dTax = 0;
            double dTotalAmount = 0;
            double dBalanceDue = 0;

            //Get the totals
            GetInvoiceTotals01(db01, nInvoiceId, ref dSalesAmount, ref dTotalTax, ref dTax, ref dTotalAmount, ref dBalanceDue);
            dAmount = dTotalAmount;

            return Convert.ToDecimal(dAmount);
        }

        private string GetCustomerDataInvoice(TimelyDepotContext db01, string szCustomerId)
        {
            int nCustomerid = Convert.ToInt32(szCustomerId);
            string szCustomerNo = "";

            Customers customeraddress = db01.Customers.Where(ctad => ctad.Id == nCustomerid).FirstOrDefault<Customers>();
            if (customeraddress != null)
            {
                szCustomerNo = customeraddress.CustomerNo;
            }

            return szCustomerNo;
        }

        //
        // GET: /Invoice/MemoryInvoiceReport
        public void MemoryInvoiceReport(int id = 0)
        {
            //Verify the required folder
            string szReportFolderPath = string.Format("~/Pdf");
            szReportFolderPath = Server.MapPath(szReportFolderPath);
            if (!Directory.Exists(szReportFolderPath))
            {
                Directory.CreateDirectory(szReportFolderPath);
            }


            //new InMemoryPdfReport().CreatePdfReport();

            new InvoiceReport().CreateReport01(szReportFolderPath, id);

        }



        //
        // GET: /Invoice/InvoiceReport
        public ActionResult InvoiceReport(int id = 0)
        {
            int Invoiceid = id;
            string szOutputFilePath = "";
            string szOutputFilePathHlp = "";

            //Verify the required folder
            string szReportFolderPath = string.Format("~/Pdf");
            szReportFolderPath = Server.MapPath(szReportFolderPath);
            if (!Directory.Exists(szReportFolderPath))
            {
                Directory.CreateDirectory(szReportFolderPath);
            }

            //Save the report folder path
            Parameters parameter = db.Parameters.Where(prmt => prmt.Parameter == "PdfFolderPath").FirstOrDefault<Parameters>();
            if (parameter == null)
            {
                parameter = new Parameters();
                parameter.Parameter = "PdfFolderPath";
                parameter.ParameterValue = szReportFolderPath;
                db.Parameters.Add(parameter);
                db.SaveChanges();
            }
            else
            {
                szReportFolderPath = parameter.ParameterValue;
            }

            //Get the first invoice
            if (Invoiceid == 0)
            {
                Invoice invoiceHlp = db.Invoices.FirstOrDefault<Invoice>();
                if (invoiceHlp == null)
                {
                    return null;
                }
                Invoiceid = invoiceHlp.InvoiceId;
                id = Invoiceid;
            }

            var rpt = new InvoiceReport().CreateReport(szReportFolderPath, Invoiceid);
            //szOutputFilePath = rpt.FileName.Replace(HttpRuntime.AppDomainAppPath, string.Empty);  //This fails. does not found the pdf file !!
            szOutputFilePathHlp = string.Format("~/Pdf/InvoiceReport-{0}.pdf", id.ToString());
            return Redirect(szOutputFilePathHlp);

        }

        //
        // GET: /Invoice/InsertItem
        public ActionResult InsertItem(string salesorderid, string itemOrder, int itemPos = 0)
        {
            double nItemOrder = 0;
            int nitemPosNext = 0;
            int nItemPos = 0;
            int nCurrentItemPos = 0;
            int nNextItemPos = 0;
            double dItemOrder = 0;
            double dCurrentItemOrder = 0;
            double dNextItemOrder = 0;
            string szCurentItemId = "";
            string szNextItemId = "";
            InvoiceDetail salesdetailcurrent = null;
            InvoiceDetail salesdetailnext = null;

            int nSalesOrderId = Convert.ToInt32(salesorderid);


            if (!string.IsNullOrEmpty(itemOrder))
            {
                //itemOrder = itemOrder.Replace(".", ",");
                nItemOrder = Convert.ToDouble(itemOrder);
            }

            //Get the current salesorderdetail
            salesdetailcurrent = db.InvoiceDetails.Where(sldt => sldt.InvoiceId == nSalesOrderId && sldt.ItemPosition == itemPos && sldt.ItemOrder == nItemOrder).FirstOrDefault<InvoiceDetail>();
            if (salesdetailcurrent != null)
            {
                nCurrentItemPos = Convert.ToInt32(salesdetailcurrent.ItemPosition);
                dCurrentItemOrder = Convert.ToDouble(salesdetailcurrent.ItemOrder);
                szCurentItemId = salesdetailcurrent.ItemID;
            }

            //Get the next salesorderdetail
            salesdetailnext = db.InvoiceDetails.Where(sldt => sldt.InvoiceId == nSalesOrderId && sldt.ItemPosition == nCurrentItemPos && sldt.ItemOrder > dCurrentItemOrder).OrderBy(sldt => sldt.ItemOrder).FirstOrDefault<InvoiceDetail>();
            if (salesdetailnext != null)
            {
                nNextItemPos = Convert.ToInt32(salesdetailnext.ItemPosition);
                szNextItemId = salesdetailnext.ItemID;
                dNextItemOrder = Convert.ToDouble(salesdetailnext.ItemOrder);

                nItemPos = nCurrentItemPos;
                dItemOrder = (dCurrentItemOrder + dNextItemOrder) / 2;
            }
            else
            {
                nItemPos = nCurrentItemPos;
                dItemOrder = dCurrentItemOrder + 1;
            }

            InvoiceDetail salesdetail = null;

            salesdetail = new InvoiceDetail();
            salesdetail.InvoiceId = nSalesOrderId;
            salesdetail.ItemID = string.Empty;
            salesdetail.Sub_ItemID = string.Empty;
            salesdetail.BackOrderQuantity = 0;
            salesdetail.Description = string.Empty;
            salesdetail.Quantity = 0;
            salesdetail.ShipQuantity = 0;
            salesdetail.Tax = 0;
            salesdetail.UnitPrice = 0;
            salesdetail.ItemPosition = nItemPos;
            salesdetail.ItemOrder = dItemOrder;
            db.InvoiceDetails.Add(salesdetail);
            db.SaveChanges();

            return RedirectToAction("Edit", new { id = nSalesOrderId });
        }

        //
        // GET: /Invoice/DeleteDetail
        public ActionResult DeleteDetail(string shipment, int id = 0)
        {
            int salesorderId = 0;

            InvoiceDetail salesdetail = db.InvoiceDetails.Find(id);
            if (salesdetail != null)
            {
                salesorderId = Convert.ToInt32(salesdetail.InvoiceId);
                db.InvoiceDetails.Remove(salesdetail);
                db.SaveChanges();
            }


            if (!string.IsNullOrEmpty(shipment))
            {
                return RedirectToAction("Index", "Shipment", new { id = salesorderId });
            }

            return RedirectToAction("Edit", new { id = salesorderId });
        }

        //
        // GET: /Invoice/UpdateDetail
        public ActionResult UpdateDetail(int? id, string salesorderid, string qty, string shipqty, string boqty, string desc, string price, string tax,
            string logo, string imprt, string qtysc, string qtyrc, string pricesc, string pricerc, string shipment)
        {
            double dHlp = 0;
            decimal dcHlp = 0;
            decimal dcHlp1 = 0;
            double dTaxRate = 0;
            int nSalesOrderId = Convert.ToInt32(salesorderid);
            int nPriceId = 0;
            string szSalesOrder = "";
            string szSalesOredidHlp = "";
            string szSalesOrderIdHlp = "";
            string szItemIdHlp = "";

            InvoiceDetail sodetail = db.InvoiceDetails.Find(id);
            InvoiceDetail setupcharge = null;
            InvoiceDetail runcharge = null;


            PRICE price01 = null;
            IQueryable<PRICE> qryPrice = null;

            List<KeyValuePair<double, int>> qtyprcList = new List<KeyValuePair<double, int>>();

            if (sodetail != null)
            {
                //
                // Get the price and qty list for this item
                qryPrice = db.PRICEs.Where(prc => prc.Item == sodetail.ItemID).OrderBy(prc => prc.Qty);
                if (qryPrice.Count() > 0)
                {
                    foreach (var item in qryPrice)
                    {
                        qtyprcList.Add(new KeyValuePair<double, int>(item.Qty, item.Id));
                    }
                }

                sodetail.Description = desc;
                sodetail.Logo = logo;
                sodetail.ImprintMethod = imprt;

                if (!string.IsNullOrEmpty(qty))
                {
                    dHlp = Convert.ToDouble(qty);
                    sodetail.Quantity = dHlp;
                }
                if (!string.IsNullOrEmpty(shipqty))
                {
                    dHlp = Convert.ToDouble(shipqty);
                    sodetail.ShipQuantity = dHlp;
                }
                if (!string.IsNullOrEmpty(boqty))
                {
                    dHlp = Convert.ToDouble(boqty);
                    sodetail.BackOrderQuantity = dHlp;
                }
                if (!string.IsNullOrEmpty(tax))
                {
                    dHlp = Convert.ToDouble(tax);
                    sodetail.Tax = dHlp;
                }
                if (!string.IsNullOrEmpty(price))
                {
                    price = price.Replace("$", "");
                    price = price.Replace(",", "");
                    dcHlp = Convert.ToDecimal(price);
                    sodetail.UnitPrice = dcHlp;

                    //Set the price according with the Quantity
                    for (int i = 0; i < qtyprcList.Count; i++)
                    {
                        if (i == 0)
                        {
                            if (sodetail.Quantity <= qtyprcList[i].Key)
                            {
                                nPriceId = qtyprcList[i].Value;
                            }
                            else
                            {
                                if (sodetail.Quantity >= qtyprcList[i].Key && sodetail.Quantity < qtyprcList[i + 1].Key)
                                {
                                    nPriceId = qtyprcList[i + 1].Value;
                                }
                            }
                        }
                        else
                        {
                            if (i == qtyprcList.Count - 1)
                            {
                                if (sodetail.Quantity > qtyprcList[i].Key)
                                {
                                    nPriceId = qtyprcList[i].Value;

                                }
                            }
                            else
                            {
                                if (sodetail.Quantity >= qtyprcList[i].Key && sodetail.Quantity < qtyprcList[i + 1].Key)
                                {
                                    nPriceId = qtyprcList[i + 1].Value;
                                }
                            }
                        }
                    }
                    if (nPriceId > 0)
                    {
                        price01 = db.PRICEs.Find(nPriceId);
                        if (price01 != null)
                        {
                            double dDiscountPrc = TimelyDepotMVC.Controllers.InventoryController.GetDiscount(db, price01.Discount_Code);
                            sodetail.UnitPrice = price01.thePrice * (1 - Convert.ToDecimal(dDiscountPrc));
                        }
                    }


                }

                db.Entry(sodetail).State = EntityState.Modified;
                db.SaveChanges();

                //Update Set up Charge
                if (!string.IsNullOrEmpty(pricesc) && !string.IsNullOrEmpty(qtysc))
                {
                    szSalesOredidHlp = string.Format("Set up Charge {0} {1}", sodetail.InvoiceId.ToString(), sodetail.ItemID);
                    setupcharge = db.InvoiceDetails.Where(spch => spch.InvoiceId == sodetail.InvoiceId && spch.Description == szSalesOredidHlp).FirstOrDefault<InvoiceDetail>();
                    if (setupcharge != null)
                    {
                        pricesc = pricesc.Replace("$", "");
                        pricesc = pricesc.Replace(",", "");
                        dcHlp = Convert.ToDecimal(pricesc);
                        setupcharge.UnitPrice = dcHlp;

                        qtysc = qtysc.Replace("$", "");
                        qtysc = qtysc.Replace(",", "");
                        dcHlp1 = Convert.ToDecimal(qtysc);
                        setupcharge.Quantity = Convert.ToDouble(dcHlp1);
                        db.Entry(setupcharge).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        pricesc = pricesc.Replace("$", "");
                        pricesc = pricesc.Replace(",", "");
                        dcHlp = Convert.ToDecimal(pricesc);
                        //setupcharge.UnitPrice = dcHlp;

                        qtysc = qtysc.Replace("$", "");
                        qtysc = qtysc.Replace(",", "");
                        dcHlp1 = Convert.ToDecimal(qtysc);

                        setupcharge = new InvoiceDetail();
                        setupcharge.InvoiceId = nSalesOrderId;
                        setupcharge.ItemID = string.Empty;
                        setupcharge.Sub_ItemID = string.Empty;
                        setupcharge.Description = string.Format("Set up Charge {0} {1}", sodetail.InvoiceId.ToString(), sodetail.ItemID);
                        setupcharge.Quantity = Convert.ToDouble(dcHlp1);
                        setupcharge.ShipQuantity = 0;
                        setupcharge.BackOrderQuantity = 0;
                        setupcharge.Tax = 0;
                        setupcharge.UnitPrice = dcHlp;
                        setupcharge.ItemPosition = 0;
                        setupcharge.ItemOrder = 0;
                        setupcharge.Tax = Convert.ToDouble(dTaxRate);
                        db.InvoiceDetails.Add(setupcharge);
                        db.SaveChanges();
                    }
                }

                //Update Run Charge
                if (!string.IsNullOrEmpty(pricesc) && !string.IsNullOrEmpty(qtysc))
                {
                    szSalesOredidHlp = string.Format("Run Charge {0} {1}", sodetail.InvoiceId.ToString(), sodetail.ItemID);
                    runcharge = db.InvoiceDetails.Where(spch => spch.InvoiceId == sodetail.InvoiceId && spch.Description == szSalesOredidHlp).FirstOrDefault<InvoiceDetail>();
                    if (runcharge != null)
                    {
                        pricerc = pricerc.Replace("$", "");
                        pricerc = pricerc.Replace(",", "");
                        dcHlp = Convert.ToDecimal(pricerc);
                        runcharge.UnitPrice = dcHlp;

                        qtyrc = qtyrc.Replace("$", "");
                        qtyrc = qtyrc.Replace(",", "");
                        dcHlp1 = Convert.ToDecimal(qtyrc);
                        runcharge.Quantity = Convert.ToDouble(dcHlp1);
                        db.Entry(runcharge).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        pricerc = pricerc.Replace("$", "");
                        pricerc = pricerc.Replace(",", "");
                        dcHlp = Convert.ToDecimal(pricerc);
                        //runcharge.UnitPrice = dcHlp;

                        qtyrc = qtyrc.Replace("$", "");
                        qtyrc = qtyrc.Replace(",", "");
                        dcHlp1 = Convert.ToDecimal(qtyrc);

                        runcharge = new InvoiceDetail();
                        runcharge.InvoiceId = nSalesOrderId;
                        runcharge.ItemID = string.Empty;
                        runcharge.Sub_ItemID = string.Empty;
                        runcharge.Description = string.Format("Run Charge {0} {1}", sodetail.InvoiceId.ToString(), sodetail.ItemID);
                        runcharge.Quantity = Convert.ToDouble(dcHlp1);
                        runcharge.ShipQuantity = 0;
                        runcharge.BackOrderQuantity = 0;
                        runcharge.Tax = 0;
                        runcharge.UnitPrice = dcHlp;
                        runcharge.ItemPosition = 0;
                        runcharge.ItemOrder = 0;
                        runcharge.Tax = Convert.ToDouble(dTaxRate);
                        db.InvoiceDetails.Add(runcharge);
                        db.SaveChanges();
                    }
                }
            }


            if (!string.IsNullOrEmpty(shipment))
            {
                return RedirectToAction("Index", "Shipment", new { id = nSalesOrderId });
            }
            
            return RedirectToAction("Edit", new { id = nSalesOrderId });
        }

        //
        // GET: /Invoice/AddSalesOrderDetails
        [NoCache]
        public ActionResult AddSalesOrderDetails(string itemOrder, string shipment, int id = 0, int salesorderId = 0, int itemPos = 0)
        {
            double nItemOrder = 0;
            int nitemPosNext = 0;
            int nItemPos = 0;
            int nCurrentItemPos = 0;
            int nNextItemPos = 0;
            double dItemOrder = 0;
            double dCurrentItemOrder = 0;
            double dNextItemOrder = 0;
            double dQty = 0;
            decimal dPrice = 0;
            string szCurentItemId = "";
            string szNextItemId = "";
            InvoiceDetail salesdetail = null;
            InvoiceDetail salesdetailcurrent = null;
            InvoiceDetail salesdetailnext = null;
            IQueryable<PRICE> qryPrice = null;

            TimelyDepotContext db01 = new TimelyDepotContext();

            SUB_ITEM subitem = db.SUB_ITEM.Find(id);
            if (subitem != null)
            {
                if (!string.IsNullOrEmpty(itemOrder))
                {
                    //itemOrder = itemOrder.Replace(".", ",");
                    nItemOrder = Convert.ToDouble(itemOrder);
                }

                //Get the current invoicedetail
                salesdetailcurrent = db.InvoiceDetails.Where(sldt => sldt.InvoiceId == salesorderId && sldt.ItemPosition == itemPos && sldt.ItemOrder == nItemOrder).FirstOrDefault<InvoiceDetail>();
                if (salesdetailcurrent != null)
                {
                    nCurrentItemPos = Convert.ToInt32(salesdetailcurrent.ItemPosition);
                    dCurrentItemOrder = Convert.ToDouble(salesdetailcurrent.ItemOrder);
                    szCurentItemId = salesdetailcurrent.ItemID;
                }

                //Get the next salesorderdetail
                salesdetailnext = db.InvoiceDetails.Where(sldt => sldt.InvoiceId == salesorderId && sldt.ItemPosition == nCurrentItemPos && sldt.ItemOrder > dCurrentItemOrder).OrderBy(sldt => sldt.ItemOrder).FirstOrDefault<InvoiceDetail>();
                if (salesdetailnext != null)
                {
                    nNextItemPos = Convert.ToInt32(salesdetailnext.ItemPosition);
                    szNextItemId = salesdetailnext.ItemID;
                    dNextItemOrder = Convert.ToDouble(salesdetailnext.ItemOrder);
                }

                if (subitem.ItemID == szNextItemId)
                {
                    nItemPos = nCurrentItemPos;
                    dItemOrder = (dCurrentItemOrder + dNextItemOrder) / 2;
                }
                else
                {
                    nItemPos = nCurrentItemPos;
                    dItemOrder = dCurrentItemOrder + 1;
                }


                //
                // Set the price and qty search to the lowest price for this item
                double dDiscountPrc = 0;
                qryPrice = db.PRICEs.Where(prc => prc.Item == subitem.ItemID).OrderBy(prc => prc.Qty);
                if (qryPrice.Count() > 0)
                {
                    foreach (var item in qryPrice)
                    {
                        dQty = item.Qty;
                        dPrice = item.thePrice;
                        dDiscountPrc = TimelyDepotMVC.Controllers.InventoryController.GetDiscount(db01, item.Discount_Code);
                        break;
                    }
                }

                salesdetail = new InvoiceDetail();
                salesdetail.InvoiceId = salesorderId;
                salesdetail.ItemID = subitem.ItemID;
                salesdetail.Sub_ItemID = subitem.Sub_ItemID;
                salesdetail.BackOrderQuantity = 0;
                salesdetail.Description = subitem.Description;
                salesdetail.Quantity = dQty;
                salesdetail.ShipQuantity = 0;
                salesdetail.Tax = 0;
                salesdetail.UnitPrice = dPrice * (1 - Convert.ToDecimal(dDiscountPrc));
                salesdetail.ItemPosition = nItemPos;
                salesdetail.ItemOrder = dItemOrder;
                db.InvoiceDetails.Add(salesdetail);
                db.SaveChanges();
            }

            if (!string.IsNullOrEmpty(shipment))
            {
                return RedirectToAction("Index", "Shipment", new { id = salesorderId });
            }

            return RedirectToAction("Edit", new { id = salesorderId });
        }

        //
        // GET: /Invoice/AddDetail
        [NoCache]
        public PartialViewResult AddDetail(string searchitem, string itemOrder, int? page, int salesorderid = 0, int itemPos = 0)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            pageSize = 15;

            IQueryable<SUB_ITEM> qrysubitem = null;

            List<SUB_ITEM> subitemList = new List<SUB_ITEM>();

            if (string.IsNullOrEmpty(searchitem))
            {
                qrysubitem = db.SUB_ITEM.OrderBy(sbit => sbit.Sub_ItemID);
            }
            else
            {
                ViewBag.SearchItem = searchitem;
                qrysubitem = db.SUB_ITEM.Where(sbit => sbit.Sub_ItemID.StartsWith(searchitem)).OrderBy(sbit => sbit.Sub_ItemID);
            }

            if (qrysubitem.Count() > 0)
            {
                foreach (var item in qrysubitem)
                {
                    subitemList.Add(item);
                }
            }

            ViewBag.SalesOrderId = salesorderid;
            ViewBag.ItemPos = itemPos;
            ViewBag.ItemOrder = itemOrder;

            //Set the page
            if (page == null)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = Convert.ToInt32(page);
            }


            var onePageOfData = subitemList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;

            return PartialView(subitemList.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /Invoice/BlindShip
        [NoCache]
        public PartialViewResult BlindShip(string salesorderid, string invoiceid)
        {
            int nSalesOrderid = Convert.ToInt32(salesorderid);
            IQueryable<SalesOrderBlindShip> qrySalesBlid = null;
            SalesOrderBlindShip salesblind = null;

            qrySalesBlid = db.SalesOrderBlindShips.Where(blsp => blsp.SalesOrderId == nSalesOrderid);
            if (qrySalesBlid.Count() > 0)
            {
                salesblind = qrySalesBlid.FirstOrDefault<SalesOrderBlindShip>();
            }
            else
            {
                salesblind = new SalesOrderBlindShip();
                salesblind.SalesOrderId = nSalesOrderid;
                db.SalesOrderBlindShips.Add(salesblind);
                db.SaveChanges();
            }

            ViewBag.InvoiceId = invoiceid;

            return PartialView(salesblind);
        }

        //
        // GET: /Invoice/GetSalesDetailsSRC
        [NoCache]
        public PartialViewResult GetSalesDetailsSRC(int? page, string salesorderid)
        {
            TimelyDepotContext db01 = new TimelyDepotContext();

            int pageIndex = 0;
            int pageSize = PageSize;
            pageSize = 10;
            int nSalesOrderid = Convert.ToInt32(salesorderid);
            string szSalesOredidHlp = "";
            string szShipped = "";

            IQueryable<InvoiceDetail> qrysalesdetails = null;
            IQueryable<ImprintMethods> qryImprint = null;

            InvoiceDetail setupcharge = null;
            InvoiceDetail runcharge = null;
            InvoiceDetailSRC salesorderdetailSRC = null;
            ShipmentDetails shipped = null;

            List<InvoiceDetailSRC> salesdetailsList = new List<InvoiceDetailSRC>();
            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            //qrysalesdetails = db.SalesOrderDetails.Where(sldt => sldt.SalesOrderId == nSalesOrderid).OrderBy(sldt => sldt.Sub_ItemID).ThenBy(sldt => sldt.ItemOrder);
            qrysalesdetails = db.InvoiceDetails.Where(sldt => sldt.InvoiceId == nSalesOrderid && sldt.ItemID != "").OrderBy(sldt => sldt.ItemPosition).ThenBy(sldt => sldt.ItemOrder);
            if (qrysalesdetails.Count() > 0)
            {
                foreach (var item in qrysalesdetails)
                {
                    salesorderdetailSRC = new InvoiceDetailSRC();
                    salesorderdetailSRC.BackOrderQuantity = item.BackOrderQuantity;
                    salesorderdetailSRC.Description = item.Description;
                    salesorderdetailSRC.Id = item.Id;
                    salesorderdetailSRC.ImprintMethod = item.ImprintMethod;
                    salesorderdetailSRC.ItemID = item.ItemID;
                    salesorderdetailSRC.ItemOrder = item.ItemOrder;
                    salesorderdetailSRC.ItemPosition = item.ItemPosition;
                    salesorderdetailSRC.Logo = item.Logo;
                    salesorderdetailSRC.Quantity = item.Quantity;
                    salesorderdetailSRC.SalesOrderId = item.InvoiceId;
                    salesorderdetailSRC.ShipQuantity = item.ShipQuantity;
                    salesorderdetailSRC.Sub_ItemID = item.Sub_ItemID;
                    salesorderdetailSRC.Tax = item.Tax;
                    salesorderdetailSRC.UnitPrice = item.UnitPrice;
                    salesorderdetailSRC.QuantitySC = 0;
                    salesorderdetailSRC.UnitPricSRC = 0;
                    salesorderdetailSRC.QuantityRC = 0;
                    salesorderdetailSRC.UnitPriceRC = 0;

                    //Set Up Charge
                    szSalesOredidHlp = string.Format("Set up Charge {0} {1}", salesorderdetailSRC.SalesOrderId.ToString(), salesorderdetailSRC.ItemID);
                    setupcharge = db01.InvoiceDetails.Where(stup => stup.InvoiceId == item.InvoiceId && stup.Description == szSalesOredidHlp).FirstOrDefault<InvoiceDetail>();
                    if (setupcharge != null)
                    {
                        salesorderdetailSRC.QuantitySC = setupcharge.Quantity;
                        salesorderdetailSRC.UnitPricSRC = setupcharge.UnitPrice;
                    }
                    //Run Charge
                    szSalesOredidHlp = string.Format("Run Charge {0} {1}", salesorderdetailSRC.SalesOrderId.ToString(), salesorderdetailSRC.ItemID);
                    runcharge = db01.InvoiceDetails.Where(stup => stup.InvoiceId == item.InvoiceId && stup.Description == szSalesOredidHlp).FirstOrDefault<InvoiceDetail>();
                    if (runcharge != null)
                    {
                        salesorderdetailSRC.QuantityRC = runcharge.Quantity;
                        salesorderdetailSRC.UnitPriceRC = runcharge.UnitPrice;
                    }


                    salesdetailsList.Add(salesorderdetailSRC);

                    //Get the shipped items
                    shipped = db01.ShipmentDetails.Where(shpdtl => shpdtl.DetailId == item.Id).FirstOrDefault<ShipmentDetails>();
                    if (shipped != null)
                    {
                        if (string.IsNullOrEmpty(szShipped))
                        {
                            szShipped = string.Format("lkshiped_{0}", item.Id);
                        }
                        else
                        {
                            szShipped = string.Format("{0};lkshiped_{1}", szShipped ,item.Id);
                        }
                    }

                }
            }
            ViewBag.SalesOrderId = salesorderid;
            if (!string.IsNullOrEmpty(szShipped))
            {
                ViewBag.ShippedItems = szShipped;
            }


            //Get the imprint methods
            qryImprint = db.ImprintMethods.OrderBy(trd => trd.Description);
            if (qryImprint.Count() > 0)
            {
                foreach (var item in qryImprint)
                {
                    listSelector.Add(new KeyValuePair<string, string>(item.Description, item.Description));
                }
            }
            SelectList imprintlist = new SelectList(listSelector, "Key", "Value");
            ViewBag.ImprintList = imprintlist;



            //Set the page
            if (page == null)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = Convert.ToInt32(page);
            }


            var onePageOfData = salesdetailsList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return PartialView(salesdetailsList.ToPagedList(pageIndex, pageSize));
        }



        //
        // GET: /Invoice/GetSalesDetails
        [NoCache]
        public PartialViewResult GetSalesDetails(int? page, string salesorderid)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            pageSize = 10;
            int nSalesOrderid = Convert.ToInt32(salesorderid);

            IQueryable<InvoiceDetail> qrysalesdetails = null;

            List<InvoiceDetail> salesdetailsList = new List<InvoiceDetail>();

            //qrysalesdetails = db.SalesOrderDetails.Where(sldt => sldt.SalesOrderId == nSalesOrderid).OrderBy(sldt => sldt.Sub_ItemID).ThenBy(sldt => sldt.ItemOrder);
            qrysalesdetails = db.InvoiceDetails.Where(sldt => sldt.InvoiceId == nSalesOrderid).OrderBy(sldt => sldt.ItemPosition).ThenBy(sldt => sldt.ItemOrder);
            if (qrysalesdetails.Count() > 0)
            {
                foreach (var item in qrysalesdetails)
                {
                    salesdetailsList.Add(item);
                }
            }
            ViewBag.SalesOrderId = salesorderid;

            //Set the page
            if (page == null)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = Convert.ToInt32(page);
            }


            var onePageOfData = salesdetailsList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return PartialView(salesdetailsList.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET:/Invoice/DeleteInvoice
        [NoCache]
        public ActionResult DeleteInvoice(int id = 0)
        {
            InvoiceDetail invdetail = null;
            IQueryable<InvoiceDetail> qryInvoiceDetail = db.InvoiceDetails.Where(ivdt => ivdt.InvoiceId == id);
            if (qryInvoiceDetail.Count() > 0)
            {
                foreach (var item in qryInvoiceDetail)
                {
                    invdetail = db.InvoiceDetails.Find(item.Id);
                    if (invdetail != null)
                    {
                        db.InvoiceDetails.Remove(invdetail);
                    }
                }
            }

            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //
        // GET: //GenerateInvoice
        public ActionResult GenerateInvoice(int id = 0)
        {
            int nInvoiceNo = 0;
            int nTrackingNo = 0;
            int nPos = -1;
            double dTax = 0;
            string szMsg = "";
            string[] szHlp = null;
            InitialInfo initialinfo = null;
            Invoice invoice = null;
            InvoiceDetail invoicedetail = null;

            TimelyDepotContext db01 = new TimelyDepotContext();

            IQueryable<SalesOrderDetail> qrySODetails = null;

            SalesOrder salesorder = db.SalesOrders.Find(id);
            if (salesorder != null)
            {
                //Get the next payment No
                initialinfo = db.InitialInfoes.FirstOrDefault<InitialInfo>();
                if (initialinfo == null)
                {
                    initialinfo = new InitialInfo();
                    initialinfo.InvoiceNo = 1;
                    initialinfo.PaymentNo = 0;
                    initialinfo.PurchaseOrderNo = 0;
                    initialinfo.SalesOrderNo = 0;
                    initialinfo.TaxRate = 0;
                    initialinfo.TrackingNo = 1;
                    db.InitialInfoes.Add(initialinfo);
                }
                else
                {
                    nInvoiceNo = initialinfo.InvoiceNo;
                    nInvoiceNo++;
                    initialinfo.InvoiceNo = nInvoiceNo;
                    nTrackingNo = Convert.ToInt32(initialinfo.TrackingNo);
                    nTrackingNo++;
                    initialinfo.TrackingNo = nTrackingNo;
                    dTax = initialinfo.TaxRate;
                    db.Entry(initialinfo).State = EntityState.Modified;
                }

                //Use the sales order tax information
                if (salesorder.Tax_rate != null)
                {
                    if (Convert.ToDecimal(salesorder.Tax_rate) >= 0)
                    {
                        dTax = Convert.ToDouble(salesorder.Tax_rate);
                    }
                }


                //Create the Invoice
                invoice = new Invoice();
                invoice.InvoiceNo = nInvoiceNo.ToString();
                invoice.CustomerId = salesorder.CustomerId;
                invoice.BussinesType = salesorder.BussinesType;
                invoice.CreaditCardNo = salesorder.CreaditCardNo;
                invoice.CustomerShipLocation = salesorder.CustomerShipLocation;
                invoice.CustomerShiptoId = salesorder.CustomerShiptoId;
                //invoice.InvoiceDate = DateTime.Now;
                invoice.InvoiceDate = Convert.ToDateTime(salesorder.SODate);
                invoice.IsBlindShip = salesorder.IsBlindShip;
                invoice.Note = salesorder.Note;
                invoice.PaymentAmount = salesorder.PaymentAmount;
                invoice.PaymentDate = salesorder.PaymentDate;
                invoice.PaymentTerms = salesorder.PaymentTerms;
                invoice.PurchaseOrderNo = salesorder.PurchaseOrderNo;
                invoice.SalesOrderId = salesorder.SalesOrderId;
                invoice.SalesOrderNo = salesorder.SalesOrderNo;
                invoice.SalesRep = salesorder.SalesRep;
                invoice.ShipDate = salesorder.ShipDate;
                invoice.ShippingHandling = salesorder.ShippingHandling;
                invoice.ShipVia = salesorder.ShipVia;
                invoice.TrackingNo = nTrackingNo.ToString();
                invoice.TradeId = salesorder.TradeId;
                invoice.VendorAddress = salesorder.VendorAddress;
                invoice.VendorId = salesorder.VendorId;
                invoice.Tax_rate = Convert.ToDecimal(dTax);
                invoice.Invs_Tax = Convert.ToDecimal(dTax);

                //Set the shipment information
                invoice.FromAddress1 = salesorder.FromAddress1;
                invoice.FromAddress2 = salesorder.FromAddress2;
                invoice.FromCity = salesorder.FromCity;
                invoice.FromCompany = salesorder.FromCompany;
                invoice.FromCountry = salesorder.FromCountry;
                invoice.FromEmail = salesorder.FromEmail;
                invoice.FromFax = salesorder.FromFax;
                invoice.FromName = salesorder.FromName;
                invoice.FromState = salesorder.FromState;
                invoice.FromTel = salesorder.FromTel;
                invoice.FromTitle = salesorder.FromTitle;
                invoice.FromZip = salesorder.FromZip;

                invoice.ToAddress1 = salesorder.ToAddress1;
                invoice.ToAddress2 = salesorder.ToAddress2;
                invoice.ToCity = salesorder.ToCity;
                invoice.ToCompany = salesorder.ToCompany;
                invoice.ToCountry = salesorder.ToCountry;
                invoice.ToEmail = salesorder.ToEmail;
                invoice.ToFax = salesorder.ToFax;
                invoice.ToName = salesorder.ToName;
                invoice.ToState = salesorder.ToState;
                invoice.ToTel = salesorder.ToTel;
                invoice.ToTitle = salesorder.ToTitle;
                invoice.ToZip = salesorder.ToZip;

                db.Invoices.Add(invoice);
                db.SaveChanges();

                //Create the details
                qrySODetails = db.SalesOrderDetails.Where(sodt => sodt.SalesOrderId == salesorder.SalesOrderId);
                if (qrySODetails.Count() > 0)
                {
                    foreach (var item in qrySODetails)
                    {
                        invoicedetail = new InvoiceDetail();
                        invoicedetail.BackOrderQuantity = 0;
                        invoicedetail.Description = item.Description;

                        nPos = -1;
                        nPos = item.Description.IndexOf("Set up Charge");
                        if (nPos != -1)
                        {
                            szHlp = item.Description.Split(' ');
                            szHlp[3] = invoice.InvoiceId.ToString();
                            szMsg = string.Format("{0} {1} {2} {3} {4}", szHlp[0], szHlp[1], szHlp[2], szHlp[3], szHlp[4]);
                            invoicedetail.Description = szMsg;
                        }

                        nPos = -1;
                        nPos = item.Description.IndexOf("Run Charge");
                        if (nPos != -1)
                        {
                            szHlp = item.Description.Split(' ');
                            szHlp[2] = invoice.InvoiceId.ToString();
                            szMsg = string.Format("{0} {1} {2} {3}", szHlp[0], szHlp[1], szHlp[2], szHlp[3]);
                            invoicedetail.Description = szMsg;
                        }

                        invoicedetail.InvoiceId = invoice.InvoiceId;
                        invoicedetail.ItemID = item.ItemID;
                        invoicedetail.ItemOrder = item.ItemOrder;
                        invoicedetail.ItemPosition = item.ItemPosition;
                        invoicedetail.Quantity = item.Quantity;
                        invoicedetail.ShipQuantity = item.Quantity;
                        invoicedetail.Sub_ItemID = item.Sub_ItemID;
                        invoicedetail.Tax = item.Tax;
                        invoicedetail.UnitPrice = item.UnitPrice;
                        invoicedetail.Tax = item.Tax;
                        db.InvoiceDetails.Add(invoicedetail);

                    }

                    db.SaveChanges();
                }

            }

            return RedirectToAction("Edit", new { id = invoice.InvoiceId });
        }

        //
        // GET: /Invoice/SelectSalesOrder
        [NoCache]
        public PartialViewResult SelectSalesOrder(int? page, string searchOrderNo, string searchCustomer, string searchEmail)
        {
            bool bStatus = false;
            int pageIndex = 0;
            int pageSize = PageSize;
            int nHas = 0;
            Customers customer = null;
            SalesOrder salesorder = null;

            IQueryable<SalesOrder> qrySalesOrder = null;
            IQueryable<CustomersContactAddress> qryAddress = null;

            List<int> customerIdsList = new List<int>();
            List<SalesOrder> SalesOrderList = new List<SalesOrder>();

            if (string.IsNullOrEmpty(searchOrderNo) && string.IsNullOrEmpty(searchCustomer) && string.IsNullOrEmpty(searchEmail))
            {
                qrySalesOrder = db.SalesOrders.OrderBy(slor => slor.SalesOrderNo);
                if (qrySalesOrder.Count() > 0)
                {
                    foreach (var item in qrySalesOrder)
                    {
                        bStatus = VerifyInvoiceSalesOrder(item);
                        if (bStatus == false)
                        {
                            SalesOrderList.Add(item);
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(searchEmail))
                {
                    ViewBag.SearchEmail = searchEmail;

                    qryAddress = db.CustomersContactAddresses.Where(ctad => ctad.Email.StartsWith(searchEmail));
                    if (qryAddress.Count() > 0)
                    {
                        foreach (var item in qryAddress)
                        {
                            nHas = Convert.ToInt32(item.CustomerId);
                            if (nHas > 0)
                            {
                                customerIdsList.Add(nHas);
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(searchCustomer))
                {
                    ViewBag.SearchCustomer = searchCustomer;

                    qryAddress = db.CustomersContactAddresses.Where(ctad => ctad.CompanyName.StartsWith(searchCustomer));
                    if (qryAddress.Count() > 0)
                    {
                        foreach (var item in qryAddress)
                        {
                            nHas = Convert.ToInt32(item.CustomerId);
                            if (nHas > 0)
                            {
                                customerIdsList.Add(nHas);
                            }
                        }
                    }
                }

                if (customerIdsList.Count > 0)
                {
                    foreach (var itemCustomer in customerIdsList)
                    {
                        qrySalesOrder = db.SalesOrders.Where(slor => slor.CustomerId == itemCustomer).OrderBy(slor => slor.SalesOrderNo);
                        if (qrySalesOrder.Count() > 0)
                        {
                            foreach (var item in qrySalesOrder)
                            {
                                SalesOrderList.Add(item);
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(searchOrderNo))
                {
                    ViewBag.SearchOrderNo = searchOrderNo;

                    qrySalesOrder = db.SalesOrders.Where(slor => slor.SalesOrderNo.StartsWith(searchOrderNo)).OrderBy(slor => slor.SalesOrderNo);
                    if (qrySalesOrder.Count() > 0)
                    {
                        foreach (var item in qrySalesOrder)
                        {
                            SalesOrderList.Add(item);
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


            var onePageOfData = SalesOrderList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return PartialView(SalesOrderList.ToPagedList(pageIndex, pageSize));
        }

        private bool VerifyInvoiceSalesOrder(SalesOrder item)
        {
            bool bStatus = false;
            Invoice invoice = null;

            TimelyDepotContext db01 = new TimelyDepotContext();

            invoice = db01.Invoices.Where(invc => invc.SalesOrderNo == item.SalesOrderNo).FirstOrDefault<Invoice>();
            if (invoice != null)
            {
                bStatus = true;
            }

            return bStatus;
        }


        //
        // GET: /Invoice/

        public ActionResult Index(int? page, string searchItem, string ckActive, string ckCriteria)
        {
            bool bHasData = true;
            int nYear = 0;
            int nMonth = 0;
            int nDay = 0;
            int pageIndex = 0;
            int pageSize = PageSize;
            string[] szFecha = null;
            DateTime dFecha = DateTime.Now;
            IQueryable<Invoice> qryInvoice = null;

            List<Invoice> InvoiceList = new List<Invoice>();
            if (string.IsNullOrEmpty(searchItem) || searchItem == "0")
            {
                //qryItem = db.ITEMs.OrderBy(it => it.ItemID);
                ViewBag.ckActiveHlp = "true";
                ViewBag.ckCriteriaHlp = "invoice";
                ViewBag.CurrentDate = dFecha.ToString("yyyy/MM/dd");
                bHasData = false;

                if (searchItem == "0")
                {
                    ViewBag.SearchItem = searchItem;
                    bHasData = true;

                    if (ckCriteria == "invoice")
                    {
                        if (ckActive == "true")
                        {
                            qryInvoice = db.Invoices.OrderBy(vd => vd.InvoiceNo);
                        }
                        else
                        {
                            qryInvoice = db.Invoices.OrderBy(vd => vd.InvoiceNo);
                        }
                    }
                }
            }
            else
            {
                ViewBag.SearchItem = searchItem;
                ViewBag.ckActiveHlp = ckActive;
                ViewBag.ckCriteriaHlp = ckCriteria;

                if (ckCriteria == "invoice")
                {
                    if (ckActive == "true")
                    {
                        qryInvoice = db.Invoices.Where(vd => vd.InvoiceNo.StartsWith(searchItem)).OrderBy(vd => vd.InvoiceNo);
                    }
                    else
                    {
                        qryInvoice = db.Invoices.Where(vd => vd.InvoiceNo.StartsWith(searchItem)).OrderBy(vd => vd.InvoiceNo);
                    }
                }

                if (ckCriteria == "salesorder")
                {
                    if (ckActive == "true")
                    {
                        qryInvoice = db.Invoices.Where(vd => vd.SalesOrderNo.StartsWith(searchItem)).OrderBy(vd => vd.SalesOrderNo);
                    }
                    else
                    {
                        qryInvoice = db.Invoices.Where(vd => vd.SalesOrderNo.StartsWith(searchItem)).OrderBy(vd => vd.SalesOrderNo);
                    }
                }

                if (ckCriteria == "shippeddate")
                {
                    szFecha = searchItem.Split('/');
                    if (szFecha != null)
                    {
                        nYear = Convert.ToInt32(szFecha[0]);
                        nMonth = Convert.ToInt32(szFecha[1]);
                        nDay = Convert.ToInt32(szFecha[2]);
                        dFecha = new DateTime(nYear, nMonth, nDay);
                    }

                    if (ckActive == "true")
                    {
                        qryInvoice = db.Invoices.Where(vd => vd.ShipDate >= dFecha).OrderBy(vd => vd.SalesOrderNo);
                    }
                    else
                    {
                        qryInvoice = db.Invoices.Where(vd => vd.ShipDate >= dFecha).OrderBy(vd => vd.SalesOrderNo);
                    }
                }

                if (ckCriteria == "customerpo")
                {
                    if (ckActive == "true")
                    {
                        qryInvoice = db.Invoices.Where(vd => vd.PurchaseOrderNo.StartsWith(searchItem)).OrderBy(vd => vd.PurchaseOrderNo);
                    }
                    else
                    {
                        qryInvoice = db.Invoices.Where(vd => vd.PurchaseOrderNo.StartsWith(searchItem)).OrderBy(vd => vd.PurchaseOrderNo);
                    }
                }

                if (ckCriteria == "customername")
                {
                    if (ckActive == "true")
                    {
                        var qryMainContact = db.Invoices.Join(db.CustomersContactAddresses, ctc => ctc.CustomerId, cus => cus.CustomerId, (ctc, cus)
                             => new { ctc, cus }).Where(Nctcs => Nctcs.cus.CompanyName.StartsWith(searchItem)).OrderBy(Nctcs => Nctcs.cus.CompanyName);
                        if (qryMainContact.Count() > 0)
                        {
                            foreach (var item in qryMainContact)
                            {
                                InvoiceList.Add(item.ctc);
                            }
                        }
                    }
                    else
                    {
                        var qryMainContact = db.Invoices.Join(db.CustomersContactAddresses, ctc => ctc.CustomerId, cus => cus.CustomerId, (ctc, cus)
                             => new { ctc, cus }).Where(Nctcs => Nctcs.cus.CompanyName.StartsWith(searchItem)).OrderBy(Nctcs => Nctcs.cus.CompanyName);
                        if (qryMainContact.Count() > 0)
                        {
                            foreach (var item in qryMainContact)
                            {
                                InvoiceList.Add(item.ctc);
                            }
                        }
                    }
                    bHasData = false;
                }
            }



            if (bHasData)
            {
                if (qryInvoice != null)
                {
                    if (qryInvoice.Count() > 0)
                    {
                        foreach (var item in qryInvoice)
                        {
                            InvoiceList.Add(item);
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


            var onePageOfData = InvoiceList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(InvoiceList.ToPagedList(pageIndex, pageSize));
            //return View(db.Invoices.ToList());
        }

        //
        // GET: /Invoice/Details/5

        public ActionResult Details(int id = 0)
        {
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        //
        // GET: /Invoice/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Invoice/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Invoices.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(invoice);
        }

        //
        // GET: /Invoice/Edit/5
        [NoCache]
        public ActionResult Edit(int id = 0)
        {
            double dSalesAmount = 0;
            double dTax = 0;
            double dTotalTax = 0;
            double dTotalAmount = 0;
            double dBalanceDue = 0;
            DateTime dPODate = DateTime.Now;
            string szMsg = "";

            Customers customer = null;
            CustomersContactAddress soldto = null;
            CustomersBillingDept billto = null;
            CustomersShipAddress shipto = null;
            VendorsContactAddress venaddress = null;
            VendorsSalesContact vendorsalescontact = null;
            SalesOrderBlindShip salesblind = null;
            PurchaseOrders purchaseorder = null;
            SalesOrder salesorder = null;

            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();
            List<SalesOrderDetail> salesdetailList = new List<SalesOrderDetail>();

            IQueryable<Trade> qryTrade = null;
            IQueryable<CustomersContactAddress> qryAddress = null;
            IQueryable<VendorsContactAddress> qryVenAddres = null;
            IQueryable<CustomersShipAddress> qryshipto = null;
            IQueryable<CustomersBillingDept> qryBill = null;
            IQueryable<VendorsSalesContact> qrysalescontact = null;
            IQueryable<SalesOrderDetail> qrysalesdetail = null;
            IQueryable<SalesOrderBlindShip> qryBlind = null;
            IQueryable<CustomersSalesContact> qryCusSal = null;
            IQueryable<Warehouses> qryWarehouse = null;
            IQueryable<Terms> qryTerms = null;

            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            decimal sumRefunds = 0;

          

            //Get the dropdown data
            qryTrade = db.Trades.OrderBy(trd => trd.TradeName);
            if (qryTrade.Count() > 0)
            {
                foreach (var item in qryTrade)
                {
                    listSelector.Add(new KeyValuePair<string, string>(item.TradeId.ToString(), item.TradeName));
                }
            }
            SelectList tradeselectorlist = new SelectList(listSelector, "Key", "Value");
            ViewBag.TradeList = tradeselectorlist;


            //Get the SolTo Data
            listSelector = new List<KeyValuePair<string, string>>();
            qryAddress = db.CustomersContactAddresses.Where(ctad => ctad.CustomerId == invoice.CustomerId);
            if (qryAddress.Count() > 0)
            {
                soldto = qryAddress.FirstOrDefault<CustomersContactAddress>();
                if (soldto != null)
                {
                    ViewBag.SoldTo = soldto;
                }
            }
            SelectList mainContactlist = new SelectList(listSelector, "Key", "Value");
            ViewBag.MainContactList = mainContactlist;

            //Get the CustomerNo
            if (soldto != null)
            {
                customer = db.Customers.Where(cust => cust.Id == soldto.CustomerId).FirstOrDefault<Customers>();
                if (customer != null)
                {
                    ViewBag.Customer = customer;

                    //Update payment terms
                    if (string.IsNullOrEmpty(invoice.PaymentTerms))
                    {
                        invoice.PaymentTerms = customer.PaymentTerms;
                    }

                    listSelector = new List<KeyValuePair<string, string>>();
                    qryCusSal = db.CustomersSalesContacts.Where(csp => csp.CustomerId == customer.Id).OrderBy(csp => csp.FirstName).ThenBy(csp => csp.LastName);
                    if (qryCusSal.Count() > 0)
                    {
                        foreach (var item in qryCusSal)
                        {
                            szMsg = string.Format("{0} {1}", item.FirstName, item.LastName);
                            listSelector.Add(new KeyValuePair<string, string>(item.Id.ToString(), szMsg));
                        }
                    }
                    SelectList cusdefaultlist = new SelectList(listSelector, "Key", "Value");
                    ViewBag.SalesContactList = cusdefaultlist;

                    if (string.IsNullOrEmpty(invoice.SalesRep))
                    {
                        invoice.SalesRep = customer.SalesPerson;
                    }
                }
            }
            else
            {
                customer = db.Customers.Where(cust => cust.Id == invoice.CustomerId).FirstOrDefault<Customers>();
                if (customer != null)
                {
                    ViewBag.Customer = customer;

                    //Update payment terms
                    if (string.IsNullOrEmpty(invoice.PaymentTerms))
                    {
                        invoice.PaymentTerms = customer.PaymentTerms;
                    }

                    listSelector = new List<KeyValuePair<string, string>>();
                    qryCusSal = db.CustomersSalesContacts.Where(csp => csp.CustomerId == customer.Id).OrderBy(csp => csp.FirstName).ThenBy(csp => csp.LastName);
                    if (qryCusSal.Count() > 0)
                    {
                        foreach (var item in qryCusSal)
                        {
                            szMsg = string.Format("{0} {1}", item.FirstName, item.LastName);
                            listSelector.Add(new KeyValuePair<string, string>(item.Id.ToString(), szMsg));
                        }
                    }
                    SelectList cusdefaultlist = new SelectList(listSelector, "Key", "Value");
                    ViewBag.SalesContactList = cusdefaultlist;

                    if (string.IsNullOrEmpty(invoice.SalesRep))
                    {
                        invoice.SalesRep = customer.SalesPerson;
                    }
                }

            }

            listSelector = new List<KeyValuePair<string, string>>();
            qryWarehouse = db.Warehouses.OrderBy(csp => csp.Warehouse);
            if (qryWarehouse.Count() > 0)
            {
                foreach (var item in qryWarehouse)
                {
                    listSelector.Add(new KeyValuePair<string, string>(item.Warehouse, item.Warehouse));
                }
            }
            SelectList warehouselist = new SelectList(listSelector, "Key", "Value");
            ViewBag.WarehouseList = warehouselist;

            //Get the Bill to data
            listSelector = new List<KeyValuePair<string, string>>();
            qryBill = db.CustomersBillingDepts.Where(ctbi => ctbi.CustomerId == invoice.CustomerId);
            if (qryBill.Count() > 0)
            {
                //billto = qryBill.FirstOrDefault<CustomersBillingDept>();
                //if (billto != null)
                //{
                //    ViewBag.BillTo = billto;
                //}
                foreach (var item in qryBill)
                {
                    if (billto == null)
                    {
                        //billto = qryBill.FirstOrDefault<CustomersBillingDept>();
                        billto = item;
                        ViewBag.BillTo = billto;
                    }

                    szMsg = string.Format("{0} {1}", item.FirstName, item.LastName);
                    listSelector.Add(new KeyValuePair<string, string>(item.Id.ToString(), szMsg));
                }
            }
            SelectList billtolist = new SelectList(listSelector, "Key", "Value");
            ViewBag.BillToList = billtolist;

            //Get the ship to data
            listSelector = new List<KeyValuePair<string, string>>();
            qryshipto = db.CustomersShipAddresses.Where(ctsp => ctsp.Id == invoice.CustomerId);
            if (qryshipto.Count() > 0)
            {
                //shipto = qryshipto.FirstOrDefault<CustomersShipAddress>();
                //if (shipto != null)
                //{
                //    ViewBag.ShipTo = shipto;
                //}
                foreach (var item in qryshipto)
                {
                    if (shipto == null)
                    {
                        shipto = item;
                        ViewBag.ShipTo = shipto;
                    }
                    if (!string.IsNullOrEmpty(item.ShippingPreference))
                    {
                        listSelector.Add(new KeyValuePair<string, string>(item.ShippingPreference, item.ShippingPreference));
                    }
                }
                SelectList shipvialist = new SelectList(listSelector, "Key", "Value");
                ViewBag.ShipViaList = shipvialist;

            }

            //Get the Vendor address data
            qryVenAddres = db.VendorsContactAddresses.Where(vnad => vnad.VendorId == invoice.VendorId);
            if (qryVenAddres.Count() > 0)
            {
                venaddress = qryVenAddres.FirstOrDefault<VendorsContactAddress>();
                if (venaddress != null)
                {
                    ViewBag.VendorAddress = venaddress;
                }
            }

            //Get the sales contact
            qrysalescontact = db.VendorsSalesContacts.Where(vdsc => vdsc.Id == invoice.VendorId);
            if (qrysalescontact.Count() > 0)
            {
                vendorsalescontact = qrysalescontact.FirstOrDefault<VendorsSalesContact>();
                if (vendorsalescontact != null)
                {
                    ViewBag.VendorSalesContact = vendorsalescontact;
                }
            }

            //Get the blind ship addres
            qryBlind = db.SalesOrderBlindShips.Where(slbd => slbd.SalesOrderId == invoice.SalesOrderId);
            if (qryBlind.Count() > 0)
            {
                salesblind = qryBlind.FirstOrDefault<SalesOrderBlindShip>();
                if (salesblind != null)
                {
                    ViewBag.BlindShip = salesblind;
                }
            }

            //Get the Purchase order
            if (string.IsNullOrEmpty(invoice.PurchaseOrderNo))
            {
                purchaseorder = db.PurchaseOrders.Where(po => po.SalesOrderNo == invoice.SalesOrderNo).FirstOrDefault<PurchaseOrders>();
                if (purchaseorder != null)
                {
                    invoice.PurchaseOrderNo = purchaseorder.PurchaseOrderNo;
                    if (purchaseorder.PODate != null)
                    {
                        dPODate = Convert.ToDateTime(purchaseorder.PODate);
                        ViewBag.PODate = dPODate.ToString("MM/dd/yyyy");
                    }
                }
                else
                {
                    ViewBag.PODate = string.Empty;
                }
            }

            //Get the sales order
            if (!string.IsNullOrEmpty(invoice.SalesOrderNo))
            {
                salesorder = db.SalesOrders.Where(slod => slod.SalesOrderNo == invoice.SalesOrderNo).FirstOrDefault<SalesOrder>();
                if (salesorder != null)
                {
                    ViewBag.SalesOrderDate = Convert.ToDateTime(salesorder.SODate).ToString("MM/dd/yyyy");
                }
                else
                {
                    ViewBag.SalesOrderDate = string.Empty;
                }
            }
            else
            {
                ViewBag.SalesOrderDate = string.Empty;
            }
         

            //Get the totals
           GetSalesOrderTotals(
               salesorder.SalesOrderId,
               ref dSalesAmount,
               ref dTotalTax,
               ref dTax,
               ref dTotalAmount,
               ref dBalanceDue);
            //GetInvoiceTotals(invoice.InvoiceId, ref dSalesAmount, ref dTotalTax, ref dTax, ref dTotalAmount, ref dBalanceDue);
            ViewBag.SalesAmount = dSalesAmount.ToString("C");
            ViewBag.TotalTax = dTotalTax.ToString("C");
            ViewBag.Tax = dTax.ToString("F2");
            ViewBag.TotalAmount = dTotalAmount.ToString("C");
            ViewBag.BalanceDue = dBalanceDue.ToString("C");
           


            //Get the terms data
            listSelector = new List<KeyValuePair<string, string>>();
            qryTerms = db.Terms.OrderBy(trm => trm.Term);
            if (qryTerms.Count() > 0)
            {
                foreach (var item in qryTerms)
                {
                    listSelector.Add(new KeyValuePair<string, string>(item.Term, item.Term));
                }
                SelectList termslist = new SelectList(listSelector, "Key", "Value");
                ViewBag.TermsList = termslist;
            }


            return View(invoice);
        }
        public void GetInvoiceTotals01(TimelyDepotContext db01, int nInvoiceId, ref double dSalesAmount, ref double dTotalTax, ref double dTax, ref double dTotalAmount, ref double dBalanceDue)
        {
            double dShipping = 0;
            double dPayment = 0;
            double dSOTax = 0;

            IQueryable<InvoiceDetail> qryDetails = null;
            InitialInfo initialinfo = null;

            dSalesAmount = 0;
            dTax = 0;
            dTotalAmount = 0;
            dBalanceDue = 0;
            dTotalTax = 0;

            initialinfo = db01.InitialInfoes.FirstOrDefault<InitialInfo>();
            if (initialinfo == null)
            {
                initialinfo = new InitialInfo();
                initialinfo.InvoiceNo = 0;
                initialinfo.PaymentNo = 0;
                initialinfo.PurchaseOrderNo = 0;
                initialinfo.SalesOrderNo = 1;
                initialinfo.TaxRate = 0;
                db01.InitialInfoes.Add(initialinfo);
                dTax = initialinfo.TaxRate;
            }
            else
            {
                dTax = initialinfo.TaxRate;
            }


            Invoice salesorder = db01.Invoices.Find(nInvoiceId);
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

                qryDetails = db01.InvoiceDetails.Where(sldt => sldt.InvoiceId == salesorder.InvoiceId);
                if (qryDetails.Count() > 0)
                {
                    foreach (var item in qryDetails)
                    {
                        //use the tax on product
                        if (item.Tax != null)
                        {
                            if (Convert.ToDecimal(item.Tax) >= 0)
                            {
                                dTax = Convert.ToDouble(item.Tax);
                            }
                        }
                        dSalesAmount = dSalesAmount + (Convert.ToDouble(item.Quantity) * Convert.ToDouble(item.UnitPrice));
                        dTotalTax = dTotalTax + (Convert.ToDouble(item.Quantity) * Convert.ToDouble(item.UnitPrice) * (dTax / 100));
                    }
                }

                dTotalAmount = dSalesAmount + dTotalTax + dShipping;
                dBalanceDue = dTotalAmount - dPayment;

                //Set the sales order tax again
                dTax = dSOTax;
            }
        }

        public void GetInvoiceTotals(int nInvoiceId, ref double dSalesAmount, ref double dTotalTax, ref double dTax, ref double dTotalAmount, ref double dBalanceDue)
        {
            double dShipping = 0;
            double dPayment = 0;
            double dSOTax = 0;

            IQueryable<InvoiceDetail> qryDetails = null;
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


            Invoice salesorder = db.Invoices.Find(nInvoiceId);
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

                qryDetails = db.InvoiceDetails.Where(sldt => sldt.InvoiceId == salesorder.InvoiceId);
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

        //
        // POST: /Invoice/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Invoice invoice, string InvoiceDateHlp01, string InvoiceDateHlp02)
        {
            int nPos = -1;
            int nYear = 0;
            int nMonth = 0;
            int nDay = 0;
            DateTime dDate = DateTime.Now;
            DateTime dDate02 = DateTime.Now;
            string szMsg = "";
            string[] szdateHlp = null;
            if (!string.IsNullOrEmpty(InvoiceDateHlp01))
            {
                szdateHlp = InvoiceDateHlp01.Split('/');
                if (szdateHlp != null)
                {
                    nMonth = Convert.ToInt32(szdateHlp[0]);
                    nDay = Convert.ToInt32(szdateHlp[1]);
                    nYear = Convert.ToInt32(szdateHlp[2]);
                    dDate = new DateTime(nYear, nMonth, nDay);
                }
            }
            if (!string.IsNullOrEmpty(InvoiceDateHlp02))
            {
                szdateHlp = InvoiceDateHlp02.Split('/');
                if (szdateHlp != null)
                {
                    nMonth = Convert.ToInt32(szdateHlp[0]);
                    nDay = Convert.ToInt32(szdateHlp[1]);
                    nYear = Convert.ToInt32(szdateHlp[2]);
                    dDate02 = new DateTime(nYear, nMonth, nDay);
                }
            }
            if (ModelState.IsValid)
            {
                invoice.InvoiceDate = dDate;
                invoice.ShipDate = dDate02;

                if (invoice.Tax_rate == null)
                {
                    invoice.Tax_rate = 0;
                }

                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                szMsg = string.Format("(Date Help: {0} - {1})", InvoiceDateHlp01, dDate.ToString());
                foreach (var item in ModelState.Values)
                {
                    if (item.Errors.Count > 0)
                    {
                        foreach (var itemError in item.Errors)
                        {
                            szMsg = string.Format("{0} {1}", szMsg, itemError.ErrorMessage);
                        }
                    }
                }

                if (invoice.Tax_rate == null)
                {
                    invoice.Tax_rate = 0;
                }

                nPos = szMsg.IndexOf("is not valid for Invoice Date.");
                if (nPos != -1)
                {
                    if (invoice.InvoiceDate != dDate)
                    {
                        invoice.InvoiceDate = dDate;
                    }
                    db.Entry(invoice).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                nPos = -1;
                nPos = szMsg.IndexOf("no es válido para Invoice Date.");
                if (nPos != -1)
                {
                    if (invoice.InvoiceDate != dDate)
                    {
                        invoice.InvoiceDate = dDate;
                    }
                    if (invoice.ShipDate != dDate02)
                    {
                        invoice.ShipDate = dDate02;
                    }
                    db.Entry(invoice).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                //if (invoice.InvoiceDate != dDate)
                //{
                //    invoice.InvoiceDate = dDate;
                //}
                //db.Entry(invoice).State = EntityState.Modified;
                //db.SaveChanges();

                //return RedirectToAction("Index");
            }

            return RedirectToAction("Edit0", new { errorMsg = szMsg });
            //return View(invoice);
        }

        //
        // GET: /Invoice/Edit0
        public ActionResult Edit0(string errorMsg)
        {
            ViewBag.ErrorMsg = errorMsg;

            return View();
        }

        //
        // GET: /Invoice/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        //
        // POST: /Invoice/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public static void GetCustomerEmail(TimelyDepotContext db01, ref string szName, ref string szEmail, int nCustomerId = 0)
        {
            CustomersContactAddress contactaddress = db01.CustomersContactAddresses.Where(ctad => ctad.CustomerId == nCustomerId).FirstOrDefault<CustomersContactAddress>();
            if (contactaddress != null)
            {
                szName = contactaddress.CompanyName;
                szEmail = contactaddress.Email;
            }
            else
            {
                szName = "";
                szEmail = "";
            }
        }

    }
}
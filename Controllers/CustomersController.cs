using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

using System.Text;
using System.Globalization;

using System.Data.SqlClient;
using System.Data.Common;

using TimelyDepotMVC.Models.Admin;
using TimelyDepotMVC.DAL;
using PagedList;
using TimelyDepotMVC.ModelsView;
using TimelyDepotMVC.CommonCode;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.UI;

namespace TimelyDepotMVC.Controllers
{
    using System.Data.Entity;

    public class CustomersController : Controller
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
        // GET: /Customer/PaymentListTab
        [NoCache]
        public ActionResult PaymentListTab(string customerid)
        {
            int nId = Convert.ToInt32(customerid);

            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            //Get theActive/Inactive list
            listSelector = new List<KeyValuePair<string, string>>();
            listSelector.Add(new KeyValuePair<string, string>("Y", "Active"));
            listSelector.Add(new KeyValuePair<string, string>("N", "Inactive"));
            SelectList activeinactivelist = new SelectList(listSelector, "Key", "Value");
            ViewBag.ActiveInactivelist = activeinactivelist;

            Customers customer = db.Customers.Find(nId);
            if (customer == null)
            {
                customer = new Customers();
            }

            return View(customer);
        }

        //
        // GET: /Customer/InvoiceListTab
        [NoCache]
        public ActionResult InvoiceListTab(string customerid)
        {
            int nId = Convert.ToInt32(customerid);

            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            //Get theActive/Inactive list
            listSelector = new List<KeyValuePair<string, string>>();
            listSelector.Add(new KeyValuePair<string, string>("Y", "Active"));
            listSelector.Add(new KeyValuePair<string, string>("N", "Inactive"));
            SelectList activeinactivelist = new SelectList(listSelector, "Key", "Value");
            ViewBag.ActiveInactivelist = activeinactivelist;

            Customers customer = db.Customers.Find(nId);
            if (customer == null)
            {
                customer = new Customers();
            }

            return View(customer);
        }

        //
        // GET: /Customer/OutstandinginvoiceTab
        [NoCache]
        public ActionResult OutstandinginvoiceTab(string customerid)
        {
            int nId = Convert.ToInt32(customerid);

            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            //Get theActive/Inactive list
            listSelector = new List<KeyValuePair<string, string>>();
            listSelector.Add(new KeyValuePair<string, string>("Y", "Active"));
            listSelector.Add(new KeyValuePair<string, string>("N", "Inactive"));
            SelectList activeinactivelist = new SelectList(listSelector, "Key", "Value");
            ViewBag.ActiveInactivelist = activeinactivelist;

            Customers customer = db.Customers.Find(nId);
            if (customer == null)
            {
                customer = new Customers();
            }

            return View(customer);
        }

        //
        // GET: /Customer/OutsandingSalesOrderTab
        [NoCache]
        public ActionResult OutsandingSalesOrderTab(string customerid)
        {
            int nId = Convert.ToInt32(customerid);

            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            //Get theActive/Inactive list
            listSelector = new List<KeyValuePair<string, string>>();
            listSelector.Add(new KeyValuePair<string, string>("Y", "Active"));
            listSelector.Add(new KeyValuePair<string, string>("N", "Inactive"));
            SelectList activeinactivelist = new SelectList(listSelector, "Key", "Value");
            ViewBag.ActiveInactivelist = activeinactivelist;

            Customers customer = db.Customers.Find(nId);
            if (customer == null)
            {
                customer = new Customers();
            }

            return View(customer);
        }

        //
        // GET: /Customer/PreviousItem
        public ActionResult PreviousCustomer(string customerNo, string opcion, int id)
        {
            string szNextId = string.Empty;


            szNextId = GetPreviousCustomer(customerNo);
            if (string.IsNullOrEmpty(szNextId))
            {
                szNextId = id.ToString();
            }

            if (!string.IsNullOrEmpty(opcion))
            {
                if (opcion == "salesorder")
                {
                    return RedirectToAction("OutsandingSalesOrderTab", new { customerid = szNextId });
                }
                if (opcion == "outsandinginvoice")
                {
                    return RedirectToAction("OutstandinginvoiceTab", new { customerid = szNextId });
                }
                if (opcion == "history")
                {
                    return RedirectToAction("InvoiceListTab", new { customerid = szNextId });
                }
                if (opcion == "Statement")
                {
                    return RedirectToAction("PaymentListTab", new { customerid = szNextId });
                }
            }

            return RedirectToAction("Edit", new { id = szNextId });
        }

        private string GetPreviousCustomer(string customerNo)
        {
            int nHas = -1;
            string szNext = string.Empty;
            string szError = string.Empty;
            string szSql = string.Empty;
            string szConnString = string.Empty;
            SqlDataSource sqlds = new SqlDataSource();
            DataView dv = null;
            ConnectionStringSettingsCollection connSettings = ConfigurationManager.ConnectionStrings;

            try
            {
                szConnString = connSettings["TimelyDepotContext"].ToString();
                sqlds.ConnectionString = szConnString;

                szSql = string.Format("SELECT TOP (100) PERCENT Id, CustomerNo FROM Customers " +
                    "WHERE (CustomerNo < N'{0}') ORDER BY CustomerNo DESC", customerNo);
                sqlds.SelectCommand = szSql;
                dv = (DataView)sqlds.Select(DataSourceSelectArguments.Empty);
                nHas = dv.Count;
                if (nHas > 0)
                {
                    szNext = dv[0]["Id"].ToString();
                }
            }
            catch (Exception exc)
            {
                szNext = string.Empty;
                szError = exc.Message;
            }

            return szNext;
        }

        //
        // GET: /Customer/NextItem
        public ActionResult NextCustomer(string customerNo, string opcion, int id)
        {
            string szNextId = string.Empty;

            szNextId = GetNextCustomer(customerNo);
            if (string.IsNullOrEmpty(szNextId))
            {
                szNextId = id.ToString();
            }

            if (!string.IsNullOrEmpty(opcion))
            {
                if (opcion == "salesorder")
                {
                    return RedirectToAction("OutsandingSalesOrderTab", new { customerid = szNextId });
                }
                if (opcion == "outsandinginvoice")
                {
                    return RedirectToAction("OutstandinginvoiceTab", new { customerid = szNextId });
                }
                if (opcion == "history")
                {
                    return RedirectToAction("InvoiceListTab", new { customerid = szNextId });
                }
                if (opcion == "Statement")
                {
                    return RedirectToAction("PaymentListTab", new { customerid = szNextId });
                }
            }

            return RedirectToAction("Edit", new { id = szNextId });
        }

        private string GetNextCustomer(string id)
        {
            bool bStatus = false;
            int nHas = -1;
            string szNext = string.Empty;
            string szError = string.Empty;
            string szSql = string.Empty;
            string szConnString = string.Empty;
            SqlDataSource sqlds = new SqlDataSource();
            DataView dv = null;
            ConnectionStringSettingsCollection connSettings = ConfigurationManager.ConnectionStrings;

            try
            {
                //szNext = id;
                szConnString = connSettings["TimelyDepotContext"].ToString();
                sqlds.ConnectionString = szConnString;

                szSql = string.Format("SELECT TOP (100) PERCENT Id, CustomerNo FROM Customers " +
                    "WHERE (CustomerNo > N'{0}') ORDER BY CustomerNo", id);
                sqlds.SelectCommand = szSql;
                dv = (DataView)sqlds.Select(DataSourceSelectArguments.Empty);
                nHas = dv.Count;
                if (nHas > 0)
                {
                    szNext = dv[0]["Id"].ToString();
                    bStatus = true;
                }
            }
            catch (Exception exc)
            {
                szNext = string.Empty;
                szError = exc.Message;
            }

            return szNext;
        }


        //
        // /Customer/GetCustomerMainData
        public static void GetCustomerMainData(TimelyDepotContext db01, int customerid, ref string CompanyName, ref string BussinesType, ref string State, ref string Phone, ref string Email, ref string ZipCode)
        {
            int nLen = 0;
            int nBussinesType = 0;
            long telHlp = 0;
            long faxHlp = 0;
            string telfmt = "000-000-0000";
            string telfmtOver = "000-0000000000";

            CompanyName = string.Empty;
            BussinesType = string.Empty;
            State = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            ZipCode = string.Empty;

            Bussines bussines = null;
            CustomersContactAddress maincontact = null;

            Customers customer = db01.Customers.Find(customerid);
            if (customer != null)
            {
                maincontact = db01.CustomersContactAddresses.Where(ctmn => ctmn.CustomerId == customer.Id).FirstOrDefault<CustomersContactAddress>();
                if (maincontact != null)
                {
                    CompanyName = maincontact.CompanyName;
                    State = maincontact.State;
                    ZipCode = maincontact.Zip;
                    Email = maincontact.Email;

                    telHlp = Convert.ToInt64(maincontact.Tel);
                    faxHlp = Convert.ToInt64(maincontact.Fax);

                    if (!string.IsNullOrEmpty(customer.BussinesType))
                    {
                        Phone = telHlp.ToString(telfmt);
                        if (customer.BussinesType.ToUpper() == "OVERSEA" || customer.BussinesType.ToUpper() == "OVERSEAS")
                        {
                            nLen = maincontact.Tel.Length;
                            switch (nLen)
                            {
                                case 8:
                                    telfmtOver = "000-00000";
                                    Phone = telHlp.ToString(telfmtOver);
                                    break;
                                case 9:
                                    telfmtOver = "000-000000";
                                    Phone = telHlp.ToString(telfmtOver);
                                    break;
                                case 10:
                                    telfmtOver = "000-0000000";
                                    Phone = telHlp.ToString(telfmtOver);
                                    break;
                                case 11:
                                    telfmtOver = "000-00000000";
                                    Phone = telHlp.ToString(telfmtOver);
                                    break;
                                case 12:
                                    telfmtOver = "000-000000000";
                                    Phone = telHlp.ToString(telfmtOver);
                                    break;
                                case 13:
                                    telfmtOver = "000-0000000000";
                                    Phone = telHlp.ToString(telfmtOver);
                                    break;
                                default:
                                    Phone = telHlp.ToString(telfmtOver);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Phone = telHlp.ToString(telfmt);
                    }
                }

                //nBussinesType = Convert.ToInt32(customer.BussinesType);
                //bussines = db01.Bussines.Where(bsn => bsn.Id == nBussinesType).FirstOrDefault<Bussines>();
                bussines = db01.Bussines.Where(bsn => bsn.BussinesType == customer.BussinesType).FirstOrDefault<Bussines>();
                if (bussines != null)
                {
                    BussinesType = bussines.BussinesType;
                }
            }
        }

        //
        // GET: /Customers/InvoiceListExcel
        public ActionResult OutstandingInvoiceExcel(string customerid)
        {
            //DataTable hlpTbl = GetCustomerListTable();

            ExportCSV(GetOutstandingInvoiceTable(customerid), "OutstandingInvoiceList");

            return RedirectToAction("Index", "ReportsExcel");
        }

        private DataTable GetOutstandingInvoiceTable(string customerid)
        {
            bool bPaidinFull = false;
            int nInvoiceIdHlp = 0;
            double dAmount = 0;
            double dSalesAmount = 0;
            double dTotalTax = 0;
            double dTax = 0;
            double dTotalAmount = 0;
            double dBalanceDue = 0;
            long telHlp = 0;
            long faxHlp = 0;
            int nCustomerId = 0;
            string telfmt = "000-000-0000";
            string szTel = string.Empty;
            string szMsg = string.Empty;

            TimelyDepotContext db01 = new TimelyDepotContext();

            DataTable table = null;
            DataRow row = null;

            PurchaseOrdersbyVendor purchaseorderbyvendor = null;
            List<PurchaseOrdersbyVendor> purchaseorderList = new List<PurchaseOrdersbyVendor>();

            //qryPOs = db.PurchaseOrders.Where(vd => vd.VendorId == id ).OrderByDescending(vd => vd.PurchaseOrderNo);
            if (!string.IsNullOrEmpty(customerid))
            {
                nCustomerId = Convert.ToInt32(customerid);
            }

            var qryPOs = db.Invoices.Where(invc => invc.CustomerId == nCustomerId).OrderByDescending(invc => invc.InvoiceId);
            if (qryPOs.Count() > 0)
            {
                foreach (var item in qryPOs)
                {
                    //Display here all the invoice for this customer
                    if (item.InvoiceId != nInvoiceIdHlp)
                    {
                        //Get the totals
                        GetInvoiceTotals01(db01, item.InvoiceId, ref dSalesAmount, ref dTotalTax, ref dTax, ref dTotalAmount, ref dBalanceDue);
                        nInvoiceIdHlp = item.InvoiceId;
                        if (item.PaymentAmount == null)
                        {
                            dAmount = 0;
                        }
                        else
                        {
                            dAmount = Convert.ToDouble(item.PaymentAmount);
                        }
                        if (dBalanceDue == 0)
                        {
                            bPaidinFull = true;
                        }
                        else
                        {
                            bPaidinFull = false;
                        }
                    }

                    if (bPaidinFull == false)
                    {
                        purchaseorderbyvendor = new PurchaseOrdersbyVendor();
                        purchaseorderbyvendor.PurchaseOrderId = item.InvoiceId;
                        purchaseorderbyvendor.PurchaseOrderNo = item.InvoiceNo;
                        purchaseorderbyvendor.SODate = item.InvoiceDate;
                        purchaseorderbyvendor.VendorNo = dTotalAmount.ToString();
                        purchaseorderbyvendor.Sub_ItemID = dAmount.ToString(); ;
                        purchaseorderbyvendor.Description = dBalanceDue.ToString();
                        //purchaseorderbyvendor.Quantity = item.podet.Quantity;
                        //purchaseorderbyvendor.UnitPrice = item.podet.UnitPrice;

                        purchaseorderList.Add(purchaseorderbyvendor);

                    }

                }
            }

            table = new DataTable("OpenPurchaseOrder");

            // Set the header
            DataColumn col01 = new DataColumn("InvoiceNo", System.Type.GetType("System.String"));
            DataColumn col02 = new DataColumn("Date", System.Type.GetType("System.String"));
            //DataColumn col03 = new DataColumn("CustomerNo", System.Type.GetType("System.String"));
            DataColumn col04 = new DataColumn("Invoice Amount", System.Type.GetType("System.String"));
            DataColumn col05 = new DataColumn("Paid Amount", System.Type.GetType("System.String"));
            DataColumn col06 = new DataColumn("Outstanding Amount", System.Type.GetType("System.String"));
            //DataColumn col07 = new DataColumn("UnitPrice", System.Type.GetType("System.String"));
            table.Columns.Add(col01);
            table.Columns.Add(col02);
            //table.Columns.Add(col03);
            table.Columns.Add(col04);
            table.Columns.Add(col05);
            table.Columns.Add(col06);
            //table.Columns.Add(col07);

            //Set the data row
            foreach (var item in purchaseorderList)
            {
                row = table.NewRow();
                row["InvoiceNo"] = item.PurchaseOrderNo;
                row["Date"] = item.SODate;
                //row["CustomerNo"] = item.VendorNo;
                row["Invoice Amount"] = item.VendorNo;
                row["Paid Amount"] = item.Sub_ItemID;
                row["Outstanding Amount"] = item.Description;
                //row["UnitPrice"] = item.UnitPrice;
                table.Rows.Add(row);
            }

            return table;
        }


        // Outstanding invoice: not paid in full
        // GET:/Customers/Outstandinginvoice
        [NoCache]
        public PartialViewResult Outstandinginvoice(int? page, string customerid)
        {
            bool bPaidinFull = false;
            int pageIndex = 0;
            int pageSize = PageSize;
            int nCustomerId = 0;
            int nInvoiceIdHlp = 0;
            double dAmount = 0;
            double dSalesAmount = 0;
            double dTotalTax = 0;
            double dTax = 0;
            double dTotalAmount = 0;
            double dBalanceDue = 0;
            string szMsg = string.Empty;

            PurchaseOrdersbyVendor purchaseorderbyvendor = null;
            List<PurchaseOrdersbyVendor> purchaseorderList = new List<PurchaseOrdersbyVendor>();

            TimelyDepotContext db01 = new TimelyDepotContext();

            //qryPOs = db.PurchaseOrders.Where(vd => vd.VendorId == id ).OrderByDescending(vd => vd.PurchaseOrderNo);

            if (!string.IsNullOrEmpty(customerid))
            {
                nCustomerId = Convert.ToInt32(customerid);
            }

            //var qryPOs = db.InvoiceDetails.Join(db.Invoices, podet => podet.InvoiceId, po => po.InvoiceId, (podet, po)
            //    => new { podet, po }).Where(prod => prod.po.CustomerId == nCustomerId).OrderByDescending(prod => prod.po.InvoiceId).ThenBy(prod => prod.podet.ItemPosition).ThenBy(prod => prod.podet.ItemOrder);
            var qryPOs = db.Invoices.Where(invc => invc.CustomerId == nCustomerId).OrderByDescending(invc => invc.InvoiceId);
            if (qryPOs.Count() > 0)
            {
                foreach (var item in qryPOs)
                {

                    //Display here all the invoice for this customer
                    if (item.InvoiceId != nInvoiceIdHlp)
                    {
                        //Get the totals
                        GetInvoiceTotals01(db01, item.InvoiceId, ref dSalesAmount, ref dTotalTax, ref dTax, ref dTotalAmount, ref dBalanceDue);
                        nInvoiceIdHlp = item.InvoiceId;
                        if (item.PaymentAmount == null)
                        {
                            dAmount = 0;
                        }
                        else
                        {
                            dAmount = Convert.ToDouble(item.PaymentAmount);
                        }
                        if (dBalanceDue == 0)
                        {
                            bPaidinFull = true;
                        }
                        else
                        {
                            bPaidinFull = false;
                        }
                    }

                    if (bPaidinFull == false)
                    {
                        purchaseorderbyvendor = new PurchaseOrdersbyVendor();
                        purchaseorderbyvendor.PurchaseOrderId = item.InvoiceId;
                        purchaseorderbyvendor.PurchaseOrderNo = item.InvoiceNo;
                        purchaseorderbyvendor.SODate = item.InvoiceDate;
                        purchaseorderbyvendor.VendorNo = dTotalAmount.ToString();
                        purchaseorderbyvendor.Sub_ItemID = dAmount.ToString(); ;
                        purchaseorderbyvendor.Description = dBalanceDue.ToString();
                        //purchaseorderbyvendor.Quantity = item.podet.Quantity;
                        //purchaseorderbyvendor.UnitPrice = item.podet.UnitPrice;

                        purchaseorderList.Add(purchaseorderbyvendor);

                    }
                }
            }
            ViewBag.CustomerId = customerid;


            //Set the page
            if (page == null)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = Convert.ToInt32(page);
            }


            var onePageOfData = purchaseorderList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return PartialView(purchaseorderList.ToPagedList(pageIndex, pageSize));
        }


        //
        // GET: /Customers/InvoiceListExcel
        public ActionResult PaymentListExcel(string customerid)
        {
            //DataTable hlpTbl = GetCustomerListTable();

            ExportCSV(GetPaymentListTable(customerid), "PaymentTransactionList");

            return RedirectToAction("Index", "ReportsExcel");
        }

        private DataTable GetPaymentListTable(string customerid)
        {
            bool bPaidinFull = false;
            int nInvoiceIdHlp = 0;
            double dAmount = 0;
            double dSalesAmount = 0;
            double dTotalTax = 0;
            double dTax = 0;
            double dTotalAmount = 0;
            double dBalanceDue = 0;
            long telHlp = 0;
            long faxHlp = 0;
            int nCustomerId = 0;
            string telfmt = "000-000-0000";
            string szTel = string.Empty;
            string szMsg = string.Empty;
            string szCustomerNo = string.Empty;
            string szInvoiceNo = string.Empty;

            int nHas = 0;
            int nPos = -1;
            string szClass = string.Empty;
            string szError = string.Empty;
            string szDecriptedData = string.Empty;
            string szDecriptedCode = string.Empty;

            TimelyDepotContext db01 = new TimelyDepotContext();

            DataTable table = null;
            DataRow row = null;

            Invoice invoice = null;
            Customers customer = null;
            PurchaseOrdersbyVendor purchaseorderbyvendor = null;
            List<PurchaseOrdersbyVendor> purchaseorderList = new List<PurchaseOrdersbyVendor>();

            //qryPOs = db.PurchaseOrders.Where(vd => vd.VendorId == id ).OrderByDescending(vd => vd.PurchaseOrderNo);
            if (!string.IsNullOrEmpty(customerid))
            {
                nCustomerId = Convert.ToInt32(customerid);
            }

            if (!string.IsNullOrEmpty(customerid))
            {
                nCustomerId = Convert.ToInt32(customerid);
                customer = db.Customers.Find(nCustomerId);
                if (customer != null)
                {
                    szCustomerNo = customer.CustomerNo;
                }
            }

            var qryPOs = db.Payments.Where(pymt => pymt.CustomerNo == szCustomerNo).OrderByDescending(pymt => pymt.PaymentDate);
            if (qryPOs.Count() > 0)
            {
                foreach (var item in qryPOs)
                {
                    szError = string.Empty;
                    szDecriptedData = TimelyDepotMVC.Controllers.PaymentController.DecodeInfo02(item.PaymentType, ref szError);
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


                    if (!string.IsNullOrEmpty(item.SalesOrderNo))
                    {
                        invoice = db.Invoices.Where(invc => invc.SalesOrderNo == item.SalesOrderNo).FirstOrDefault<Invoice>();
                        if (invoice != null)
                        {
                            szInvoiceNo = invoice.InvoiceNo;
                        }
                    }
                    else
                    {
                        szInvoiceNo = string.Empty;
                    }

                    purchaseorderbyvendor = new PurchaseOrdersbyVendor();
                    purchaseorderbyvendor.PurchaseOrderId = item.Id;
                    purchaseorderbyvendor.PurchaseOrderNo = item.PaymentNo;
                    purchaseorderbyvendor.VendorNo = szInvoiceNo;
                    purchaseorderbyvendor.SODate = item.PaymentDate;
                    purchaseorderbyvendor.Description = item.PaymentType;
                    purchaseorderbyvendor.UnitPrice = item.Amount;

                    purchaseorderList.Add(purchaseorderbyvendor);

                }
            }

            table = new DataTable("OpenPurchaseOrder");

            // Set the header
            DataColumn col01 = new DataColumn("PaymentNo", System.Type.GetType("System.String"));
            DataColumn col02 = new DataColumn("Date", System.Type.GetType("System.String"));
            //DataColumn col03 = new DataColumn("CustomerNo", System.Type.GetType("System.String"));
            DataColumn col04 = new DataColumn("InvoiceNo", System.Type.GetType("System.String"));
            DataColumn col05 = new DataColumn("Description", System.Type.GetType("System.String"));
            DataColumn col07 = new DataColumn("Amount", System.Type.GetType("System.String"));
            table.Columns.Add(col01);
            table.Columns.Add(col02);
            //table.Columns.Add(col03);
            table.Columns.Add(col04);
            table.Columns.Add(col05);
            //table.Columns.Add(col06);
            table.Columns.Add(col07);

            //Set the data row
            foreach (var item in purchaseorderList)
            {
                row = table.NewRow();
                row["PaymentNo"] = item.PurchaseOrderNo;
                row["Date"] = item.SODate;
                //row["CustomerNo"] = item.VendorNo;
                row["InvoiceNo"] = item.VendorNo;
                row["Description"] = item.Description;
                //row["Quantity"] = item.Quantity;
                row["Amount"] = item.UnitPrice;
                table.Rows.Add(row);
            }

            return table;
        }


        // 
        // GET:/Customers/PaymentTransactionList
        [NoCache]
        public PartialViewResult PaymentList(int? page, string customerid)
        {
            bool bPaidinFull = false;
            int pageIndex = 0;
            int pageSize = PageSize;
            int nCustomerId = 0;
            int nInvoiceIdHlp = 0;
            double dAmount = 0;
            double dSalesAmount = 0;
            double dTotalTax = 0;
            double dTax = 0;
            double dTotalAmount = 0;
            double dBalanceDue = 0;
            string szMsg = string.Empty;
            string szCustomerNo = string.Empty;
            string szInvoiceNo = string.Empty;

            Invoice invoice = null;
            Customers customer = null;
            PurchaseOrdersbyVendor purchaseorderbyvendor = null;
            List<PurchaseOrdersbyVendor> purchaseorderList = new List<PurchaseOrdersbyVendor>();

            TimelyDepotContext db01 = new TimelyDepotContext();

            //qryPOs = db.PurchaseOrders.Where(vd => vd.VendorId == id ).OrderByDescending(vd => vd.PurchaseOrderNo);

            if (!string.IsNullOrEmpty(customerid))
            {
                nCustomerId = Convert.ToInt32(customerid);
                customer = db.Customers.Find(nCustomerId);
                if (customer != null)
                {
                    szCustomerNo = customer.CustomerNo;
                }
            }

            //var qryPOs = db.InvoiceDetails.Join(db.Invoices, podet => podet.InvoiceId, po => po.InvoiceId, (podet, po)
            //    => new { podet, po }).Where(prod => prod.po.CustomerId == nCustomerId).OrderByDescending(prod => prod.po.InvoiceId).ThenBy(prod => prod.podet.ItemPosition).ThenBy(prod => prod.podet.ItemOrder);
            var qryPOs = db.Payments.Where(pymt => pymt.CustomerNo == szCustomerNo).OrderByDescending(pymt => pymt.PaymentDate);
            if (qryPOs.Count() > 0)
            {
                foreach (var item in qryPOs)
                {
                    if (!string.IsNullOrEmpty(item.SalesOrderNo))
                    {
                        invoice = db01.Invoices.Where(invc => invc.SalesOrderNo == item.SalesOrderNo).FirstOrDefault<Invoice>();
                        if (invoice != null)
                        {
                            szInvoiceNo = invoice.InvoiceNo;
                        }
                    }
                    else
                    {
                        szInvoiceNo = string.Empty;
                    }

                    purchaseorderbyvendor = new PurchaseOrdersbyVendor();
                    purchaseorderbyvendor.PurchaseOrderId = item.Id;
                    purchaseorderbyvendor.PurchaseOrderNo = item.PaymentNo;
                    purchaseorderbyvendor.VendorNo = szInvoiceNo;
                    purchaseorderbyvendor.SODate = item.PaymentDate;
                    purchaseorderbyvendor.Description = item.PaymentType;
                    purchaseorderbyvendor.UnitPrice = item.Amount;

                    purchaseorderList.Add(purchaseorderbyvendor);

                }
            }
            ViewBag.CustomerId = customerid;


            //Set the page
            if (page == null)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = Convert.ToInt32(page);
            }


            var onePageOfData = purchaseorderList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return PartialView(purchaseorderList.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /Customers/InvoiceListExcel
        public ActionResult InvoiceListExcel(string customerid)
        {
            //DataTable hlpTbl = GetCustomerListTable();

            ExportCSV(GetInvoiceListTable(customerid), "InvoiceList");

            return RedirectToAction("Index", "ReportsExcel");
        }

        private DataTable GetInvoiceListTable(string customerid)
        {
            bool bPaidinFull = false;
            int nInvoiceIdHlp = 0;
            double dAmount = 0;
            double dSalesAmount = 0;
            double dTotalTax = 0;
            double dTax = 0;
            double dTotalAmount = 0;
            double dBalanceDue = 0;
            long telHlp = 0;
            long faxHlp = 0;
            int nCustomerId = 0;
            string telfmt = "000-000-0000";
            string szTel = string.Empty;
            string szMsg = string.Empty;

            TimelyDepotContext db01 = new TimelyDepotContext();

            DataTable table = null;
            DataRow row = null;

            PurchaseOrdersbyVendor purchaseorderbyvendor = null;
            List<PurchaseOrdersbyVendor> purchaseorderList = new List<PurchaseOrdersbyVendor>();

            //qryPOs = db.PurchaseOrders.Where(vd => vd.VendorId == id ).OrderByDescending(vd => vd.PurchaseOrderNo);
            if (!string.IsNullOrEmpty(customerid))
            {
                nCustomerId = Convert.ToInt32(customerid);
            }

            var qryPOs = db.InvoiceDetails.Join(db.Invoices, podet => podet.InvoiceId, po => po.InvoiceId, (podet, po)
                => new { podet, po }).Where(prod => prod.po.CustomerId == nCustomerId).OrderByDescending(prod => prod.po.InvoiceId).ThenBy(prod => prod.podet.ItemPosition).ThenBy(prod => prod.podet.ItemOrder);
            if (qryPOs.Count() > 0)
            {
                foreach (var item in qryPOs)
                {
                    szMsg = item.po.PurchaseOrderNo;

                    if (item.po.InvoiceId != nInvoiceIdHlp)
                    {
                        //Get the totals
                        GetInvoiceTotals01(db01, item.po.InvoiceId, ref dSalesAmount, ref dTotalTax, ref dTax, ref dTotalAmount, ref dBalanceDue);
                        dAmount = dTotalAmount;
                        nInvoiceIdHlp = item.po.InvoiceId;
                        if (dBalanceDue == 0)
                        {
                            bPaidinFull = true;
                        }
                        else
                        {
                            bPaidinFull = false;
                        }
                    }

                    if (bPaidinFull == false)
                    {
                        purchaseorderbyvendor = new PurchaseOrdersbyVendor();
                        purchaseorderbyvendor.PurchaseOrderId = item.po.InvoiceId;
                        purchaseorderbyvendor.PurchaseOrderNo = item.po.InvoiceNo;
                        purchaseorderbyvendor.SODate = item.po.InvoiceDate;
                        //purchaseorderbyvendor.VendorNo = GetCustomerNo(db01, item.po.CustomerId);
                        purchaseorderbyvendor.Sub_ItemID = item.podet.Sub_ItemID;
                        purchaseorderbyvendor.Description = item.podet.Description;
                        purchaseorderbyvendor.Quantity = item.podet.Quantity;
                        purchaseorderbyvendor.UnitPrice = item.podet.UnitPrice;

                        purchaseorderList.Add(purchaseorderbyvendor);

                    }
                }
            }

            table = new DataTable("OpenPurchaseOrder");

            // Set the header
            DataColumn col01 = new DataColumn("InvoiceNo", System.Type.GetType("System.String"));
            DataColumn col02 = new DataColumn("Date", System.Type.GetType("System.String"));
            //DataColumn col03 = new DataColumn("CustomerNo", System.Type.GetType("System.String"));
            DataColumn col04 = new DataColumn("ItemID", System.Type.GetType("System.String"));
            DataColumn col05 = new DataColumn("Description", System.Type.GetType("System.String"));
            DataColumn col06 = new DataColumn("Quantity", System.Type.GetType("System.String"));
            DataColumn col07 = new DataColumn("UnitPrice", System.Type.GetType("System.String"));
            table.Columns.Add(col01);
            table.Columns.Add(col02);
            //table.Columns.Add(col03);
            table.Columns.Add(col04);
            table.Columns.Add(col05);
            table.Columns.Add(col06);
            table.Columns.Add(col07);

            //Set the data row
            foreach (var item in purchaseorderList)
            {
                row = table.NewRow();
                row["InvoiceNo"] = item.PurchaseOrderNo;
                row["Date"] = item.SODate;
                //row["CustomerNo"] = item.VendorNo;
                row["ItemID"] = item.Sub_ItemID;
                row["Description"] = item.Description;
                row["Quantity"] = item.Quantity;
                row["UnitPrice"] = item.UnitPrice;
                table.Rows.Add(row);
            }

            return table;
        }

        // Outstanding invoice: not paid in full
        // Display here all the invoice for this customer
        // GET:/Customers/InvoiceList
        [NoCache]
        public PartialViewResult InvoiceList(int? page, string customerid)
        {
            bool bPaidinFull = false;
            int pageIndex = 0;
            int pageSize = PageSize;
            int nCustomerId = 0;
            int nInvoiceIdHlp = 0;
            double dAmount = 0;
            double dSalesAmount = 0;
            double dTotalTax = 0;
            double dTax = 0;
            double dTotalAmount = 0;
            double dBalanceDue = 0;
            string szMsg = string.Empty;

            PurchaseOrdersbyVendor purchaseorderbyvendor = null;
            List<PurchaseOrdersbyVendor> purchaseorderList = new List<PurchaseOrdersbyVendor>();

            TimelyDepotContext db01 = new TimelyDepotContext();

            //qryPOs = db.PurchaseOrders.Where(vd => vd.VendorId == id ).OrderByDescending(vd => vd.PurchaseOrderNo);

            if (!string.IsNullOrEmpty(customerid))
            {
                nCustomerId = Convert.ToInt32(customerid);
            }

            //var qryPOs = db.InvoiceDetails.Join(db.Invoices, podet => podet.InvoiceId, po => po.InvoiceId, (podet, po)
            //    => new { podet, po }).Where(prod => prod.po.CustomerId == nCustomerId && (prod.po.PaymentAmount == null || prod.po.PaymentAmount == 0)).OrderByDescending(prod => prod.po.InvoiceId).ThenBy(prod => prod.podet.ItemPosition).ThenBy(prod => prod.podet.ItemOrder);
            var qryPOs = db.InvoiceDetails.Join(db.Invoices, podet => podet.InvoiceId, po => po.InvoiceId, (podet, po)
                => new { podet, po }).Where(prod => prod.po.CustomerId == nCustomerId).OrderByDescending(prod => prod.po.InvoiceId).ThenBy(prod => prod.podet.ItemPosition).ThenBy(prod => prod.podet.ItemOrder);
            if (qryPOs.Count() > 0)
            {
                foreach (var item in qryPOs)
                {
                    szMsg = item.po.PurchaseOrderNo;

                    //Display here all the invoice for this customer
                    //if (item.po.InvoiceId != nInvoiceIdHlp)
                    //{
                    //    //Get the totals
                    //    GetInvoiceTotals01(db01, item.po.InvoiceId, ref dSalesAmount, ref dTotalTax, ref dTax, ref dTotalAmount, ref dBalanceDue);
                    //    dAmount = dTotalAmount;
                    //    nInvoiceIdHlp = item.po.InvoiceId;
                    //    if (dBalanceDue == 0)
                    //    {
                    //        bPaidinFull = true;
                    //    }
                    //    else
                    //    {
                    //        bPaidinFull = false;
                    //    }
                    //}

                    if (bPaidinFull == false)
                    {
                        purchaseorderbyvendor = new PurchaseOrdersbyVendor();
                        purchaseorderbyvendor.PurchaseOrderId = item.po.InvoiceId;
                        purchaseorderbyvendor.PurchaseOrderNo = item.po.InvoiceNo;
                        purchaseorderbyvendor.SODate = item.po.InvoiceDate;
                        //purchaseorderbyvendor.VendorNo = GetCustomerNo(db01, item.po.CustomerId);
                        purchaseorderbyvendor.Sub_ItemID = item.podet.Sub_ItemID;
                        purchaseorderbyvendor.Description = item.podet.Description;
                        purchaseorderbyvendor.Quantity = item.podet.Quantity;
                        purchaseorderbyvendor.UnitPrice = item.podet.UnitPrice;

                        purchaseorderList.Add(purchaseorderbyvendor);

                    }
                }
            }
            ViewBag.CustomerId = customerid;


            //Set the page
            if (page == null)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = Convert.ToInt32(page);
            }


            var onePageOfData = purchaseorderList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return PartialView(purchaseorderList.ToPagedList(pageIndex, pageSize));
        }

        public void GetInvoiceTotals01(TimelyDepotContext db01, int nInvoiceId, ref double dSalesAmount, ref double dTotalTax, ref double dTax, ref double dTotalAmount, ref double dBalanceDue)
        {
            double dShipping = 0;
            double dPayment = 0;
            double dSOTax = 0;

            double dAmount = 0;

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
                        //dSalesAmount = dSalesAmount + (Convert.ToDouble(item.Quantity) * Convert.ToDouble(item.UnitPrice));
                        //dTotalTax = dTotalTax + (Convert.ToDouble(item.Quantity) * Convert.ToDouble(item.UnitPrice) * (dTax / 100));

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
            }
        }

        //
        // GET: /Customers/OutsandingSalesOrderExcel
        public ActionResult OutsandingSalesOrderExcel(string customerid)
        {
            //DataTable hlpTbl = GetCustomerListTable();

            ExportCSV(GetOutstandingSalesOrderTable(customerid), "OutstandingSalesOrderList");

            return RedirectToAction("Index", "ReportsExcel");
        }

        private DataTable GetOutstandingSalesOrderTable(string customerid)
        {
            long telHlp = 0;
            long faxHlp = 0;
            int nCustomerId = 0;
            string telfmt = "000-000-0000";
            string szTel = string.Empty;
            string szMsg = string.Empty;

            TimelyDepotContext db01 = new TimelyDepotContext();

            DataTable table = null;
            DataRow row = null;

            PurchaseOrdersbyVendor purchaseorderbyvendor = null;
            List<PurchaseOrdersbyVendor> purchaseorderList = new List<PurchaseOrdersbyVendor>();

            //qryPOs = db.PurchaseOrders.Where(vd => vd.VendorId == id ).OrderByDescending(vd => vd.PurchaseOrderNo);
            if (!string.IsNullOrEmpty(customerid))
            {
                nCustomerId = Convert.ToInt32(customerid);
            }

            var qryPOs = db.SalesOrderDetails.Join(db.SalesOrders, podet => podet.SalesOrderId, po => po.SalesOrderId, (podet, po)
                => new { podet, po }).Where(prod => prod.po.CustomerId == nCustomerId && prod.podet.Quantity != prod.podet.ShipQuantity).OrderByDescending(prod => prod.po.SalesOrderId).ThenBy(prod => prod.podet.ItemPosition).ThenBy(prod => prod.podet.ItemOrder);
            if (qryPOs.Count() > 0)
            {
                foreach (var item in qryPOs)
                {
                    szMsg = item.po.PurchaseOrderNo;

                    purchaseorderbyvendor = new PurchaseOrdersbyVendor();
                    purchaseorderbyvendor.PurchaseOrderId = item.po.SalesOrderId;
                    purchaseorderbyvendor.PurchaseOrderNo = item.po.SalesOrderNo;
                    purchaseorderbyvendor.SODate = item.po.SODate;
                    //purchaseorderbyvendor.VendorNo = GetCustomerNo(db01, item.po.CustomerId);
                    purchaseorderbyvendor.Sub_ItemID = item.podet.Sub_ItemID;
                    purchaseorderbyvendor.Description = item.podet.Description;
                    purchaseorderbyvendor.Quantity = item.podet.Quantity;
                    purchaseorderbyvendor.UnitPrice = item.podet.UnitPrice;

                    purchaseorderList.Add(purchaseorderbyvendor);
                }
            }

            table = new DataTable("OpenPurchaseOrder");

            // Set the header
            DataColumn col01 = new DataColumn("SalesOrderNo", System.Type.GetType("System.String"));
            DataColumn col02 = new DataColumn("Date", System.Type.GetType("System.String"));
            //DataColumn col03 = new DataColumn("CustomerNo", System.Type.GetType("System.String"));
            DataColumn col04 = new DataColumn("ItemID", System.Type.GetType("System.String"));
            DataColumn col05 = new DataColumn("Description", System.Type.GetType("System.String"));
            DataColumn col06 = new DataColumn("Quantity", System.Type.GetType("System.String"));
            DataColumn col07 = new DataColumn("UnitPrice", System.Type.GetType("System.String"));
            table.Columns.Add(col01);
            table.Columns.Add(col02);
            //table.Columns.Add(col03);
            table.Columns.Add(col04);
            table.Columns.Add(col05);
            table.Columns.Add(col06);
            table.Columns.Add(col07);

            //Set the data row
            foreach (var item in purchaseorderList)
            {
                row = table.NewRow();
                row["SalesOrderNo"] = item.PurchaseOrderNo;
                row["Date"] = item.SODate;
                //row["CustomerNo"] = item.VendorNo;
                row["ItemID"] = item.Sub_ItemID;
                row["Description"] = item.Description;
                row["Quantity"] = item.Quantity;
                row["UnitPrice"] = item.UnitPrice;
                table.Rows.Add(row);
            }

            return table;
        }


        // Outstanding sales order= order not completely shipped
        // GET:/Customers/OpenPurchaseOrder
        [NoCache]
        public PartialViewResult OutsandingSalesOrder(int? page, string customerid)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            int nCustomerId = 0;
            string szMsg = string.Empty;

            PurchaseOrdersbyVendor purchaseorderbyvendor = null;
            List<PurchaseOrdersbyVendor> purchaseorderList = new List<PurchaseOrdersbyVendor>();

            TimelyDepotContext db01 = new TimelyDepotContext();

            //qryPOs = db.PurchaseOrders.Where(vd => vd.VendorId == id ).OrderByDescending(vd => vd.PurchaseOrderNo);

            if (!string.IsNullOrEmpty(customerid))
            {
                nCustomerId = Convert.ToInt32(customerid);
            }

            var qryPOs = db.SalesOrderDetails.Join(db.SalesOrders, podet => podet.SalesOrderId, po => po.SalesOrderId, (podet, po)
                => new { podet, po }).Where(prod => prod.po.CustomerId == nCustomerId && prod.podet.Quantity != prod.podet.ShipQuantity).OrderByDescending(prod => prod.po.SalesOrderId).ThenBy(prod => prod.podet.ItemPosition).ThenBy(prod => prod.podet.ItemOrder);
            if (qryPOs.Count() > 0)
            {
                foreach (var item in qryPOs)
                {
                    szMsg = item.po.PurchaseOrderNo;

                    purchaseorderbyvendor = new PurchaseOrdersbyVendor();
                    purchaseorderbyvendor.PurchaseOrderId = item.po.SalesOrderId;
                    purchaseorderbyvendor.PurchaseOrderNo = item.po.SalesOrderNo;
                    purchaseorderbyvendor.SODate = item.po.SODate;
                    //purchaseorderbyvendor.VendorNo = GetCustomerNo(db01, item.po.CustomerId);
                    purchaseorderbyvendor.Sub_ItemID = item.podet.Sub_ItemID;
                    purchaseorderbyvendor.Description = item.podet.Description;
                    purchaseorderbyvendor.Quantity = item.podet.Quantity;
                    purchaseorderbyvendor.UnitPrice = item.podet.UnitPrice;

                    purchaseorderList.Add(purchaseorderbyvendor);

                }
            }
            ViewBag.CustomerId = customerid;


            //Set the page
            if (page == null)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = Convert.ToInt32(page);
            }


            var onePageOfData = purchaseorderList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return PartialView(purchaseorderList.ToPagedList(pageIndex, pageSize));
        }


        //
        // GET: /Customer/CustomerListExcel
        public ActionResult CustomerListExcel()
        {
            //DataTable hlpTbl = GetCustomerListTable();

            ExportCSV(GetCustomerListTable(), "CustomerList");

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

        private DataTable GetCustomerListTable()
        {
            long telHlp = 0;
            long faxHlp = 0;
            string telfmt = "000-000-0000";
            string szTel = string.Empty;

            DataTable table = null;
            DataRow row = null;

            CustomerList thecustomerlist = null;
            List<CustomerList> customerList = new List<CustomerList>();

            var qryCustomers = db.CustomersContactAddresses.Join(db.Customers, ctad => ctad.CustomerId, cst => cst.Id, (ctad, cst)
                => new { ctad, cst }).OrderBy(cact => cact.cst.CustomerNo);
            if (qryCustomers.Count() > 0)
            {
                foreach (var item in qryCustomers)
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

                    thecustomerlist = new CustomerList();
                    thecustomerlist.Id = item.cst.Id;
                    thecustomerlist.CustomerNo = item.cst.CustomerNo;
                    thecustomerlist.CompanyName = item.ctad.CompanyName;
                    thecustomerlist.FirstName = item.ctad.FirstName;
                    thecustomerlist.LastName = item.ctad.LastName;
                    thecustomerlist.State = item.ctad.State;
                    thecustomerlist.Country = item.ctad.Country;
                    thecustomerlist.Tel = szTel;

                    customerList.Add(thecustomerlist);
                }
            }

            table = new DataTable("CustomerList");

            // Set the header
            DataColumn col01 = new DataColumn("CustomerNo", System.Type.GetType("System.String"));
            DataColumn col02 = new DataColumn("CompanyName", System.Type.GetType("System.String"));
            DataColumn col03 = new DataColumn("ContactName", System.Type.GetType("System.String"));
            DataColumn col04 = new DataColumn("Tel", System.Type.GetType("System.String"));
            DataColumn col05 = new DataColumn("State", System.Type.GetType("System.String"));
            DataColumn col06 = new DataColumn("Country", System.Type.GetType("System.String"));
            table.Columns.Add(col01);
            table.Columns.Add(col02);
            table.Columns.Add(col03);
            table.Columns.Add(col04);
            table.Columns.Add(col05);
            table.Columns.Add(col06);

            //Set the data row
            foreach (var item in customerList)
            {
                row = table.NewRow();
                row["CustomerNo"] = item.CustomerNo;
                row["CompanyName"] = item.CompanyName;
                row["ContactName"] = string.Format("{0} {1}", item.FirstName, item.LastName); ;
                row["Tel"] = item.Tel;
                row["State"] = item.State;
                row["Country"] = item.Country;
                table.Rows.Add(row);
            }

            return table;
        }

        //
        // GET: /Customer/CustomerList
        [NoCache]
        public PartialViewResult CustomerList(int? page)
        {

            int pageIndex = 0;
            int pageSize = PageSize;

            CustomerList thecustomerlist = null;
            List<CustomerList> customerList = new List<CustomerList>();

            var qryCustomers = db.CustomersContactAddresses.Join(db.Customers, ctad => ctad.CustomerId, cst => cst.Id, (ctad, cst)
                => new { ctad, cst }).OrderBy(cact => cact.cst.CustomerNo);
            if (qryCustomers.Count() > 0)
            {
                foreach (var item in qryCustomers)
                {
                    thecustomerlist = new CustomerList();
                    thecustomerlist.Id = item.cst.Id;
                    thecustomerlist.CustomerNo = item.cst.CustomerNo;
                    thecustomerlist.CompanyName = item.ctad.CompanyName;
                    thecustomerlist.FirstName = item.ctad.FirstName;
                    thecustomerlist.LastName = item.ctad.LastName;
                    thecustomerlist.State = item.ctad.State;
                    thecustomerlist.Country = item.ctad.Country;
                    thecustomerlist.Tel = item.ctad.Tel;

                    customerList.Add(thecustomerlist);
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


            var onePageOfData = customerList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return PartialView(customerList.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /Customer/DeleteNote
        public ActionResult DeleteCreditCard(int id, int customerdefaultid = 0)
        {
            int nCustomerId = 0;
            CustomerDefaults customerDefaults = db.CustomerDefaults.Find(customerdefaultid);
            if (customerDefaults != null)
            {
                if (customerDefaults.NoteId == id)
                {
                    customerDefaults.NoteId = null;
                    customerDefaults.NoteName = null;
                }
            }

            CustomersCreditCardShipping customersspecialnotes = db.CustomersCreditCardShippings.Find(id);
            nCustomerId = Convert.ToInt32(customersspecialnotes.CustomerId);
            db.CustomersCreditCardShippings.Remove(customersspecialnotes);
            db.SaveChanges();
            return RedirectToAction("Edit", new { id = nCustomerId });
            //return RedirectToAction("Index");
        }
        // POST: /Customer/UpdateCustomerNote
        [HttpPost]
        public ActionResult CreateEditCreditCardFromPayment(CustomersCreditCardShipping customenote, string customerdefault, string ExpirationDateHlp, string CreditNumber01, string SecureCode01, string PaymentUrl = "")
        {
            int nCustomerDefault = Convert.ToInt32(customerdefault);
            int nPos = -1;
            string szError = string.Empty;
            string szEncriptedData = string.Empty;
            DateTime dDate = DateTime.Now;
            CustomerDefaults custdefault = null;

            if (customenote != null)
            {
                if (ModelState.IsValid)
                {
                    if (customenote.Id == 0)
                    {
                        if (string.IsNullOrEmpty(customenote.Tel))
                        {
                            customenote.Tel = "0";
                        }
                        customenote.Tel = customenote.Tel.Replace("-", string.Empty);
                        db.CustomersCreditCardShippings.Add(customenote);

                        //Set Card number
                        //Encript the credit card info
                        //use the user supplied data
                        if (!string.IsNullOrEmpty(CreditNumber01))
                        {
                            nPos = CreditNumber01.IndexOf("*");
                            if (nPos == -1)
                            {
                                customenote.CreditNumber = CreditNumber01;

                                //Encode the credit card info
                                if (!string.IsNullOrEmpty(customenote.CreditNumber))
                                {
                                    szEncriptedData = PaymentController.EncriptInfo02(customenote.CreditNumber, ref szError);
                                    customenote.CreditNumber = szEncriptedData;
                                }
                            }
                            else
                            {
                                //Do not replace the credit card number
                            }
                        }

                        //Set Secure Code
                        if (!string.IsNullOrEmpty(customenote.SecureCode))
                        {
                            //Encode the credit card info
                            if (!string.IsNullOrEmpty(customenote.SecureCode))
                            {
                                szEncriptedData = PaymentController.EncriptInfo02(customenote.SecureCode, ref szError);
                                customenote.SecureCode = szEncriptedData;
                            }

                        }
                        db.SaveChanges();

                        //Set the default customer value
                        custdefault = db.CustomerDefaults.Find(nCustomerDefault);
                        if (custdefault != null)
                        {
                            custdefault.NoteId = customenote.Id;
                            custdefault.NoteName = string.Format("{0}", customenote.CreditNumber);
                            db.Entry(custdefault).State = EntityState.Modified;
                        }
                        db.SaveChanges();

                    }
                    else
                    {
                        //Set the default customer value
                        custdefault = db.CustomerDefaults.Find(nCustomerDefault);
                        if (custdefault != null)
                        {
                            custdefault.NoteId = customenote.Id;
                            custdefault.NoteName = string.Format("{0}", customenote.CreditNumber);
                            db.Entry(custdefault).State = EntityState.Modified;
                        }

                        if (!string.IsNullOrEmpty(ExpirationDateHlp))
                        {
                            dDate = Convert.ToDateTime(ExpirationDateHlp);
                            customenote.ExpirationDate = dDate;
                        }

                        if (string.IsNullOrEmpty(customenote.Tel))
                        {
                            customenote.Tel = "0";
                        }
                        customenote.Tel = customenote.Tel.Replace("-", string.Empty);

                        //Encript the credit card info
                        //use the user supplied data
                        if (!string.IsNullOrEmpty(CreditNumber01))
                        {
                            nPos = CreditNumber01.IndexOf("*");
                            if (nPos == -1)
                            {
                                customenote.CreditNumber = CreditNumber01;

                                //Encode the credit card info
                                if (!string.IsNullOrEmpty(customenote.CreditNumber))
                                {
                                    szEncriptedData = TimelyDepotMVC.Controllers.PaymentController.EncriptInfo02(customenote.CreditNumber, ref szError);
                                    customenote.CreditNumber = szEncriptedData;
                                }
                            }

                        }

                        //Encode the credit card info
                        if (!string.IsNullOrEmpty(customenote.SecureCode))
                        {
                            szEncriptedData = PaymentController.EncriptInfo02(customenote.SecureCode, ref szError);
                            customenote.SecureCode = szEncriptedData;
                        }



                        db.Entry(customenote).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                   
                    
                }
            }


            return RedirectToAction("AddCreditCardPayment", "Payment", new { salesOrderNumber = 12 });

        }
        [HttpPost]
        public ActionResult UpdateCreditCard(CustomersCreditCardShipping customenote, string customerdefault, string ExpirationDateHlp, string CreditNumber01, string SecureCode01)
        {
            int nCustomerDefault = Convert.ToInt32(customerdefault);
            int nPos = -1;
            string szError = string.Empty;
            string szEncriptedData = string.Empty;
            DateTime dDate = DateTime.Now;
            CustomerDefaults custdefault = null;

            if (customenote != null)
            {
                if (ModelState.IsValid)
                {
                    if (customenote.Id == 0)
                    {
                        if (string.IsNullOrEmpty(customenote.Tel))
                        {
                            customenote.Tel = "0";
                        }
                        customenote.Tel = customenote.Tel.Replace("-", string.Empty);
                        db.CustomersCreditCardShippings.Add(customenote);

                        //Set Card number
                        //Encript the credit card info
                        //use the user supplied data
                        if (!string.IsNullOrEmpty(CreditNumber01))
                        {
                            nPos = CreditNumber01.IndexOf("*");
                            if (nPos == -1)
                            {
                                customenote.CreditNumber = CreditNumber01;

                                //Encode the credit card info
                                if (!string.IsNullOrEmpty(customenote.CreditNumber))
                                {
                                    szEncriptedData = PaymentController.EncriptInfo02(customenote.CreditNumber, ref szError);
                                    customenote.CreditNumber = szEncriptedData;
                                }
                            }
                            else
                            {
                                //Do not replace the credit card number
                            }
                        }

                        //Set Secure Code
                        if (!string.IsNullOrEmpty(customenote.SecureCode))
                        {
                                //Encode the credit card info
                                if (!string.IsNullOrEmpty(customenote.SecureCode))
                                {
                                    szEncriptedData = PaymentController.EncriptInfo02(customenote.SecureCode, ref szError);
                                    customenote.SecureCode = szEncriptedData;
                                }
                         
                        }
                        db.SaveChanges();

                        //Set the default customer value
                        custdefault = db.CustomerDefaults.Find(nCustomerDefault);
                        if (custdefault != null)
                        {
                            custdefault.NoteId = customenote.Id;
                            custdefault.NoteName = string.Format("{0}", customenote.CreditNumber);
                            db.Entry(custdefault).State = EntityState.Modified;
                        }
                        db.SaveChanges();

                    }
                    else
                    {
                        //Set the default customer value
                        custdefault = db.CustomerDefaults.Find(nCustomerDefault);
                        if (custdefault != null)
                        {
                            custdefault.NoteId = customenote.Id;
                            custdefault.NoteName = string.Format("{0}", customenote.CreditNumber);
                            db.Entry(custdefault).State = EntityState.Modified;
                        }

                        if (!string.IsNullOrEmpty(ExpirationDateHlp))
                        {
                            dDate = Convert.ToDateTime(ExpirationDateHlp);
                            customenote.ExpirationDate = dDate;
                        }

                        if (string.IsNullOrEmpty(customenote.Tel))
                        {
                            customenote.Tel = "0";
                        }
                        customenote.Tel = customenote.Tel.Replace("-", string.Empty);

                        //Encript the credit card info
                        //use the user supplied data
                        if (!string.IsNullOrEmpty(CreditNumber01))
                        {
                            nPos = CreditNumber01.IndexOf("*");
                            if (nPos == -1)
                            {
                                customenote.CreditNumber = CreditNumber01;

                                //Encode the credit card info
                                if (!string.IsNullOrEmpty(customenote.CreditNumber))
                                {
                                    szEncriptedData = TimelyDepotMVC.Controllers.PaymentController.EncriptInfo02(customenote.CreditNumber, ref szError);
                                    customenote.CreditNumber = szEncriptedData;
                                }
                            }
                           
                        }

                                //Encode the credit card info
                                if (!string.IsNullOrEmpty(customenote.SecureCode))
                                {
                                    szEncriptedData = PaymentController.EncriptInfo02(customenote.SecureCode, ref szError);
                                    customenote.SecureCode = szEncriptedData;
                                }
                       
                        

                        db.Entry(customenote).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    TempData["ActiveTab"] = "4";
                    return RedirectToAction("Edit", new { id = Convert.ToInt32(customenote.CustomerId) });
                }
            }

            return RedirectToAction("Index");
        }
        [NoCache]
        public PartialViewResult CreateEditCreditCardFromPayment(string customerid, int id = 0)
        {
            int nCustomerId = Convert.ToInt32(customerid);
            CustomerDefaults customerDefault = null;
            CustomersCreditCardShipping customercredit = null;
            IQueryable<CustomersCardType> qryCardType = null;

            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            if (id == 0)
            {
                customercredit = new CustomersCreditCardShipping();
                customercredit.CustomerId = nCustomerId;
                customercredit.ExpirationDate = DateTime.Now;
            }
            else
            {
                customercredit = db.CustomersCreditCardShippings.Find(id);
            }

            //Get the customer default id
            ViewBag.CustomerDefaultId = 0;
            customerDefault = db.CustomerDefaults.Where(ctdf => ctdf.CustomerId == nCustomerId).FirstOrDefault<CustomerDefaults>();
            if (customerDefault != null)
            {
                ViewBag.CustomerDefaultId = customerDefault.Id;
            }

            listSelector = new List<KeyValuePair<string, string>>();
            qryCardType = db.CustomersCardTypes.OrderBy(cucr => cucr.CardType);
            if (qryCardType.Count() > 0)
            {
                foreach (var item in qryCardType)
                {
                    listSelector.Add(new KeyValuePair<string, string>(item.CardType, item.CardType));
                }
            }
            SelectList cardtypelist = new SelectList(listSelector, "Key", "Value");
            ViewBag.CardTypeList = cardtypelist;

            return PartialView(customercredit);
        }
        //
        // GET: /Customer/CreateEditCreditCard
        [NoCache]
        public PartialViewResult CreateEditCreditCard(string customerid, int id = 0)
        {
            int nCustomerId = Convert.ToInt32(customerid);
            CustomerDefaults customerDefault = null;
            CustomersCreditCardShipping customercredit = null;
            IQueryable<CustomersCardType> qryCardType = null;

            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            if (id == 0)
            {
                customercredit = new CustomersCreditCardShipping();
                customercredit.CustomerId = nCustomerId;
                customercredit.ExpirationDate = DateTime.Now;
            }
            else
            {
                customercredit = db.CustomersCreditCardShippings.Find(id);
            }

            //Get the customer default id
            ViewBag.CustomerDefaultId = 0;
            customerDefault = db.CustomerDefaults.Where(ctdf => ctdf.CustomerId == nCustomerId).FirstOrDefault<CustomerDefaults>();
            if (customerDefault != null)
            {
                ViewBag.CustomerDefaultId = customerDefault.Id;
            }

            listSelector = new List<KeyValuePair<string, string>>();
            qryCardType = db.CustomersCardTypes.OrderBy(cucr => cucr.CardType);
            if (qryCardType.Count() > 0)
            {
                foreach (var item in qryCardType)
                {
                    listSelector.Add(new KeyValuePair<string, string>(item.CardType, item.CardType));
                }
            }
            SelectList cardtypelist = new SelectList(listSelector, "Key", "Value");
            ViewBag.CardTypeList = cardtypelist;

            return PartialView(customercredit);
        }

        //
        // Get customer email and name
        public static string GetCustomerEmailName(TimelyDepotContext db01, int CustomerId, ref string szName)
        {
            string szEmail = string.Empty;
            szName = string.Empty;
            IQueryable<CustomersContactAddress> qryAddress = null;
            CustomersContactAddress customeraddress = null;

            qryAddress = db01.CustomersContactAddresses.Where(ctad => ctad.CustomerId == CustomerId);
            if (qryAddress.Count() > 0)
            {
                customeraddress = qryAddress.FirstOrDefault<CustomersContactAddress>();
                if (customeraddress != null)
                {
                    szEmail = customeraddress.Email;
                    szName = customeraddress.CompanyName;
                }
            }


            return szEmail;
        }


        //
        // Get customer email
        public static string GetCustomerEmail(TimelyDepotContext db01, int CustomerId)
        {
            string szEmail = string.Empty;
            IQueryable<CustomersContactAddress> qryAddress = null;
            CustomersContactAddress customeraddress = null;

            qryAddress = db01.CustomersContactAddresses.Where(ctad => ctad.CustomerId == CustomerId);
            if (qryAddress.Count() > 0)
            {
                customeraddress = qryAddress.FirstOrDefault<CustomersContactAddress>();
                if (customeraddress != null)
                {
                    szEmail = customeraddress.Email;
                }
            }


            return szEmail;
        }

        //
        // GET: /Customer/DeleteSales
        public ActionResult DeleteShip(int id, int customerdefaultid = 0)
        {
            int nCustomerId = 0;
            CustomerDefaults customerDefaults = db.CustomerDefaults.Find(customerdefaultid);
            if (customerDefaults != null)
            {
                if (customerDefaults.ShiptoAddressId == id)
                {
                    customerDefaults.ShiptoAddressId = null;
                    customerDefaults.ShiptoName = null;
                }
            }

            CustomersShipAddress customersspecialnotes = db.CustomersShipAddresses.Find(id);
            nCustomerId = Convert.ToInt32(customersspecialnotes.CustomerId);
            db.CustomersShipAddresses.Remove(customersspecialnotes);
            db.SaveChanges();
            return RedirectToAction("Edit", new { id = nCustomerId });
            //return RedirectToAction("Index");
        }

        //
        // POST: /Customer/UpdateCustomerNote
        [HttpPost]
        public ActionResult UpdateCustomerShip(CustomersShipAddress customenote, string customerdefault)
        {
            int nCustomerDefault = Convert.ToInt32(customerdefault);
            CustomerDefaults custdefault = null;

            if (customenote != null)
            {
                if (ModelState.IsValid)
                {
                    if (customenote.Id == 0)
                    {
                        if (string.IsNullOrEmpty(customenote.Tel))
                        {
                            customenote.Tel = "0";
                        }
                        if (string.IsNullOrEmpty(customenote.Fax))
                        {
                            customenote.Fax = "0";
                        }
                        customenote.Tel = customenote.Tel.Replace("-", string.Empty);
                        customenote.Fax = customenote.Fax.Replace("-", string.Empty);
                        db.CustomersShipAddresses.Add(customenote);
                        db.SaveChanges();

                        //Set the default customer value
                        custdefault = db.CustomerDefaults.Find(nCustomerDefault);
                        if (custdefault != null)
                        {
                            custdefault.ShiptoAddressId = customenote.Id;
                            custdefault.ShiptoName = string.Format("{0} {1}", customenote.FirstName, customenote.LastName);
                            db.Entry(custdefault).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        //Set the default customer value
                        custdefault = db.CustomerDefaults.Find(nCustomerDefault);
                        if (custdefault != null)
                        {
                            custdefault.ShiptoAddressId = customenote.Id;
                            custdefault.ShiptoName = string.Format("{0} {1}", customenote.FirstName, customenote.LastName);
                            db.Entry(custdefault).State = EntityState.Modified;
                        }

                        if (string.IsNullOrEmpty(customenote.Tel))
                        {
                            customenote.Tel = "0";
                        }
                        if (string.IsNullOrEmpty(customenote.Fax))
                        {
                            customenote.Fax = "0";
                        }
                        customenote.Tel = customenote.Tel.Replace("-", string.Empty);
                        customenote.Fax = customenote.Fax.Replace("-", string.Empty);
                        db.Entry(customenote).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    TempData["ActiveTab"] = "2";
                    //return RedirectToAction("Index");
                    return RedirectToAction("Edit", new { id = Convert.ToInt32(customenote.CustomerId) });
                }

            }

            return RedirectToAction("Index");
        }

        //
        // GET: /Customer/CreateEditShip
        [NoCache]
        public PartialViewResult CreateEditShip(string customerid, int id = 0)
        {
            int nCustomerId = Convert.ToInt32(customerid);
            string szCustomerInfo = string.Empty;
            //IQueryable<CustomersSalesContact> qryAddress = null;

            CustomersShipAddress customernote = null;
            Customers customer = null;
            CustomerDefaults customerDefault = null;
            if (id == 0)
            {
                customernote = new CustomersShipAddress();
                customernote.CustomerId = nCustomerId;
            }
            else
            {
                customernote = db.CustomersShipAddresses.Find(id);
            }

            //Get Customer info
            if (customernote != null)
            {
                customer = db.Customers.Find(customernote.CustomerId);
                if (customer != null)
                {
                    szCustomerInfo = string.Format("{0}", customer.CustomerNo);
                }
            }
            ViewBag.CustomerInfo = szCustomerInfo;

            //Get the customer default id
            ViewBag.CustomerDefaultId = 0;
            customerDefault = db.CustomerDefaults.Where(ctdf => ctdf.CustomerId == nCustomerId).FirstOrDefault<CustomerDefaults>();
            if (customerDefault != null)
            {
                ViewBag.CustomerDefaultId = customerDefault.Id;
            }


            return PartialView(customernote);
        }


        //
        // GET: /Customer/DeleteSales
        public ActionResult DeleteSales(int id, int customerdefaultid = 0)
        {
            int nCustomerId = 0;
            CustomerDefaults customerDefaults = db.CustomerDefaults.Find(customerdefaultid);
            if (customerDefaults != null)
            {
                if (customerDefaults.SalesContactId == id)
                {
                    customerDefaults.SalesContactId = null;
                    customerDefaults.SalesName = null;
                }
            }

            CustomersSalesContact customersspecialnotes = db.CustomersSalesContacts.Find(id);
            nCustomerId = Convert.ToInt32(customersspecialnotes.CustomerId);
            db.CustomersSalesContacts.Remove(customersspecialnotes);
            db.SaveChanges();
            return RedirectToAction("Edit", new { id = nCustomerId });
            //return RedirectToAction("Index");
        }

        //
        // POST: /Customer/UpdateCustomerNote
        [HttpPost]
        public ActionResult UpdateCustomerSales(CustomersSalesContact customenote, string customerdefault)
        {
            int nCustomerDefault = Convert.ToInt32(customerdefault);
            CustomerDefaults custdefault = null;

            if (customenote != null)
            {
                if (ModelState.IsValid)
                {
                    if (customenote.Id == 0)
                    {
                        if (string.IsNullOrEmpty(customenote.Tel))
                        {
                            customenote.Tel = "0";
                        }
                        if (string.IsNullOrEmpty(customenote.Fax))
                        {
                            customenote.Fax = "0";
                        }
                        customenote.Tel = customenote.Tel.Replace("-", string.Empty);
                        customenote.Fax = customenote.Fax.Replace("-", string.Empty);

                        db.CustomersSalesContacts.Add(customenote);
                        db.SaveChanges();

                        //Set the default customer value
                        custdefault = db.CustomerDefaults.Find(nCustomerDefault);
                        if (custdefault != null)
                        {
                            custdefault.SalesContactId = customenote.Id;
                            custdefault.SalesName = string.Format("{0} {1}", customenote.FirstName, customenote.LastName);
                            db.Entry(custdefault).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        //Set the default customer value
                        custdefault = db.CustomerDefaults.Find(nCustomerDefault);
                        if (custdefault != null)
                        {
                            custdefault.SalesContactId = customenote.Id;
                            custdefault.SalesName = string.Format("{0} {1}", customenote.FirstName, customenote.LastName);
                            db.Entry(custdefault).State = EntityState.Modified;
                        }
                        if (string.IsNullOrEmpty(customenote.Tel))
                        {
                            customenote.Tel = "0";
                        }
                        if (string.IsNullOrEmpty(customenote.Fax))
                        {
                            customenote.Fax = "0";
                        }
                        customenote.Tel = customenote.Tel.Replace("-", string.Empty);
                        customenote.Fax = customenote.Fax.Replace("-", string.Empty);

                        db.Entry(customenote).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    //return RedirectToAction("Index");
                    return RedirectToAction("Edit", new { id = Convert.ToInt32(customenote.CustomerId) });
                }

            }

            return RedirectToAction("Index");
        }


        //
        // GET: /Customer/CreateEditSales
        [NoCache]
        public PartialViewResult CreateEditSales(string customerid, int id = 0)
        {
            int nCustomerId = Convert.ToInt32(customerid);
            string szCustomerInfo = string.Empty;
            //IQueryable<CustomersSalesContact> qryAddress = null;

            CustomersSalesContact customersalescontact = null;
            Customers customer = null;
            CustomerDefaults customerDefault = null;
            //CustomersSubsidiaryAddress customeraddress = null;

            if (id == 0)
            {
                customersalescontact = new CustomersSalesContact();
                customersalescontact.CustomerId = nCustomerId;
            }
            else
            {
                customersalescontact = db.CustomersSalesContacts.Find(id);
            }

            //Get Customer info
            if (customersalescontact != null)
            {
                customer = db.Customers.Find(customersalescontact.CustomerId);
                if (customer != null)
                {
                    szCustomerInfo = string.Format("{0}", customer.CustomerNo);

                    //qryAddress = db.CustomersSubsidiaryAddresses.Where(cust => cust.CustomerId == customer.Id);
                    //if (qryAddress.Count() > 0)
                    //{
                    //    customeraddress = qryAddress.FirstOrDefault<CustomersSubsidiaryAddress>();
                    //    if (customeraddress != null)
                    //    {
                    //        szCustomerInfo = string.Format("{0} {1}", szCustomerInfo, customeraddress.CompanyName);
                    //    }
                    //}
                }
            }
            ViewBag.CustomerInfo = szCustomerInfo;

            //Get the customer default id
            ViewBag.CustomerDefaultId = 0;
            customerDefault = db.CustomerDefaults.Where(ctdf => ctdf.CustomerId == nCustomerId).FirstOrDefault<CustomerDefaults>();
            if (customerDefault != null)
            {
                ViewBag.CustomerDefaultId = customerDefault.Id;
            }

            return PartialView(customersalescontact);
        }

        //
        // GET: /Customer/DeleteNote
        public ActionResult DeleteSubsidiary(int id, int customerdefaultid = 0)
        {
            int nCustomerId = 0;
            CustomerDefaults customerDefaults = db.CustomerDefaults.Find(customerdefaultid);
            if (customerDefaults != null)
            {
                if (customerDefaults.SubsidiaryId == id)
                {
                    customerDefaults.SubsidiaryId = null;
                    customerDefaults.SubsidiaryName = null;
                }
            }

            CustomersSubsidiaryAddress customersspecialnotes = db.CustomersSubsidiaryAddresses.Find(id);
            nCustomerId = Convert.ToInt32(customersspecialnotes.CustomerId);
            db.CustomersSubsidiaryAddresses.Remove(customersspecialnotes);
            db.SaveChanges();
            return RedirectToAction("Edit", new { id = nCustomerId });
            //return RedirectToAction("Index");
        }


        //
        // POST: /Customer/UpdateCustomerNote
        [HttpPost]
        public ActionResult UpdateCustomerSubsidiary(CustomersSubsidiaryAddress customenote, string customerdefault)
        {
            int nCustomerDefault = Convert.ToInt32(customerdefault);
            CustomerDefaults custdefault = null;

            if (customenote != null)
            {
                if (ModelState.IsValid)
                {
                    if (customenote.Id == 0)
                    {
                        if (string.IsNullOrEmpty(customenote.Tel))
                        {
                            customenote.Tel = "0";
                        }
                        if (string.IsNullOrEmpty(customenote.Fax))
                        {
                            customenote.Fax = "0";
                        }
                        customenote.Tel = customenote.Tel.Replace("-", string.Empty);
                        customenote.Fax = customenote.Fax.Replace("-", string.Empty);
                        db.CustomersSubsidiaryAddresses.Add(customenote);
                        db.SaveChanges();

                        //Set the default customer value
                        custdefault = db.CustomerDefaults.Find(nCustomerDefault);
                        if (custdefault != null)
                        {
                            custdefault.SubsidiaryId = customenote.Id;
                            custdefault.SubsidiaryName = string.Format("{0} {1}", customenote.FirstName, customenote.LastName);
                            db.Entry(custdefault).State = EntityState.Modified;
                        }
                        db.SaveChanges();

                    }
                    else
                    {
                        //Set the default customer value
                        custdefault = db.CustomerDefaults.Find(nCustomerDefault);
                        if (custdefault != null)
                        {
                            custdefault.SubsidiaryId = customenote.Id;
                            custdefault.SubsidiaryName = string.Format("{0} {1}", customenote.FirstName, customenote.LastName);
                            db.Entry(custdefault).State = EntityState.Modified;
                        }
                        if (string.IsNullOrEmpty(customenote.Tel))
                        {
                            customenote.Tel = "0";
                        }
                        if (string.IsNullOrEmpty(customenote.Fax))
                        {
                            customenote.Fax = "0";
                        }
                        customenote.Tel = customenote.Tel.Replace("-", string.Empty);
                        customenote.Fax = customenote.Fax.Replace("-", string.Empty);
                        db.Entry(customenote).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    TempData["ActiveTab"] = "1";
                    //return RedirectToAction("Index");
                    return RedirectToAction("Edit", new { id = Convert.ToInt32(customenote.CustomerId) });
                }

            }

            return RedirectToAction("Index");
        }

        //
        // GET: /Customer/CreateEditSubsidiary
        [NoCache]
        public PartialViewResult CreateEditSubsidiary(string customerid, int id = 0)
        {
            int nCustomerId = Convert.ToInt32(customerid);
            string szCustomerInfo = string.Empty;
            IQueryable<CustomersSubsidiaryAddress> qryAddress = null;

            CustomersSubsidiaryAddress customernote = null;
            Customers customer = null;
            CustomersSubsidiaryAddress customeraddress = null;
            CustomerDefaults customerDefault = null;

            if (id == 0)
            {
                customernote = new CustomersSubsidiaryAddress();
                customernote.CustomerId = nCustomerId;
            }
            else
            {
                customernote = db.CustomersSubsidiaryAddresses.Find(id);
            }

            //Get Customer info
            if (customernote != null)
            {
                customer = db.Customers.Find(customernote.CustomerId);
                if (customer != null)
                {
                    szCustomerInfo = string.Format("{0}", customer.CustomerNo);

                    qryAddress = db.CustomersSubsidiaryAddresses.Where(cust => cust.CustomerId == customer.Id);
                    if (qryAddress.Count() > 0)
                    {
                        customeraddress = qryAddress.FirstOrDefault<CustomersSubsidiaryAddress>();
                        if (customeraddress != null)
                        {
                            szCustomerInfo = string.Format("{0} {1}", szCustomerInfo, customeraddress.CompanyName);
                        }
                    }
                }
            }
            ViewBag.CustomerInfo = szCustomerInfo;

            //Get the customer default id
            ViewBag.CustomerDefaultId = 0;
            customerDefault = db.CustomerDefaults.Where(ctdf => ctdf.CustomerId == nCustomerId).FirstOrDefault<CustomerDefaults>();
            if (customerDefault != null)
            {
                ViewBag.CustomerDefaultId = customerDefault.Id;
            }

            return PartialView(customernote);
        }

        //
        // POST: /Customer/UpdateCustomerNote
        [HttpPost]
        public ActionResult UpdateCustomerNote(CustomersSpecialNotes customenote, int customerid = 0)
        {

            if (customenote != null)
            {
                if (ModelState.IsValid)
                {
                    if (customenote.Id == 0)
                    {
                        db.CustomersSpecialNotes.Add(customenote);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(customenote).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    TempData["ActiveTab"] = "6";
                    //return RedirectToAction("Index");
                    return RedirectToAction("Edit", new { id = Convert.ToInt32(customenote.CustomerId) });
                }

            }

            return RedirectToAction("Index");
        }

        //
        // GET: /Customer/CreateEditNote
        [NoCache]
        public PartialViewResult CreateEditNote(string customerid, int id = 0)
        {
            int nCustomerId = Convert.ToInt32(customerid);
            string szCustomerInfo = string.Empty;
            IQueryable<CustomersContactAddress> qryAddress = null;

            CustomersSpecialNotes customernote = null;
            Customers customer = null;
            CustomersContactAddress customeraddress = null;

            if (id == 0)
            {
                customernote = new CustomersSpecialNotes();
                customernote.CustomerId = nCustomerId;
            }
            else
            {
                customernote = db.CustomersSpecialNotes.Find(id);
            }

            //Get Customer info
            if (customernote != null)
            {
                customer = db.Customers.Find(customernote.CustomerId);
                if (customer != null)
                {
                    szCustomerInfo = string.Format("{0}", customer.CustomerNo);

                    qryAddress = db.CustomersContactAddresses.Where(cust => cust.CustomerId == customer.Id);
                    if (qryAddress.Count() > 0)
                    {
                        customeraddress = qryAddress.FirstOrDefault<CustomersContactAddress>();
                        if (customeraddress != null)
                        {
                            szCustomerInfo = string.Format("{0} {1}", szCustomerInfo, customeraddress.CompanyName);
                        }
                    }
                }
            }
            ViewBag.CustomerInfo = szCustomerInfo;


            return PartialView(customernote);
        }

        //
        // GET: /Customer/DeleteNote
        public ActionResult DeleteNote(int id)
        {
            CustomersSpecialNotes customersspecialnotes = db.CustomersSpecialNotes.Find(id);
            db.CustomersSpecialNotes.Remove(customersspecialnotes);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // POST: /Customers/UpdateCustomerCreditCard
        [HttpPost]
        public void UpdateCustomerCreditCard(CustomerView cusview)
        {
            CustomersCreditCardShipping customeraddress = null;

            if (cusview.customercredit != null)
            {
                if (cusview.customercredit.Id == 0)
                {
                    customeraddress = new CustomersCreditCardShipping();
                }
                else
                {
                    customeraddress = db.CustomersCreditCardShippings.Find(cusview.customercredit.Id);
                }

                if (customeraddress != null)
                {
                    customeraddress.Name = cusview.customercredit.Name;
                    customeraddress.CreditNumber = cusview.customercredit.CreditNumber;
                    customeraddress.ExpirationDate = cusview.customercredit.ExpirationDate;
                    customeraddress.SecureCode = cusview.customercredit.SecureCode;
                    customeraddress.Address1 = cusview.customercredit.Address1;
                    customeraddress.Address2 = cusview.customercredit.Address2;
                    customeraddress.Country = cusview.customercredit.Country;
                    customeraddress.State = cusview.customercredit.State;
                    customeraddress.Tel = cusview.customercredit.Tel.Replace("-", string.Empty);
                    customeraddress.Zip = cusview.customercredit.Zip;
                    customeraddress.CustomerId = cusview.customercredit.CustomerId;

                    db.Entry(customeraddress).State = EntityState.Modified;
                    db.SaveChanges();

                }
            }

     
        }

        //
        // POST: /Customers/UpdateCustomerHistory
        [HttpPost]
        public ActionResult UpdateCustomerHistory(CustomerView cusview)
        {
            CustomersHistory customeraddress = null;

            if (cusview.customerhistory != null)
            {
                if (cusview.customerhistory.Id == 0)
                {
                    customeraddress = new CustomersHistory();
                }
                else
                {
                    customeraddress = db.CustomersHistories.Find(cusview.customerhistory.Id);
                }

                if (customeraddress != null)
                {
                    customeraddress.OpenPurchaseOrder = cusview.customerhistory.OpenPurchaseOrder;
                    customeraddress.OutstandingBalance = cusview.customerhistory.OutstandingBalance;
                    customeraddress.CustomerId = cusview.customerhistory.CustomerId;

                    db.Entry(customeraddress).State = EntityState.Modified;
                    db.SaveChanges();

                }
            }

            TempData["ActiveTab"] = "5";
            //return RedirectToAction("Index");
            return RedirectToAction("Edit", new { id = Convert.ToInt32(customeraddress.CustomerId) });
        }

        //
        // POST: /Customers/UpdateCustomerBilling
        [HttpPost]
        public ActionResult UpdateCustomerBilling(CustomerView cusview)
        {
            CustomersBillingDept customeraddress = null;

            if (cusview.customerbilling != null)
            {
                if (cusview.customerbilling.Id == 0)
                {
                    customeraddress = new CustomersBillingDept();
                }
                else
                {
                    customeraddress = db.CustomersBillingDepts.Find(cusview.customerbilling.Id);
                }

                if (customeraddress != null)
                {
                    if (string.IsNullOrEmpty(cusview.customerbilling.Tel))
                    {
                        cusview.customerbilling.Tel = "0";
                    }
                    if (string.IsNullOrEmpty(cusview.customerbilling.Fax))
                    {
                        cusview.customerbilling.Fax = "0";
                    }

                    customeraddress.Address1 = cusview.customerbilling.Address1;
                    customeraddress.Address2 = cusview.customerbilling.Address2;
                    customeraddress.City = cusview.customerbilling.City;
                    customeraddress.Country = cusview.customerbilling.Country;
                    customeraddress.CustomerId = cusview.customerbilling.CustomerId;
                    customeraddress.Email = cusview.customerbilling.Email;
                    customeraddress.Fax = cusview.customerbilling.Fax.Replace("-", string.Empty);
                    customeraddress.FirstName = cusview.customerbilling.FirstName;
                    customeraddress.LastName = cusview.customerbilling.LastName;
                    customeraddress.State = cusview.customerbilling.State;
                    customeraddress.Tel = cusview.customerbilling.Tel.Replace("-", string.Empty);
                    customeraddress.Title = cusview.customerbilling.Title;
                    customeraddress.Zip = cusview.customerbilling.Zip;

                    db.Entry(customeraddress).State = EntityState.Modified;
                    db.SaveChanges();

                }
            }

            TempData["ActiveTab"] = "3";
            //return RedirectToAction("Index");
            return RedirectToAction("Edit", new { id = Convert.ToInt32(customeraddress.CustomerId) });
        }

        //
        // POST: /Customers/UpdateCustomerAddress
        [HttpPost]
        public ActionResult UpdateCustomerAddress(CustomerView cusview)
        {
            CustomersContactAddress customeraddress = null;

            if (cusview.customeraddress != null)
            {
                if (cusview.customeraddress.Id == 0)
                {
                    customeraddress = new CustomersContactAddress();
                }
                else
                {
                    customeraddress = db.CustomersContactAddresses.Find(cusview.customeraddress.Id);
                }

                if (customeraddress != null)
                {
                    customeraddress.Address = cusview.customeraddress.Address;
                    customeraddress.City = cusview.customeraddress.City;
                    customeraddress.CompanyName = cusview.customeraddress.CompanyName;
                    customeraddress.Country = cusview.customeraddress.Country;
                    customeraddress.CustomerId = cusview.customeraddress.CustomerId;
                    customeraddress.Email = cusview.customeraddress.Email;
                    customeraddress.Fax = cusview.customeraddress.Fax;
                    customeraddress.FirstName = cusview.customeraddress.FirstName;
                    customeraddress.LastName = cusview.customeraddress.LastName;
                    customeraddress.Note = cusview.customeraddress.Note;
                    customeraddress.State = cusview.customeraddress.State;
                    customeraddress.Tel = cusview.customeraddress.Tel;
                    customeraddress.Title = cusview.customeraddress.Title;
                    customeraddress.Website = cusview.customeraddress.Website;
                    customeraddress.Zip = cusview.customeraddress.Zip;

                    db.Entry(customeraddress).State = EntityState.Modified;
                    db.SaveChanges();

                }
            }

            return RedirectToAction("Index");
        }

        //
        // POST: /Customers/UpdateCustomer
        [HttpPost]
        public ActionResult UpdateCustomer(CustomerView cusview)
        {
            Customers customers = null;

            //Update Customer General Information
            if (Convert.ToInt32(cusview.customer.Id) != null)
            {
                customers = db.Customers.Find(cusview.customer.Id);
                if (customers != null)
                {
                    customers.ASINo = cusview.customer.ASINo;
                    customers.BussinesType = cusview.customer.BussinesType;
                    customers.CreditLimit = cusview.customer.CreditLimit;
                    //customers.CustomerNo = cusview.customer.CustomerNo;
                    customers.DeptoNo = cusview.customer.DeptoNo;
                    customers.Origin = cusview.customer.Origin;
                    customers.PaymentTerms = cusview.customer.PaymentTerms;
                    customers.PPAINo = cusview.customer.PPAINo;
                    customers.SageNo = cusview.customer.SageNo;
                    customers.SalesPerson = cusview.customer.SalesPerson;
                    customers.SellerPermintNo = cusview.customer.SellerPermintNo;
                    customers.Status = cusview.customer.Status;

                    db.Entry(customers).State = EntityState.Modified;
                    db.SaveChanges();

                }
            }
            return RedirectToAction("Index");
        }

        //
        // POST: /Customers/UpdateCustomerandAddress
        [HttpPost]
        public ActionResult UpdateCustomerandAddress(CustomerView cusview)
        {
            string szError = string.Empty;
            string szMsg = string.Empty;

            Customers customers = null;
            Customers customersDup = null;
            CustomersContactAddress customeraddress = null;
            IQueryable<Customers> qryCust = null;

            //Update Customer General Information
            if (Convert.ToInt32(cusview.customer.Id) != 0)
            {
                customers = db.Customers.Find(cusview.customer.Id);
                if (customers != null)
                {
                    customers.ASINo = cusview.customer.ASINo;
                    customers.BussinesType = cusview.customer.BussinesType;
                    customers.CreditLimit = cusview.customer.CreditLimit;
                    customers.DeptoNo = cusview.customer.DeptoNo;
                    customers.Origin = cusview.customer.Origin;
                    customers.PaymentTerms = cusview.customer.PaymentTerms;
                    customers.PPAINo = cusview.customer.PPAINo;
                    customers.SageNo = cusview.customer.SageNo;
                    customers.SalesPerson = cusview.customer.SalesPerson;
                    customers.SellerPermintNo = cusview.customer.SellerPermintNo;
                    customers.Status = cusview.customer.Status;

                    //Find duplicates for CustomerNo
                    customers.CustomerNo = cusview.customer.CustomerNo;

                    qryCust = db.Customers.Where(cut => cut.CustomerNo == customers.CustomerNo);
                    if (qryCust.Count() > 0)
                    {
                        customersDup = qryCust.FirstOrDefault<Customers>();
                        if (customersDup != null)
                        {
                            customeraddress = db.CustomersContactAddresses.Where(ctad => ctad.CustomerId == customersDup.Id).FirstOrDefault<CustomersContactAddress>();
                            if (customeraddress != null)
                            {
                                szMsg = customeraddress.CompanyName;
                            }
                        }
                        if (customers.Id != customeraddress.CustomerId)
                        {
                            szError = string.Format("Customer No. {0} already exist for {1}", customers.CustomerNo, szMsg);
                            TempData["Error"] = szError;
                            return RedirectToAction("Edit", new { id = Convert.ToInt32(cusview.customer.Id) });
                        }
                    }


                    db.Entry(customers).State = EntityState.Modified;
                    //db.SaveChanges();

                }
            }

            if (cusview.customeraddress != null)
            {
                if (cusview.customeraddress.Id == 0)
                {
                    customeraddress = new CustomersContactAddress();
                }
                else
                {
                    customeraddress = db.CustomersContactAddresses.Find(cusview.customeraddress.Id);
                }

                if (customeraddress != null)
                {
                    if (string.IsNullOrEmpty(cusview.customeraddress.Fax))
                    {
                        cusview.customeraddress.Fax = "0";
                    }
                    if (string.IsNullOrEmpty(cusview.customeraddress.Tel))
                    {
                        cusview.customeraddress.Tel = "0";
                    }

                    customeraddress.Address = cusview.customeraddress.Address;
                    customeraddress.City = cusview.customeraddress.City;
                    customeraddress.CompanyName = cusview.customeraddress.CompanyName;
                    customeraddress.Country = cusview.customeraddress.Country;
                    customeraddress.CustomerId = cusview.customeraddress.CustomerId;
                    customeraddress.Email = cusview.customeraddress.Email;
                    customeraddress.FirstName = cusview.customeraddress.FirstName;
                    customeraddress.LastName = cusview.customeraddress.LastName;
                    customeraddress.Note = cusview.customeraddress.Note;
                    customeraddress.State = cusview.customeraddress.State;
                    customeraddress.Fax = cusview.customeraddress.Fax.Replace("-", string.Empty);
                    customeraddress.Tel = cusview.customeraddress.Tel.Replace("-", string.Empty);
                    customeraddress.Title = cusview.customeraddress.Title;
                    customeraddress.Website = cusview.customeraddress.Website;
                    customeraddress.Zip = cusview.customeraddress.Zip;

                    db.Entry(customeraddress).State = EntityState.Modified;
                    //db.SaveChanges();

                }
            }

            db.SaveChanges();

            return RedirectToAction("Edit", new { id = Convert.ToInt32(cusview.customer.Id) });
        }

        //
        // GET: /Customers/Edit
        public ActionResult Edit(string modomultiple, int? page, int id = 0)
        {
            int nHas = 0;
            int nPos = -1;
            string szError = string.Empty;
            string szDecriptedData = string.Empty;
            string szDecriptedCode = string.Empty;

            //int? page = null;
            int pageNote = 0;
            int pageSubsidiary = 0;
            int pageSales = 0;
            int pageShipp = 0;
            int pageIndex = 0;
            int pageSize = 10;
            string szCustomerInfo = string.Empty;
            string szCustomerId = string.Empty;
            string szMsg = string.Empty;

            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            IQueryable<CustomersContactAddress> qryCusAdd = null;
            IQueryable<CustomersSubsidiaryAddress> qryCusSub = null;
            IQueryable<CustomersBillingDept> qryCusBil = null;
            IQueryable<CustomersSalesContact> qryCusSal = null;
            IQueryable<CustomersHistory> qryCusHty = null;
            IQueryable<CustomersCreditCardShipping> qryCusCre = null;
            IQueryable<CustomersSpecialNotes> qryCusNote = null;
            IQueryable<CustomersShipAddress> qryCusShip = null;
            IQueryable<Deptos> qryDepto = null;
            IQueryable<Bussines> qryBussines = null;
            IQueryable<Origin> qryOrigin = null;
            IQueryable<Terms> qryTerms = null;
            IQueryable<CustomerDefaults> qryCusDefault = null;

            CustomersContactAddress custMainContact = null;
            CustomersBillingDept custBilling = null;
            CustomersSalesContact custSalesContact = null;
            CustomersHistory custHistory = null;
            CustomersCreditCardShipping CustCredit = null;
            CustomersSubsidiaryAddress custSubsidiary = null;
            CustomersShipAddress CusShip = null;
            CustomersSpecialNotes CustNotes = null;
            CustomerDefaults customerdefaults = null;

            List<CustomersSpecialNotes> notesList = new List<CustomersSpecialNotes>();
            List<CustomersSubsidiaryAddress> subsidiaryList = new List<CustomersSubsidiaryAddress>();
            List<CustomersSalesContact> salesList = new List<CustomersSalesContact>();
            List<CustomersShipAddress> shipList = new List<CustomersShipAddress>();


            //Get the customer's data
            Customers customers = db.Customers.Find(id);
            if (customers != null)
            {
                //Use the customer defaults 
                customerdefaults = db.CustomerDefaults.Where(cudf => cudf.CustomerId == id).FirstOrDefault<CustomerDefaults>();
                if (customerdefaults == null)
                {
                    customerdefaults = new CustomerDefaults();
                    customerdefaults.CustomerId = id;
                    db.CustomerDefaults.Add(customerdefaults);
                    db.SaveChanges();
                }

                szCustomerInfo = customers.CustomerNo;

                qryCusAdd = db.CustomersContactAddresses.Where(cutadd => cutadd.CustomerId == customers.Id);
                if (qryCusAdd.Count() > 0)
                {
                    custMainContact = qryCusAdd.FirstOrDefault<CustomersContactAddress>();
                    szCustomerInfo = string.Format("{0} {1}", szCustomerInfo, custMainContact.CompanyName);
                    szCustomerId = customers.Id.ToString();
                }
                else
                {
                    custMainContact = new CustomersContactAddress();
                    custMainContact.CustomerId = customers.Id;
                    db.CustomersContactAddresses.Add(custMainContact);
                    szCustomerId = customers.Id.ToString();
                }

                qryCusBil = db.CustomersBillingDepts.Where(cutadd => cutadd.CustomerId == customers.Id);
                if (qryCusBil.Count() > 0)
                {
                    custBilling = qryCusBil.FirstOrDefault<CustomersBillingDept>();
                }
                else
                {
                    custBilling = new CustomersBillingDept();
                    custBilling.CustomerId = customers.Id;
                    db.CustomersBillingDepts.Add(custBilling);
                }
                qryCusHty = db.CustomersHistories.Where(cutadd => cutadd.CustomerId == customers.Id);
                if (qryCusHty.Count() > 0)
                {
                    custHistory = qryCusHty.FirstOrDefault<CustomersHistory>();
                }
                else
                {
                    custHistory = new CustomersHistory();
                    custHistory.CustomerId = customers.Id;
                    db.CustomersHistories.Add(custHistory);
                }

                qryCusCre = db.CustomersCreditCardShippings.Where(cutadd => cutadd.CustomerId == customers.Id);
                if (qryCusCre.Count() > 0)
                {
                    CustCredit = qryCusCre.FirstOrDefault<CustomersCreditCardShipping>();
                    if (customerdefaults.NoteId == null)
                    {
                        customerdefaults.NoteId = CustCredit.Id;
                        customerdefaults.NoteName = string.Format("{0}", CustCredit.CreditNumber);
                    }
                }
                else
                {
                    CustCredit = new CustomersCreditCardShipping();
                    CustCredit.CustomerId = customers.Id;
                    db.CustomersCreditCardShippings.Add(CustCredit);
                    if (customerdefaults.NoteId == null)
                    {
                        customerdefaults.NoteId = CustCredit.Id;
                        customerdefaults.NoteName = string.Format("{0}", CustCredit.Name);
                    }
                }

                qryCusShip = db.CustomersShipAddresses.Where(cutadd => cutadd.CustomerId == customers.Id).OrderBy(cutadd => cutadd.FirstName).ThenBy(cutadd => cutadd.LastName);
                if (qryCusShip.Count() > 0)
                {
                    //CusShip = qryCusShip.FirstOrDefault<CustomersShipAddress>();
                    foreach (var item in qryCusShip)
                    {
                        shipList.Add(item);
                        if (customerdefaults.ShiptoAddressId == null)
                        {
                            customerdefaults.ShiptoAddressId = item.Id;
                            customerdefaults.ShiptoName = item.Address1;
                        }
                    }
                }
                else
                {
                    CusShip = new CustomersShipAddress();
                    CusShip.CustomerId = customers.Id;
                    db.CustomersShipAddresses.Add(CusShip);
                    db.SaveChanges();
                    shipList.Add(CusShip);
                    if (customerdefaults.ShiptoAddressId == null)
                    {
                        customerdefaults.ShiptoAddressId = CusShip.Id;
                        customerdefaults.ShiptoName = CusShip.Address1;
                    }
                }

                qryCusSal = db.CustomersSalesContacts.Where(cutadd => cutadd.CustomerId == customers.Id).OrderBy(cutadd => cutadd.FirstName).ThenBy(cutadd => cutadd.LastName);
                if (qryCusSal.Count() > 0)
                {
                    //custSalesContact = qryCusSal.FirstOrDefault<CustomersSalesContact>();
                    foreach (var item in qryCusSal)
                    {
                        salesList.Add(item);
                        if (customerdefaults.SalesContactId == null)
                        {
                            customerdefaults.SalesContactId = item.Id;
                            customerdefaults.SalesName = string.Format("{0} {1}", item.FirstName, item.LastName);
                            custSalesContact = db.CustomersSalesContacts.Find(item.Id);
                        }
                    }
                }
                else
                {
                    custSalesContact = new CustomersSalesContact();
                    custSalesContact.CustomerId = customers.Id;
                    db.CustomersSalesContacts.Add(custSalesContact);
                    db.SaveChanges();
                    salesList.Add(custSalesContact);
                    if (customerdefaults.SalesContactId == null)
                    {
                        customerdefaults.SalesContactId = custSalesContact.Id;
                        customerdefaults.SalesName = string.Format("{0} {1}", custSalesContact.FirstName, custSalesContact.LastName);
                    }
                }

                qryCusSub = db.CustomersSubsidiaryAddresses.Where(cutadd => cutadd.CustomerId == customers.Id).OrderBy(cutadd => cutadd.CompanyName);
                if (qryCusSub.Count() > 0)
                {
                    //custSubsidiary = qryCusSub.FirstOrDefault<CustomersSubsidiaryAddress>();
                    foreach (var item in qryCusSub)
                    {
                        subsidiaryList.Add(item);
                        if (customerdefaults.SubsidiaryId == null)
                        {
                            customerdefaults.SubsidiaryId = item.Id;
                            customerdefaults.SubsidiaryName = string.Format("{0}", item.CompanyName);
                        }
                        if (custSubsidiary == null)
                        {
                            custSubsidiary = item;
                            customerdefaults.SubsidiaryId = item.Id;
                            customerdefaults.SubsidiaryName = string.Format("{0}", item.CompanyName);
                        }
                    }
                }
                else
                {
                    custSubsidiary = new CustomersSubsidiaryAddress();
                    custSubsidiary.CustomerId = customers.Id;
                    db.CustomersSubsidiaryAddresses.Add(custSubsidiary);
                    db.SaveChanges();
                    subsidiaryList.Add(custSubsidiary);
                    if (customerdefaults.SubsidiaryId == null)
                    {
                        customerdefaults.SubsidiaryId = custSubsidiary.Id;
                        customerdefaults.SubsidiaryName = string.Format("{0}", custSubsidiary.CompanyName);
                    }
                }


                qryCusNote = db.CustomersSpecialNotes.Where(cutadd => cutadd.CustomerId == customers.Id).OrderByDescending(cutadd => cutadd.Id);
                if (qryCusNote.Count() > 0)
                {
                    //CustNotes = qryCusNote.FirstOrDefault<CustomersSpecialNotes>();
                    foreach (var item in qryCusNote)
                    {
                        notesList.Add(item);
                        //if (customerdefaults.NoteId == null)
                        //{
                        //    customerdefaults.NoteId = item.Id;
                        //    customerdefaults.NoteName = string.Format("{0}", item.SpecialNote);
                        //}
                        if (CustNotes == null)
                        {
                            CustNotes = item;
                        }
                    }
                }
                else
                {
                    CustNotes = new CustomersSpecialNotes();
                    CustNotes.CustomerId = customers.Id;
                    db.CustomersSpecialNotes.Add(CustNotes);
                    notesList.Add(CustNotes);
                    if (customerdefaults.NoteId == null)
                    {
                        customerdefaults.NoteId = CustNotes.Id;
                        customerdefaults.NoteName = string.Format("{0}", CustNotes.SpecialNote);
                    }
                }

            }

            db.SaveChanges();

            ViewBag.CustomerInfo = szCustomerInfo;
            ViewBag.CustomerId = szCustomerId;

            //Get the dropdown data
            qryDepto = db.Deptos.OrderBy(dpt => dpt.Name);
            if (qryDepto.Count() > 0)
            {
                foreach (var item in qryDepto)
                {
                    listSelector.Add(new KeyValuePair<string, string>(item.DeptoNo, item.Name));
                }
            }
            SelectList deptoslist = new SelectList(listSelector, "Key", "Value");
            ViewBag.DeptosList = deptoslist;

            listSelector = new List<KeyValuePair<string, string>>();
            qryBussines = db.Bussines.OrderBy(bss => bss.BussinesType);
            if (qryBussines.Count() > 0)
            {
                foreach (var item in qryBussines)
                {
                    listSelector.Add(new KeyValuePair<string, string>(item.BussinesType, item.BussinesType));
                }
            }
            SelectList bussineslist = new SelectList(listSelector, "Key", "Value");
            ViewBag.BussinesList = bussineslist;

            listSelector = new List<KeyValuePair<string, string>>();
            qryOrigin = db.Origins.OrderBy(org => org.Name);
            if (qryOrigin.Count() > 0)
            {
                foreach (var item in qryOrigin)
                {
                    listSelector.Add(new KeyValuePair<string, string>(item.Name, item.Name));
                }
            }
            SelectList Originlist = new SelectList(listSelector, "Key", "Value");
            ViewBag.OriginList = Originlist;

            listSelector = new List<KeyValuePair<string, string>>();
            qryTerms = db.Terms.OrderBy(trm => trm.Term);
            if (qryTerms.Count() > 0)
            {
                foreach (var item in qryTerms)
                {
                    listSelector.Add(new KeyValuePair<string, string>(item.Term, item.Term));
                }
            }
            SelectList Termslist = new SelectList(listSelector, "Key", "Value");
            ViewBag.TermsList = Termslist;

            listSelector = new List<KeyValuePair<string, string>>();
            qryCusSal = db.CustomersSalesContacts.Where(csp => csp.CustomerId == customers.Id).OrderBy(csp => csp.FirstName).ThenBy(csp => csp.LastName);
            if (qryCusSal.Count() > 0)
            {
                foreach (var item in qryCusSal)
                {
                    szMsg = string.Format("{0} {1}", item.FirstName, item.LastName);
                    listSelector.Add(new KeyValuePair<string, string>(szMsg, szMsg));
                }
            }
            SelectList SalesPersonslist = new SelectList(listSelector, "Key", "Value");
            ViewBag.SalesPersonsList = SalesPersonslist;

            listSelector = new List<KeyValuePair<string, string>>();
            qryCusSal = db.CustomersSalesContacts.Where(csp => csp.CustomerId == customers.Id).OrderBy(csp => csp.FirstName).ThenBy(csp => csp.LastName);
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

            listSelector = new List<KeyValuePair<string, string>>();
            qryCusSub = db.CustomersSubsidiaryAddresses.Where(cusb => cusb.CustomerId == customers.Id).OrderBy(cusb => cusb.CompanyName);
            if (qryCusSub.Count() > 0)
            {
                foreach (var item in qryCusSub)
                {
                    listSelector.Add(new KeyValuePair<string, string>(item.Id.ToString(), item.CompanyName));
                }
            }
            SelectList cussublist = new SelectList(listSelector, "Key", "Value");
            ViewBag.SubsidiaryList = cussublist;

            listSelector = new List<KeyValuePair<string, string>>();
            qryCusShip = db.CustomersShipAddresses.Where(cusb => cusb.CustomerId == customers.Id).OrderBy(cusb => cusb.FirstName).ThenBy(cusb => cusb.LastName);
            if (qryCusShip.Count() > 0)
            {
                foreach (var item in qryCusShip)
                {
                    szMsg = string.Format("{0} {1}", item.FirstName, item.LastName);
                    listSelector.Add(new KeyValuePair<string, string>(item.Id.ToString(), szMsg));
                }
            }
            SelectList cusshiplist = new SelectList(listSelector, "Key", "Value");
            ViewBag.ShipList = cusshiplist;

            listSelector = new List<KeyValuePair<string, string>>();
            qryCusCre = db.CustomersCreditCardShippings.Where(cucr => cucr.CustomerId == customers.Id).OrderBy(cucr => cucr.CreditNumber);
            if (qryCusCre.Count() > 0)
            {
                foreach (var item in qryCusCre)
                {
                    szError = string.Empty;
                    szDecriptedData = TimelyDepotMVC.Controllers.PaymentController.DecodeInfo02(item.CreditNumber, ref szError);
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

                    //listSelector.Add(new KeyValuePair<string, string>(item.Id.ToString(), item.CreditNumber));
                    listSelector.Add(new KeyValuePair<string, string>(item.Id.ToString(), szDecriptedData));
                }
            }
            SelectList cuscreditlist = new SelectList(listSelector, "Key", "Value");
            ViewBag.CreditCardList = cuscreditlist;


            //Get the missing (null) objects
            if (custSalesContact == null)
            {
                custSalesContact = db.CustomersSalesContacts.Find(customerdefaults.SalesContactId);
                if (custSalesContact == null)
                {
                    custSalesContact = new CustomersSalesContact();
                }
            }
            if (custSubsidiary == null)
            {
                custSubsidiary = db.CustomersSubsidiaryAddresses.Find(customerdefaults.SubsidiaryId);
                if (custSubsidiary == null)
                {
                    custSubsidiary = new CustomersSubsidiaryAddress();
                }
            }
            if (CusShip == null)
            {
                CusShip = db.CustomersShipAddresses.Find(customerdefaults.ShiptoAddressId);
                if (CusShip == null)
                {
                    CusShip = new CustomersShipAddress();
                }
            }

            //Get theActive/Inactive list
            listSelector = new List<KeyValuePair<string, string>>();
            listSelector.Add(new KeyValuePair<string, string>("Y", "Active"));
            listSelector.Add(new KeyValuePair<string, string>("N", "Inactive"));
            SelectList activeinactivelist = new SelectList(listSelector, "Key", "Value");
            ViewBag.ActiveInactivelist = activeinactivelist;


            CustomerView cusview = new CustomerView();
            cusview.customer = customers;
            cusview.customeraddress = custMainContact;
            cusview.customerbilling = custBilling;
            cusview.customerhistory = custHistory;
            cusview.customercredit = CustCredit;
            cusview.customerdefaults = customerdefaults;
            cusview.customersalescontact = custSalesContact;
            cusview.custmersubsidiary = custSubsidiary;
            cusview.customershipaddress = CusShip;
            cusview.customernote = CustNotes;

            if (TempData["AddCustomer"] != null)
            {
                ViewBag.AddCustomerHlp = TempData["AddCustomer"].ToString();
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

            if (string.IsNullOrEmpty(modomultiple))
            {
                pageNote = pageIndex;
                pageSubsidiary = pageIndex;
                pageSales = pageIndex;
                pageShipp = pageIndex;
            }
            else
            {
                switch (modomultiple)
                {
                    case "SpecialNote":
                        pageNote = pageIndex;
                        break;
                    case "Subsidiary":
                        pageSubsidiary = pageIndex;
                        break;
                    case "Sales":
                        pageSales = pageIndex;
                        PageSize = 1;
                        break;
                    case "Shipp":
                        pageShipp = pageIndex;
                        break;
                    default:
                        break;
                }
            }

            //Verify page numbre
            if (pageNote == 0)
            {
                pageNote = 1;
            }

            var noteListHlp = notesList.ToPagedList(1, 1);
            ViewBag.OnePageOfNotesData = noteListHlp;
            cusview.customernotesList = noteListHlp;

            var subsidiaryListHlp = subsidiaryList.ToPagedList(1, 1);
            ViewBag.OnePageOfsubsidiarysData = subsidiaryListHlp;
            cusview.customersubsidiaryList = subsidiaryListHlp;

            //var salesListHlp = salesList.ToPagedList(pageSales, pageSize);        
            var salesListHlp = salesList.ToPagedList(1, 1);
            ViewBag.OnePageOfsalesData = salesListHlp;
            cusview.customersalesList = salesListHlp;

            var shippListHlp = shipList.ToPagedList(1, 1);
            ViewBag.OnePageOfshipsData = shippListHlp;
            cusview.customershipList = shippListHlp;

            //Set the error, if any
            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"].ToString();
            }

            //Set the active tab
            if (TempData["ActiveTab"] != null)
            {
                ViewBag.ActiveTab = TempData["ActiveTab"].ToString();
            }

            return View(cusview);
        }

        //
        // POST: /CustomersAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customers customers)
        {
            string szError = string.Empty;

            IQueryable<Customers> qryCust = null;

            if (ModelState.IsValid)
            {
                //Check CustmerNo does not existe
                qryCust = db.Customers.Where(cut => cut.CustomerNo == customers.CustomerNo);
                if (qryCust.Count() > 0)
                {
                    szError = string.Format("Customer No. {0} already exist.", customers.CustomerNo);
                    ModelState.AddModelError("CustomerNo", szError);
                    return View(customers);
                }

                db.Customers.Add(customers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customers);
        }

        //
        // GET: /Customers/Create01
        public ActionResult Create01()
        {
            int nCustomerId = 0;

            DateTime dDate = DateTime.Now;
            Customers customers = new Customers();
            customers.BussinesSice = dDate;
            customers.CustomerNo = GetNextCustomerNo();
            db.Customers.Add(customers);
            db.SaveChanges();

            nCustomerId = customers.Id;
            TempData["AddCustomer"] = "AddCustomer";

            return RedirectToAction("Edit", new { id = nCustomerId });
        }

        private string GetNextCustomerNo()
        {
            string szCustomerNo = "0";

            int qryHlp = db.Customers.Max(cst => cst.Id);
            if (qryHlp > 0)
            {
                qryHlp++;
                szCustomerNo = qryHlp.ToString();
            }

            return szCustomerNo;
        }

        //
        // GET: /CustomersAdmin/Create
        public PartialViewResult Create()
        {
            DateTime dDate = DateTime.Now;
            Customers customers = new Customers();
            customers.BussinesSice = dDate;
            customers.CustomerNo = "0";

            return PartialView(customers);
        }

        //
        // GET: /Customers/Delete/5
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            IQueryable<CustomersContactAddress> qryAddress = null;
            IQueryable<CustomersSpecialNotes> qryNote = null;
            IQueryable<CustomersSubsidiaryAddress> qrySubsidiary = null;
            IQueryable<CustomersBillingDept> qryBilling = null;
            IQueryable<CustomersSalesContact> qrySales = null;
            IQueryable<CustomersShipAddress> qryShip = null;
            IQueryable<CustomersHistory> qryHistory = null;
            IQueryable<CustomersCreditCardShipping> qryCredit = null;

            CustomersContactAddress customerAddress = null;
            CustomersSpecialNotes customerNote = null;
            CustomersSubsidiaryAddress customerSubsidiary = null;
            CustomersBillingDept customerBilling = null;
            CustomersSalesContact customerSales = null;
            CustomersShipAddress customerShip = null;
            CustomersHistory customerHistory = null;
            CustomersCreditCardShipping customerCredit = null;

            Customers customers = db.Customers.Find(id);
            if (customers != null)
            {

                qryNote = db.CustomersSpecialNotes.Where(cust => cust.CustomerId == customers.Id);
                if (qryNote.Count() > 0)
                {
                    foreach (var item in qryNote)
                    {
                        db.CustomersSpecialNotes.Remove(item);
                    }
                }

                qryAddress = db.CustomersContactAddresses.Where(cust => cust.CustomerId == customers.Id);
                if (qryAddress.Count() > 0)
                {
                    customerAddress = qryAddress.FirstOrDefault<CustomersContactAddress>();
                    db.CustomersContactAddresses.Remove(customerAddress);
                }


                qrySubsidiary = db.CustomersSubsidiaryAddresses.Where(cust => cust.CustomerId == customers.Id);
                if (qrySubsidiary.Count() > 0)
                {
                    customerSubsidiary = qrySubsidiary.FirstOrDefault<CustomersSubsidiaryAddress>();
                    db.CustomersSubsidiaryAddresses.Remove(customerSubsidiary);
                }


                qryBilling = db.CustomersBillingDepts.Where(cust => cust.CustomerId == customers.Id);
                if (qryBilling.Count() > 0)
                {
                    customerBilling = qryBilling.FirstOrDefault<CustomersBillingDept>();
                    db.CustomersBillingDepts.Remove(customerBilling);
                }


                qrySales = db.CustomersSalesContacts.Where(cust => cust.CustomerId == customers.Id);
                if (qrySales.Count() > 0)
                {
                    foreach (var item in qrySales)
                    {
                        db.CustomersSalesContacts.Remove(item);
                    }
                }


                qryShip = db.CustomersShipAddresses.Where(cust => cust.CustomerId == customers.Id);
                if (qryShip.Count() > 0)
                {
                    foreach (var item in qryShip)
                    {
                        db.CustomersShipAddresses.Remove(item);
                    }
                }

                qryHistory = db.CustomersHistories.Where(cust => cust.CustomerId == customers.Id);
                if (qryHistory.Count() > 0)
                {
                    customerHistory = qryHistory.FirstOrDefault<CustomersHistory>();
                    db.CustomersHistories.Remove(customerHistory);
                }

                qryCredit = db.CustomersCreditCardShippings.Where(cust => cust.CustomerId == customers.Id);
                if (qryCredit.Count() > 0)
                {
                    customerCredit = qryCredit.FirstOrDefault<CustomersCreditCardShipping>();
                    db.CustomersCreditCardShippings.Remove(customerCredit);
                }

                db.Customers.Remove(customers);
                db.SaveChanges();

            }

            return RedirectToAction("Index");
        }

        //
        // GET: /Customers/
        [NoCache]
        public ActionResult  Index(int? page, string searchItem, string ckActive, string ckCriteria)
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


        public static string GetBussinesType(TimelyDepotContext db01, int nCustomerId, string CustomerNo)
        {
            string szBussinesType = "Domestic";

            Customers customer;
            CustomersContactAddress maincontact = null;

            if (nCustomerId != null)
            {
                customer = db01.Customers.Find(nCustomerId);
                if (customer != null)
                {
                    szBussinesType = customer.BussinesType;
                }
            }
            else
            {

            }

            return szBussinesType;
        }
    }
}

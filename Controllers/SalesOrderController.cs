using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimelyDepotMVC.CommonCode;
using TimelyDepotMVC.Models.Admin;
using TimelyDepotMVC.DAL;
using PagedList;
using TimelyDepotMVC.ModelsView;

namespace TimelyDepotMVC.Controllers
{
    using System.Globalization;

    public class SalesOrderController : Controller
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
        // GET: //CreateSalesOrder
        public ActionResult CreateSalesOrder(string id)
        {
            int nSalesOrderNo = 0;
            int nTradeid = 0;
            int nCustomerId = 0;
            double dTaxRate = 0;
            InitialInfo initialinfo = null;
            Customers customer = null;
            SalesOrder salesorder = null;

            //Get the sales order no
            initialinfo = db.InitialInfoes.FirstOrDefault<InitialInfo>();
            if (initialinfo == null)
            {
                initialinfo = new InitialInfo();
                initialinfo.InvoiceNo = 0;
                initialinfo.PaymentNo = 0;
                initialinfo.PurchaseOrderNo = 0;
                initialinfo.SalesOrderNo = 1;
                nSalesOrderNo = 1;
                initialinfo.TaxRate = 0;
                db.InitialInfoes.Add(initialinfo);
            }
            else
            {
                nSalesOrderNo = initialinfo.SalesOrderNo;
                nSalesOrderNo++;
                initialinfo.SalesOrderNo = nSalesOrderNo;
                dTaxRate = initialinfo.TaxRate;
                db.Entry(initialinfo).State = EntityState.Modified;
            }

            //Get the customer id
            customer = db.Customers.Where(cst => cst.CustomerNo == id).FirstOrDefault<Customers>();
            if (customer != null)
            {
                nCustomerId = customer.Id;

                salesorder = new SalesOrder();
                salesorder.CustomerId = nCustomerId;
                salesorder.SODate = DateTime.Now;
                salesorder.IsBlindShip = false;
                salesorder.TradeId = nTradeid;
                salesorder.SalesOrderNo = nSalesOrderNo.ToString();
                salesorder.Tax_rate = Convert.ToDouble(dTaxRate);
                salesorder.Invs_Tax = Convert.ToDouble(dTaxRate);
                db.SalesOrders.Add(salesorder);
                db.SaveChanges();
            }


            return RedirectToAction("Edit", new { id = salesorder.SalesOrderId });
            //return View("Edit", new { id = salesorder.SalesOrderId });
        }


        //
        // GET: /Quotes/SelectCustomer
        [NoCache]
        public PartialViewResult SelectCustomer(string companyname, string email, int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;

            SelectCustomer selectcustomer = null;
            List<SelectCustomer> selectcustomerList = new List<SelectCustomer>();

            ViewBag.SearchCompanyName = companyname;
            ViewBag.SearchEmail = email;

            if (string.IsNullOrEmpty(companyname) && string.IsNullOrEmpty(email))
            {
                var qryCustomer = db.CustomersContactAddresses.Join(db.Customers, cuca => cuca.CustomerId, cust => cust.Id, (cuca, cust) => new { cuca, cust }).OrderBy(custCA => custCA.cust.CustomerNo);
                if (qryCustomer.Count() > 0)
                {
                    foreach (var item in qryCustomer)
                    {
                        selectcustomer = new SelectCustomer();
                        selectcustomer.CustomerNo = item.cust.CustomerNo;
                        selectcustomer.Companyname = item.cuca.CompanyName;
                        selectcustomer.Email = item.cuca.Email;
                        selectcustomerList.Add(selectcustomer);
                    }
                }
            }

            if (!string.IsNullOrEmpty(companyname))
            {
                var qryCustomer = db.CustomersContactAddresses.Join(db.Customers, cuca => cuca.CustomerId, cust => cust.Id, (cuca, cust) => new { cuca, cust }).Where(custca => custca.cuca.CompanyName.StartsWith(companyname)).OrderBy(custCA => custCA.cust.CustomerNo);
                if (qryCustomer.Count() > 0)
                {
                    foreach (var item in qryCustomer)
                    {
                        selectcustomer = new SelectCustomer();
                        selectcustomer.CustomerNo = item.cust.CustomerNo;
                        selectcustomer.Companyname = item.cuca.CompanyName;
                        selectcustomer.Email = item.cuca.Email;
                        selectcustomerList.Add(selectcustomer);
                    }
                }

            }

            if (!string.IsNullOrEmpty(email))
            {
                var qryCustomer = db.CustomersContactAddresses.Join(db.Customers, cuca => cuca.CustomerId, cust => cust.Id, (cuca, cust) => new { cuca, cust }).Where(custca => custca.cuca.Email.StartsWith(email)).OrderBy(custCA => custCA.cust.CustomerNo);
                if (qryCustomer.Count() > 0)
                {
                    foreach (var item in qryCustomer)
                    {
                        selectcustomer = new SelectCustomer();
                        selectcustomer.CustomerNo = item.cust.CustomerNo;
                        selectcustomer.Companyname = item.cuca.CompanyName;
                        selectcustomer.Email = item.cuca.Email;
                        selectcustomerList.Add(selectcustomer);
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

            var onePageOfData = selectcustomerList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return PartialView(selectcustomerList.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: //Payment
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
        // GET: /SalesOrder/SalesOrderListExcel
        public ActionResult SalesOrderListExcel()
        {
            //DataTable hlpTbl = GetCustomerListTable();

            ExportCSV(GetSalesOrderTable(), "SalesOrderList");

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

        private DataTable GetSalesOrderTable()
        {
            long telHlp = 0;
            long faxHlp = 0;
            string telfmt = "000-000-0000";
            string szTel = "";

            TimelyDepotContext db01 = new TimelyDepotContext();

            DataTable table = null;
            DataRow row = null;

            SalesOrderList thesalesorderlist = null;
            List<SalesOrderList> vendorList = new List<SalesOrderList>();

            var qrySalesOrder = db.CustomersContactAddresses.Join(db.SalesOrders, ctad => ctad.CustomerId, slod => slod.CustomerId, (ctad, slod)
                => new { ctad, slod }).OrderBy(cact => cact.slod.SalesOrderId);
            if (qrySalesOrder.Count() > 0)
            {
                foreach (var item in qrySalesOrder)
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

                    thesalesorderlist = new SalesOrderList();
                    thesalesorderlist.SalesOrderId = item.slod.SalesOrderId;
                    thesalesorderlist.SalesOrderNo = item.slod.SalesOrderNo;
                    thesalesorderlist.SODate = item.slod.SODate;
                    thesalesorderlist.CustomerNo = GetCustomerDataSO(db01, item.ctad.CustomerId.ToString());
                    thesalesorderlist.CompanyName = item.ctad.CompanyName;
                    thesalesorderlist.PurchaseOrderNo = item.slod.PurchaseOrderNo;
                    thesalesorderlist.ShipDate = item.slod.ShipDate;
                    thesalesorderlist.PaymentAmount = GetSalesOrderAmount(db01, item.slod.SalesOrderId);

                    vendorList.Add(thesalesorderlist);
                }
            }

            table = new DataTable("SalesOrderList");

            // Set the header
            DataColumn col01 = new DataColumn("SalesOrderNo", System.Type.GetType("System.String"));
            DataColumn col02 = new DataColumn("SODate", System.Type.GetType("System.String"));
            DataColumn col03 = new DataColumn("CustomerNo", System.Type.GetType("System.String"));
            DataColumn col04 = new DataColumn("CompanyName", System.Type.GetType("System.String"));
            DataColumn col05 = new DataColumn("PurchaseOrderNo", System.Type.GetType("System.String"));
            DataColumn col06 = new DataColumn("ShipDate", System.Type.GetType("System.String"));
            DataColumn col07 = new DataColumn("Amount", System.Type.GetType("System.String"));
            table.Columns.Add(col01);
            table.Columns.Add(col02);
            table.Columns.Add(col03);
            table.Columns.Add(col04);
            table.Columns.Add(col05);
            table.Columns.Add(col06);
            table.Columns.Add(col07);

            //Set the data row
            foreach (var item in vendorList)
            {
                row = table.NewRow();
                row["SalesOrderNo"] = item.SalesOrderNo;
                row["SODate"] = item.SODate;
                row["CustomerNo"] = item.CustomerNo;
                row["CompanyName"] = item.CompanyName;
                row["PurchaseOrderNo"] = item.PurchaseOrderNo;
                row["ShipDate"] = item.ShipDate;
                row["Amount"] = item.PaymentAmount;
                table.Rows.Add(row);
            }

            return table;
        }


        //
        // GET: /SalesOrder/CustomerList
        [NoCache]
        public PartialViewResult SalesOrderList(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;

            TimelyDepotContext db01 = new TimelyDepotContext();

            SalesOrderList thesalesorderlist = null;
            List<SalesOrderList> customerList = new List<SalesOrderList>();

            var qrySalesOrder = db.CustomersContactAddresses.Join(db.SalesOrders, ctad => ctad.CustomerId, slod => slod.CustomerId, (ctad, slod)
                => new { ctad, slod }).OrderBy(cact => cact.slod.SalesOrderId);
            if (qrySalesOrder.Count() > 0)
            {
                foreach (var item in qrySalesOrder)
                {
                    thesalesorderlist = new SalesOrderList();
                    thesalesorderlist.SalesOrderId = item.slod.SalesOrderId;
                    thesalesorderlist.SalesOrderNo = item.slod.SalesOrderNo;
                    thesalesorderlist.SODate = item.slod.SODate;
                    thesalesorderlist.CustomerNo = GetCustomerDataSO(db01, item.ctad.CustomerId.ToString());
                    thesalesorderlist.CompanyName = item.ctad.CompanyName;
                    thesalesorderlist.PurchaseOrderNo = item.slod.PurchaseOrderNo;
                    thesalesorderlist.ShipDate = item.slod.ShipDate;
                    thesalesorderlist.PaymentAmount = GetSalesOrderAmount(db01, item.slod.SalesOrderId);

                    customerList.Add(thesalesorderlist);
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

        public decimal GetSalesOrderAmount(TimelyDepotContext db01, int nSalesOrderId)
        {
            double dAmount = 0;
            double dSalesAmount = 0;
            double dTotalTax = 0;
            double dTax = 0;
            double dTotalAmount = 0;
            double dBalanceDue = 0;

            //Get the totals
            GetSalesOrderTotals01(db01, nSalesOrderId, ref dSalesAmount, ref dTotalTax, ref dTax, ref dTotalAmount, ref dBalanceDue);
            dAmount = dTotalAmount;

            return Convert.ToDecimal(dAmount);
        }


        private string GetCustomerDataSO(TimelyDepotContext db01, string szCustomerId)
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
        // GET: /SalesOrder/InsertItem
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
            SalesOrderDetail salesdetailcurrent = null;
            SalesOrderDetail salesdetailnext = null;

            int nSalesOrderId = Convert.ToInt32(salesorderid);


            if (!string.IsNullOrEmpty(itemOrder))
            {
                //itemOrder = itemOrder.Replace(".", ",");
                nItemOrder = Convert.ToDouble(itemOrder);
            }

            //Get the current salesorderdetail
            salesdetailcurrent = db.SalesOrderDetails.Where(sldt => sldt.SalesOrderId == nSalesOrderId && sldt.ItemPosition == itemPos && sldt.ItemOrder == nItemOrder).FirstOrDefault<SalesOrderDetail>();
            if (salesdetailcurrent != null)
            {
                nCurrentItemPos = Convert.ToInt32(salesdetailcurrent.ItemPosition);
                dCurrentItemOrder = Convert.ToDouble(salesdetailcurrent.ItemOrder);
                szCurentItemId = salesdetailcurrent.ItemID;
            }

            //Get the next salesorderdetail
            salesdetailnext = db.SalesOrderDetails.Where(sldt => sldt.SalesOrderId == nSalesOrderId && sldt.ItemPosition == nCurrentItemPos && sldt.ItemOrder > dCurrentItemOrder).OrderBy(sldt => sldt.ItemOrder).FirstOrDefault<SalesOrderDetail>();
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

            //if (subitem.ItemID == szNextItemId)
            //{
            //    nItemPos = nCurrentItemPos;
            //    dItemOrder = (dCurrentItemOrder + dNextItemOrder) / 2;
            //}
            //else
            //{
            //    nItemPos = nCurrentItemPos;
            //    dItemOrder = dCurrentItemOrder + 1;
            //}



            SalesOrderDetail salesdetail = null;

            salesdetail = new SalesOrderDetail();
            salesdetail.SalesOrderId = nSalesOrderId;
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
            db.SalesOrderDetails.Add(salesdetail);
            db.SaveChanges();

            return RedirectToAction("Edit", new { id = nSalesOrderId });
        }

        //
        // GET: /SalesOrder/UpdateDetail
        public ActionResult UpdateDetail(int? id, string salesorderid, string qty, string shipqty, string boqty, string desc, string price, string tax,
            string logo, string imprt, string qtysc, string qtyrc, string pricesc, string pricerc)
        {
            int nPos = 0;
            int nPriceId = 0;
            double dHlp = 0;
            decimal dcHlp = 0;
            decimal dcHlp1 = 0;
            int nSalesOrderId = Convert.ToInt32(salesorderid);
            string szSalesOrder = "";
            string szSalesOredidHlp = "";

            SalesOrderDetail sodetail = db.SalesOrderDetails.Find(id);
            SalesOrderDetail setupcharge = null;
            SalesOrderDetail runcharge = null;

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
                    szSalesOredidHlp = string.Format("Set up Charge {0} {1}", sodetail.SalesOrderId.ToString(), sodetail.ItemID);
                    setupcharge = db.SalesOrderDetails.Where(spch => spch.SalesOrderId == sodetail.SalesOrderId && spch.Description == szSalesOredidHlp).FirstOrDefault<SalesOrderDetail>();
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
                }

                //Update Run Charge
                if (!string.IsNullOrEmpty(pricesc) && !string.IsNullOrEmpty(qtysc))
                {
                    szSalesOredidHlp = string.Format("Run Charge {0} {1}", sodetail.SalesOrderId.ToString(), sodetail.ItemID);
                    runcharge = db.SalesOrderDetails.Where(spch => spch.SalesOrderId == sodetail.SalesOrderId && spch.Description == szSalesOredidHlp).FirstOrDefault<SalesOrderDetail>();
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
                }
            }

            return RedirectToAction("Edit", new { id = nSalesOrderId });
        }

        //
        // GET: /SalesOrder/DeleteDetail
        public ActionResult DeleteDetail(int id = 0)
        {
            int salesorderId = 0;
            string szSetup = "";
            string szRun = "";

            SalesOrderDetail salesdetailSetup = null;
            SalesOrderDetail salesdetailRun = null;
            SalesOrderDetail salesdetail = db.SalesOrderDetails.Find(id);
            if (salesdetail != null)
            {
                szSetup = string.Format("Set up Charge {0} {1}", salesdetail.SalesOrderId.ToString(), salesdetail.ItemID);
                szRun = string.Format("Run Charge {0} {1}", salesdetail.SalesOrderId.ToString(), salesdetail.ItemID);
                salesorderId = Convert.ToInt32(salesdetail.SalesOrderId);
                db.SalesOrderDetails.Remove(salesdetail);

                salesdetailSetup = db.SalesOrderDetails.Where(sldt => sldt.Description == szSetup).FirstOrDefault<SalesOrderDetail>();
                if (salesdetailSetup != null)
                {
                    db.SalesOrderDetails.Remove(salesdetailSetup);
                }

                salesdetailRun = db.SalesOrderDetails.Where(sldt => sldt.Description == szRun).FirstOrDefault<SalesOrderDetail>();
                if (salesdetailRun != null)
                {
                    db.SalesOrderDetails.Remove(salesdetailRun);
                }
                db.SaveChanges();
            }

            return RedirectToAction("Edit", new { id = salesorderId });
        }

        //
        // GET: /SalesOrder/AddSalesOrderDetails
        [NoCache]
        public ActionResult AddSalesOrderDetails(string itemOrder, int id = 0, int salesorderId = 0, int itemPos = 0)
        {
            int nitemPosNext = 0;
            int nItemPos = 0;
            int nCurrentItemPos = 0;
            int nNextItemPos = 0;
            int nSalesOrderId = 0;
            double nItemOrder = 0;
            double dItemOrder = 0;
            double dCurrentItemOrder = 0;
            double dNextItemOrder = 0;
            double dQty = 0;
            decimal dPrice = 0;
            string szCurentItemId = "";
            string szNextItemId = "";
            string szSalesOrderIdHlp = "";
            string szItemId = "";
            SalesOrderDetail salesdetail = null;
            SalesOrderDetail salesdetailcurrent = null;
            SalesOrderDetail salesdetailnext = null;
            SalesOrderDetail salesorderdetail = null;
            SetupChargeDetail setupcharge = null;
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

                //Get the current salesorderdetail
                salesdetailcurrent = db.SalesOrderDetails.Where(sldt => sldt.SalesOrderId == salesorderId && sldt.ItemPosition == itemPos && sldt.ItemOrder == nItemOrder).FirstOrDefault<SalesOrderDetail>();
                if (salesdetailcurrent != null)
                {
                    nCurrentItemPos = Convert.ToInt32(salesdetailcurrent.ItemPosition);
                    dCurrentItemOrder = Convert.ToDouble(salesdetailcurrent.ItemOrder);
                    szCurentItemId = salesdetailcurrent.ItemID;
                }

                //Get the next salesorderdetail
                salesdetailnext = db.SalesOrderDetails.Where(sldt => sldt.SalesOrderId == salesorderId && sldt.ItemPosition == nCurrentItemPos && sldt.ItemOrder > dCurrentItemOrder).OrderBy(sldt => sldt.ItemOrder).FirstOrDefault<SalesOrderDetail>();
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

                salesdetail = new SalesOrderDetail();
                salesdetail.SalesOrderId = salesorderId;
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
                db.SalesOrderDetails.Add(salesdetail);
                db.SaveChanges();

                szSalesOrderIdHlp = salesdetail.SalesOrderId.ToString();
                nSalesOrderId = Convert.ToInt32(szSalesOrderIdHlp);
                szItemId = salesdetail.ItemID;

                //Create setup charge
                setupcharge = db.SetupChargeDetails.Where(stup => stup.itemid == subitem.ItemID).FirstOrDefault<SetupChargeDetail>();
                if (setupcharge != null)
                {
                    // Setup charge
                    double dDiscount = TimelyDepotMVC.Controllers.InventoryController.GetDiscount(db, setupcharge.SetupChargeDiscountCode);

                    salesorderdetail = new SalesOrderDetail();
                    salesorderdetail.SalesOrderId = nSalesOrderId;
                    salesorderdetail.ItemID = string.Empty;
                    salesorderdetail.Sub_ItemID = string.Empty;
                    salesorderdetail.Description = string.Format("Set up Charge {0} {1}", szSalesOrderIdHlp, szItemId);
                    salesorderdetail.Quantity = 0;
                    salesorderdetail.ShipQuantity = 0;
                    salesorderdetail.BackOrderQuantity = 0;
                    salesorderdetail.Tax = 0;
                    salesorderdetail.UnitPrice = setupcharge.SetUpCharge * (1 - Convert.ToDecimal(dDiscount));
                    salesorderdetail.ItemPosition = 0;
                    salesorderdetail.ItemOrder = 0;
                    salesorderdetail.Tax = 0;
                    db.SalesOrderDetails.Add(salesorderdetail);

                    //Create run charge
                    dDiscount = TimelyDepotMVC.Controllers.InventoryController.GetDiscount(db, setupcharge.RunChargeDiscountCode);
                    salesorderdetail = new SalesOrderDetail();
                    salesorderdetail.SalesOrderId = nSalesOrderId;
                    salesorderdetail.ItemID = string.Empty;
                    salesorderdetail.Sub_ItemID = string.Empty;
                    salesorderdetail.Description = string.Format("Run Charge {0} {1}", szSalesOrderIdHlp, szItemId);
                    salesorderdetail.Quantity = 0;
                    salesorderdetail.ShipQuantity = 0;
                    salesorderdetail.BackOrderQuantity = 0;
                    salesorderdetail.Tax = 0;
                    salesorderdetail.UnitPrice = setupcharge.RunCharge * (1 - Convert.ToDecimal(dDiscount));
                    salesorderdetail.ItemPosition = 0;
                    salesorderdetail.ItemOrder = 0;
                    salesorderdetail.Tax = 0;
                    db.SalesOrderDetails.Add(salesorderdetail);

                    db.SaveChanges();

                }
            }

            return RedirectToAction("Edit", new { id = salesorderId });
        }

        //
        // GET: /SalesOrder/AddDetail
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
        // POST: /SalesOrder/UpdateSalesOrderDetail
        public ActionResult UpdateSalesOrderDetail(SalesOrderDetail salesorderdetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salesorderdetail).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Edit", new { id = salesorderdetail.SalesOrderId });
        }

        //
        // /SalesOrder/EditDetail
        [NoCache]
        public PartialViewResult EditDetail(int id = 0)
        {
            SalesOrderDetail salesdetail = db.SalesOrderDetails.Find(id);

            return PartialView(salesdetail);
        }

        //
        // /SalesOrder/GetCompanyName
        public static string GetCompanyName(TimelyDepotContext db01, string VendorId)
        {
            int nVendorId = Convert.ToInt32(VendorId);
            string szCompanyName = "";
            IQueryable<VendorsContactAddress> qryAddress = null;
            VendorsContactAddress vendoraddress = null;

            qryAddress = db01.VendorsContactAddresses.Where(vad => vad.VendorId == nVendorId);
            if (qryAddress.Count() > 0)
            {
                vendoraddress = qryAddress.FirstOrDefault<VendorsContactAddress>();
                if (vendoraddress != null)
                {
                    szCompanyName = vendoraddress.CompanyName;
                }
            }

            return szCompanyName;
        }

        //
        // POST: /SalesOrder/UpdateBlindShip
        public ActionResult UpdateBlindShip(SalesOrderBlindShip salesorderblindship)
        {
            SalesOrder salesorder = null;
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
                db.SaveChanges();
            }

            return RedirectToAction("Edit", new { id = salesorderblindship.SalesOrderId });
        }

        //
        // GET: /SalesOrder/BlindShip
        [NoCache]
        public PartialViewResult BlindShip(string salesorderid)
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

            return PartialView(salesblind);
        }

        //
        // GET: /SalesOrder/GetSalesDetails
        [NoCache]
        public PartialViewResult GetSalesDetailsSRC(int? page, string salesorderid)
        {
            TimelyDepotContext db01 = new TimelyDepotContext();

            int pageIndex = 0;
            int pageSize = PageSize;
            pageSize = 10;
            int nSalesOrderid = Convert.ToInt32(salesorderid);
            string szSalesOredidHlp = "";

            IQueryable<SalesOrderDetail> qrysalesdetails = null;
            IQueryable<ImprintMethods> qryImprint = null;

            SalesOrderDetail setupcharge = null;
            SalesOrderDetail runcharge = null;
            SalesOrderDetailSRC salesorderdetailSRC = null;

            List<SalesOrderDetailSRC> salesdetailsList = new List<SalesOrderDetailSRC>();
            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            //qrysalesdetails = db.SalesOrderDetails.Where(sldt => sldt.SalesOrderId == nSalesOrderid).OrderBy(sldt => sldt.Sub_ItemID).ThenBy(sldt => sldt.ItemOrder);
            qrysalesdetails = db.SalesOrderDetails.Where(sldt => sldt.SalesOrderId == nSalesOrderid && sldt.ItemID != "").OrderBy(sldt => sldt.ItemPosition).ThenBy(sldt => sldt.ItemOrder);
            if (qrysalesdetails.Count() > 0)
            {
                foreach (var item in qrysalesdetails)
                {
                    salesorderdetailSRC = new SalesOrderDetailSRC();
                    salesorderdetailSRC.BackOrderQuantity = item.BackOrderQuantity;
                    salesorderdetailSRC.Description = item.Description;
                    salesorderdetailSRC.Id = item.Id;
                    salesorderdetailSRC.ImprintMethod = item.ImprintMethod;
                    salesorderdetailSRC.ItemID = item.ItemID;
                    salesorderdetailSRC.ItemOrder = item.ItemOrder;
                    salesorderdetailSRC.ItemPosition = item.ItemPosition;
                    salesorderdetailSRC.Logo = item.Logo;
                    salesorderdetailSRC.Quantity = item.Quantity;
                    salesorderdetailSRC.SalesOrderId = item.SalesOrderId;
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
                    setupcharge = db01.SalesOrderDetails.Where(stup => stup.SalesOrderId == item.SalesOrderId && stup.Description == szSalesOredidHlp).FirstOrDefault<SalesOrderDetail>();
                    if (setupcharge != null)
                    {
                        salesorderdetailSRC.QuantitySC = setupcharge.Quantity;
                        salesorderdetailSRC.UnitPricSRC = setupcharge.UnitPrice;
                    }
                    //Run Charge
                    szSalesOredidHlp = string.Format("Run Charge {0} {1}", salesorderdetailSRC.SalesOrderId.ToString(), salesorderdetailSRC.ItemID);
                    runcharge = db01.SalesOrderDetails.Where(stup => stup.SalesOrderId == item.SalesOrderId && stup.Description == szSalesOredidHlp).FirstOrDefault<SalesOrderDetail>();
                    if (runcharge != null)
                    {
                        salesorderdetailSRC.QuantityRC = runcharge.Quantity;
                        salesorderdetailSRC.UnitPriceRC = runcharge.UnitPrice;
                    }


                    salesdetailsList.Add(salesorderdetailSRC);
                }
            }
            ViewBag.SalesOrderId = salesorderid;

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
        // GET: /SalesOrder/GetSalesDetails
        [NoCache]
        public PartialViewResult GetSalesDetails(int? page, string salesorderid)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            pageSize = 10;
            int nSalesOrderid = Convert.ToInt32(salesorderid);

            IQueryable<SalesOrderDetail> qrysalesdetails = null;

            List<SalesOrderDetail> salesdetailsList = new List<SalesOrderDetail>();

            //qrysalesdetails = db.SalesOrderDetails.Where(sldt => sldt.SalesOrderId == nSalesOrderid).OrderBy(sldt => sldt.Sub_ItemID).ThenBy(sldt => sldt.ItemOrder);
            qrysalesdetails = db.SalesOrderDetails.Where(sldt => sldt.SalesOrderId == nSalesOrderid).OrderBy(sldt => sldt.ItemPosition).ThenBy(sldt => sldt.ItemOrder);
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
        // GET: /SalesOrder/SelectLocation
        [NoCache]
        public PartialViewResult SelectLocation(int? page, string customerid, string searchLocation)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            pageSize = 15;
            int nCustomerId = Convert.ToInt32(customerid);

            List<CustomersShipAddress> shiptoList = new List<CustomersShipAddress>();
            IQueryable<CustomersShipAddress> qryShipto = null;

            if (string.IsNullOrEmpty(searchLocation))
            {
                qryShipto = db.CustomersShipAddresses.Where(spt => spt.CustomerId == nCustomerId).OrderBy(spt => spt.City).ThenBy(spt => spt.Address1);
            }
            else
            {
                ViewBag.SearchLocation = searchLocation;
                qryShipto = db.CustomersShipAddresses.Where(spt => spt.CustomerId == nCustomerId && spt.Address1.StartsWith(searchLocation)).OrderBy(spt => spt.City).ThenBy(spt => spt.Address1);
            }

            if (qryShipto.Count() > 0)
            {
                foreach (var item in qryShipto)
                {
                    shiptoList.Add(item);
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


            var onePageOfData = shiptoList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return PartialView(shiptoList.ToPagedList(pageIndex, pageSize));
        }


        //
        // GET: /SalesOrder/SelectVendor
        [NoCache]
        public PartialViewResult SelectVendor(int? page, string searchItemVendor)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            pageSize = 15;

            VendorsSalesContact salescontact = null;
            List<VendorsSalesContact> venaddressList = new List<VendorsSalesContact>();
            //IQueryable<VendorsSalesContact> qryVenAddress = null;

            if (string.IsNullOrEmpty(searchItemVendor))
            {
                //qryVenAddress = db.VendorsContactAddresses.OrderBy(vad => vad.CompanyName);
                var qryVenAddress = db.VendorsContactAddresses.OrderBy(vad => vad.CompanyName).Join(db.VendorsSalesContacts, vad => vad.VendorId, vsc => vsc.VendorId,
                    (vad, vsc) => new { vad, vsc });

                if (qryVenAddress.Count() > 0)
                {
                    foreach (var item in qryVenAddress)
                    {
                        salescontact = item.vsc;
                        venaddressList.Add(salescontact);
                    }
                }
            }
            else
            {
                ViewBag.SearchItemVendor = searchItemVendor;
                var qryVenAddress = db.VendorsContactAddresses.Where(vad => vad.CompanyName.StartsWith(searchItemVendor)).OrderBy(vad => vad.CompanyName).Join(db.VendorsSalesContacts, vad => vad.VendorId, vsc => vsc.VendorId,
                    (vad, vsc) => new { vad, vsc });

                if (qryVenAddress.Count() > 0)
                {
                    foreach (var item in qryVenAddress)
                    {
                        salescontact = item.vsc;
                        venaddressList.Add(salescontact);
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


            var onePageOfData = venaddressList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return PartialView(venaddressList.ToPagedList(pageIndex, pageSize));
        }

        //
        // GetTradeData
        public static void GetTradeData(TimelyDepotContext db01, ref string szAddress, ref string szCity, ref string szState, ref string szZip,
            ref string szCountry, ref string szTel, ref string szFax, ref string szWebSite, ref string szEmail, ref string szTradeName,
            ref string szAsiTrade, ref string szSageTrade, ref string szPpaiTrade, int TradeId = 0)
        {
            Trade trade = db01.Trades.Find(TradeId);
            if (trade != null)
            {
                szTradeName = trade.TradeName;
                szAddress = trade.Address;
                szCity = trade.City;
                szState = trade.State;
                szZip = trade.PostCode;
                szCountry = trade.Country;
                szTel = trade.Tel;
                szFax = trade.Fax;
                szWebSite = trade.WebSite;
                szEmail = trade.Email;
                szAsiTrade = trade.ASINo;
                szSageTrade = trade.SageNo;
                szPpaiTrade = trade.PPAINo;
            }
        }

        //
        // GetCustomerData
        public static void GetCustomerData(TimelyDepotContext db01, ref string szASI, ref string szSAGE, ref string szWebSite, ref string szEmail, int CustomerId = 0)
        {
            Customers customer = null;
            CustomersContactAddress contactaddress = null;

            IQueryable<CustomersContactAddress> qryAddress = null;

            customer = db01.Customers.Find(CustomerId);
            if (customer != null)
            {
                szASI = customer.ASINo;
                szSAGE = customer.SageNo;

                qryAddress = db01.CustomersContactAddresses.Where(ctad => ctad.CustomerId == customer.Id);
                if (qryAddress.Count() > 0)
                {
                    contactaddress = qryAddress.FirstOrDefault<CustomersContactAddress>();
                    if (contactaddress != null)
                    {
                        szWebSite = contactaddress.Website;
                        szEmail = contactaddress.Email;
                    }
                }

            }
        }

        //
        // GetCustomerData01
        public static void GetCustomerData01(TimelyDepotContext db01, ref string szASI, ref string szSAGE, ref string szWebSite, ref string szEmail, ref string szCustomerNo, int CustomerId = 0)
        {
            Customers customer = null;
            CustomersContactAddress contactaddress = null;

            IQueryable<CustomersContactAddress> qryAddress = null;

            customer = db01.Customers.Find(CustomerId);
            if (customer != null)
            {
                szCustomerNo = customer.CustomerNo;
                szASI = customer.ASINo;
                szSAGE = customer.SageNo;

                qryAddress = db01.CustomersContactAddresses.Where(ctad => ctad.CustomerId == customer.Id);
                if (qryAddress.Count() > 0)
                {
                    contactaddress = qryAddress.FirstOrDefault<CustomersContactAddress>();
                    if (contactaddress != null)
                    {
                        szWebSite = contactaddress.Website;
                        szEmail = contactaddress.Email;
                    }
                }

            }
        }

        //
        //
        public static void GetCustomerEmail(TimelyDepotContext db01, ref string szName, ref string szEmail, int SalesOrderId = 0)
        {
            IQueryable<Customers> qrycustomer = null;
            IQueryable<CustomersContactAddress> qryAddress = null;
            Customers customer = null;
            CustomersContactAddress contactaddres = null;

            SalesOrder salesorder = db01.SalesOrders.Find(SalesOrderId);

            if (salesorder != null)
            {
                qrycustomer = db01.Customers.Where(cst => cst.Id == salesorder.CustomerId);
                if (qrycustomer.Count() > 0)
                {
                    customer = qrycustomer.FirstOrDefault<Customers>();

                    if (customer != null)
                    {
                        qryAddress = db01.CustomersContactAddresses.Where(ctad => ctad.CustomerId == customer.Id);
                        if (qryAddress.Count() > 0)
                        {
                            contactaddres = qryAddress.FirstOrDefault<CustomersContactAddress>();
                            if (contactaddres != null)
                            {
                                szName = contactaddres.CompanyName;
                                szEmail = contactaddres.Email;
                            }
                        }

                    }
                }
            }
        }


        //
        // GET: /SalesOrder/
        public ActionResult Index(int? page, string searchOrderNo, string searchCustomer, string searchEmail, string searchItem, string ckActive, string ckCriteria)
        {
            bool bHasData = false;
            int pageIndex = 0;
            int pageSize = PageSize;
            int nHas = 0;
            Customers customer = null;
            SalesOrder salesorder = null;
            DateTime dFecha = DateTime.Now;

            IQueryable<SalesOrder> qrySalesOrder = null;
            IQueryable<CustomersContactAddress> qryAddress = null;

            List<int> customerIdsList = new List<int>();
            List<SalesOrder> SalesOrderList = new List<SalesOrder>();

            if (string.IsNullOrEmpty(searchItem) || searchItem == "0")
            {
                //dFecha = dFecha.AddMonths(-1);
                //qryItem = db.ITEMs.OrderBy(it => it.ItemID);
                ViewBag.ckActiveHlp = "true";
                ViewBag.ckCriteriaHlp = "salesorder";
                ViewBag.CurrentDate = dFecha.ToString("yyyy/MM/dd");

                if (searchItem == "0")
                {
                    ViewBag.SearchItem = searchItem;

                    if (ckCriteria == "salesorder")
                    {
                        if (ckActive == "true")
                        {
                            qrySalesOrder = db.SalesOrders.OrderBy(slor => slor.SalesOrderNo);
                        }
                        else
                        {
                            qrySalesOrder = db.SalesOrders.OrderBy(slor => slor.SalesOrderNo);
                        }
                        bHasData = true;
                    }
                }
            }
            else
            {
                ViewBag.SearchItem = searchItem;
                ViewBag.ckActiveHlp = ckActive;
                ViewBag.ckCriteriaHlp = ckCriteria;

                if (ckCriteria == "salesorder")
                {
                    if (ckActive == "true")
                    {
                        qrySalesOrder = db.SalesOrders.Where(slor => slor.SalesOrderNo == searchItem).OrderBy(slor => slor.SalesOrderNo);
                    }
                    else
                    {
                        qrySalesOrder = db.SalesOrders.Where(slor => slor.SalesOrderNo == searchItem).OrderBy(slor => slor.SalesOrderNo);
                    }
                    bHasData = true;
                }

                if (ckCriteria == "customername")
                {
                    if (ckActive == "true")
                    {
                        var qryMainContact = db.SalesOrders.Join(db.CustomersContactAddresses, ctc => ctc.CustomerId, cus => cus.CustomerId, (ctc, cus)
                             => new { ctc, cus }).Where(Nctcs => Nctcs.cus.CompanyName.StartsWith(searchItem)).OrderBy(Nctcs => Nctcs.cus.CompanyName);
                        if (qryMainContact.Count() > 0)
                        {
                            foreach (var item in qryMainContact)
                            {
                                SalesOrderList.Add(item.ctc);
                            }
                        }
                    }
                    else
                    {
                        var qryMainContact = db.SalesOrders.Join(db.CustomersContactAddresses, ctc => ctc.CustomerId, cus => cus.CustomerId, (ctc, cus)
                             => new { ctc, cus }).Where(Nctcs => Nctcs.cus.CompanyName.StartsWith(searchItem)).OrderBy(Nctcs => Nctcs.cus.CompanyName);
                        if (qryMainContact.Count() > 0)
                        {
                            foreach (var item in qryMainContact)
                            {
                                SalesOrderList.Add(item.ctc);
                            }
                        }
                    }
                }
                if (ckCriteria == "customerpo")
                {
                    if (ckActive == "true")
                    {
                        qrySalesOrder = db.SalesOrders.Where(slor => slor.PurchaseOrderNo == searchItem).OrderBy(slor => slor.SalesOrderNo);
                    }
                    else
                    {
                        qrySalesOrder = db.SalesOrders.Where(slor => slor.PurchaseOrderNo == searchItem).OrderBy(slor => slor.SalesOrderNo);
                    }
                    bHasData = true;
                }

                if (ckCriteria == "itemno")
                {
                    if (ckActive == "true")
                    {
                        var qryMainContact = db.SalesOrders.Join(db.SalesOrderDetails, ctc => ctc.SalesOrderId, cus => cus.SalesOrderId, (ctc, cus)
                             => new { ctc, cus }).Where(Nctcs => Nctcs.cus.ItemID.StartsWith(searchItem)).OrderBy(Nctcs => Nctcs.cus.SalesOrderId);
                        if (qryMainContact.Count() > 0)
                        {
                            foreach (var item in qryMainContact)
                            {
                                SalesOrderList.Add(item.ctc);
                            }
                        }
                    }
                    else
                    {
                        var qryMainContact = db.SalesOrders.Join(db.SalesOrderDetails, ctc => ctc.SalesOrderId, cus => cus.SalesOrderId, (ctc, cus)
                             => new { ctc, cus }).Where(Nctcs => Nctcs.cus.ItemID.StartsWith(searchItem)).OrderBy(Nctcs => Nctcs.cus.SalesOrderId);
                        if (qryMainContact.Count() > 0)
                        {
                            foreach (var item in qryMainContact)
                            {
                                SalesOrderList.Add(item.ctc);
                            }
                        }
                    }
                }
            }

            if (bHasData)
            {
                if (qrySalesOrder != null)
                {
                    if (qrySalesOrder.Count() > 0)
                    {
                        foreach (var item in qrySalesOrder)
                        {
                            SalesOrderList.Add(item);
                        }
                    }
                }

            }
            //if (string.IsNullOrEmpty(searchOrderNo) && string.IsNullOrEmpty(searchCustomer) && string.IsNullOrEmpty(searchEmail))
            //{
            //    qrySalesOrder = db.SalesOrders.OrderBy(slor => slor.SalesOrderNo);
            //    if (qrySalesOrder.Count() > 0)
            //    {
            //        foreach (var item in qrySalesOrder)
            //        {
            //            SalesOrderList.Add(item);
            //        }
            //    }
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(searchEmail))
            //    {
            //        ViewBag.SearchEmail = searchEmail;

            //        qryAddress = db.CustomersContactAddresses.Where(ctad => ctad.Email.StartsWith(searchEmail));
            //        if (qryAddress.Count() > 0)
            //        {
            //            foreach (var item in qryAddress)
            //            {
            //                nHas = Convert.ToInt32(item.CustomerId);
            //                if (nHas > 0)
            //                {
            //                    customerIdsList.Add(nHas);
            //                }
            //            }
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(searchCustomer))
            //    {
            //        ViewBag.SearchCustomer = searchCustomer;

            //        qryAddress = db.CustomersContactAddresses.Where(ctad => ctad.CompanyName.StartsWith(searchCustomer));
            //        if (qryAddress.Count() > 0)
            //        {
            //            foreach (var item in qryAddress)
            //            {
            //                nHas = Convert.ToInt32(item.CustomerId);
            //                if (nHas > 0)
            //                {
            //                    customerIdsList.Add(nHas);
            //                }
            //            }
            //        }
            //    }

            //    if (customerIdsList.Count > 0)
            //    {
            //        foreach (var itemCustomer in customerIdsList)
            //        {
            //            qrySalesOrder = db.SalesOrders.Where(slor => slor.CustomerId == itemCustomer).OrderBy(slor => slor.SalesOrderNo);
            //            if (qrySalesOrder.Count() > 0)
            //            {
            //                foreach (var item in qrySalesOrder)
            //                {
            //                    SalesOrderList.Add(item);
            //                }
            //            }
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(searchOrderNo))
            //    {
            //        ViewBag.SearchOrderNo = searchOrderNo;

            //        qrySalesOrder = db.SalesOrders.Where(slor => slor.SalesOrderNo.StartsWith(searchOrderNo)).OrderBy(slor => slor.SalesOrderNo);
            //        if (qrySalesOrder.Count() > 0)
            //        {
            //            foreach (var item in qrySalesOrder)
            //            {
            //                SalesOrderList.Add(item);
            //            }
            //        }

            //    }
            //}



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
            return View(SalesOrderList.ToPagedList(pageIndex, pageSize));
            //return View(db.SalesOrders.ToList());
        }

        //
        // GET: /SalesOrder/Details/5

        public ActionResult Details(int id = 0)
        {
            SalesOrder salesorder = db.SalesOrders.Find(id);
            if (salesorder == null)
            {
                return HttpNotFound();
            }
            return View(salesorder);
        }

        //
        // GET: /SalesOrder/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SalesOrder/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SalesOrder salesorder)
        {
            if (ModelState.IsValid)
            {
                db.SalesOrders.Add(salesorder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(salesorder);
        }

        //
        // GET: /SalesOrder/Edit/5
        [NoCache]
        public ActionResult Edit(int id = 0)
        {
            double dSalesAmount = 0;
            double dTax = 0;
            double dTotalTax = 0;
            double dTotalAmount = 0;
            double dBalanceDue = 0;
            string szMsg = "";

            Trade trade = null;
            Customers customer = null;
            CustomersContactAddress soldto = null;
            CustomersBillingDept billto = null;
            CustomersShipAddress shipto = null;
            CustomersShipAddress shipto01 = null;
            VendorsContactAddress venaddress = null;
            VendorsSalesContact vendorsalescontact = null;
            SalesOrderBlindShip salesblind = null;

            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> list01Selector = new List<KeyValuePair<string, string>>();
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

            SalesOrder salesorder = db.SalesOrders.Find(id);
            if (salesorder == null)
            {
                return HttpNotFound();
            }
            decimal sumRefunds = 0;
            var listOfRefund = (from refundlist in this.db.Refunds
                              where refundlist.SalesOrderNo == salesorder.SalesOrderNo
                              select refundlist.RefundAmount).ToList();
            if (listOfRefund.Any())
            {
                sumRefunds= listOfRefund.Sum();
            }
           
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
            qryAddress = db.CustomersContactAddresses.Where(ctad => ctad.CustomerId == salesorder.CustomerId);
            if (qryAddress.Count() > 0)
            {
                foreach (var item in qryAddress)
                {
                    if (soldto == null)
                    {
                        //soldto = qryAddress.FirstOrDefault<CustomersContactAddress>();
                        soldto = item;
                        ViewBag.SoldTo = soldto;
                    }
                }
            }

            //Get the CustomerNo
            if (soldto != null)
            {
                customer = db.Customers.Where(cust => cust.Id == soldto.CustomerId).FirstOrDefault<Customers>();
                if (customer != null)
                {
                    ViewBag.Customer = customer;

                    //Update payment terms
                    if (string.IsNullOrEmpty(salesorder.PaymentTerms))
                    {
                        salesorder.PaymentTerms = customer.PaymentTerms;
                    }

                    listSelector = new List<KeyValuePair<string, string>>();
                    qryCusSal = db.CustomersSalesContacts.Where(csp => csp.CustomerId == customer.Id).OrderBy(csp => csp.FirstName).ThenBy(csp => csp.LastName);
                    if (qryCusSal.Count() > 0)
                    {
                        foreach (var item in qryCusSal)
                        {
                            szMsg = string.Format("{0} {1}", item.FirstName, item.LastName);
                            //listSelector.Add(new KeyValuePair<string, string>(item.Id.ToString(), szMsg));
                            listSelector.Add(new KeyValuePair<string, string>(szMsg, szMsg));

                            list01Selector.Add(new KeyValuePair<string, string>(item.Id.ToString(), szMsg));
                        }
                    }
                    SelectList cusdefaultlist = new SelectList(listSelector, "Key", "Value");
                    ViewBag.SalesContactList = cusdefaultlist;

                    SelectList cusdefault01list = new SelectList(list01Selector, "Key", "Value");
                    ViewBag.SalesContac01tList = cusdefault01list;

                    if (string.IsNullOrEmpty(salesorder.SalesRep))
                    {
                        salesorder.SalesRep = customer.SalesPerson;
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
            qryBill = db.CustomersBillingDepts.Where(ctbi => ctbi.CustomerId == salesorder.CustomerId);
            if (qryBill.Count() > 0)
            {
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
            qryshipto = db.CustomersShipAddresses.Where(ctsp => ctsp.Id == salesorder.CustomerId);
            if (qryshipto.Count() > 0)
            {
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

            //Get the Vendor address data
            qryVenAddres = db.VendorsContactAddresses.Where(vnad => vnad.VendorId == salesorder.VendorId);
            if (qryVenAddres.Count() > 0)
            {
                venaddress = qryVenAddres.FirstOrDefault<VendorsContactAddress>();
                if (venaddress != null)
                {
                    ViewBag.VendorAddress = venaddress;
                }
            }

            //Get the sales contact
            qrysalescontact = db.VendorsSalesContacts.Where(vdsc => vdsc.Id == salesorder.VendorId);
            if (qrysalescontact.Count() > 0)
            {
                foreach (var item in qrysalescontact)
                {
                    if (vendorsalescontact != null)
                    {
                        //vendorsalescontact = qrysalescontact.FirstOrDefault<VendorsSalesContact>();
                        vendorsalescontact = item;
                        ViewBag.VendorSalesContact = item;
                    }

                    szMsg = string.Format("{0} {1}", item.FirstName, item.LastName);
                    if (!string.IsNullOrEmpty(szMsg))
                    {
                        listSelector.Add(new KeyValuePair<string, string>(szMsg, szMsg));
                    }
                }
            }

            //Get the blind ship addres
            qryBlind = db.SalesOrderBlindShips.Where(slbd => slbd.SalesOrderId == salesorder.SalesOrderId);
            if (qryBlind.Count() > 0)
            {
                salesblind = qryBlind.FirstOrDefault<SalesOrderBlindShip>();
                if (salesblind != null)
                {
                    ViewBag.BlindShip = salesblind;
                }
            }

            //Get refunds


            //Get the totals
            
            GetSalesOrderTotals(salesorder.SalesOrderId, ref dSalesAmount, ref dTotalTax, ref dTax, ref dTotalAmount, ref dBalanceDue);
            ViewBag.SalesAmount = dSalesAmount.ToString("C");
            ViewBag.TotalTax = dTotalTax.ToString("C");
            ViewBag.Tax = dTax.ToString("F2");
            ViewBag.TotalAmount = dTotalAmount.ToString("C");
            ViewBag.BalanceDue = (dBalanceDue - (double)sumRefunds).ToString("C");
            ViewBag.Refunds = sumRefunds.ToString("C");

            // Get the Ship From and Ship to address if needed
            if (string.IsNullOrEmpty(salesorder.FromCompany))
            {
                trade = db.Trades.Find(salesorder.TradeId);
                if (trade != null)
                {
                    salesorder.FromAddress1 = trade.Address;
                    salesorder.FromCity = trade.City;
                    salesorder.FromCompany = trade.TradeName;
                    salesorder.FromCountry = trade.Country;
                    salesorder.FromEmail = trade.Email;
                    salesorder.FromFax = trade.Fax;
                    salesorder.FromState = trade.State;
                    salesorder.FromTel = trade.Tel;
                    salesorder.FromZip = trade.PostCode;
                }
            }

            if (string.IsNullOrEmpty(salesorder.ToCompany))
            {
                shipto01 = db.CustomersShipAddresses.Find(salesorder.CustomerShiptoId);
                if (shipto01 != null)
                {
                    salesorder.ToAddress1 = shipto01.Address1;
                    salesorder.ToAddress2 = shipto01.Address2;
                    salesorder.ToCity = shipto01.City;
                    salesorder.ToCompany = soldto.CompanyName;
                    salesorder.ToCountry = shipto01.Country;
                    salesorder.ToEmail = shipto01.Email;
                    salesorder.ToFax = shipto01.Fax;
                    salesorder.ToName = string.Format("{0} {1}", shipto01.FirstName, shipto01.LastName);
                    salesorder.ToState = shipto01.State;
                    salesorder.ToTel = shipto01.Tel;
                    salesorder.ToTitle = shipto01.Title;
                    salesorder.ToZip = shipto01.Zip;
                }
            }

            return View(salesorder);
        }

        private void GetSalesOrderTotals01(TimelyDepotContext db01, int nSalesOrderId, ref double dSalesAmount, ref double dTotalTax, ref double dTax, ref double dTotalAmount, ref double dBalanceDue)
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

            initialinfo = db01.InitialInfoes.FirstOrDefault<InitialInfo>();
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


            //Each sales order should save it own tax information. Also the tax should be on product only, no tax on service.
            SalesOrder salesorder = db01.SalesOrders.Find(nSalesOrderId);
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

                qryDetails = db01.SalesOrderDetails.Where(sldt => sldt.SalesOrderId == salesorder.SalesOrderId);
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

        public void GetSalesOrderTotals(int nSalesOrderId, ref double dSalesAmount, ref double dTotalTax, ref double dTax, ref double dTotalAmount, ref double dBalanceDue)
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
                    dPayment = Convert.ToDouble(salesorder.PaymentAmount,CultureInfo.CurrentCulture);

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
        // POST: /SalesOrder/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SalesOrder salesorder, string SODateHlp, string ShipDateHlp, string RequiredateHlp, string AprovedDateHlp, string billtoid)
        {
            int nBilltoId = 0;
            int nYear = 0;
            int nMonth = 0;
            int nDay = 0;
            string szMsg = "";
            string szError = "";
            string[] szShipDateHlp = null;
            string[] szSODateHlp = null;
            DateTime dShipDate = DateTime.Now;
            DateTime dSODate = DateTime.Now;
            DateTime dRequiredDate = DateTime.Now;
            DateTime dApprovedDate = DateTime.Now;

            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();
            IQueryable<Trade> qryTrade = null;
            IQueryable<Warehouses> qryWarehouse = null;
            CultureInfo provider = CultureInfo.InvariantCulture;

            dSODate = !string.IsNullOrEmpty(SODateHlp) ? DateTime.ParseExact(SODateHlp, "MM-dd-yyyy", provider) : Convert.ToDateTime(salesorder.SODate);
            dRequiredDate = !string.IsNullOrEmpty(RequiredateHlp) ? DateTime.ParseExact(RequiredateHlp, "MM-dd-yyyy", provider).Date : Convert.ToDateTime(salesorder.Requiredate);
            dShipDate = !string.IsNullOrEmpty(ShipDateHlp) ? DateTime.ParseExact(ShipDateHlp, "MM-dd-yyyy", provider).Date : DateTime.Now;
            dApprovedDate = !string.IsNullOrEmpty(AprovedDateHlp) ? DateTime.ParseExact(AprovedDateHlp, "MM-dd-yyyy", provider).Date : Convert.ToDateTime(salesorder.ShipDate);
             
            if (ModelState.IsValid)
            {
                salesorder.SODate = dSODate;
                salesorder.ShipDate = dShipDate;
                salesorder.Requiredate = dRequiredDate;
                salesorder.AprovedDate = dApprovedDate;

                if (salesorder.Tax_rate == null)
                {
                    salesorder.Tax_rate = 0;
                }

                if (!string.IsNullOrEmpty(billtoid))
                {
                    try
                    {
                        nBilltoId = Convert.ToInt32(billtoid);
                    }
                    catch (Exception)
                    {
                        nBilltoId = 0;
                    }

                    if (nBilltoId == 0)
                    {
                        salesorder.CustomerShiptoId = null;
                    }
                    else
                    {
                        salesorder.CustomerShiptoId = nBilltoId;
                    }
                }

                db.Entry(salesorder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                //Get the drop down data to enabel the display of errors
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

                //Get the error
                foreach (var item in ModelState.Values)
                {
                    if (item.Errors.Count > 0)
                    {
                        foreach (var itemError in item.Errors)
                        {
                            szMsg = itemError.ErrorMessage;
                            szError = string.Format("{0} {1}", szError, szMsg);
                        }
                    }
                }
                ViewBag.ErrorSalesOrder = szError;

            }
            return View(salesorder);
        }

        //
        // GET: /SalesOrder/Delete/5

        public ActionResult Delete(int id = 0)
        {
            SalesOrder salesorder = db.SalesOrders.Find(id);
            if (salesorder == null)
            {
                return HttpNotFound();
            }
            return View(salesorder);
        }

        //
        // POST: /SalesOrder/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            //Delete Sales Order Details
            DeleteSalesOrderDetails(id);

            SalesOrder salesorder = db.SalesOrders.Find(id);

            db.SalesOrders.Remove(salesorder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void DeleteSalesOrderDetails(int id)
        {
            int nCustomerID = 0;
            int nUserID = 0;
            int nQuoteId = 0;
            string szItemID = "";
            string szEmail = "";

            //IQueryable<UserQuotation> qryQuoteUsr = null;
            IQueryable<UserQuotationDetail> qryQuoteDetails = null;
            UserQuotation userquotation = null;
            CustomersContactAddress customercontactaddress = null;

            TimelyDepotContext db01 = new TimelyDepotContext();
            //SalesOrderDetail salesorderdetails = null;

            //Get customer email
            SalesOrder salesorder = db.SalesOrders.Find(id);
            if (salesorder != null)
            {
                customercontactaddress = db01.CustomersContactAddresses.Where(cuad => cuad.CustomerId == salesorder.CustomerId).FirstOrDefault<CustomersContactAddress>();
                if (customercontactaddress != null)
                {
                    szEmail = customercontactaddress.Email;
                }
            }

            IQueryable<SalesOrderDetail> qryalesorderdetails = db.SalesOrderDetails.Where(sodt => sodt.SalesOrderId == id);
            if (qryalesorderdetails.Count() > 0)
            {
                foreach (var item in qryalesorderdetails)
                {
                    if (string.IsNullOrEmpty(szItemID))
                    {
                        szItemID = item.ItemID;
                    }

                    db.SalesOrderDetails.Remove(item);
                }

                //Restore Quotation Details
                //qryQuoteUsr = db.UserQuotations.Where(qtus => qtus.ProductId == szItemID);
                var qryQuoteUsr = db01.UserQuotations.Join(db01.UserRegistrations, usqt => usqt.UserId, usrg => usrg.RId, (usqt, usrg)
                    => new { usqt, usrg }).Where(uqus => uqus.usqt.ProductId == szItemID && uqus.usrg.UserName == szEmail);
                if (qryQuoteUsr.Count() > 0)
                {
                    //userquotation = qryQuoteUsr.FirstOrDefault<UserQuotation>();
                    //if (userquotation != null)
                    //{
                    //    nQuoteId = userquotation.Id;
                    //}

                    foreach (var item in qryQuoteUsr)
                    {
                        //Get the quoteid
                        if (nQuoteId == 0)
                        {
                            nQuoteId = item.usqt.Id;
                        }
                    }

                }

                if (nQuoteId > 0)
                {
                    qryQuoteDetails = db.UserQuotationDetails.Where(qtdt => qtdt.DetailId == nQuoteId);
                    if (qryQuoteDetails.Count() > 0)
                    {
                        foreach (var item in qryQuoteDetails)
                        {
                            item.ShippedQuantity = null;
                            item.BOQuantity = null;
                            item.Status = 0;
                            db.Entry(item).State = EntityState.Modified;

                        }
                    }

                }

                db01.SaveChanges();
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public static void GetInvoiceData(TimelyDepotContext db01, string SalesOrderNo, ref string szShippedDate01, ref string szTrackingNo, ref string szInvoice)
        {
            DateTime dDate = DateTime.Now;
            szShippedDate01 = "";
            szTrackingNo = "";
            szInvoice = "";

            var qryInvoice = db01.Invoices.Join(db01.SalesOrders, invc => invc.SalesOrderNo, slor => slor.SalesOrderNo, (invc, slor)
                => new { invc, slor }).Where(Ndata => Ndata.slor.SalesOrderNo == SalesOrderNo);
            if (qryInvoice.Count() > 0)
            {
                foreach (var item in qryInvoice)
                {
                    szInvoice = item.invc.InvoiceNo;
                    szTrackingNo = item.invc.TrackingNo;
                    if (item.invc.ShipDate != null)
                    {
                        dDate = Convert.ToDateTime(item.invc.ShipDate);
                        szShippedDate01 = dDate.ToShortDateString();
                    }

                    break;
                }
            }

        }
    }
}
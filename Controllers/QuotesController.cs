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
using TimelyDepotMVC.ModelsView;
using PagedList;

namespace TimelyDepotMVC.Controllers
{
    public class QuotesController : Controller
    {
        private TimelyDepotContext db = new TimelyDepotContext();

        int _pageIndex = 0;
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }

        int _pageSize = 20;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        //
        // GET: /Quote/DeleteQuotes
        public ActionResult DeleteQuotes(string searchItem, string ckActive, string ckCriteria, string delete)
        {
            int nQuoteId = 0;
            string[] szDelete = null;

            UserQuotation quotes = null;
            IQueryable<UserQuotationDetail> qryDetails = null;

            if (!string.IsNullOrEmpty(delete))
            {
                szDelete = delete.Split(',');
                if (szDelete != null)
                {
                    foreach (var item in szDelete)
                    {
                        nQuoteId = Convert.ToInt32(item);

                        quotes = db.UserQuotations.Find(nQuoteId);
                        if (quotes != null)
                        {
                            //Delete details
                            qryDetails = db.UserQuotationDetails.Where(qdt => qdt.DetailId == quotes.Id);
                            if (qryDetails != null)
                            {
                                foreach (var itemdet in qryDetails)
                                {
                                    db.UserQuotationDetails.Remove(itemdet);
                                }
                            }
                            db.UserQuotations.Remove(quotes);
                        }
                    }
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index", new { searchItem = searchItem, ckActive = ckActive, ckCriteria = ckCriteria });
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
        // POST: /Quotes/UpdateCustomer
        [HttpPost]
        public ActionResult UpdateCustomer(Customers customer, string CompanyName, string FirtsName, string LastName, string customeremailHlpId)
        {
            bool bError = false;
            string szMsg = "";
            IQueryable<Customers> qryCustomer = null;
            IQueryable<CustomersContactAddress> qryConAdd = null;
            IQueryable<SalesOrder> qrySalesOrder = null;
            CustomersContactAddress contactaddress = null;
            Customers customerHlp = null;
            SalesOrder salesorder = null;

            if (ModelState.IsValid)
            {
                //validate the Customer No
                qryCustomer = db.Customers.Where(cst => cst.CustomerNo == customer.CustomerNo);
                if (qryCustomer.Count() > 0)
                {
                    customerHlp = qryCustomer.FirstOrDefault<Customers>();

                    szMsg = string.Format("The Custmer No: {0} is already assigend", customerHlp.CustomerNo);

                    qryConAdd = db.CustomersContactAddresses.Where(cut => cut.CustomerId == customerHlp.Id);
                    if (qryConAdd.Count() > 0)
                    {
                        contactaddress = qryConAdd.FirstOrDefault<CustomersContactAddress>();
                        if (contactaddress != null)
                        {
                            szMsg = string.Format("The Custmer No: {0} is assigned to {1}", customerHlp.CustomerNo, contactaddress.CompanyName);
                        }
                    }

                    //ModelState.AddModelError("", szMsg);
                    TempData["ErrorCustomer"] = szMsg;
                    bError = true;

                    //Verify the email
                    if (customer.CustomerNo == contactaddress.Email)
                    {
                        bError = false;
                    }

                    //If there is no email, use assign that record
                    if (string.IsNullOrEmpty(contactaddress.Email))
                    {
                        customerHlp.ASINo = customer.ASINo;
                        customerHlp.BussinesSice = customer.BussinesSice;
                        customerHlp.BussinesType = customer.BussinesType;
                        customerHlp.CreditLimit = customer.CreditLimit;
                        customerHlp.DeptoNo = customer.DeptoNo;
                        customerHlp.Origin = customer.Origin;
                        customerHlp.PaymentTerms = customer.PaymentTerms;
                        customerHlp.PPAINo = customer.PPAINo;
                        customerHlp.SageNo = customer.SageNo;
                        customerHlp.SalesPerson = customer.SalesPerson;
                        customerHlp.SellerPermintNo = customer.SellerPermintNo;
                        customerHlp.Status = customer.Status;

                        //Keep the same Customer No
                        customerHlp.CustomerNo = customerHlp.CustomerNo;
                        db.Entry(customerHlp).State = EntityState.Modified;

                        //Update the Customer address data
                        contactaddress.Email = customeremailHlpId;
                        contactaddress.CompanyName = CompanyName;
                        contactaddress.FirstName = FirtsName;
                        contactaddress.LastName = LastName;
                        db.Entry(contactaddress).State = EntityState.Modified;


                        db.SaveChanges();

                        //Update the sales order data
                        qrySalesOrder = db.SalesOrders.Where(slor => slor.CustomerId == customer.Id);
                        if (qrySalesOrder.Count() > 0)
                        {
                            foreach (var item in qrySalesOrder)
                            {
                                salesorder = db.SalesOrders.Find(item.SalesOrderId);
                                if (salesorder != null)
                                {
                                    salesorder.CustomerId = customerHlp.Id;
                                    db.Entry(salesorder).State = EntityState.Modified;
                                }
                            }
                            db.SaveChanges();
                        }

                        //Delete support data
                        DeleteSupport(customer.Id);

                        bError = false;
                    }

                    if (bError)
                    {
                        return RedirectToAction("Index", new { CreateCustomer = true, CustomerId = customer.Id });
                    }
                    return RedirectToAction("Index", new { CreateCustomer = false, CustomerId = customer.Id });

                }

                db.Entry(customer).State = EntityState.Modified;

                qryConAdd = db.CustomersContactAddresses.Where(cut => cut.CustomerId == customer.Id);
                if (qryConAdd.Count() > 0)
                {
                    contactaddress = qryConAdd.FirstOrDefault<CustomersContactAddress>();
                    if (contactaddress != null)
                    {
                        contactaddress.CompanyName = CompanyName;
                        contactaddress.FirstName = FirtsName;
                        contactaddress.LastName = LastName;
                        db.Entry(contactaddress).State = EntityState.Modified;
                    }
                }

                db.SaveChanges();

            }

            return RedirectToAction("Index");
        }

        private void DeleteSupport(int id)
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

        }

        //
        // GET: /Quotes/CompleteCustomerData
        [NoCache]
        public PartialViewResult CompleteCustomerData(int customerId)
        {
            IQueryable<CustomersContactAddress> qryConAdd = null;
            CustomersContactAddress contactaddress = null;

            Customers customer = db.Customers.Find(customerId);
            if (customer != null)
            {
                qryConAdd = db.CustomersContactAddresses.Where(cut => cut.CustomerId == customer.Id);
                if (qryConAdd.Count() > 0)
                {
                    contactaddress = qryConAdd.FirstOrDefault<CustomersContactAddress>();
                    if (contactaddress != null)
                    {
                        ViewBag.CustomerEmail = contactaddress.Email;
                        ViewBag.CompanyName = contactaddress.CompanyName;
                        ViewBag.FirstName = contactaddress.FirstName;
                        ViewBag.LastName = contactaddress.LastName;
                    }
                }
            }

            return PartialView(customer);
        }

        //
        // POST: /Quotes/CreateSalesOrder
        [HttpPost]
        [NoCache]
        public ActionResult CreateSalesOrder(string username, string tradeddl, string customerNoHlp01)
        {
            bool bStatus = false;
            bool bCreateCustomer = false;
            int nCustomerId = 0;
            int nHas = 0;
            int nSalesOrderId = 0;
            string szError = "";
            string[] szDetailsIds = null;

            Customers customer = null;
            CustomersContactAddress contactaddress = null;
            IQueryable<CustomersContactAddress> qryContactAddres = null;

            nHas = Request.Form.Count;

            if (nHas > 0)
            {
                //Get the quotes details ids
                szDetailsIds = Request.Form.AllKeys;

                //Get the CustomerID by the username (email)
                //qryContactAddres = db.CustomersContactAddresses.Where(ctad => ctad.Email == username);
                //if (qryContactAddres.Count() > 0)
                //{
                //    contactaddress = qryContactAddres.FirstOrDefault<CustomersContactAddress>();
                //    if (contactaddress != null)
                //    {
                //        nCustomerId = Convert.ToInt32(contactaddress.CustomerId);
                //    }
                //}
                //else
                //{
                //    //Create the customer records
                //    nCustomerId = CreateCustomerRecords(username);
                //    bCreateCustomer = true;
                //}

                //Fix the email
                customerNoHlp01 = customerNoHlp01.Replace("%40", "@");

                //Get the CustomerID by the CustomerNo
                customer = db.Customers.Where(cust => cust.CustomerNo == customerNoHlp01).FirstOrDefault<Customers>();
                if (customer != null)
                {
                    nCustomerId = customer.Id;
                }

                if (nCustomerId != 0)
                {
                    //Create Sales Order Data
                    bStatus = CreateSalesOrderRecords(nCustomerId, szDetailsIds, tradeddl, ref nSalesOrderId,
                        ref szError);
                }

            }

            return RedirectToAction("Edit", "SalesOrder", new { id = nSalesOrderId });
            //return RedirectToAction("Index", new { CreateCustomer = bCreateCustomer, CustomerId = nCustomerId });
        }

        private bool CreateSalesOrderRecords(int nCustomerId, string[] szDetailsIds, string tradeId, ref int nSalesOrderIdHlp, ref string szError)
        {
            bool bStatus = false;
            int nQuoteid = 0;
            int nItemPos = 1;
            int nSalesOrderId = 0;
            int nQuoteDetailsId = 0;
            int nSubItemid = 0;
            int nPos = -1;
            int nTradeid = 0;
            int nSalesOrderNo = 0;
            double dOnHand = 0;
            double dQtyQuote = 0;
            double dReqQty = 0;
            double dShipQty = 0;
            double dPOQty = 0;
            double dUnitPrice = 0;
            double dTaxRate = 0;
            string szDetailsId = "";
            string szDescription = "";
            string szSalesOrderIdHlp = "";
            string szItemIdHlp = "";

            List<String> quotedetailsList = null;
            IQueryable<SUB_ITEM> qySubItem = null;
            SUB_ITEM subitem = null;
            UserQuotation quotation = null;
            UserQuotationDetail quotationdetail = null;
            SalesOrderDetail salesorderdetail = null;
            InitialInfo initialinfo = null;
            Trade trade = null;
            SetupChargeDetail setupcharge = null;

            try
            {
                if (string.IsNullOrEmpty(tradeId))
                {
                    trade = db.Trades.FirstOrDefault<Trade>();
                    if (trade != null)
                    {
                        nTradeid = trade.TradeId;
                    }
                }
                else
                {
                    nTradeid = Convert.ToInt32(tradeId);
                }

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


                quotedetailsList = szDetailsIds.ToList<string>();

                SalesOrder salesorder = new SalesOrder();
                salesorder.CustomerId = nCustomerId;
                salesorder.SODate = DateTime.Now;
                salesorder.IsBlindShip = false;
                salesorder.TradeId = nTradeid;
                salesorder.SalesOrderNo = nSalesOrderNo.ToString();
                salesorder.Tax_rate = Convert.ToDouble(dTaxRate);
                salesorder.Invs_Tax = Convert.ToDouble(dTaxRate);
                db.SalesOrders.Add(salesorder);
                db.SaveChanges();

                nSalesOrderIdHlp = salesorder.SalesOrderId;

                nSalesOrderId = salesorder.SalesOrderId;

                //
                // Con szDetailsIds leer UserQuotationDetails, luego leer el inventario para establecer las cantidades disponible y
                // luego crear el detalle de la Sales Order, lee el inventario e incluye setup charge y runcharge
                foreach (var item in quotedetailsList)
                {
                    nPos = -1;
                    nPos = item.IndexOf("chk_");
                    if (nPos != -1)
                    {

                        szDetailsId = item.Replace("chk_", "");

                        nQuoteDetailsId = Convert.ToInt16(szDetailsId);
                        quotationdetail = db.UserQuotationDetails.Find(nQuoteDetailsId);
                        if (quotationdetail != null)
                        {
                            if (quotationdetail.Status == 0)
                            {
                                if (nQuoteid == 0)
                                {
                                    nQuoteid = Convert.ToInt32(quotationdetail.DetailId);
                                }

                                dQtyQuote = Convert.ToDouble(quotationdetail.Quantity) - Convert.ToDouble(quotationdetail.ShippedQuantity);
                                dReqQty = Convert.ToDouble(quotationdetail.Quantity);
                                dShipQty = 0;
                                dPOQty = 0;

                                quotation = db.UserQuotations.Find(quotationdetail.DetailId);
                                if (quotation != null)
                                {
                                    subitem = db.SUB_ITEM.Where(sbit => sbit.ItemID == quotation.ProductId && sbit.Description == quotationdetail.ProductType).FirstOrDefault<SUB_ITEM>();

                                }
                                else
                                {
                                    nSubItemid = Convert.ToInt32(quotationdetail.ItemID);
                                    subitem = db.SUB_ITEM.Find(nSubItemid);
                                }

                                if (subitem != null)
                                {
                                    szDescription = subitem.Description;

                                    //Get OnHand inventory
                                    dOnHand = Convert.ToDouble(subitem.OnHand_Db) - Convert.ToDouble(subitem.OnHand_Cr);

                                    if (dQtyQuote >= dOnHand)
                                    {
                                        if (dOnHand == 0)
                                        {
                                            dPOQty = dQtyQuote;
                                        }
                                        else
                                        {
                                            dPOQty = dQtyQuote - dOnHand;
                                            dShipQty = dOnHand;
                                        }
                                    }
                                    else
                                    {
                                    }

                                    //Create sales order detail
                                    salesorderdetail = new SalesOrderDetail();
                                    salesorderdetail.SalesOrderId = nSalesOrderId;
                                    salesorderdetail.ItemID = subitem.ItemID;
                                    salesorderdetail.Sub_ItemID = subitem.Sub_ItemID;
                                    salesorderdetail.Description = szDescription;
                                    salesorderdetail.Quantity = dReqQty;
                                    salesorderdetail.ShipQuantity = dShipQty;
                                    salesorderdetail.BackOrderQuantity = dPOQty;
                                    salesorderdetail.Tax = 0;
                                    salesorderdetail.UnitPrice = quotationdetail.Amount;
                                    salesorderdetail.ItemPosition = nItemPos;
                                    salesorderdetail.ItemOrder = 0;
                                    salesorderdetail.Tax = Convert.ToDouble(dTaxRate);
                                    db.SalesOrderDetails.Add(salesorderdetail);
                                    db.SaveChanges();
                                    nItemPos++;

                                    szSalesOrderIdHlp = salesorderdetail.SalesOrderId.ToString();
                                    szItemIdHlp = salesorderdetail.ItemID;

                                    //Create setup charge
                                    setupcharge = db.SetupChargeDetails.Where(stup => stup.itemid == subitem.ItemID).FirstOrDefault<SetupChargeDetail>();
                                    if (setupcharge != null)
                                    {
                                        salesorderdetail = new SalesOrderDetail();
                                        salesorderdetail.SalesOrderId = nSalesOrderId;
                                        salesorderdetail.ItemID = string.Empty;
                                        salesorderdetail.Sub_ItemID = string.Empty;
                                        salesorderdetail.Description = string.Format("Set up Charge {0} {1}", szSalesOrderIdHlp, szItemIdHlp);
                                        salesorderdetail.Quantity = dReqQty;
                                        salesorderdetail.ShipQuantity = 0;
                                        salesorderdetail.BackOrderQuantity = 0;
                                        salesorderdetail.Tax = 0;
                                        salesorderdetail.UnitPrice = setupcharge.SetUpCharge;
                                        salesorderdetail.ItemPosition = 0;
                                        salesorderdetail.ItemOrder = 0;
                                        salesorderdetail.Tax = Convert.ToDouble(dTaxRate);
                                        db.SalesOrderDetails.Add(salesorderdetail);

                                        //Create run charge
                                        salesorderdetail = new SalesOrderDetail();
                                        salesorderdetail.SalesOrderId = nSalesOrderId;
                                        salesorderdetail.ItemID = string.Empty;
                                        salesorderdetail.Sub_ItemID = string.Empty;
                                        salesorderdetail.Description = string.Format("Run Charge {0} {1}", szSalesOrderIdHlp, szItemIdHlp);
                                        salesorderdetail.Quantity = dReqQty;
                                        salesorderdetail.ShipQuantity = 0;
                                        salesorderdetail.BackOrderQuantity = 0;
                                        salesorderdetail.Tax = 0;
                                        salesorderdetail.UnitPrice = setupcharge.RunCharge;
                                        salesorderdetail.ItemPosition = 0;
                                        salesorderdetail.ItemOrder = 0;
                                        salesorderdetail.Tax = Convert.ToDouble(dTaxRate);
                                        db.SalesOrderDetails.Add(salesorderdetail);

                                    }


                                    //Update Quote Details
                                    quotationdetail.ShippedQuantity = dShipQty;
                                    quotationdetail.BOQuantity = dPOQty;
                                    if (dShipQty >= Convert.ToDouble(quotationdetail.Quantity))
                                    {
                                        quotationdetail.Status = 1;
                                    }
                                    else
                                    {
                                        quotationdetail.Status = 1;
                                    }

                                }
                                else
                                {
                                    //szDescription = quotationdetail.ProductType;

                                    ////Create sales order detail
                                    //salesorderdetail = new SalesOrderDetail();
                                    //salesorderdetail.SalesOrderId = nSalesOrderId;
                                    //salesorderdetail.Description = szDescription;
                                    //salesorderdetail.Quantity = dReqQty;
                                    ////salesorderdetail.ShipQuantity = dShipQty;
                                    ////salesorderdetail.BackOrderQuantity = dPOQty;
                                    //salesorderdetail.Tax = 0;
                                    //salesorderdetail.UnitPrice = quotationdetail.Amount;
                                    //salesorderdetail.ItemPosition = nItemPos;
                                    //salesorderdetail.ItemOrder = 0;
                                    //db.SalesOrderDetails.Add(salesorderdetail);
                                    //nItemPos++;

                                    //quotationdetail.Status = 1;
                                }



                                //Update Sub Item Inventory


                            }
                        }
                    }
                }

                //Update the sales order record with the quoteid
                if (nQuoteid != 0)
                {
                    SalesOrder salesorderHlp = db.SalesOrders.Find(nSalesOrderId);
                    if (salesorderHlp != null)
                    {
                        salesorderHlp.QuoteId = nQuoteid;
                        db.Entry(salesorderHlp).State = EntityState.Modified;
                    }
                }


                //Save all changes
                db.SaveChanges();

                bStatus = true;

            }
            catch (Exception err)
            {
                szError = err.Message;
            }

            return bStatus;
        }

        private int CreateCustomerRecords(string username)
        {
            int nCustomerId = 0;

            Customers customers = new Customers();
            customers.CustomerNo = username;
            customers.BussinesSice = DateTime.Now;

            db.Customers.Add(customers);
            db.SaveChanges();

            nCustomerId = customers.Id;

            CustomersContactAddress contactAddress = new CustomersContactAddress();
            contactAddress.CustomerId = nCustomerId;
            contactAddress.Email = username;
            db.CustomersContactAddresses.Add(contactAddress);
            db.SaveChanges();

            return nCustomerId;
        }

        // Get the ItemID value for the selected itemId Detail
        // Get subitem id
        public static string GetSubItemId01(TimelyDepotContext db01, int ItemId = 0)
        {
            int nItemId = Convert.ToInt32(ItemId);
            string szItemId = "";

            UserQuotation quote = null;
            UserQuotationDetail quotedetail = db01.UserQuotationDetails.Find(nItemId);
            if (quotedetail != null)
            {
                quote = db01.UserQuotations.Find(quotedetail.DetailId);
                if (quote != null)
                {
                    szItemId = quote.ProductId;
                }
            }

            return szItemId;
        }

        // Get the ItemID value for the selected itemId Detail
        // Get subitem id
        public static string GetSubItemId(TimelyDepotContext db01, string ItemId)
        {
            int nItemId = Convert.ToInt32(ItemId);

            string szItemId = "";
            IQueryable<SUB_ITEM> qrysubitem = null;
            SUB_ITEM subitem = null;

            qrysubitem = db01.SUB_ITEM.Where(sbit => sbit.Id == nItemId);
            if (qrysubitem.Count() > 0)
            {
                subitem = qrysubitem.FirstOrDefault<SUB_ITEM>();
                if (subitem != null)
                {
                    szItemId = subitem.ItemID;
                }
            }

            return szItemId;
        }

        //
        // GET: /Quotes/SelectQuote
        [NoCache]
        public PartialViewResult SelectQuote(string username, int? page, int id = 0)
        {
            int pageIndex = 0;
            int pageSize = PageSize;

            //GET details quotes
            IQueryable<UserQuotationDetail> qryQuoteDetail = null;
            IQueryable<Trade> qryTrade = null;

            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();
            List<UserQuotationDetail> detailsList = new List<UserQuotationDetail>();

            qryQuoteDetail = db.UserQuotationDetails.Where(qtdt => qtdt.DetailId == id && qtdt.Status == 0);
            if (qryQuoteDetail.Count() > 0)
            {
                foreach (var item in qryQuoteDetail)
                {
                    detailsList.Add(item);
                }
            }

            ViewBag.userName = username;

            //Set the page
            if (page == null)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = Convert.ToInt32(page);
            }


            var onePageOfData = detailsList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;

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


            return PartialView(detailsList.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /Quotes/SearchUserRegistration
        public PartialViewResult SearchUserRegistration(int? page, string username)
        {

            int pageIndex = 0;
            int pageSize = PageSize;

            IQueryable<UserRegistration> qryUserReg = null;

            List<UserRegistration> UserRegList = new List<UserRegistration>();

            qryUserReg = db.UserRegistrations.OrderBy(usrg => usrg.UserName);
            if (qryUserReg.Count() > 0)
            {
                foreach (var item in qryUserReg)
                {
                    UserRegList.Add(item);
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


            var onePageOfData = UserRegList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;

            return PartialView(UserRegList.ToPagedList(pageIndex, pageSize));
        }

        //
        // CountDetails
        public static string CountDetails(TimelyDepotContext db, int DetailId, string szItemId, string szDescription)
        {
            bool bStatus = false;
            int nIdSubItem = 0;
            string szHas = "0";
            UserQuotationDetail userquotationdetail = null;
            SUB_ITEM subitem = null;

            TimelyDepotContext db02 = new TimelyDepotContext();

            IQueryable<SUB_ITEM> qrySubItem = null;
            IQueryable<UserQuotationDetail> qryUsrDet = db.UserQuotationDetails.Where(usdt => usdt.DetailId == DetailId);

            if (qryUsrDet.Count() > 0)
            {
                szHas = qryUsrDet.Count().ToString();
                foreach (var item in qryUsrDet)
                {
                    userquotationdetail = db.UserQuotationDetails.Find(item.Id);
                    if (userquotationdetail != null)
                    {
                        qrySubItem = db02.SUB_ITEM.Where(sbit => sbit.ItemID == szItemId && sbit.Description == userquotationdetail.ProductType);
                        if (qrySubItem.Count() > 0)
                        {
                            subitem = qrySubItem.FirstOrDefault<SUB_ITEM>();
                            if (subitem != null)
                            {
                                nIdSubItem = subitem.Id;
                            }
                        }
                        else
                        {
                            nIdSubItem = 0;
                        }

                        if (string.IsNullOrEmpty(userquotationdetail.ItemID))
                        {
                            userquotationdetail.ItemID = nIdSubItem.ToString();
                            bStatus = true;
                        }
                    }
                }
                if (bStatus)
                {
                    db.SaveChanges();

                }
            }


            return szHas;
        }

        //
        // Get user name
        public static string GetUserRegistrationName(TimelyDepotContext db, int Id)
        {
            string szName = "";

            UserRegistration userregistration = db.UserRegistrations.Find(Id);
            if (userregistration != null)
            {
                szName = userregistration.UserName;
            }

            return szName;
        }

        //
        // GET: /Quotes/
        [NoCache]
        public ActionResult Index(int? page, string searchName, bool? CreateCustomer, int? CustomerId, string searchItem, string ckActive, string ckCriteria)
        {
            bool bHasQuote = false;
            int nStatus = 0;
            int nYear = 0;
            int nMonth = 0;
            int nDay = 0;
            int pageIndex = 0;
            int pageSize = PageSize;
            string[] szSearch = new string[1];
            string[] szFecha = null;
            DateTime dFecha = DateTime.Now;

            //Used for Testing only
            //ViewBag.CreateCustomer = "True";
            //ViewBag.CustomerId = "8004";

            UserQuotation userquotation = null;
            List<UserQuotation> UserQuotationList = new List<UserQuotation>();

            if (string.IsNullOrEmpty(searchItem) || searchItem == "0")
            {
                dFecha = dFecha.AddMonths(-1);
                //qryItem = db.ITEMs.OrderBy(it => it.ItemID);
                ViewBag.ckActiveHlp = "true";
                ViewBag.ckCriteriaHlp = "date";
                ViewBag.CurrentDate = dFecha.ToString("yyyy/MM/dd");
                if (searchItem == "0")
                {
                    ViewBag.SearchItem = searchItem;

                    if (ckCriteria == "date")
                    {
                        if (ckActive == "true")
                        {
                            nStatus = 1;

                            var qryQuotes = db.UserQuotations.Where(quote => quote.Invoicestatus == nStatus).OrderByDescending(quote => quote.Date);
                            foreach (var item in qryQuotes)
                            {
                                UserQuotationList.Add(item);
                            }
                        }
                        else
                        {
                            nStatus = 0;

                            var qryQuotes = db.UserQuotations.Where(quote => quote.Invoicestatus == nStatus).OrderByDescending(quote => quote.Date);
                            foreach (var item in qryQuotes)
                            {
                                UserQuotationList.Add(item);
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

                if (ckActive == "true")
                {
                    nStatus = 1;
                }
                else
                {
                    nStatus = 0;
                }

                if (ckCriteria == "date")
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
                        var qryQuotes = db.UserQuotations.Where(quote => quote.Date > dFecha && quote.Invoicestatus == nStatus).OrderByDescending(quote => quote.Date);
                        foreach (var item in qryQuotes)
                        {
                            UserQuotationList.Add(item);
                        }
                    }
                    else
                    {
                        var qryQuotes = db.UserQuotations.Where(quote => quote.Date > dFecha && quote.Invoicestatus == nStatus).OrderByDescending(quote => quote.Date);
                        foreach (var item in qryQuotes)
                        {
                            UserQuotationList.Add(item);
                        }
                    }
                    bHasQuote = true;
                }

                if (ckCriteria == "username")
                {
                    if (ckActive == "true")
                    {
                        var qryQuotes = db.UserRegistrations.Join(db.UserQuotations, reguser => reguser.RId, quotes => quotes.UserId, (reguser, quotes)
                            => new { reguser, quotes }).Where(rgqt => rgqt.reguser.UserName.StartsWith(searchItem) && rgqt.quotes.Invoicestatus == nStatus).OrderByDescending(rgqt => rgqt.quotes.Date).ThenBy(rgqt => rgqt.reguser.UserName);

                        if (qryQuotes.Count() > 0)
                        {
                            foreach (var item in qryQuotes)
                            {
                                //UserQuotationList.Add(item);
                                userquotation = item.quotes;
                                UserQuotationList.Add(userquotation);
                            }
                        }
                    }
                    else
                    {
                        var qryQuotes = db.UserRegistrations.Join(db.UserQuotations, reguser => reguser.RId, quotes => quotes.UserId, (reguser, quotes)
                            => new { reguser, quotes }).Where(rgqt => rgqt.reguser.UserName.StartsWith(searchItem) && rgqt.quotes.Invoicestatus == nStatus).OrderByDescending(rgqt => rgqt.quotes.Date).ThenBy(rgqt => rgqt.reguser.UserName);

                        if (qryQuotes.Count() > 0)
                        {
                            foreach (var item in qryQuotes)
                            {
                                //UserQuotationList.Add(item);
                                userquotation = item.quotes;
                                UserQuotationList.Add(userquotation);
                            }
                        }
                    }
                    bHasQuote = true;
                }
                if (ckCriteria == "itemno")
                {
                    if (ckActive == "true")
                    {
                        var qryQuotes = db.UserRegistrations.Join(db.UserQuotations, reguser => reguser.RId, quotes => quotes.UserId, (reguser, quotes)
                            => new { reguser, quotes }).Where(rgqt => rgqt.quotes.ProductId.StartsWith(searchItem) && rgqt.quotes.Invoicestatus == nStatus).OrderByDescending(rgqt => rgqt.quotes.Date).ThenBy(rgqt => rgqt.reguser.UserName);

                        if (qryQuotes.Count() > 0)
                        {
                            foreach (var item in qryQuotes)
                            {
                                //UserQuotationList.Add(item);
                                userquotation = item.quotes;
                                UserQuotationList.Add(userquotation);
                            }
                        }
                    }
                    else
                    {
                        var qryQuotes = db.UserRegistrations.Join(db.UserQuotations, reguser => reguser.RId, quotes => quotes.UserId, (reguser, quotes)
                            => new { reguser, quotes }).Where(rgqt => rgqt.quotes.ProductId.StartsWith(searchItem) && rgqt.quotes.Invoicestatus == nStatus).OrderByDescending(rgqt => rgqt.quotes.Date).ThenBy(rgqt => rgqt.reguser.UserName);

                        if (qryQuotes.Count() > 0)
                        {
                            foreach (var item in qryQuotes)
                            {
                                //UserQuotationList.Add(item);
                                userquotation = item.quotes;
                                UserQuotationList.Add(userquotation);
                            }
                        }
                    }
                    bHasQuote = true;
                }


            }

            //if (CreateCustomer != null)
            //{
            //    ViewBag.CreateCustomer = CreateCustomer;
            //    ViewBag.CustomerId = CustomerId;
            //}

            if (TempData["ErrorCustomer"] != null)
            {
                ViewBag.ErrorCustomer = TempData["ErrorCustomer"].ToString();
            }

            szSearch[0] = searchName;

            //IQueryable<Vendors> qryVendors = null;


            //qryVendors = db.Vendors.OrderBy(vd => vd.VendorNo);

            //if (string.IsNullOrEmpty(searchName))
            //{
            //    var qryQuotes = db.UserRegistrations.Join(db.UserQuotations, reguser => reguser.RId, quotes => quotes.UserId, (reguser, quotes)
            //        => new { reguser, quotes }).OrderByDescending(rgqt => rgqt.quotes.Date).ThenBy(rgqt => rgqt.reguser.UserName);
            //    if (qryQuotes.Count() > 0)
            //    {
            //        foreach (var item in qryQuotes)
            //        {
            //            //UserQuotationList.Add(item);
            //            userquotation = item.quotes;
            //            UserQuotationList.Add(userquotation);
            //        }
            //    }
            //}
            //else
            //{
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


            var onePageOfData = UserQuotationList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(UserQuotationList.ToPagedList(pageIndex, pageSize));
            //return View(db.UserQuotations.ToList());
        }

        //
        // GET: /Quotes/Details/5

        public ActionResult Details(int id = 0)
        {
            UserQuotation userquotation = db.UserQuotations.Find(id);
            if (userquotation == null)
            {
                return HttpNotFound();
            }
            return View(userquotation);
        }

        //
        // GET: /Quotes/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Quotes/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserQuotation userquotation)
        {
            if (ModelState.IsValid)
            {
                db.UserQuotations.Add(userquotation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userquotation);
        }

        //
        // GET: /Quotes/Edit/5

        public ActionResult Edit(int id = 0)
        {
            UserQuotation userquotation = db.UserQuotations.Find(id);
            if (userquotation == null)
            {
                return HttpNotFound();
            }
            return View(userquotation);
        }

        //
        // POST: /Quotes/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserQuotation userquotation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userquotation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userquotation);
        }

        //
        // GET: /Quotes/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UserQuotation userquotation = db.UserQuotations.Find(id);
            if (userquotation == null)
            {
                return HttpNotFound();
            }
            return View(userquotation);
        }

        //
        // POST: /Quotes/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserQuotation userquotation = db.UserQuotations.Find(id);
            db.UserQuotations.Remove(userquotation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public static string GetSalesOrderNo(TimelyDepotContext db01, int qutoeId)
        {
            string szSalesOrderNo = "";

            SalesOrder salesorder = db01.SalesOrders.Where(slod => slod.QuoteId == qutoeId).FirstOrDefault<SalesOrder>();
            if (salesorder != null)
            {
                szSalesOrderNo = salesorder.SalesOrderNo;
            }


            return szSalesOrderNo;
        }
    }
}
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
using System.IO;
using TimelyDepotMVC.PDFReporting;
using TimelyDepotMVC.Controllers.PDFReporting;
using TimelyDepotMVC.ModelsView;

namespace TimelyDepotMVC.Controllers
{
    public class PurchaseOrderController : Controller
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
        // GET: /Invoice/UpdateDetail
        public ActionResult UpdateDetail(int? id, string salesorderid, string qty, string shipqty, string boqty, string desc, string price, string tax,
            string logo, string imprt, string qtysc, string qtyrc, string pricesc, string pricerc)
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

            PurchasOrderDetail sodetail = db.PurchasOrderDetails.Find(id);
            PurchasOrderDetail setupcharge = null;
            PurchasOrderDetail runcharge = null;


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
                //if (!string.IsNullOrEmpty(shipqty))
                //{
                //    dHlp = Convert.ToDouble(shipqty);
                //    sodetail.ShipQuantity = dHlp;
                //}
                //if (!string.IsNullOrEmpty(boqty))
                //{
                //    dHlp = Convert.ToDouble(boqty);
                //    sodetail.BackOrderQuantity = dHlp;
                //}
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
                    szSalesOredidHlp = string.Format("Set up Charge {0} {1}", sodetail.PurchaseOrderId.ToString(), sodetail.ItemID);
                    setupcharge = db.PurchasOrderDetails.Where(spch => spch.PurchaseOrderId == sodetail.PurchaseOrderId && spch.Description == szSalesOredidHlp).FirstOrDefault<PurchasOrderDetail>();
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

                        setupcharge = new PurchasOrderDetail();
                        setupcharge.PurchaseOrderId = nSalesOrderId;
                        setupcharge.ItemID = string.Empty;
                        setupcharge.Sub_ItemID = string.Empty;
                        setupcharge.Description = string.Format("Set up Charge {0} {1}", sodetail.PurchaseOrderId.ToString(), sodetail.ItemID);
                        setupcharge.Quantity = Convert.ToDouble(dcHlp1);
                        //setupcharge.ShipQuantity = 0;
                        //setupcharge.BackOrderQuantity = 0;
                        setupcharge.Tax = 0;
                        setupcharge.UnitPrice = dcHlp;
                        setupcharge.ItemPosition = 0;
                        setupcharge.ItemOrder = 0;
                        setupcharge.Tax = Convert.ToDouble(dTaxRate);
                        db.PurchasOrderDetails.Add(setupcharge);
                        db.SaveChanges();
                    }
                }

                //Update Run Charge
                if (!string.IsNullOrEmpty(pricesc) && !string.IsNullOrEmpty(qtysc))
                {
                    szSalesOredidHlp = string.Format("Run Charge {0} {1}", sodetail.PurchaseOrderId.ToString(), sodetail.ItemID);
                    runcharge = db.PurchasOrderDetails.Where(spch => spch.PurchaseOrderId == sodetail.PurchaseOrderId && spch.Description == szSalesOredidHlp).FirstOrDefault<PurchasOrderDetail>();
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

                        runcharge = new PurchasOrderDetail();
                        runcharge.PurchaseOrderId = nSalesOrderId;
                        runcharge.ItemID = string.Empty;
                        runcharge.Sub_ItemID = string.Empty;
                        runcharge.Description = string.Format("Run Charge {0} {1}", sodetail.PurchaseOrderId.ToString(), sodetail.ItemID);
                        runcharge.Quantity = Convert.ToDouble(dcHlp1);
                        //runcharge.ShipQuantity = 0;
                        //runcharge.BackOrderQuantity = 0;
                        runcharge.Tax = 0;
                        runcharge.UnitPrice = dcHlp;
                        runcharge.ItemPosition = 0;
                        runcharge.ItemOrder = 0;
                        runcharge.Tax = Convert.ToDouble(dTaxRate);
                        db.PurchasOrderDetails.Add(runcharge);
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Edit", new { id = nSalesOrderId });
        }


        //
        // GET: /PutchaseOrder/PurhaseOrderListExcel
        public ActionResult PurhaseOrderListExcel()
        {
            //DataTable hlpTbl = GetCustomerListTable();

            ExportCSV(GetPurchaseOrderTable(), "PurchaseOrderList");

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

        private DataTable GetPurchaseOrderTable()
        {
            long telHlp = 0;
            long faxHlp = 0;
            string telfmt = "000-000-0000";
            string szTel = "";

            TimelyDepotContext db01 = new TimelyDepotContext();

            DataTable table = null;
            DataRow row = null;

            PurchaseOrderList thepurchaseorderlist = null;
            List<PurchaseOrderList> purchaseList = new List<PurchaseOrderList>();

            var qryPurchaseOrder = db.Vendors.Join(db.PurchaseOrders, vdad => vdad.VendorNo, prod => prod.VendorId, (vdad, prod)
                => new { vdad, prod }).OrderBy(vdod => vdod.prod.PurchaseOrderNo);
            if (qryPurchaseOrder.Count() > 0)
            {
                foreach (var item in qryPurchaseOrder)
                {
                    thepurchaseorderlist = new PurchaseOrderList();
                    thepurchaseorderlist.PurchaseOrderId = item.prod.PurchaseOrderId;
                    thepurchaseorderlist.PurchaseOrderNo = item.prod.PurchaseOrderNo;
                    thepurchaseorderlist.SODate = item.prod.PODate;
                    thepurchaseorderlist.VendorNo = item.prod.VendorId;
                    //thepurchaseorderlist.VendorNo = GetCustomerDataSO(db01, item.ctad.VendorId.ToString());
                    thepurchaseorderlist.CompanyName = GetVendorData01(db01, item.prod.VendorId);
                    thepurchaseorderlist.SalesOrderNo = item.prod.SalesOrderNo;
                    thepurchaseorderlist.ShipDate = item.prod.ShipDate;
                    thepurchaseorderlist.PaymentAmount = (double?)this.GetTotalPO01(db01, item.prod.PurchaseOrderId);

                    purchaseList.Add(thepurchaseorderlist);
                }
            }

            table = new DataTable("SalesOrderList");

            // Set the header
            DataColumn col01 = new DataColumn("PurchaseOrderNo", System.Type.GetType("System.String"));
            DataColumn col02 = new DataColumn("Date", System.Type.GetType("System.String"));
            DataColumn col03 = new DataColumn("VendorNo", System.Type.GetType("System.String"));
            DataColumn col04 = new DataColumn("VendorCompanyName", System.Type.GetType("System.String"));
            DataColumn col05 = new DataColumn("SalesOrderNo", System.Type.GetType("System.String"));
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
            foreach (var item in purchaseList)
            {
                row = table.NewRow();
                row["PurchaseOrderNo"] = item.PurchaseOrderNo;
                row["Date"] = item.SODate;
                row["VendorNo"] = item.VendorNo;
                row["VendorCompanyName"] = item.CompanyName;
                row["SalesOrderNo"] = item.SalesOrderNo;
                row["ShipDate"] = item.ShipDate;
                row["Amount"] = item.PaymentAmount;
                table.Rows.Add(row);
            }

            return table;
        }

        //
        // GET: /PurchaseOrder/PurchaseOrderList
        [NoCache]
        public PartialViewResult PurchaseOrderList(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;

            TimelyDepotContext db01 = new TimelyDepotContext();

            PurchaseOrderList thepurchaseorderlist = null;
            List<PurchaseOrderList> customerList = new List<PurchaseOrderList>();

            var qryPurchaseOrder = db.Vendors.Join(db.PurchaseOrders, vdad => vdad.VendorNo, prod => prod.VendorId, (vdad, prod)
                => new { vdad, prod }).OrderBy(vdod => vdod.prod.PurchaseOrderNo);
            if (qryPurchaseOrder.Count() > 0)
            {
                foreach (var item in qryPurchaseOrder)
                {
                    thepurchaseorderlist = new PurchaseOrderList();
                    thepurchaseorderlist.PurchaseOrderId = item.prod.PurchaseOrderId;
                    thepurchaseorderlist.PurchaseOrderNo = item.prod.PurchaseOrderNo;
                    thepurchaseorderlist.SODate = item.prod.PODate;
                    thepurchaseorderlist.VendorNo = item.prod.VendorId;
                    //thepurchaseorderlist.VendorNo = GetCustomerDataSO(db01, item.ctad.VendorId.ToString());
                    thepurchaseorderlist.CompanyName = GetVendorData01(db01, item.prod.VendorId);
                    thepurchaseorderlist.SalesOrderNo = item.prod.SalesOrderNo;
                    thepurchaseorderlist.ShipDate = item.prod.ShipDate;
                    thepurchaseorderlist.PaymentAmount = (double?)this.GetTotalPO01(db01, item.prod.PurchaseOrderId);

                    customerList.Add(thepurchaseorderlist);
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

        private string GetVendorData01(TimelyDepotContext db01, string szVendorNo)
        {
            string szCompanyName = "";

            var qryvendordata = db01.VendorsContactAddresses.Join(db01.Vendors, vdad => vdad.VendorId, vnd => vnd.Id, (vdad, vnd)
                => new { vdad, vnd }).Where(vddt => vddt.vnd.VendorNo == szVendorNo);
            if (qryvendordata.Count() > 0)
            {
                foreach (var item in qryvendordata)
                {
                    szCompanyName = item.vdad.CompanyName;
                    break;
                }
            }

            return szCompanyName;
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
        private decimal GetSalesOrderAmount(TimelyDepotContext db01, int nSalesOrderId)
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
        private void GetSalesOrderTotals01(TimelyDepotContext db01, int nSalesOrderId, ref double dSalesAmount, ref double dTotalTax, ref double dTax, ref double dTotalAmount, ref double dBalanceDue)
        {
            double dShipping = 0;
            double dPayment = 0;

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


            SalesOrder salesorder = db01.SalesOrders.Find(nSalesOrderId);
            if (salesorder != null)
            {
                dShipping = Convert.ToDouble(salesorder.ShippingHandling);
                dPayment = Convert.ToDouble(salesorder.PaymentAmount);

                qryDetails = db01.SalesOrderDetails.Where(sldt => sldt.SalesOrderId == salesorder.SalesOrderId);
                if (qryDetails.Count() > 0)
                {
                    foreach (var item in qryDetails)
                    {
                        dSalesAmount = dSalesAmount + (Convert.ToDouble(item.Quantity) * Convert.ToDouble(item.UnitPrice));
                        dTotalTax = dTotalTax + (Convert.ToDouble(item.Quantity) * Convert.ToDouble(item.UnitPrice) * (dTax / 100));
                    }
                }

                dTotalAmount = dSalesAmount + dTotalTax + dShipping;
                dBalanceDue = dTotalAmount - dPayment;
            }
        }

        //
        // GET: /PurchaseOrder/POReport
        public ActionResult POReport(int id = 0)
        {
            int purchaseorderId = id;
            string szOutputFilePath = "";

            //Verify the required folder
            string szReportFolderPath = string.Format("~/Pdf");
            szReportFolderPath = Server.MapPath(szReportFolderPath);
            if (!Directory.Exists(szReportFolderPath))
            {
                Directory.CreateDirectory(szReportFolderPath);
            }

            var rpt = new PurchaseOrderReport().CreateReport(purchaseorderId);
            szOutputFilePath = rpt.FileName.Replace(HttpRuntime.AppDomainAppPath, string.Empty);

            return Redirect(szOutputFilePath);
        }

        //
        // GET: /PurchaseOrder/InsertItem
        public ActionResult InsertItem(string purchaseorderid, string itemOrder, int itemPos = 0)
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
            PurchasOrderDetail salesdetailcurrent = null;
            PurchasOrderDetail salesdetailnext = null;

            int nSalesOrderId = Convert.ToInt32(purchaseorderid);


            if (!string.IsNullOrEmpty(itemOrder))
            {
                //itemOrder = itemOrder.Replace(".", ",");
                nItemOrder = Convert.ToDouble(itemOrder);
            }

            //Get the current salesorderdetail
            salesdetailcurrent = db.PurchasOrderDetails.Where(sldt => sldt.PurchaseOrderId == nSalesOrderId && sldt.ItemPosition == itemPos && sldt.ItemOrder == nItemOrder).FirstOrDefault<PurchasOrderDetail>();
            if (salesdetailcurrent != null)
            {
                nCurrentItemPos = Convert.ToInt32(salesdetailcurrent.ItemPosition);
                dCurrentItemOrder = Convert.ToDouble(salesdetailcurrent.ItemOrder);
                szCurentItemId = salesdetailcurrent.ItemID;
            }

            //Get the next salesorderdetail
            salesdetailnext = db.PurchasOrderDetails.Where(sldt => sldt.PurchaseOrderId == nSalesOrderId && sldt.ItemPosition == nCurrentItemPos && sldt.ItemOrder > dCurrentItemOrder).OrderBy(sldt => sldt.ItemOrder).FirstOrDefault<PurchasOrderDetail>();
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



            PurchasOrderDetail salesdetail = null;

            salesdetail = new PurchasOrderDetail();
            salesdetail.PurchaseOrderId = nSalesOrderId;
            salesdetail.ItemID = string.Empty;
            salesdetail.Sub_ItemID = string.Empty;
            //salesdetail.BackOrderQuantity = 0;
            salesdetail.Description = string.Empty;
            salesdetail.Quantity = 0;
            //salesdetail.ShipQuantity = 0;
            salesdetail.Tax = 0;
            salesdetail.UnitPrice = 0;
            salesdetail.ItemPosition = nItemPos;
            salesdetail.ItemOrder = dItemOrder;
            db.PurchasOrderDetails.Add(salesdetail);
            db.SaveChanges();

            return RedirectToAction("Edit", new { id = nSalesOrderId });
        }

        //
        // GET: /PurchaseOrder/DeleteDetail
        public ActionResult DeleteDetail(int id = 0)
        {
            int salesorderId = 0;

            PurchasOrderDetail salesdetail = db.PurchasOrderDetails.Find(id);
            if (salesdetail != null)
            {
                salesorderId = Convert.ToInt32(salesdetail.PurchaseOrderId);
                db.PurchasOrderDetails.Remove(salesdetail);
                db.SaveChanges();
            }

            return RedirectToAction("Edit", new { id = salesorderId });
        }

        //
        // GET: /PurchaseOrder/AddPurchaseOrderDetails
        [NoCache]
        public ActionResult AddPurchaseOrderDetails(string itemOrder, int id = 0, int purchaseorderid = 0, int itemPos = 0)
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
            PurchasOrderDetail salesdetail = null;
            PurchasOrderDetail salesdetailcurrent = null;
            PurchasOrderDetail salesdetailnext = null;

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
                salesdetailcurrent = db.PurchasOrderDetails.Where(sldt => sldt.PurchaseOrderId == purchaseorderid && sldt.ItemPosition == itemPos && sldt.ItemOrder == nItemOrder).FirstOrDefault<PurchasOrderDetail>();
                if (salesdetailcurrent != null)
                {
                    nCurrentItemPos = Convert.ToInt32(salesdetailcurrent.ItemPosition);
                    dCurrentItemOrder = Convert.ToDouble(salesdetailcurrent.ItemOrder);
                    szCurentItemId = salesdetailcurrent.ItemID;
                }

                //Get the next salesorderdetail
                salesdetailnext = db.PurchasOrderDetails.Where(sldt => sldt.PurchaseOrderId == purchaseorderid && sldt.ItemPosition == nCurrentItemPos && sldt.ItemOrder > dCurrentItemOrder).OrderBy(sldt => sldt.ItemOrder).FirstOrDefault<PurchasOrderDetail>();
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


                salesdetail = new PurchasOrderDetail();
                salesdetail.PurchaseOrderId = purchaseorderid;
                salesdetail.ItemID = subitem.ItemID;
                salesdetail.Sub_ItemID = subitem.Sub_ItemID;
                //salesdetail.BackOrderQuantity = 0;
                salesdetail.Description = subitem.Description;
                salesdetail.Quantity = dQty;
                //salesdetail.ShipQuantity = 0;
                salesdetail.Tax = 0;
                salesdetail.UnitPrice = dPrice * (1 - Convert.ToDecimal(dDiscountPrc));
                salesdetail.ItemPosition = nItemPos;
                salesdetail.ItemOrder = dItemOrder;
                db.PurchasOrderDetails.Add(salesdetail);
                db.SaveChanges();
            }

            return RedirectToAction("Edit", new { id = purchaseorderid });
        }


        //
        // GET: /PurchaseOrder/AddDetail
        [NoCache]
        public PartialViewResult AddDetail(string searchitem, string itemOrder, int? page, int purchaseorderid = 0, int itemPos = 0)
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

            ViewBag.PurchaseOrderId = purchaseorderid;
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
        // GET: /PurchaseOrder/ReceivePurchaseOrder
        public ActionResult ReceivePurchaseOrder(int id)
        {
            double dPO_Dr = 0;
            DateTime dDate = DateTime.Now;
            TimelyDepotContext db01 = new TimelyDepotContext();

            SUB_ITEM subitem = null;
            IQueryable<PurchasOrderDetail> qrypodetails = null;

            PurchaseOrders purchaseorders = db.PurchaseOrders.Find(id);
            if (string.IsNullOrEmpty(purchaseorders.ReceiveStatus))
            {
                //Upate inventory
                qrypodetails = db.PurchasOrderDetails.Where(podt => podt.PurchaseOrderId == id);
                if (qrypodetails.Count() > 0)
                {
                    foreach (var item in qrypodetails)
                    {
                        if (!string.IsNullOrEmpty(item.ItemID))
                        {
                            subitem = db01.SUB_ITEM.Where(sbit => sbit.ItemID == item.ItemID && sbit.Sub_ItemID == item.Sub_ItemID).FirstOrDefault<SUB_ITEM>();
                            if (subitem != null)
                            {
                                dPO_Dr = Convert.ToDouble(item.Quantity);
                                subitem.OpenPO_Db = subitem.OpenPO_Db + dPO_Dr;
                                db01.Entry(subitem).State = EntityState.Modified;
                            }

                        }
                    }
                    db01.SaveChanges();
                }


                purchaseorders.ReceiveStatus = string.Format("Received on {0}", dDate.ToString("MMM/dd/yyyy"));

                db.Entry(purchaseorders).State = EntityState.Modified;
                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        //
        // GET: /PutchaseOrder/EditDetail
        public ActionResult EditDetail(string qty, string price, string description, string subitem, string reference, int id = 0, int purchaseorderid = 0)
        {
            double dHlp = 0;
            decimal decHlp = 0;

            PurchasOrderDetail podetail = db.PurchasOrderDetails.Where(podt => podt.PurchaseOrderId == purchaseorderid && podt.Id == id).FirstOrDefault<PurchasOrderDetail>();
            if (podetail != null)
            {
                dHlp = Convert.ToDouble(qty);
                podetail.Quantity = dHlp;

                price = price.Replace("$", "");
                price = price.Replace(",", "");

                decHlp = Convert.ToDecimal(price);
                podetail.UnitPrice = decHlp;

                if (!string.IsNullOrEmpty(description))
                {
                    podetail.Description = description;
                }
                if (!string.IsNullOrEmpty(subitem))
                {
                    podetail.Sub_ItemID = subitem;
                }
                if (!string.IsNullOrEmpty(reference))
                {
                    podetail.VendorReference = reference;
                }

                db.Entry(podetail).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Edit", new { id = purchaseorderid });
        }

        //
        // GET: /Invoice/GetSalesDetailsSRC
        [NoCache]
        public PartialViewResult GetSalesDetailsSRC(int? page, string purchaseorderid)
        {
            decimal dTotal = 0;

            TimelyDepotContext db01 = new TimelyDepotContext();

            int pageIndex = 0;
            int pageSize = PageSize;
            pageSize = 10;
            int nSalesOrderid = Convert.ToInt32(purchaseorderid);
            string szSalesOredidHlp = "";

            IQueryable<PurchasOrderDetail> qrysalesdetails = null;
            IQueryable<ImprintMethods> qryImprint = null;

            PurchasOrderDetail setupcharge = null;
            PurchasOrderDetail runcharge = null;
            PurchaseOrderDetailSRC salesorderdetailSRC = null;

            List<PurchaseOrderDetailSRC> salesdetailsList = new List<PurchaseOrderDetailSRC>();
            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            //qrysalesdetails = db.SalesOrderDetails.Where(sldt => sldt.SalesOrderId == nSalesOrderid).OrderBy(sldt => sldt.Sub_ItemID).ThenBy(sldt => sldt.ItemOrder);
            qrysalesdetails = db.PurchasOrderDetails.Where(sldt => sldt.PurchaseOrderId == nSalesOrderid && sldt.ItemID != "").OrderBy(sldt => sldt.ItemPosition).ThenBy(sldt => sldt.ItemOrder);
            if (qrysalesdetails.Count() > 0)
            {
                foreach (var item in qrysalesdetails)
                {
                    salesorderdetailSRC = new PurchaseOrderDetailSRC();
                    salesorderdetailSRC.BackOrderQuantity = 0;
                    salesorderdetailSRC.Description = item.Description;
                    salesorderdetailSRC.Id = item.Id;
                    salesorderdetailSRC.ImprintMethod = item.ImprintMethod;
                    salesorderdetailSRC.ItemID = item.ItemID;
                    salesorderdetailSRC.ItemOrder = item.ItemOrder;
                    salesorderdetailSRC.ItemPosition = item.ItemPosition;
                    salesorderdetailSRC.Logo = item.Logo;
                    salesorderdetailSRC.Quantity = item.Quantity;
                    salesorderdetailSRC.SalesOrderId = item.PurchaseOrderId;
                    salesorderdetailSRC.ShipQuantity = 0;
                    salesorderdetailSRC.Sub_ItemID = item.Sub_ItemID;
                    salesorderdetailSRC.Tax = item.Tax;
                    salesorderdetailSRC.UnitPrice = item.UnitPrice;
                    salesorderdetailSRC.QuantitySC = 0;
                    salesorderdetailSRC.UnitPricSRC = 0;
                    salesorderdetailSRC.QuantityRC = 0;
                    salesorderdetailSRC.UnitPriceRC = 0;

                    //Set Up Charge
                    szSalesOredidHlp = string.Format("Set up Charge {0} {1}", salesorderdetailSRC.SalesOrderId.ToString(), salesorderdetailSRC.ItemID);
                    setupcharge = db01.PurchasOrderDetails.Where(stup => stup.PurchaseOrderId == item.PurchaseOrderId && stup.Description == szSalesOredidHlp).FirstOrDefault<PurchasOrderDetail>();
                    if (setupcharge != null)
                    {
                        salesorderdetailSRC.QuantitySC = setupcharge.Quantity;
                        salesorderdetailSRC.UnitPricSRC = setupcharge.UnitPrice;
                    }
                    //Run Charge
                    szSalesOredidHlp = string.Format("Run Charge {0} {1}", salesorderdetailSRC.SalesOrderId.ToString(), salesorderdetailSRC.ItemID);
                    runcharge = db01.PurchasOrderDetails.Where(stup => stup.PurchaseOrderId == item.PurchaseOrderId && stup.Description == szSalesOredidHlp).FirstOrDefault<PurchasOrderDetail>();
                    if (runcharge != null)
                    {
                        salesorderdetailSRC.QuantityRC = runcharge.Quantity;
                        salesorderdetailSRC.UnitPriceRC = runcharge.UnitPrice;
                    }


                    salesdetailsList.Add(salesorderdetailSRC);
                }
            }
            ViewBag.SalesOrderId = purchaseorderid;

            //Get the total
            dTotal = GetTotalPO(nSalesOrderid);
            ViewBag.TotalPO = dTotal.ToString("C");


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
        // GET:/PutchaseOrder/GetPODetail
        [NoCache]
        public PartialViewResult GetPODetail(int? page, int purchaseorderid = 0)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            decimal dTotal = 0;

            IQueryable<PurchasOrderDetail> qryPOdetail = null;

            List<PurchasOrderDetail> podetailList = new List<PurchasOrderDetail>();

            qryPOdetail = db.PurchasOrderDetails.Where(podt => podt.PurchaseOrderId == purchaseorderid).OrderBy(podt => podt.ItemPosition).ThenBy(podt => podt.ItemOrder);
            if (qryPOdetail.Count() > 0)
            {
                foreach (var item in qryPOdetail)
                {
                    podetailList.Add(item);
                }
            }

            //Get the total
            dTotal = GetTotalPO(purchaseorderid);
            ViewBag.TotalPO = dTotal.ToString("C");
            ViewBag.PurchaseOrderId = purchaseorderid;

            //Set the page
            if (page == null)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = Convert.ToInt32(page);
            }


            var onePageOfData = podetailList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return PartialView(podetailList.ToPagedList(pageIndex, pageSize));
        }

        public decimal GetTotalPO01(TimelyDepotContext db01, int purchaseorderid)
        {
            decimal dTotal = 0;
            double dPartial = 0;

            IQueryable<PurchasOrderDetail> qrypodetail = null;

            qrypodetail = db01.PurchasOrderDetails.Where(podt => podt.PurchaseOrderId == purchaseorderid);
            if (qrypodetail.Count() > 0)
            {
                foreach (var item in qrypodetail)
                {
                    dPartial = Convert.ToDouble(item.Quantity) * Convert.ToDouble(item.UnitPrice);
                    dTotal = dTotal + Convert.ToDecimal(dPartial);
                }
            }

            return dTotal;
        }

        public decimal GetTotalPO(int purchaseorderid)
        {
            decimal dTotal = 0;
            double dPartial = 0;

            IQueryable<PurchasOrderDetail> qrypodetail = null;

            qrypodetail = db.PurchasOrderDetails.Where(podt => podt.PurchaseOrderId == purchaseorderid);
            if (qrypodetail.Count() > 0)
            {
                foreach (var item in qrypodetail)
                {
                    dPartial = Convert.ToDouble(item.Quantity) * Convert.ToDouble(item.UnitPrice);
                    dTotal = dTotal + Convert.ToDecimal(dPartial);
                }
            }

            return dTotal;
        }


        //
        // GET: /PutchaseOrder/GetVendorData
        public static void GetVendorData(TimelyDepotContext db01, string szVendorNo, ref string CompanyName, ref string FirstName, ref string LastName,
            ref string Title, ref string Address1, ref string Address2, ref string Address3, ref string City, ref string State, ref string Zip, ref string Country, ref string Tel, ref string Fax,
            ref string Email, ref string Website, ref string Tel1, ref string Tel2)
        {
            var qryVendor = db01.VendorsContactAddresses.Join(db01.Vendors, vdca => vdca.VendorId, vd => vd.Id, (vdca, vd)
                => new { vdca, vd }).Where(Nvcda => Nvcda.vd.VendorNo == szVendorNo);
            if (qryVendor.Count() > 0)
            {
                foreach (var item in qryVendor)
                {
                    CompanyName = item.vdca.CompanyName;
                    FirstName = item.vdca.FirstName;
                    LastName = item.vdca.LastName;
                    Title = item.vdca.Title;
                    Address1 = item.vdca.Address;
                    Address2 = item.vdca.Note;
                    Address3 = item.vdca.Address3;
                    City = item.vdca.City;
                    State = item.vdca.State;
                    Zip = item.vdca.Zip;
                    Country = item.vdca.Country;
                    Fax = item.vdca.Fax;
                    Email = item.vdca.Email;
                    Website = item.vdca.Website;
                    Tel1 = item.vdca.Tel1;
                    Tel2 = item.vdca.Tel2;
                    Tel = item.vdca.Tel;
                    break;
                }
            }

        }

        //
        // GET: /PurchaseOrder/DeletePurchaseOrder
        public ActionResult DeletePurchaseOrder(int id)
        {
            PurchasOrderDetail purchaseorderdetail = null;
            IQueryable<PurchasOrderDetail> qryPODetails = null;

            //Delete Details
            qryPODetails = db.PurchasOrderDetails.Where(podt => podt.PurchaseOrderId == id);
            if (qryPODetails.Count() > 0)
            {
                foreach (var item in qryPODetails)
                {
                    purchaseorderdetail = db.PurchasOrderDetails.Find(item.Id);
                    if (purchaseorderdetail != null)
                    {
                        db.PurchasOrderDetails.Remove(purchaseorderdetail);
                    }
                }
            }

            PurchaseOrders purchaseorders = db.PurchaseOrders.Find(id);
            db.PurchaseOrders.Remove(purchaseorders);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //
        // GET: /PurchaseOrder/SelectVendor
        public PartialViewResult SelectVendor(int? page, string searchVendorNo)
        {
            int pageIndex = 0;
            int pageSize = PageSize;

            IQueryable<Vendors> qryVendors = null;

            List<Vendors> VendorsList = new List<Vendors>();

            qryVendors = db.Vendors.OrderBy(vd => vd.VendorNo);
            if (!string.IsNullOrEmpty(searchVendorNo))
            {
                qryVendors = db.Vendors.Where(vd => vd.VendorNo.StartsWith(searchVendorNo)).OrderBy(vd => vd.VendorNo);
            }

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
            return PartialView(VendorsList.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /PurchaseOrder/SelectSalesOrder
        [NoCache]
        public PartialViewResult SelectSalesOrder(int? page, string searchOrderNo, string searchCustomer, string searchEmail)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            int nHas = 0;
            Customers customer = null;

            List<int> customerIdsList = new List<int>();
            List<SalesOrder> SalesOrderList = new List<SalesOrder>();
            IQueryable<SalesOrder> qrySalesOrder = null;
            IQueryable<CustomersContactAddress> qryAddress = null;

            if (string.IsNullOrEmpty(searchOrderNo) && string.IsNullOrEmpty(searchCustomer) && string.IsNullOrEmpty(searchEmail))
            {
                qrySalesOrder = db.SalesOrders.OrderBy(slor => slor.SalesOrderNo);
                if (qrySalesOrder.Count() > 0)
                {
                    foreach (var item in qrySalesOrder)
                    {
                        SalesOrderList.Add(item);
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

        //
        // GET: /PurchaseOrder/GetTradeName
        public static string GetTradeName(TimelyDepotContext db01, int tradeid = 0)
        {
            string szTradeName = "";

            Trade trade = db01.Trades.Find(tradeid);
            if (trade != null)
            {
                szTradeName = trade.TradeName;
            }

            return szTradeName;
        }

        //
        // GET: /PurchaseOrder/
        [NoCache]
        public ActionResult Index(int? page, string searchItem, string ckActive, string ckCriteria)
        {
            bool bHasData = true;
            int pageIndex = 0;
            int pageSize = PageSize;
            int nYear = 0;
            int nMonth = 0;
            int nDay = 0;
            string searchPONo = "";
            string[] szFecha = null;
            DateTime dFecha = DateTime.Now;

            TimelyDepotContext db01 = new TimelyDepotContext();

            IQueryable<PurchaseOrders> qryPOs = null;

            List<PurchaseOrders> purchaseorderList01 = new List<PurchaseOrders>();
            List<PurchaseOrderListHlp> purchaseorderListHlp = new List<PurchaseOrderListHlp>();

            if (string.IsNullOrEmpty(searchItem) || searchItem == "0")
            {
                //qryItem = db.ITEMs.OrderBy(it => it.ItemID);
                ViewBag.ckActiveHlp = "true";
                ViewBag.ckCriteriaHlp = "purchaseorder";
                ViewBag.CurrentDate = dFecha.ToString("yyyy/MM/dd");
                bHasData = false;

                if (searchItem == "0")
                {
                    ViewBag.SearchItem = searchItem;
                    bHasData = true;

                    if (ckCriteria == "purchaseorder")
                    {
                        if (ckActive == "true")
                        {
                            qryPOs = db.PurchaseOrders.OrderBy(vd => vd.PurchaseOrderNo);
                        }
                        else
                        {
                            qryPOs = db.PurchaseOrders.OrderBy(vd => vd.PurchaseOrderNo);
                        }
                    }
                }

            }
            else
            {
                ViewBag.SearchItem = searchItem;
                ViewBag.ckActiveHlp = ckActive;
                ViewBag.ckCriteriaHlp = ckCriteria;
                ViewBag.CurrentDate = dFecha.ToString("yyyy/MM/dd");

                if (ckCriteria == "purchaseorder")
                {
                    if (ckActive == "true")
                    {
                        qryPOs = db.PurchaseOrders.Where(vd => vd.PurchaseOrderNo.StartsWith(searchItem)).OrderBy(vd => vd.PurchaseOrderNo);
                    }
                    else
                    {
                        qryPOs = db.PurchaseOrders.Where(vd => vd.PurchaseOrderNo.StartsWith(searchItem)).OrderBy(vd => vd.PurchaseOrderNo);
                    }
                }

                if (ckCriteria == "salesorder")
                {
                    if (ckActive == "true")
                    {
                        qryPOs = db.PurchaseOrders.Where(vd => vd.SalesOrderNo.StartsWith(searchItem)).OrderBy(vd => vd.PurchaseOrderNo);
                    }
                    else
                    {
                        qryPOs = db.PurchaseOrders.Where(vd => vd.SalesOrderNo.StartsWith(searchItem)).OrderBy(vd => vd.PurchaseOrderNo);
                    }
                }

                if (ckCriteria == "vendor")
                {
                    if (ckActive == "true")
                    {
                        qryPOs = db.PurchaseOrders.Where(vd => vd.VendorId.StartsWith(searchItem)).OrderBy(vd => vd.PurchaseOrderNo);
                    }
                    else
                    {
                        qryPOs = db.PurchaseOrders.Where(vd => vd.VendorId.StartsWith(searchItem)).OrderBy(vd => vd.PurchaseOrderNo);
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
                        qryPOs = db.PurchaseOrders.Where(vd => vd.ShipDate >= dFecha).OrderBy(vd => vd.PurchaseOrderNo);
                    }
                    else
                    {
                        qryPOs = db.PurchaseOrders.Where(vd => vd.ShipDate >= dFecha).OrderBy(vd => vd.PurchaseOrderNo);
                    }
                }

                if (ckCriteria == "requireddate")
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
                        qryPOs = db.PurchaseOrders.Where(vd => vd.RequiredDate >= dFecha).OrderBy(vd => vd.PurchaseOrderNo);
                    }
                    else
                    {
                        qryPOs = db.PurchaseOrders.Where(vd => vd.RequiredDate >= dFecha).OrderBy(vd => vd.PurchaseOrderNo);
                    }
                }

                if (ckCriteria == "itemno")
                {
                    bHasData = false;


                    if (ckActive == "true")
                    {
                        //qryPOs = db.PurchaseOrders.Where(vd => vd.VendorId.StartsWith(searchItem)).OrderBy(vd => vd.PurchaseOrderNo);
                        var qryPOhlp = db.PurchasOrderDetails.Join(db.PurchaseOrders, podt => podt.PurchaseOrderId, po => po.PurchaseOrderId, (podt, po)
                            => new { podt, po }).Where(Ndata => Ndata.podt.Sub_ItemID.StartsWith(searchItem)).OrderBy(Ndata => Ndata.po.PurchaseOrderNo);
                        if (qryPOhlp.Count() > 0)
                        {
                            foreach (var itemPO in qryPOhlp)
                            {
                                AddPurchaseOrder(db01, itemPO.po, ref purchaseorderListHlp);
                            }
                        }
                    }
                    else
                    {
                        //qryPOs = db.PurchaseOrders.Where(vd => vd.VendorId.StartsWith(searchItem)).OrderBy(vd => vd.PurchaseOrderNo);
                        var qryPOhlp = db.PurchasOrderDetails.Join(db.PurchaseOrders, podt => podt.PurchaseOrderId, po => po.PurchaseOrderId, (podt, po)
                            => new { podt, po }).Where(Ndata => Ndata.podt.Sub_ItemID.StartsWith(searchItem)).OrderBy(Ndata => Ndata.po.PurchaseOrderNo);
                        if (qryPOhlp.Count() > 0)
                        {
                            foreach (var itemPO in qryPOhlp)
                            {
                                AddPurchaseOrder(db01, itemPO.po, ref purchaseorderListHlp);
                            }
                        }
                    }
                }

                //qryPOs = db.PurchaseOrders.OrderBy(vd => vd.PurchaseOrderNo);
                //if (!string.IsNullOrEmpty(searchPONo))
                //{
                //    ViewBag.SearchPONo = searchPONo;
                //    qryPOs = db.PurchaseOrders.Where(vd => vd.PurchaseOrderNo == searchPONo).OrderBy(vd => vd.PurchaseOrderNo);
                //}
            }

            if (bHasData)
            {
                if (qryPOs.Count() > 0)
                {
                    foreach (var item in qryPOs)
                    {
                        //purchaseorderList01.Add(item);
                        AddPurchaseOrder(db01, item, ref purchaseorderListHlp);
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


            var onePageOfData = purchaseorderList01.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(purchaseorderListHlp.ToPagedList(pageIndex, pageSize));
            //return View(purchaseorderList01.ToPagedList(pageIndex, pageSize));
            //return View(db.PurchaseOrders.ToList());
        }

        private void AddPurchaseOrder(TimelyDepotContext db01, PurchaseOrders item, ref List<PurchaseOrderListHlp> purchaseorderListHlp)
        {
            int nSalesOrderId = 0;
            decimal dAmount = 0;
            IQueryable<PurchasOrderDetail> qrypodetail = null;

            nSalesOrderId = item.PurchaseOrderId;
            dAmount = GetTotalPO01(db01, nSalesOrderId);


            qrypodetail = db01.PurchasOrderDetails.Where(podt => podt.PurchaseOrderId == nSalesOrderId);
            if (qrypodetail.Count() > 0)
            {
                foreach (var itemdetail in qrypodetail)
                {
                    if (!string.IsNullOrEmpty(itemdetail.ItemID))
                    {
                        PurchaseOrderListHlp pohlp = new PurchaseOrderListHlp();
                        pohlp.ItemNo = itemdetail.Sub_ItemID;
                        pohlp.PaymentAmount = dAmount;
                        pohlp.PODate = item.PODate;
                        pohlp.PurchaseOrderId = item.PurchaseOrderId;
                        pohlp.PurchaseOrderNo = item.PurchaseOrderNo;
                        pohlp.RequiredDate = item.RequiredDate;
                        pohlp.SalesOrderNo = item.SalesOrderNo;
                        pohlp.ShipDate = item.ShipDate;
                        pohlp.VendorId = item.VendorId;
                        pohlp.TradeId = item.TradeId;
                        pohlp.ReceiveStatus = item.ReceiveStatus;
                        purchaseorderListHlp.Add(pohlp);
                    }
                }
            }





        }

        //
        // GET: /PurchaseOrder/Details/5

        public ActionResult Details(int id = 0)
        {
            PurchaseOrders purchaseorders = db.PurchaseOrders.Find(id);
            if (purchaseorders == null)
            {
                return HttpNotFound();
            }
            return View(purchaseorders);
        }

        //
        // GET: /PurchaseOrder/Create
        [NoCache]
        public ActionResult Create()
        {
            int nPurchaseOrderNo = 0;

            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();
            IQueryable<Trade> qryTrade = null;


            InitialInfo initialinfo = null;

            //Get the next Purchase Order No
            initialinfo = db.InitialInfoes.FirstOrDefault<InitialInfo>();
            if (initialinfo == null)
            {
                initialinfo = new InitialInfo();
                initialinfo.InvoiceNo = 0;
                initialinfo.PaymentNo = 0;
                initialinfo.PurchaseOrderNo = 1;
                initialinfo.SalesOrderNo = 0;
                initialinfo.TaxRate = 0;
                db.InitialInfoes.Add(initialinfo);
            }
            else
            {
                nPurchaseOrderNo = initialinfo.PurchaseOrderNo;
                nPurchaseOrderNo++;
                initialinfo.PurchaseOrderNo = nPurchaseOrderNo;
                db.Entry(initialinfo).State = EntityState.Modified;
            }

            PurchaseOrders purchaseorder = new PurchaseOrders();
            purchaseorder.PurchaseOrderNo = nPurchaseOrderNo.ToString();
            purchaseorder.PODate = DateTime.Now;
            db.PurchaseOrders.Add(purchaseorder);
            db.SaveChanges();

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

            return View(purchaseorder);
        }

        //
        // POST: /PurchaseOrder/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PurchaseOrders purchaseorders, string PODateHlp)
        {
            int nYear = 0;
            int nMonth = 0;
            int nDay = 0;
            int nCustomerId = 0;
            int nSalesOrderId = 0;
            int nTradeId = 0;
            int nPurchaseOrderId = 0;
            int nItemPosition = 0;
            decimal dSetupCharge = 0;
            decimal dRunCharge = 0;
            string szVendorNo = "";
            string szPaymentType = "";
            string szSalesOrderNo = "";
            string szCardType = "";
            string szItemId = "";
            string[] szDateHlp = null;
            string[] szSetUpRunHlp = null;

            int nHas = 0;
            int nPos = -1;
            string szError = "";
            string szDecriptedData = "";
            string szDecriptedCode = "";
            string szMsg = "";

            DateTime dDate = DateTime.Now;
            DateTime dShipDate = DateTime.Now;

            PurchasOrderDetail purchaseorderdetail = null;
            SalesOrder salesorder = null;
            SalesOrderDetail salesorderdetail = null;
            CustomersContactAddress customeraddress = null;
            CustomersShipAddress customershipto = null;
            CustomersCreditCardShipping cardshipping = null;
            Trade trade = null;
            SalesOrderBlindShip blindship = null;
            VendorItem vendoritem = null;
            SUB_ITEM subitem = null;
            Payments payment = null;

            TimelyDepotContext db02 = new TimelyDepotContext();

            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();
            IQueryable<Trade> qryTrade = null;
            IQueryable<SalesOrderDetail> qrysalesdetail = null;

            if (!string.IsNullOrEmpty(PODateHlp))
            {
                szDateHlp = PODateHlp.Split('/');
                if (szDateHlp != null)
                {
                    nMonth = Convert.ToInt32(szDateHlp[0]);
                    nDay = Convert.ToInt32(szDateHlp[1]);
                    nYear = Convert.ToInt32(szDateHlp[2]);
                    dDate = new DateTime(nYear, nMonth, nDay);
                }
                else
                {
                    dDate = Convert.ToDateTime(purchaseorders.PODate);
                }
            }
            else
            {
                dDate = Convert.ToDateTime(purchaseorders.PODate);
            }



            if (ModelState.IsValid)
            {
                if (purchaseorders.PODate != dDate)
                {
                    purchaseorders.PODate = dDate;
                }

                //Get the Sales Order Data
                salesorder = db.SalesOrders.Where(slor => slor.SalesOrderNo == purchaseorders.SalesOrderNo).FirstOrDefault<SalesOrder>();
                if (salesorder != null)
                {
                    nSalesOrderId = salesorder.SalesOrderId;
                    nCustomerId = Convert.ToInt32(salesorder.CustomerId);
                    nTradeId = Convert.ToInt32(salesorder.TradeId);
                    szVendorNo = purchaseorders.VendorId;
                    szSalesOrderNo = purchaseorders.SalesOrderNo;

                    purchaseorders.PurchaseOrderReference = string.Format("{0}-{1}", salesorder.SalesOrderNo, nCustomerId.ToString());

                    if (salesorder.ShipDate != null)
                    {
                        dShipDate = Convert.ToDateTime(salesorder.ShipDate);
                        purchaseorders.ShipDate = dShipDate;
                    }

                    purchaseorders.IsBlindShip = salesorder.IsBlindShip;

                    if (purchaseorders.IsBlindShip)
                    {
                        //Get blind ship address
                        blindship = db02.SalesOrderBlindShips.Where(blsh => blsh.SalesOrderId == nSalesOrderId).FirstOrDefault<SalesOrderBlindShip>();
                        if (blindship != null)
                        {
                            purchaseorders.ToAddress1 = blindship.Address1;
                            purchaseorders.ToAddress2 = blindship.Address2;
                            purchaseorders.ToCity = blindship.City;
                            purchaseorders.ToCompany = blindship.Title;
                            purchaseorders.ToCountry = blindship.Country;
                            //purchaseorders.FromEmail = blindship.Email;
                            //purchaseorders.FromFax = blindship.Fax;
                            purchaseorders.ToName = string.Format("{0} {1}", blindship.FirstName, blindship.LastName);
                            purchaseorders.ToState = blindship.State;
                            purchaseorders.ToTel = blindship.Tel;
                            purchaseorders.ToTitle = blindship.Title;
                            purchaseorders.ToZip = blindship.Zip;
                        }
                        db02.Dispose();
                        db02 = new TimelyDepotContext();

                        customershipto = db02.CustomersShipAddresses.Where(ctsh => ctsh.CustomerId == nCustomerId).FirstOrDefault<CustomersShipAddress>();
                        if (customershipto != null)
                        {
                            purchaseorders.FromAddress1 = customershipto.Address1;
                            purchaseorders.FromAddress2 = customershipto.Address2;
                            purchaseorders.FromCity = customershipto.City;
                            purchaseorders.FromCompany = customershipto.Title;
                            purchaseorders.FromCountry = customershipto.Country;
                            purchaseorders.FromEmail = customershipto.Email;
                            purchaseorders.FromFax = customershipto.Fax;
                            purchaseorders.FromName = string.Format("{0} {1}", customershipto.FirstName, customershipto.LastName);
                            purchaseorders.FromState = customershipto.State;
                            purchaseorders.FromTel = customershipto.Tel;
                            //purchaseorders.ToTitle = trade.Title;
                            purchaseorders.FromZip = customershipto.Zip;

                            //Get the blind drop data
                            purchaseorders.BlindDrop = string.Format("{0} a/c: {1}", customershipto.ShippingPreference, customershipto.ShipperAccount);

                        }
                        db02.Dispose();
                        db02 = new TimelyDepotContext();
                    }
                    else
                    {
                        //Get customer address
                        customeraddress = db02.CustomersContactAddresses.Where(ctad => ctad.CustomerId == nCustomerId).FirstOrDefault<CustomersContactAddress>();
                        if (customeraddress != null)
                        {
                            purchaseorders.FromAddress1 = customeraddress.Address;
                            purchaseorders.FromAddress2 = customeraddress.Note;
                            purchaseorders.FromCity = customeraddress.City;
                            purchaseorders.FromCompany = customeraddress.CompanyName;
                            purchaseorders.FromCountry = customeraddress.Country;
                            purchaseorders.FromEmail = customeraddress.Email;
                            purchaseorders.FromFax = customeraddress.Fax;
                            purchaseorders.FromName = string.Format("{0} {1}", customeraddress.FirstName, customeraddress.LastName);
                            purchaseorders.FromState = customeraddress.State;
                            purchaseorders.FromTel = customeraddress.Tel;
                            purchaseorders.FromTitle = customeraddress.Title;
                            purchaseorders.FromZip = customeraddress.Zip;
                        }
                        db02.Dispose();
                        db02 = new TimelyDepotContext();

                        //Get the trade data
                        //trade = db02.Trades.Where(trd => trd.TradeId == nTradeId).FirstOrDefault<Trade>();
                        //if (trade != null)
                        //{
                        //    purchaseorders.ToAddress1 = trade.Address;
                        //    //purchaseorders.ToAddress2 = trade.WebSite;
                        //    purchaseorders.ToCity = trade.City;
                        //    purchaseorders.ToCompany = trade.TradeName;
                        //    purchaseorders.ToCountry = trade.Country;
                        //    purchaseorders.ToEmail = trade.Email;
                        //    purchaseorders.ToFax = trade.Fax;
                        //    //purchaseorders.ToName = string.Format("{0} {1}", trade., trade.LastName);
                        //    purchaseorders.ToState = trade.State;
                        //    purchaseorders.ToTel = customeraddress.Tel;
                        //    //purchaseorders.ToTitle = trade.Title;
                        //    purchaseorders.ToZip = trade.PostCode;
                        //}
                        //db02.Dispose();
                        //db02 = new TimelyDepotContext();

                        //Get the ship to address for the sales order
                        customershipto = db02.CustomersShipAddresses.Where(ctsh => ctsh.Id == salesorder.CustomerShiptoId).FirstOrDefault<CustomersShipAddress>();
                        if (customershipto != null)
                        {
                            purchaseorders.ToAddress1 = customershipto.Address1;
                            purchaseorders.ToAddress2 = customershipto.Address2;
                            purchaseorders.ToCity = customershipto.City;
                            purchaseorders.ToCompany = customershipto.Title;
                            purchaseorders.ToCountry = customershipto.Country;
                            purchaseorders.ToEmail = customershipto.Email;
                            purchaseorders.ToFax = customershipto.Fax;
                            purchaseorders.ToName = string.Format("{0} {1}", customershipto.FirstName, customershipto.LastName);
                            purchaseorders.ToState = customershipto.State;
                            purchaseorders.ToTel = customershipto.Tel;
                            //purchaseorders.ToTitle = trade.Title;
                            purchaseorders.ToZip = customershipto.Zip;

                            //Get the blind drop data
                            purchaseorders.BlindDrop = string.Format("{0} a/c: {1}", customershipto.ShippingPreference, customershipto.ShipperAccount);

                        }
                        db02.Dispose();
                        db02 = new TimelyDepotContext();


                        customershipto = db02.CustomersShipAddresses.Where(ctsh => ctsh.CustomerId == nCustomerId).FirstOrDefault<CustomersShipAddress>();
                        if (customershipto != null)
                        {

                            //Get the blind drop data
                            purchaseorders.BlindDrop = string.Format("{0} a/c: {1}", customershipto.ShippingPreference, customershipto.ShipperAccount);

                        }
                        db02.Dispose();
                        db02 = new TimelyDepotContext();

                    }


                }

                //Get the pay by data
                payment = db.Payments.Where(pmt => pmt.SalesOrderNo == szSalesOrderNo).FirstOrDefault<Payments>();
                if (payment != null)
                {
                    szPaymentType = payment.PaymentType;
                    if (!string.IsNullOrEmpty(szPaymentType))
                    {
                        cardshipping = db.CustomersCreditCardShippings.Where(cdsp => cdsp.CreditNumber == szPaymentType && cdsp.CustomerId == nCustomerId).FirstOrDefault<CustomersCreditCardShipping>();
                        if (cardshipping != null)
                        {
                            szCardType = cardshipping.CardType;
                            if (!string.IsNullOrEmpty(szCardType))
                            {
                                szError = "";
                                szDecriptedData = TimelyDepotMVC.Controllers.PaymentController.DecodeInfo02(szPaymentType, ref szError);
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

                                purchaseorders.PaidBy = string.Format("{0} {1}", szCardType, szDecriptedData);
                            }
                        }
                    }
                }


                db.Entry(purchaseorders).State = EntityState.Modified;
                db.SaveChanges();

                //Get the detail
                nPurchaseOrderId = purchaseorders.PurchaseOrderId;

                qrysalesdetail = db.SalesOrderDetails.Where(sldt => sldt.SalesOrderId == nSalesOrderId).OrderBy(sldt => sldt.ItemPosition).ThenBy(sldt => sldt.ItemOrder);
                if (qrysalesdetail.Count() > 0)
                {
                    foreach (var item in qrysalesdetail)
                    {

                        //Get the vendors data
                        if (item.ItemID != null)
                        {
                            if (string.IsNullOrEmpty(szItemId))
                            {
                                szItemId = item.ItemID;
                            }
                            purchaseorderdetail = new PurchasOrderDetail();
                            purchaseorderdetail.PurchaseOrderId = nPurchaseOrderId;
                            purchaseorderdetail.ItemID = item.ItemID;
                            purchaseorderdetail.Sub_ItemID = item.Sub_ItemID;
                            purchaseorderdetail.Description = item.Description;

                            nPos = -1;
                            nPos = item.Description.IndexOf("Set up");
                            if (nPos != -1)
                            {
                                szSetUpRunHlp = item.Description.Split(' ');
                                if (szSetUpRunHlp.Length > 0)
                                {
                                    purchaseorderdetail.Description = string.Format("Set up Charge {0} {1}", nPurchaseOrderId.ToString(), szSetUpRunHlp[4]);
                                }
                            }
                            nPos = -1;
                            szSetUpRunHlp = null;
                            nPos = item.Description.IndexOf("Run Charge");
                            if (nPos != -1)
                            {
                                szSetUpRunHlp = item.Description.Split(' ');
                                if (szSetUpRunHlp.Length > 0)
                                {
                                    purchaseorderdetail.Description = string.Format("Run Charge {0} {1}", nPurchaseOrderId.ToString(), szSetUpRunHlp[3]);
                                }
                            }

                            purchaseorderdetail.Quantity = item.Quantity;
                            purchaseorderdetail.UnitPrice = item.UnitPrice;
                            purchaseorderdetail.Tax = item.Tax;

                            purchaseorderdetail.ItemPosition = item.ItemPosition;
                            purchaseorderdetail.ItemOrder = item.ItemOrder;
                            nItemPosition = Convert.ToInt32(item.ItemPosition);

                            vendoritem = db02.VendorItems.Where(vdit => vdit.VendorNo == szVendorNo && vdit.ItemId == item.ItemID).FirstOrDefault<VendorItem>();
                            if (vendoritem != null)
                            {
                                if (purchaseorders.IsBlindShip)
                                {
                                    purchaseorderdetail.UnitPrice = vendoritem.CostBlind;
                                }
                                else
                                {
                                    if (vendoritem.Cost != null)
                                    {
                                        purchaseorderdetail.UnitPrice = vendoritem.Cost;
                                        
                                    }
                                }

                                purchaseorderdetail.VendorReference = vendoritem.VendorPartNo;
                            }
                            else
                            {
                                //purchaseorderdetail.UnitPrice = 0;
                            }

                            if (vendoritem != null)
                            {
                                dSetupCharge = Convert.ToDecimal(vendoritem.SetupCharge);
                                dRunCharge = Convert.ToDecimal(vendoritem.RunCharge);
                            }
                            else
                            {
                                dSetupCharge = 0;
                                dRunCharge = 0;
                            }
                            db02.Dispose();
                            db02 = new TimelyDepotContext();

                            //Get the vendor part no from the subitem table
                            subitem = db02.SUB_ITEM.Where(sbit => sbit.Sub_ItemID == item.Sub_ItemID).FirstOrDefault<SUB_ITEM>();
                            if (subitem != null)
                            {
                                purchaseorderdetail.VendorReference = subitem.PartNo;
                            }
                            db02.Dispose();
                            db02 = new TimelyDepotContext();

                            db.PurchasOrderDetails.Add(purchaseorderdetail);
                        }

                    }

                    ////Add setup and run charges
                    //purchaseorderdetail = new PurchasOrderDetail();
                    //purchaseorderdetail.PurchaseOrderId = nPurchaseOrderId;
                    //purchaseorderdetail.ItemID = szItemId;
                    //purchaseorderdetail.Description = "Setup charge";
                    //purchaseorderdetail.Quantity = 0;
                    //purchaseorderdetail.UnitPrice = dSetupCharge;
                    //purchaseorderdetail.Tax = 0;

                    //nItemPosition++;
                    //purchaseorderdetail.ItemPosition = nItemPosition;
                    //purchaseorderdetail.ItemOrder = 0;

                    //db.PurchasOrderDetails.Add(purchaseorderdetail);

                    //purchaseorderdetail = new PurchasOrderDetail();
                    //purchaseorderdetail.PurchaseOrderId = nPurchaseOrderId;
                    //purchaseorderdetail.ItemID = szItemId;
                    //purchaseorderdetail.Description = "Run charge";
                    //purchaseorderdetail.Quantity = 0;
                    //purchaseorderdetail.UnitPrice = dRunCharge;
                    //purchaseorderdetail.Tax = 0;

                    //nItemPosition++;
                    //purchaseorderdetail.ItemPosition = nItemPosition;
                    //purchaseorderdetail.ItemOrder = 0;

                    //db.PurchasOrderDetails.Add(purchaseorderdetail);

                    db.SaveChanges();
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


                return RedirectToAction("Edit", new { id = purchaseorders.PurchaseOrderId });
                //return RedirectToAction("Index");
            }

            return View(purchaseorders);
        }

        //
        // GET: /PurchaseOrder/Edit/5

        public ActionResult Edit(int id = 0)
        {
            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> list01Selector = new List<KeyValuePair<string, string>>();

            IQueryable<Trade> qryTrade = null;
            IQueryable<Warehouses> qryWarehouse = null;
            IQueryable<Terms> qryTerms = null;
            IQueryable<ShipVia> qryShipVia = null;
            IQueryable<OrderBy> qryOrderBy = null;

            PurchaseOrders purchaseorders = db.PurchaseOrders.Find(id);
            if (purchaseorders == null)
            {
                return HttpNotFound();
            }

            //Get the dropdown data
            qryTrade = db.Trades.OrderBy(trd => trd.TradeName);
            if (qryTrade.Count() > 0)
            {
                foreach (var item in qryTrade)
                {
                    listSelector.Add(new KeyValuePair<string, string>(item.TradeId.ToString(), item.TradeName));
                    list01Selector.Add(new KeyValuePair<string, string>(item.TradeName, item.TradeName));
                }
            }
            SelectList tradeselectorlist = new SelectList(listSelector, "Key", "Value");
            ViewBag.TradeList = tradeselectorlist;
            SelectList tradeselector01list = new SelectList(list01Selector, "Key", "Value");
            ViewBag.BillTo01List = tradeselector01list;

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

            listSelector = new List<KeyValuePair<string, string>>();
            qryShipVia = db.ShipVias.OrderBy(csp => csp.Description);
            if (qryShipVia.Count() > 0)
            {
                foreach (var item in qryShipVia)
                {
                    listSelector.Add(new KeyValuePair<string, string>(item.Description, item.Description));
                }
            }
            SelectList shipVialist = new SelectList(listSelector, "Key", "Value");
            ViewBag.ShipViaList = shipVialist;

            listSelector = new List<KeyValuePair<string, string>>();
            qryOrderBy = db.OrderBies.OrderBy(csp => csp.Description);
            if (qryOrderBy.Count() > 0)
            {
                foreach (var item in qryOrderBy)
                {
                    listSelector.Add(new KeyValuePair<string, string>(item.Description, item.Description));
                }
            }
            SelectList orderBylist = new SelectList(listSelector, "Key", "Value");
            ViewBag.OrderByList = orderBylist;


            return View(purchaseorders);
        }

        //
        // POST: /PurchaseOrder/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PurchaseOrders purchaseorders, string PODateHlp, string ShipDateHlp, string RequiredDateHlp)
        {
            int nYear = 0;
            int nMonth = 0;
            int nDay = 0;

            string[] szDateHlp = null;
            string[] szShipDateHlp = null;
            string[] szRequiredDateHlp = null;

            DateTime dDate = DateTime.Now;
            DateTime dShipDate = DateTime.Now;
            DateTime dRequiredDate = DateTime.Now;

            if (!string.IsNullOrEmpty(PODateHlp))
            {
                szDateHlp = PODateHlp.Split('/');
                if (szDateHlp != null)
                {
                    nMonth = Convert.ToInt32(szDateHlp[0]);
                    nDay = Convert.ToInt32(szDateHlp[1]);
                    nYear = Convert.ToInt32(szDateHlp[2]);
                    dDate = new DateTime(nYear, nMonth, nDay);
                }
                else
                {
                    dDate = Convert.ToDateTime(purchaseorders.PODate);
                }
            }
            else
            {
                dDate = Convert.ToDateTime(purchaseorders.PODate);
            }

            if (!string.IsNullOrEmpty(ShipDateHlp))
            {
                szShipDateHlp = ShipDateHlp.Split('/');
                if (szShipDateHlp != null)
                {
                    nMonth = Convert.ToInt32(szShipDateHlp[0]);
                    nDay = Convert.ToInt32(szShipDateHlp[1]);
                    nYear = Convert.ToInt32(szShipDateHlp[2]);
                    dShipDate = new DateTime(nYear, nMonth, nDay);
                }
                else
                {
                    dShipDate = Convert.ToDateTime(purchaseorders.ShipDate);
                }
            }

            if (!string.IsNullOrEmpty(RequiredDateHlp))
            {
                szRequiredDateHlp = RequiredDateHlp.Split('/');
                if (szRequiredDateHlp != null)
                {
                    nMonth = Convert.ToInt32(szRequiredDateHlp[0]);
                    nDay = Convert.ToInt32(szRequiredDateHlp[1]);
                    nYear = Convert.ToInt32(szRequiredDateHlp[2]);
                    dRequiredDate = new DateTime(nYear, nMonth, nDay);
                }
                else
                {
                    dRequiredDate = Convert.ToDateTime(purchaseorders.RequiredDate);
                }
            }


            if (ModelState.IsValid)
            {
                if (purchaseorders.PODate != dDate)
                {
                    purchaseorders.PODate = dDate;
                }
                if (purchaseorders.ShipDate != dShipDate)
                {
                    purchaseorders.ShipDate = dShipDate;
                }
                if (purchaseorders.RequiredDate != dRequiredDate)
                {
                    purchaseorders.RequiredDate = dRequiredDate;
                }

                db.Entry(purchaseorders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(purchaseorders);
        }

        //
        // GET: /PurchaseOrder/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PurchaseOrders purchaseorders = db.PurchaseOrders.Find(id);
            if (purchaseorders == null)
            {
                return HttpNotFound();
            }
            return View(purchaseorders);
        }

        //
        // POST: /PurchaseOrder/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PurchaseOrders purchaseorders = db.PurchaseOrders.Find(id);
            db.PurchaseOrders.Remove(purchaseorders);
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
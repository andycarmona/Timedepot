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
using TimelyDepotMVC.ModelsView;
using TimelyDepotMVC.CommonCode;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.UI;

namespace TimelyDepotMVC.Controllers
{
    public class InventoryController : Controller
    {
        private TimelyDepotContext db = new TimelyDepotContext();

        int _pageIndex = 0;
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }

        int _pageSize = 15;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        //
        // GET: /Inventory/Note
        [NoCache]
        public PartialViewResult Note(string id)
        {
            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            //Get theActive/Inactive list
            listSelector = new List<KeyValuePair<string, string>>();
            listSelector.Add(new KeyValuePair<string, string>("Y", "Active"));
            listSelector.Add(new KeyValuePair<string, string>("N", "Inactive"));
            SelectList activeinactivelist = new SelectList(listSelector, "Key", "Value");
            ViewBag.ActiveInactivelist = activeinactivelist;

            ITEM item = null;
            item = db.ITEMs.Find(id);

            ViewBag.Item = item.ItemID;

            return PartialView(item);
        }

        //
        // GET: /Inventory/UpdateNote
        [NoCache]
        [HttpPost]
        public ActionResult UpdateNote(string ItemID, string Note)
        {

            ITEM item = null;
            item = db.ITEMs.Find(ItemID);
            if (item != null)
            {
                item.Note = Note;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }


            return RedirectToAction("Edit", new { id = ItemID });
            //return RedirectToAction("NoteTab", new { id = ItemID });
        }

        //
        // GET:/Inventory/SalesHistorybyItemTab
        [NoCache]
        public ActionResult NoteTab(string id)
        {
            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            //Get theActive/Inactive list
            listSelector = new List<KeyValuePair<string, string>>();
            listSelector.Add(new KeyValuePair<string, string>("Y", "Active"));
            listSelector.Add(new KeyValuePair<string, string>("N", "Inactive"));
            SelectList activeinactivelist = new SelectList(listSelector, "Key", "Value");
            ViewBag.ActiveInactivelist = activeinactivelist;

            ITEM item = null;
            item = db.ITEMs.Find(id);

            return View(item);
        }



        //
        // GET: /Inventory/OpenPurchaseOrderbyItemTab
        public ActionResult CreateVendorTab(string id)
        {
            int nYear = 0;
            int nMonth = 0;
            int nDay = 0;
            string szMsg = "";
            DateTime dDate = DateTime.Now;

            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            //Get theActive/Inactive list
            listSelector = new List<KeyValuePair<string, string>>();
            listSelector.Add(new KeyValuePair<string, string>("Y", "Active"));
            listSelector.Add(new KeyValuePair<string, string>("N", "Inactive"));
            SelectList activeinactivelist = new SelectList(listSelector, "Key", "Value");
            ViewBag.ActiveInactivelist = activeinactivelist;

            ITEM item = null;
            item = db.ITEMs.Find(id);

            IQueryable<VendorItem> qryvendoritem = null;
            VendorItem vendoritem = null;

            List<VendorItem> vendoritemList = new List<VendorItem>();
            listSelector = new List<KeyValuePair<string, string>>();
            //qryvendoritem = db.VendorItems.Where(cutadd => cutadd.ItemId == item.ItemID).OrderBy(cutadd => cutadd.VendorNo);
            qryvendoritem = db.VendorItems.Where(cutadd => cutadd.ItemId == item.ItemID && cutadd.VendorNo != null).OrderBy(cutadd => cutadd.VendorNo);
            if (qryvendoritem.Count() > 0)
            {
                //custSalesContact = qryCusSal.FirstOrDefault<CustomersSalesContact>();
                foreach (var vendoritemHlp in qryvendoritem)
                {
                    vendoritemList.Add(vendoritemHlp);

                    var qryVendor = db.VendorsContactAddresses.Join(db.Vendors, vdca => vdca.VendorId, vd => vd.Id, (vdca, vd)
                        => new { vdca, vd }).Where(Nvdca => Nvdca.vd.VendorNo == vendoritemHlp.VendorNo).OrderBy(Nvdca => Nvdca.vdca.CompanyName);
                    if (qryVendor.Count() > 0)
                    {
                        foreach (var itemVendor in qryVendor)
                        {
                            szMsg = string.Format("{0} - {1} -({2})", itemVendor.vdca.CompanyName, itemVendor.vd.VendorNo, vendoritemHlp.Id.ToString());
                            listSelector.Add(new KeyValuePair<string, string>(itemVendor.vd.VendorNo, szMsg));
                            //listSelector.Add(new KeyValuePair<string, string>(vendoritemHlp.Id.ToString(), vendoritemHlp.VendorNo));
                            if (vendoritem == null)
                            {
                                vendoritem = vendoritemHlp;
                                ViewBag.VendorItemID = vendoritem.Id;
                            }
                        }
                    }

                }
            }
            else
            {
                //Get the first vendoritem for this item
                vendoritem = db.VendorItems.Where(vdit => vdit.ItemId == item.ItemID).FirstOrDefault<VendorItem>();
                if (vendoritem != null)
                {
                    vendoritemList.Add(vendoritem);
                    ViewBag.VendorItemID = vendoritem.Id;
                }
                else
                {
                    nYear = dDate.Year;
                    nMonth = dDate.Month;
                    nDay = dDate.Day;
                    dDate = new DateTime(nYear, nMonth, nDay, 0, 0, 0);
                    vendoritem = new VendorItem();
                    vendoritem.ItemId = item.ItemID;
                    //vendoritem.LeadTime = dDate;
                    db.VendorItems.Add(vendoritem);
                    db.SaveChanges();
                    vendoritemList.Add(vendoritem);
                    ViewBag.VendorItemID = vendoritem.Id;
                }
            }
            SelectList vendorItemlist = new SelectList(listSelector, "Key", "Value");
            ViewBag.VendorItemList = vendorItemlist;
            ViewBag.VendorItemListHas = listSelector.Count;

            //Get the VendorNo select list
            listSelector = new List<KeyValuePair<string, string>>();
            var qryVendorHlp = db.VendorsContactAddresses.Join(db.Vendors, vdca => vdca.VendorId, vd => vd.Id, (vdca, vd)
                => new { vdca, vd }).OrderBy(Nvdca => Nvdca.vdca.CompanyName);
            if (qryVendorHlp.Count() > 0)
            {
                foreach (var itemAllVendors in qryVendorHlp)
                {
                    szMsg = string.Format("{0} - {1}", itemAllVendors.vdca.CompanyName, itemAllVendors.vd.VendorNo);
                    listSelector.Add(new KeyValuePair<string, string>(itemAllVendors.vd.VendorNo, szMsg));
                }
            }
            SelectList vendornolist = new SelectList(listSelector, "Key", "Value");
            ViewBag.VendorNoList = vendornolist;



            return View(item);
        }


        //
        // GET: /Inventory/OpenPurchaseOrderbyItemTab
        public ActionResult OpenPurchaseOrderbyItemTab(string id)
        {
            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            //Get theActive/Inactive list
            listSelector = new List<KeyValuePair<string, string>>();
            listSelector.Add(new KeyValuePair<string, string>("Y", "Active"));
            listSelector.Add(new KeyValuePair<string, string>("N", "Inactive"));
            SelectList activeinactivelist = new SelectList(listSelector, "Key", "Value");
            ViewBag.ActiveInactivelist = activeinactivelist;

            ITEM item = null;
            item = db.ITEMs.Find(id);

            return View(item);
        }

        //
        // GET: /Inventory/OpenSalesOrderbyItemTab
        [NoCache]
        public ActionResult OpenSalesOrderbyItemTab(string id)
        {
            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            //Get theActive/Inactive list
            listSelector = new List<KeyValuePair<string, string>>();
            listSelector.Add(new KeyValuePair<string, string>("Y", "Active"));
            listSelector.Add(new KeyValuePair<string, string>("N", "Inactive"));
            SelectList activeinactivelist = new SelectList(listSelector, "Key", "Value");
            ViewBag.ActiveInactivelist = activeinactivelist;

            ITEM item = null;
            item = db.ITEMs.Find(id);

            return View(item);
        }

        //
        // GET: /Inventory/PreviousSalesHistorybyItemTab
        public ActionResult PreviousSalesHistorybyItemTab(string id, string opcion)
        {
            string szNextId = id;
            szNextId = GetPreviousItem(id);

            if (!string.IsNullOrEmpty(opcion))
            {
                if (opcion == "1")
                {
                    return RedirectToAction("OpenSalesOrderbyItemTab", new { id = szNextId });
                }
                if (opcion == "2")
                {
                    return RedirectToAction("OpenPurchaseOrderbyItemTab", new { id = szNextId });
                }
                if (opcion == "3")
                {
                    return RedirectToAction("CreateVendorTab", new { id = szNextId });
                }
                if (opcion == "4")
                {
                    return RedirectToAction("NoteTab", new { id = szNextId });
                }
            }

            return RedirectToAction("SalesHistorybyItemTab", new { id = szNextId });

        }

        //
        // GET: /Inventory/NextSalesHistorybyItemTab
        public ActionResult NextSalesHistorybyItemTab(string id, string opcion)
        {
            string szNextId = id;
            szNextId = GetNextItem(id);

            if (!string.IsNullOrEmpty(opcion))
            {
                if (opcion == "1")
                {
                    return RedirectToAction("OpenSalesOrderbyItemTab", new { id = szNextId });
                }
                if (opcion == "2")
                {
                    return RedirectToAction("OpenPurchaseOrderbyItemTab", new { id = szNextId });
                }
                if (opcion == "3")
                {
                    return RedirectToAction("CreateVendorTab", new { id = szNextId });
                }
                if (opcion == "4")
                {
                    return RedirectToAction("NoteTab", new { id = szNextId });
                }
            }

            return RedirectToAction("SalesHistorybyItemTab", new { id = szNextId });

        }

        //
        // GET:/Inventory/SalesHistorybyItemTab
        [NoCache]
        public ActionResult SalesHistorybyItemTab(string id)
        {
            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            //Get theActive/Inactive list
            listSelector = new List<KeyValuePair<string, string>>();
            listSelector.Add(new KeyValuePair<string, string>("Y", "Active"));
            listSelector.Add(new KeyValuePair<string, string>("N", "Inactive"));
            SelectList activeinactivelist = new SelectList(listSelector, "Key", "Value");
            ViewBag.ActiveInactivelist = activeinactivelist;

            ITEM item = null;
            item = db.ITEMs.Find(id);

            return View(item);
        }


        //
        // GET: /Inventory/PreviousItem
        public ActionResult PreviousItem(string id)
        {
            string szNextId = id;

            //IQueryable<ITEM> qryItem = null;
            //qryItem = db.ITEMs.Where(it => it.ItemID == id);

            szNextId = GetPreviousItem(id);

            return RedirectToAction("Edit", new { id = szNextId });
        }

        private string GetPreviousItem(string id)
        {
            int nHas = -1;
            string szNext = "";
            string szError = "";
            string szSql = "";
            string szConnString = "";
            SqlDataSource sqlds = new SqlDataSource();
            DataView dv = null;
            ConnectionStringSettingsCollection connSettings = ConfigurationManager.ConnectionStrings;

            try
            {
                szNext = id;
                szConnString = connSettings["TimelyDepotContext"].ToString();
                sqlds.ConnectionString = szConnString;

                szSql = string.Format("SELECT TOP (100) PERCENT ItemID FROM ITEM " +
                    "WHERE (ItemID < '{0}') ORDER BY ItemID  DESC", id);
                sqlds.SelectCommand = szSql;
                dv = (DataView)sqlds.Select(DataSourceSelectArguments.Empty);
                nHas = dv.Count;
                if (nHas > 0)
                {
                    szNext = dv[0]["ItemID"].ToString();
                }
            }
            catch (Exception exc)
            {
                szNext = "";
                szError = exc.Message;
            }

            return szNext;
        }

        //
        // GET: /Inventory/NextItem
        public ActionResult NextItem(string id)
        {
            string szNextId = id;

            //IQueryable<ITEM> qryItem = null;
            //qryItem = db.ITEMs.Where(it => it.ItemID == id);

            szNextId = GetNextItem(id);

            return RedirectToAction("Edit", new { id = szNextId });
        }

        private string GetNextItem(string id)
        {
            bool bStatus = false;
            int nHas = -1;
            string szNext = "";
            string szError = "";
            string szSql = "";
            string szConnString = "";
            SqlDataSource sqlds = new SqlDataSource();
            DataView dv = null;
            ConnectionStringSettingsCollection connSettings = ConfigurationManager.ConnectionStrings;

            try
            {
                szNext = id;
                szConnString = connSettings["TimelyDepotContext"].ToString();
                sqlds.ConnectionString = szConnString;

                szSql = string.Format("SELECT TOP (100) PERCENT ItemID FROM ITEM " +
                    "WHERE (ItemID > '{0}') ORDER BY ItemID", id);
                sqlds.SelectCommand = szSql;
                dv = (DataView)sqlds.Select(DataSourceSelectArguments.Empty);
                nHas = dv.Count;
                if (nHas > 0)
                {
                    szNext = dv[0]["ItemID"].ToString();
                    bStatus = true;
                }
            }
            catch (Exception exc)
            {
                szNext = "";
                szError = exc.Message;
            }

            return szNext;
        }

        //
        // GET: /Vendors/OpenPurchaseOrderExcel
        public ActionResult SalesHistoryExcel(string id)
        {
            //DataTable hlpTbl = GetCustomerListTable();

            ExportCSV(GetSalesHistoryrTable(id), "SalesHistory");

            return RedirectToAction("Index", "ReportsExcel");
        }

        private DataTable GetSalesHistoryrTable(string szItemId)
        {
            long telHlp = 0;
            long faxHlp = 0;
            string telfmt = "000-000-0000";
            string szTel = "";
            string szMsg = "";

            TimelyDepotContext db01 = new TimelyDepotContext();

            DataTable table = null;
            DataRow row = null;

            PurchaseOrdersbyVendor purchaseorderbyvendor = null;
            List<PurchaseOrdersbyVendor> purchaseorderList = new List<PurchaseOrdersbyVendor>();

            //qryPOs = db.PurchaseOrders.Where(vd => vd.VendorId == id ).OrderByDescending(vd => vd.PurchaseOrderNo);

            var qryPOs = db.InvoiceDetails.Join(db.Invoices, podet => podet.InvoiceId, po => po.InvoiceId, (podet, po)
                => new { podet, po }).Where(prod => prod.podet.ItemID == szItemId && prod.podet.Sub_ItemID != null).OrderByDescending(prod => prod.po.SalesOrderId);
            if (qryPOs.Count() > 0)
            {
                foreach (var item in qryPOs)
                {
                    szMsg = item.po.PurchaseOrderNo;

                    purchaseorderbyvendor = new PurchaseOrdersbyVendor();
                    purchaseorderbyvendor.PurchaseOrderId = item.po.InvoiceId;
                    purchaseorderbyvendor.PurchaseOrderNo = item.po.InvoiceNo;
                    purchaseorderbyvendor.SODate = item.po.InvoiceDate;
                    purchaseorderbyvendor.VendorNo = GetCustomerNo(db01, item.po.CustomerId);
                    purchaseorderbyvendor.Sub_ItemID = item.podet.Sub_ItemID;
                    purchaseorderbyvendor.Description = item.podet.Description;
                    purchaseorderbyvendor.Quantity = item.podet.Quantity;
                    purchaseorderbyvendor.UnitPrice = item.podet.UnitPrice;

                    purchaseorderList.Add(purchaseorderbyvendor);
                }
            }

            table = new DataTable("SalesHistory");

            // Set the header
            DataColumn col01 = new DataColumn("InvoiceNo", System.Type.GetType("System.String"));
            DataColumn col02 = new DataColumn("Date", System.Type.GetType("System.String"));
            DataColumn col03 = new DataColumn("CustomerNo", System.Type.GetType("System.String"));
            DataColumn col04 = new DataColumn("ItemID", System.Type.GetType("System.String"));
            DataColumn col05 = new DataColumn("Description", System.Type.GetType("System.String"));
            DataColumn col06 = new DataColumn("Quantity", System.Type.GetType("System.String"));
            DataColumn col07 = new DataColumn("UnitPrice", System.Type.GetType("System.String"));
            table.Columns.Add(col01);
            table.Columns.Add(col02);
            table.Columns.Add(col03);
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
                row["CustomerNo"] = item.VendorNo;
                row["ItemID"] = item.Sub_ItemID;
                row["Description"] = item.Description;
                row["Quantity"] = item.Quantity;
                row["UnitPrice"] = item.UnitPrice;
                table.Rows.Add(row);
            }

            return table;
        }

        //
        // GET:/Vendors/OpenPurchaseOrder
        [NoCache]
        public PartialViewResult SalesHistorybyItem(int? page, string id)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            string szMsg = "";

            PurchaseOrdersbyVendor purchaseorderbyvendor = null;
            List<PurchaseOrdersbyVendor> purchaseorderList = new List<PurchaseOrdersbyVendor>();

            TimelyDepotContext db01 = new TimelyDepotContext();

            //qryPOs = db.PurchaseOrders.Where(vd => vd.VendorId == id ).OrderByDescending(vd => vd.PurchaseOrderNo);

            var qryPOs = db.InvoiceDetails.Join(db.Invoices, podet => podet.InvoiceId, po => po.InvoiceId, (podet, po)
                => new { podet, po }).Where(prod => prod.podet.ItemID == id && prod.podet.Sub_ItemID != null).OrderByDescending(prod => prod.po.SalesOrderId);
            if (qryPOs.Count() > 0)
            {
                foreach (var item in qryPOs)
                {
                    szMsg = item.po.PurchaseOrderNo;

                    purchaseorderbyvendor = new PurchaseOrdersbyVendor();
                    purchaseorderbyvendor.PurchaseOrderId = item.po.InvoiceId;
                    purchaseorderbyvendor.PurchaseOrderNo = item.po.InvoiceNo;
                    purchaseorderbyvendor.SODate = item.po.InvoiceDate;
                    purchaseorderbyvendor.VendorNo = GetCustomerNo(db01, item.po.CustomerId);
                    purchaseorderbyvendor.Sub_ItemID = item.podet.Sub_ItemID;
                    purchaseorderbyvendor.Description = item.podet.Description;
                    purchaseorderbyvendor.Quantity = item.podet.Quantity;
                    purchaseorderbyvendor.UnitPrice = item.podet.UnitPrice;

                    purchaseorderList.Add(purchaseorderbyvendor);
                }
            }

            ViewBag.ItemId = id;

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
        // GET: /Vendors/OpenPurchaseOrderExcel
        public ActionResult OpenSalesOrderExcel(string id)
        {
            //DataTable hlpTbl = GetCustomerListTable();

            ExportCSV(GetOpenSalesOrderTable(id), "OpenSalesOrderList");

            return RedirectToAction("Index", "ReportsExcel");
        }

        private DataTable GetOpenSalesOrderTable(string szItemId)
        {
            long telHlp = 0;
            long faxHlp = 0;
            string telfmt = "000-000-0000";
            string szTel = "";
            string szMsg = "";

            TimelyDepotContext db01 = new TimelyDepotContext();

            DataTable table = null;
            DataRow row = null;

            PurchaseOrdersbyVendor purchaseorderbyvendor = null;
            List<PurchaseOrdersbyVendor> purchaseorderList = new List<PurchaseOrdersbyVendor>();

            //qryPOs = db.PurchaseOrders.Where(vd => vd.VendorId == id ).OrderByDescending(vd => vd.PurchaseOrderNo);

            var qryPOs = db.SalesOrderDetails.Join(db.SalesOrders, podet => podet.SalesOrderId, po => po.SalesOrderId, (podet, po)
                => new { podet, po }).Where(prod => prod.podet.ItemID == szItemId && prod.podet.Sub_ItemID != null).OrderByDescending(prod => prod.po.SalesOrderId);
            if (qryPOs.Count() > 0)
            {
                foreach (var item in qryPOs)
                {
                    szMsg = item.po.PurchaseOrderNo;

                    purchaseorderbyvendor = new PurchaseOrdersbyVendor();
                    purchaseorderbyvendor.PurchaseOrderId = item.po.SalesOrderId;
                    purchaseorderbyvendor.PurchaseOrderNo = item.po.SalesOrderNo;
                    purchaseorderbyvendor.SODate = item.po.SODate;
                    purchaseorderbyvendor.VendorNo = GetCustomerNo(db01, item.po.CustomerId);
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
            DataColumn col03 = new DataColumn("CustomerNo", System.Type.GetType("System.String"));
            DataColumn col04 = new DataColumn("ItemID", System.Type.GetType("System.String"));
            DataColumn col05 = new DataColumn("Description", System.Type.GetType("System.String"));
            DataColumn col06 = new DataColumn("Quantity", System.Type.GetType("System.String"));
            DataColumn col07 = new DataColumn("UnitPrice", System.Type.GetType("System.String"));
            table.Columns.Add(col01);
            table.Columns.Add(col02);
            table.Columns.Add(col03);
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
                row["CustomerNo"] = item.VendorNo;
                row["ItemID"] = item.Sub_ItemID;
                row["Description"] = item.Description;
                row["Quantity"] = item.Quantity;
                row["UnitPrice"] = item.UnitPrice;
                table.Rows.Add(row);
            }

            return table;
        }

        //
        // GET:/Vendors/OpenPurchaseOrder
        [NoCache]
        public PartialViewResult OpenSalesOrderbyItem(int? page, string id)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            string szMsg = "";

            PurchaseOrdersbyVendor purchaseorderbyvendor = null;
            List<PurchaseOrdersbyVendor> purchaseorderList = new List<PurchaseOrdersbyVendor>();

            TimelyDepotContext db01 = new TimelyDepotContext();

            //qryPOs = db.PurchaseOrders.Where(vd => vd.VendorId == id ).OrderByDescending(vd => vd.PurchaseOrderNo);

            var qryPOs = db.SalesOrderDetails.Join(db.SalesOrders, podet => podet.SalesOrderId, po => po.SalesOrderId, (podet, po)
                => new { podet, po }).Where(prod => prod.podet.ItemID == id && prod.podet.Sub_ItemID != null).OrderByDescending(prod => prod.po.SalesOrderId);
            if (qryPOs.Count() > 0)
            {
                foreach (var item in qryPOs)
                {
                    szMsg = item.po.PurchaseOrderNo;

                    purchaseorderbyvendor = new PurchaseOrdersbyVendor();
                    purchaseorderbyvendor.PurchaseOrderId = item.po.SalesOrderId;
                    purchaseorderbyvendor.PurchaseOrderNo = item.po.SalesOrderNo;
                    purchaseorderbyvendor.SODate = item.po.SODate;
                    purchaseorderbyvendor.VendorNo = GetCustomerNo(db01, item.po.CustomerId);
                    purchaseorderbyvendor.Sub_ItemID = item.podet.Sub_ItemID;
                    purchaseorderbyvendor.Description = item.podet.Description;
                    purchaseorderbyvendor.Quantity = item.podet.Quantity;
                    purchaseorderbyvendor.UnitPrice = item.podet.UnitPrice;

                    purchaseorderList.Add(purchaseorderbyvendor);
                }
            }

            ViewBag.SalesOrderId = id;

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

        private string GetCustomerNo(TimelyDepotContext db01, int? CustomerId)
        {
            int nCustomerId = 0;
            string szCustomerNo = "";

            Customers customer = null;

            if (CustomerId != null)
            {
                nCustomerId = Convert.ToInt32(CustomerId);
            }
            else
            {
                nCustomerId = 0;
            }

            customer = db01.Customers.Find(nCustomerId);
            if (customer != null)
            {
                szCustomerNo = customer.CustomerNo;
            }

            return szCustomerNo;
        }


        //
        // GET: /Vendors/OpenPurchaseOrderExcel
        public ActionResult OpenPurchaseOrderExcel(string id)
        {
            //DataTable hlpTbl = GetCustomerListTable();

            ExportCSV(GetOpenPurchaseOrderTable(id), "OpenPurchaseOrderList");

            return RedirectToAction("Index", "ReportsExcel");
        }


        private DataTable GetOpenPurchaseOrderTable(string szItemId)
        {
            long telHlp = 0;
            long faxHlp = 0;
            string telfmt = "000-000-0000";
            string szTel = "";
            string szMsg = "";

            DataTable table = null;
            DataRow row = null;

            PurchaseOrdersbyVendor purchaseorderbyvendor = null;
            List<PurchaseOrdersbyVendor> purchaseorderList = new List<PurchaseOrdersbyVendor>();

            //qryPOs = db.PurchaseOrders.Where(vd => vd.VendorId == id ).OrderByDescending(vd => vd.PurchaseOrderNo);

            var qryPOs = db.PurchasOrderDetails.Join(db.PurchaseOrders, podet => podet.PurchaseOrderId, po => po.PurchaseOrderId, (podet, po)
                => new { podet, po }).Where(prod => prod.podet.ItemID == szItemId && prod.podet.Sub_ItemID != null && prod.po.ReceiveStatus == null).OrderByDescending(prod => prod.po.PurchaseOrderId);
            if (qryPOs.Count() > 0)
            {
                foreach (var item in qryPOs)
                {
                    szMsg = item.po.PurchaseOrderNo;

                    purchaseorderbyvendor = new PurchaseOrdersbyVendor();
                    purchaseorderbyvendor.PurchaseOrderId = item.po.PurchaseOrderId;
                    purchaseorderbyvendor.PurchaseOrderNo = item.po.PurchaseOrderNo;
                    purchaseorderbyvendor.SODate = item.po.PODate;
                    purchaseorderbyvendor.VendorNo = item.po.VendorId;
                    purchaseorderbyvendor.Sub_ItemID = item.podet.Sub_ItemID;
                    purchaseorderbyvendor.Description = item.podet.Description;
                    purchaseorderbyvendor.Quantity = item.podet.Quantity;
                    purchaseorderbyvendor.UnitPrice = item.podet.UnitPrice;

                    purchaseorderList.Add(purchaseorderbyvendor);
                }
            }

            table = new DataTable("OpenPurchaseOrder");

            // Set the header
            DataColumn col01 = new DataColumn("PurchaseOrderNo", System.Type.GetType("System.String"));
            DataColumn col02 = new DataColumn("Date", System.Type.GetType("System.String"));
            DataColumn col03 = new DataColumn("VendorNo", System.Type.GetType("System.String"));
            DataColumn col04 = new DataColumn("ItemID", System.Type.GetType("System.String"));
            DataColumn col05 = new DataColumn("Description", System.Type.GetType("System.String"));
            DataColumn col06 = new DataColumn("Quantity", System.Type.GetType("System.String"));
            DataColumn col07 = new DataColumn("UnitPrice", System.Type.GetType("System.String"));
            table.Columns.Add(col01);
            table.Columns.Add(col02);
            table.Columns.Add(col03);
            table.Columns.Add(col04);
            table.Columns.Add(col05);
            table.Columns.Add(col06);
            table.Columns.Add(col07);

            //Set the data row
            foreach (var item in purchaseorderList)
            {
                row = table.NewRow();
                row["PurchaseOrderNo"] = item.PurchaseOrderNo;
                row["Date"] = item.SODate;
                row["VendorNo"] = item.VendorNo;
                row["ItemID"] = item.Sub_ItemID;
                row["Description"] = item.Description;
                row["Quantity"] = item.Quantity;
                row["UnitPrice"] = item.UnitPrice;
                table.Rows.Add(row);
            }

            return table;
        }

        //
        // GET:/Vendors/OpenPurchaseOrder
        [NoCache]
        public PartialViewResult OpenPurchaseOrderbyItem(int? page, string id)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            string szMsg = "";

            PurchaseOrdersbyVendor purchaseorderbyvendor = null;
            List<PurchaseOrdersbyVendor> purchaseorderList = new List<PurchaseOrdersbyVendor>();

            //qryPOs = db.PurchaseOrders.Where(vd => vd.VendorId == id ).OrderByDescending(vd => vd.PurchaseOrderNo);

            var qryPOs = db.PurchasOrderDetails.Join(db.PurchaseOrders, podet => podet.PurchaseOrderId, po => po.PurchaseOrderId, (podet, po)
                => new { podet, po }).Where(prod => prod.podet.ItemID == id && prod.podet.Sub_ItemID != null && prod.po.ReceiveStatus == null).OrderByDescending(prod => prod.po.PurchaseOrderId);
            if (qryPOs.Count() > 0)
            {
                foreach (var item in qryPOs)
                {
                    szMsg = item.po.PurchaseOrderNo;

                    purchaseorderbyvendor = new PurchaseOrdersbyVendor();
                    purchaseorderbyvendor.PurchaseOrderId = item.po.PurchaseOrderId;
                    purchaseorderbyvendor.PurchaseOrderNo = item.po.PurchaseOrderNo;
                    purchaseorderbyvendor.SODate = item.po.PODate;
                    purchaseorderbyvendor.VendorNo = item.po.VendorId;
                    purchaseorderbyvendor.Sub_ItemID = item.podet.Sub_ItemID;
                    purchaseorderbyvendor.Description = item.podet.Description;
                    purchaseorderbyvendor.Quantity = item.podet.Quantity;
                    purchaseorderbyvendor.UnitPrice = item.podet.UnitPrice;

                    purchaseorderList.Add(purchaseorderbyvendor);
                }
            }

            ViewBag.VendorNo = id;

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
        // GET: /Inventory/InventoryListExcel
        public ActionResult InventoryListExcel()
        {
            //DataTable hlpTbl = GetCustomerListTable();

            ExportCSV(GetInventoryListTable(), "InventoryList");

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

        private DataTable GetInventoryListTable()
        {
            long telHlp = 0;
            long faxHlp = 0;
            double dOnHand = 0;
            double dSO = 0;
            double dPO = 0;
            string telfmt = "000-000-0000";
            string szTel = "";

            DataTable table = null;
            DataRow row = null;

            SUB_ITEM thesubitemlist = null;
            List<SUB_ITEM> subitemList = new List<SUB_ITEM>();

            IQueryable<SUB_ITEM> qrySubitem = db.SUB_ITEM.OrderBy(sbit => sbit.Sub_ItemID);
            if (qrySubitem.Count() > 0)
            {
                foreach (var item in qrySubitem)
                {
                    subitemList.Add(item);
                }
            }

            table = new DataTable("InventoryList");

            // Set the header
            DataColumn col01 = new DataColumn("ItemNo", System.Type.GetType("System.String"));
            DataColumn col02 = new DataColumn("Description", System.Type.GetType("System.String"));
            DataColumn col03 = new DataColumn("OnHand", System.Type.GetType("System.String"));
            DataColumn col04 = new DataColumn("OpenOrder", System.Type.GetType("System.String"));
            DataColumn col05 = new DataColumn("OpenPO", System.Type.GetType("System.String"));
            table.Columns.Add(col01);
            table.Columns.Add(col02);
            table.Columns.Add(col03);
            table.Columns.Add(col04);
            table.Columns.Add(col05);

            //Set the data row
            foreach (var item in subitemList)
            {
                dOnHand = Convert.ToDouble(item.OnHand_Db) - Convert.ToDouble(item.OnHand_Cr);
                dSO = Convert.ToDouble(item.OpenSO_Db) - Convert.ToDouble(item.OpenSO_Cr);
                dPO = Convert.ToDouble(item.OpenPO_Db) - Convert.ToDouble(item.OpenPO_Cr);

                row = table.NewRow();
                row["ItemNo"] = item.Sub_ItemID;
                row["Description"] = item.Description;
                row["OnHand"] = dOnHand.ToString(); ;
                row["OpenOrder"] = dSO.ToString();
                row["OpenPO"] = dPO.ToString();
                table.Rows.Add(row);
            }

            return table;
        }

        // GET: /Inventory/InventoryList
        [NoCache]
        public PartialViewResult InventoryList(int? page)
        {

            int pageIndex = 0;
            int pageSize = PageSize;

            SUB_ITEM thesubitemlist = null;
            List<SUB_ITEM> subitemList = new List<SUB_ITEM>();

            IQueryable<SUB_ITEM> qrySubitem = db.SUB_ITEM.OrderBy(sbit => sbit.Sub_ItemID);
            if (qrySubitem.Count() > 0)
            {
                foreach (var item in qrySubitem)
                {
                    subitemList.Add(item);
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


            var onePageOfData = subitemList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return PartialView(subitemList.ToPagedList(pageIndex, pageSize));
        }


        //
        // GET /Inventory/
        public ActionResult CreateEditVendor01(string itemid, string vendorno, string Cost, string CostBlind, string RunCharge,
            string SetupCharge, string ReSetupCharge, string LeadTimeHrs, string LeadTimeMin, string LeadTimeSec, string LeadTime, int id = 0)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(vendoritem).State = EntityState.Modified;
            //    db.SaveChanges();
            //}
            string[] szHlp = null;
            TimeSpan timespan = TimeSpan.MinValue;

            VendorItem vendoritem = db.VendorItems.Find(id);
            if (vendoritem != null)
            {
                if (string.IsNullOrEmpty(Cost))
                {
                    Cost = "0";
                }
                if (string.IsNullOrEmpty(CostBlind))
                {
                    CostBlind = "0";
                }
                if (string.IsNullOrEmpty(RunCharge))
                {
                    RunCharge = "0";
                }
                if (string.IsNullOrEmpty(SetupCharge))
                {
                    SetupCharge = "0";
                }
                if (string.IsNullOrEmpty(ReSetupCharge))
                {
                    ReSetupCharge = "0";
                }
                if (string.IsNullOrEmpty(LeadTimeHrs))
                {
                    LeadTimeHrs = "0";
                }
                else
                {
                    if (LeadTimeHrs == "" || LeadTimeHrs == "undefined")
                    {
                        LeadTimeHrs = "0";
                    }
                }
                if (string.IsNullOrEmpty(LeadTimeMin))
                {
                    LeadTimeMin = "0";
                }
                else
                {
                    if (LeadTimeMin == "" || LeadTimeMin == "undefined")
                    {
                        LeadTimeMin = "0";
                    }
                }
                if (string.IsNullOrEmpty(LeadTimeSec))
                {
                    LeadTimeSec = "0";
                }
                else
                {
                    if (LeadTimeSec == "" || LeadTimeSec == "undefined")
                    {
                        LeadTimeSec = "0";
                    }
                }

                if (string.IsNullOrEmpty(LeadTime))
                {
                    LeadTime = string.Format("00:00:00");
                }

                vendoritem.UpdateDate = Convert.ToDateTime(DateTime.Now);

                vendoritem.VendorNo = vendorno;
                vendoritem.Cost = Convert.ToDecimal(Cost);
                vendoritem.CostBlind = Convert.ToDecimal(CostBlind);
                vendoritem.RunCharge = Convert.ToDecimal(RunCharge);
                vendoritem.SetupCharge = Convert.ToDecimal(SetupCharge);
                vendoritem.ReSetupCharge = Convert.ToDecimal(ReSetupCharge);
                vendoritem.LeadTimeHrs = Convert.ToInt32(LeadTimeHrs);
                if (Convert.ToInt32(LeadTimeHrs) > 23)
                {
                    vendoritem.LeadTimeHrs = 0;
                }
                vendoritem.LeadTimeMin = Convert.ToInt32(LeadTimeMin);
                if (Convert.ToInt32(LeadTimeMin) > 59)
                {
                    vendoritem.LeadTimeMin = 0;
                }
                vendoritem.LeadTimeSec = Convert.ToInt32(LeadTimeSec);
                if (Convert.ToInt32(LeadTimeSec) > 59)
                {
                    vendoritem.LeadTimeSec = 0;
                }

                szHlp = LeadTime.Split(':');
                if (szHlp.Length == 3)
                {
                    timespan = new TimeSpan(Convert.ToInt32(szHlp[0]), Convert.ToInt32(szHlp[1]), Convert.ToInt32(szHlp[2]));
                }
                else
                {
                    timespan = new TimeSpan(0, Convert.ToInt32(szHlp[0]), Convert.ToInt32(szHlp[0]));
                }
                vendoritem.LeadTime = timespan;

                db.Entry(vendoritem).State = EntityState.Modified;
                db.SaveChanges();
            }


            //TempData["ActiveTab"] = "3";


            TempData["EditVendor"] = "EditVendor";
            return RedirectToAction("Edit", new { id = itemid });
            //return RedirectToAction("CreateVendorTab", new { id = itemid });
        }

        //
        // POST /Inventory/
        [HttpPost]
        public ActionResult CreateEditVendor01(VendorItem vendoritem, string itemid)
        {
            if (ModelState.IsValid)
            {
                vendoritem.UpdateDate = Convert.ToDateTime(DateTime.Now);
                db.Entry(vendoritem).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Edit", new { id = itemid });
        }

        //
        // POST: /Inventory/UpdateSetupChargeDetail
        [HttpPost]
        public ActionResult UpdateSetupChargeDetail(ItemView itemview)
        {
            string szItemID = "";
            SetupChargeDetail setupchargedetail = null;

            if (itemview.setupchragedetail != null)
            {
                setupchargedetail = itemview.setupchragedetail;

                szItemID = setupchargedetail.itemid;

                db.Entry(setupchargedetail).State = EntityState.Modified;
                db.SaveChanges();
            }

            TempData["ActiveTab"] = "2";
            return RedirectToAction("Edit", new { id = szItemID });
        }

        //
        // GET: /Inventory/DeletePrice
        public ActionResult DeletePrice(int id = 0)
        {
            string szItemID = "";
            PRICE forprice = db.PRICEs.Find(id);
            if (forprice != null)
            {
                szItemID = forprice.Item;
                db.PRICEs.Remove(forprice);
                db.SaveChanges();
            }
            return RedirectToAction("Edit", new { id = szItemID });
        }


        //
        // GET: /Inventory/UpdateAddPrice
        public ActionResult UpdateAddPrice(string itemID, string discountcode, string qty, string grossprice, int id = 0)
        {
            short nQty = 0;
            decimal dcHlp = 0;
            PRICE forprice = null;

            if (id == 0)
            {
                forprice = new PRICE();
                forprice.Item = itemID;
                forprice.Discount_Code = discountcode;
                forprice.Description = qty;
                nQty = Convert.ToInt16(qty);
                forprice.Qty = nQty;
                dcHlp = Convert.ToDecimal(grossprice);
                forprice.thePrice = dcHlp;
                db.PRICEs.Add(forprice);
                db.SaveChanges();
            }
            else
            {
                forprice = db.PRICEs.Find(id);
                if (forprice != null)
                {
                    forprice.Discount_Code = discountcode;
                    forprice.Description = qty;
                    nQty = Convert.ToInt16(qty);
                    forprice.Qty = nQty;
                    dcHlp = Convert.ToDecimal(grossprice);
                    forprice.thePrice = dcHlp;
                    db.Entry(forprice).State = EntityState.Modified;
                    db.SaveChanges();

                    if (string.IsNullOrEmpty(itemID))
                    {
                        itemID = forprice.Item;
                    }
                }
            }

            return RedirectToAction("Edit", new { id = itemID });
        }

        //
        // GET: /Inventory/GetDiscount
        public static double GetDiscount(TimelyDepotContext db01, string szDiscount)
        {
            double dDiscount = 0;

            DiscountManage discountmanage = db01.DiscountManages.Where(dm => dm.DiscountName == szDiscount).FirstOrDefault<DiscountManage>();
            if (discountmanage != null)
            {
                dDiscount = Convert.ToDouble(discountmanage.DiscountPercentage) / 100;

            }

            return dDiscount;
        }

        //
        //
        public ActionResult UpdateAddSubitem(string itemID, string subitemId, string description, string onhand, string oSO, string oPO, string partno, string vendorstock, int id = 0)
        {
            double dHlp = 0;
            SUB_ITEM subitem = null;

            if (string.IsNullOrEmpty(onhand))
            {
                onhand = "0";
            }
            if (string.IsNullOrEmpty(oSO))
            {
                oSO = "0";
            }
            if (string.IsNullOrEmpty(oPO))
            {
                oPO = "0";
            }

            if (id == 0)
            {
                if (string.IsNullOrEmpty(onhand))
                {
                    onhand = "0";
                }
                if (string.IsNullOrEmpty(oSO))
                {
                    oSO = "0";
                }
                if (string.IsNullOrEmpty(oPO))
                {
                    oPO = "0";
                }
                if (string.IsNullOrEmpty(vendorstock))
                {
                    vendorstock = "0";
                }

                subitem = new SUB_ITEM();
                subitem.ItemID = itemID;
                subitem.Sub_ItemID = subitemId;
                subitem.Description = description;
                dHlp = Convert.ToDouble(onhand);
                subitem.OnHand_Db = dHlp;
                subitem.OnHand_Cr = 0;
                dHlp = Convert.ToDouble(oSO);
                subitem.OpenSO_Db = dHlp;
                subitem.OpenSO_Cr = 0;
                dHlp = Convert.ToDouble(oPO);
                subitem.OpenPO_Db = dHlp;
                subitem.OpenPO_Cr = 0;
                subitem.PartNo = partno;
                dHlp = Convert.ToDouble(vendorstock);
                subitem.VendorStock = dHlp;
                db.SUB_ITEM.Add(subitem);
                db.SaveChanges();
            }
            else
            {
                subitem = db.SUB_ITEM.Find(id);
                if (subitem != null)
                {
                    subitem.Sub_ItemID = subitemId;
                    subitem.Description = description;
                    dHlp = Convert.ToDouble(onhand);
                    subitem.OnHand_Db = dHlp;
                    subitem.OnHand_Cr = 0;
                    dHlp = Convert.ToDouble(oSO);
                    subitem.OpenSO_Db = dHlp;
                    subitem.OpenSO_Cr = 0;
                    dHlp = Convert.ToDouble(oPO);
                    subitem.OpenPO_Db = dHlp;
                    subitem.OpenPO_Cr = 0;
                    subitem.PartNo = partno;
                    dHlp = Convert.ToDouble(vendorstock);
                    subitem.VendorStock = dHlp;
                    db.Entry(subitem).State = EntityState.Modified;
                    db.SaveChanges();

                    if (string.IsNullOrEmpty(itemID))
                    {
                        itemID = subitem.ItemID;
                    }

                }
            }

            TempData["ActiveTab"] = "1";
            return RedirectToAction("Edit", new { id = itemID });
        }

        //
        // GET: /Inventory/DeleteVendor
        public ActionResult DeleteVendor(int id = 0)
        {
            string szItemID = "";
            VendorItem vendoritem = db.VendorItems.Find(id);
            if (vendoritem != null)
            {
                szItemID = vendoritem.ItemId;
                db.VendorItems.Remove(vendoritem);
                db.SaveChanges();
            }

            return RedirectToAction("Edit", new { id = szItemID });
            //return RedirectToAction("CreateVendorTab", new { id = szItemID });
        }

        //
        // GET: /Inventory/DeleteSubItem
        public ActionResult DeleteSubItem(int id = 0)
        {
            string szItemID = "";
            SUB_ITEM sub_item = db.SUB_ITEM.Find(id);
            szItemID = sub_item.ItemID;
            db.SUB_ITEM.Remove(sub_item);
            db.SaveChanges();
            TempData["ActiveTab"] = "1";
            return RedirectToAction("Edit", new { id = szItemID });
            //return RedirectToAction("Index");
        }


        //
        // POST: /Inventory/UpdateSubItem
        [HttpPost]
        public ActionResult UpdateSubItem(SUB_ITEM subitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subitem).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Edit", new { id = subitem.ItemID });
            }

            return RedirectToAction("Index");
        }

        //
        // GET: /Inventory/CreateVendor
        [NoCache]
        public PartialViewResult CreateEditSubItem(string szItemId, int Id = 0)
        {
            SUB_ITEM subitem = null;

            if (Id == 0)
            {
                subitem = new SUB_ITEM();
                subitem.ItemID = szItemId;
                subitem.Sub_ItemID = szItemId;
                db.SUB_ITEM.Add(subitem);
                db.SaveChanges();
            }
            else
            {
                subitem = db.SUB_ITEM.Find(Id);
            }

            //ViewBag.ItemId = szItemId;

            return PartialView(subitem);
        }

        //
        // POST: /Inventory/CreateEditVendor
        [HttpPost]
        [NoCache]
        public ActionResult CreateEditVendor(VendorItem vendoritem, string leadtimeHlp, string VendorNoselectedid, string itemselectedid)
        {
            int nId = 0;
            int nYear = 0;
            int nMonth = 0;
            int nDay = 0;
            int nMinutes = 0;
            int nSeconds = 0;
            string szMsg = "";
            string szId = "";
            string[] szLeadTime = null;
            DateTime dDate = DateTime.Now;

            VendorsContactAddress vendoraddres = null;

            if (ModelState.IsValid)
            {
                if (vendoritem.Id == 0)
                {
                    db.VendorItems.Add(vendoritem);
                    vendoritem.ItemId = itemselectedid;
                    vendoritem.VendorNo = VendorNoselectedid;
                    vendoritem.UpdateDate = DateTime.Now;
                    vendoritem.LeadTimeHrs = 0;
                    vendoritem.LeadTimeMin = 0;
                    vendoritem.LeadTimeSec = 0;
                }
                else
                {
                    szId = Request.Form["venaddId"];

                    if (!string.IsNullOrEmpty(szId))
                    {
                        nId = Convert.ToInt32(szId);
                        vendoraddres = db.VendorsContactAddresses.Find(nId);
                        if (vendoraddres != null)
                        {
                            vendoraddres.CompanyName = Request.Form["CompanyName"];
                            vendoraddres.FirstName = Request.Form["FirstName"];
                            vendoraddres.LastName = Request.Form["LastName"];
                            vendoraddres.Tel1 = Request.Form["Tel1"].Replace("-", "");
                            vendoraddres.Tel2 = Request.Form["Tel2"].Replace("-", "");
                            vendoraddres.Tel = Request.Form["Tel3"].Replace("-", "");
                            vendoraddres.Fax = Request.Form["Fax"].Replace("-", "");
                            vendoraddres.Address = Request.Form["Addres1"];
                            vendoraddres.Note = Request.Form["Addres2"];
                            vendoraddres.Address3 = Request.Form["Addres3"];
                            vendoraddres.City = Request.Form["City"];
                            vendoraddres.State = Request.Form["State"];
                            vendoraddres.Zip = Request.Form["Zip"];
                            vendoraddres.Country = Request.Form["Country"];
                            vendoraddres.Email = Request.Form["Email"];
                            vendoraddres.Website = Request.Form["Website"];

                            db.Entry(vendoraddres).State = EntityState.Modified;

                        }
                    }


                }
                db.SaveChanges();
                //return RedirectToAction("CreateVendorTab", new { id = vendoritem.ItemId });

                TempData["EditVendor"] = "EditVendor";
                return RedirectToAction("Edit", new { id = vendoritem.ItemId });
                //return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        //
        // GET: /Inventory/CreateVendor
        [NoCache]
        public PartialViewResult CreateVendor(string szItemId, string szVendorNo, int Id = 0)
        {
            string szLeadTime = "";
            DateTime dDate = DateTime.Now;

            VendorsContactAddress vendorcontactaddres = null;
            VendorItem vendoritem = null;
            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();


            if (Id == 0 && string.IsNullOrEmpty(szVendorNo))
            {
                //vendoritem = new VendorItem();
                //vendoritem.ItemId = szItemId;
                //db.VendorItems.Add(vendoritem);
                //db.SaveChanges();
            }
            else
            {
                if (string.IsNullOrEmpty(szVendorNo))
                {
                    vendoritem = db.VendorItems.Find(Id);
                    if (vendoritem != null)
                    {
                        var qryVendor = db.VendorsContactAddresses.Join(db.Vendors, vdca => vdca.VendorId, vd => vd.Id, (vdca, vd)
                            => new { vdca, vd }).Where(Nvdca => Nvdca.vd.VendorNo == vendoritem.VendorNo).OrderBy(Nvdca => Nvdca.vdca.CompanyName);
                        if (qryVendor.Count() > 0)
                        {
                            foreach (var item in qryVendor)
                            {
                                vendorcontactaddres = item.vdca;
                                if (vendorcontactaddres != null)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    vendoritem = db.VendorItems.Where(vdit => vdit.Id == Id && vdit.VendorNo == szVendorNo).FirstOrDefault<VendorItem>();
                    if (vendoritem != null)
                    {
                        var qryVendor = db.VendorsContactAddresses.Join(db.Vendors, vdca => vdca.VendorId, vd => vd.Id, (vdca, vd)
                            => new { vdca, vd }).Where(Nvdca => Nvdca.vd.VendorNo == vendoritem.VendorNo).OrderBy(Nvdca => Nvdca.vdca.CompanyName);
                        if (qryVendor.Count() > 0)
                        {
                            foreach (var item in qryVendor)
                            {
                                vendorcontactaddres = item.vdca;
                                if (vendorcontactaddres != null)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            //Fix null values
            if (vendorcontactaddres == null)
            {
                vendorcontactaddres = new VendorsContactAddress();
            }
            if (vendorcontactaddres.CompanyName == null)
            {
                vendorcontactaddres.CompanyName = string.Empty;
            }
            if (vendorcontactaddres.FirstName == null)
            {
                vendorcontactaddres.FirstName = string.Empty;
            }
            if (vendorcontactaddres.LastName == null)
            {
                vendorcontactaddres.LastName = string.Empty;
            }
            if (vendorcontactaddres.Tel == null)
            {
                vendorcontactaddres.Tel = "0";
            }
            if (vendorcontactaddres.Tel1 == null)
            {
                vendorcontactaddres.Tel1 = "0";
            }
            if (vendorcontactaddres.Tel2 == null)
            {
                vendorcontactaddres.Tel2 = "0";
            }
            if (vendorcontactaddres.Address == null)
            {
                vendorcontactaddres.Address = string.Empty;
            }
            if (vendorcontactaddres.Note == null)
            {
                vendorcontactaddres.Note = string.Empty;
            }
            if (vendorcontactaddres.Address3 == null)
            {
                vendorcontactaddres.Address3 = string.Empty;
            }
            if (vendorcontactaddres.City == null)
            {
                vendorcontactaddres.City = string.Empty;
            }
            if (vendorcontactaddres.State == null)
            {
                vendorcontactaddres.State = string.Empty;
            }
            if (vendorcontactaddres.Zip == null)
            {
                vendorcontactaddres.Zip = string.Empty;
            }
            if (vendorcontactaddres.Country == null)
            {
                vendorcontactaddres.Country = string.Empty;
            }
            if (vendorcontactaddres.Email == null)
            {
                vendorcontactaddres.Email = string.Empty;
            }
            if (vendorcontactaddres.Website == null)
            {
                vendorcontactaddres.Website = string.Empty;
            }
            if (vendorcontactaddres.Title == null)
            {
                vendorcontactaddres.Title = string.Empty;
            }
            if (vendorcontactaddres.Fax == null)
            {
                vendorcontactaddres.Fax = "0";
            }

            ViewBag.VendorContactAddressHlp = vendorcontactaddres;

            //dDate = Convert.ToDateTime(vendoritem.LeadTime);
            //szLeadTime = string.Format("{0}:{1}", dDate.Minute.ToString("D2"), dDate.Second.ToString("D2"));
            //ViewBag.LeadTime = szLeadTime;

            //Delete unused VendorItem records
            //if (vendoritem != null)
            //{
            //    DeleteUnusedVendorItem(vendoritem.Id, vendoritem.ItemId);

            //}


            //Get the vendors dropdown data
            ViewBag.VendorItemListHas = 0;

            int nYear = 0;
            int nMonth = 0;
            int nDay = 0;
            string szMsg = "";
            ITEM item01 = null;
            item01 = db.ITEMs.Where(itm => itm.ItemID == szItemId).FirstOrDefault<ITEM>();

            IQueryable<VendorItem> qryvendoritem = null;
            vendoritem = null;

            List<VendorItem> vendoritemList = new List<VendorItem>();
            listSelector = new List<KeyValuePair<string, string>>();
            //qryvendoritem = db.VendorItems.Where(cutadd => cutadd.ItemId == item.ItemID).OrderBy(cutadd => cutadd.VendorNo);
            qryvendoritem = db.VendorItems.Where(cutadd => cutadd.ItemId == item01.ItemID && cutadd.VendorNo != null).OrderBy(cutadd => cutadd.VendorNo);
            if (qryvendoritem.Count() > 0)
            {
                //custSalesContact = qryCusSal.FirstOrDefault<CustomersSalesContact>();
                foreach (var vendoritemHlp in qryvendoritem)
                {
                    vendoritemList.Add(vendoritemHlp);

                    var qryVendor = db.VendorsContactAddresses.Join(db.Vendors, vdca => vdca.VendorId, vd => vd.Id, (vdca, vd)
                        => new { vdca, vd }).Where(Nvdca => Nvdca.vd.VendorNo == vendoritemHlp.VendorNo).OrderBy(Nvdca => Nvdca.vdca.CompanyName);
                    if (qryVendor.Count() > 0)
                    {
                        foreach (var itemVendor in qryVendor)
                        {
                            szMsg = string.Format("{0} - {1} -({2})", itemVendor.vdca.CompanyName, itemVendor.vd.VendorNo, vendoritemHlp.Id.ToString());
                            listSelector.Add(new KeyValuePair<string, string>(itemVendor.vd.VendorNo, szMsg));
                            //listSelector.Add(new KeyValuePair<string, string>(vendoritemHlp.Id.ToString(), vendoritemHlp.VendorNo));
                            if (vendoritem == null)
                            {
                                vendoritem = vendoritemHlp;
                                ViewBag.VendorItemID = vendoritem.Id;
                            }
                        }
                    }

                }
            }
            else
            {
                //Get the first vendoritem for this item
                vendoritem = db.VendorItems.Where(vdit => vdit.ItemId == item01.ItemID).FirstOrDefault<VendorItem>();
                if (vendoritem != null)
                {
                    vendoritemList.Add(vendoritem);
                    ViewBag.VendorItemID = vendoritem.Id;
                }
                else
                {
                    //nYear = dDate.Year;
                    //nMonth = dDate.Month;
                    //nDay = dDate.Day;
                    //dDate = new DateTime(nYear, nMonth, nDay, 0, 0, 0);
                    //vendoritem = new VendorItem();
                    //vendoritem.ItemId = item01.ItemID;
                    ////vendoritem.LeadTime = dDate;
                    //db.VendorItems.Add(vendoritem);
                    //db.SaveChanges();
                    //vendoritemList.Add(vendoritem);
                    //ViewBag.VendorItemID = vendoritem.Id;
                }
            }
            SelectList vendorItemlist = new SelectList(listSelector, "Key", "Value");
            ViewBag.VendorItemList = vendorItemlist;
            ViewBag.VendorItemListHas = listSelector.Count;

            //Get the VendorNo select list
            listSelector = new List<KeyValuePair<string, string>>();
            var qryVendorHlp = db.VendorsContactAddresses.Join(db.Vendors, vdca => vdca.VendorId, vd => vd.Id, (vdca, vd)
                => new { vdca, vd }).OrderBy(Nvdca => Nvdca.vdca.CompanyName);
            if (qryVendorHlp.Count() > 0)
            {
                foreach (var itemAllVendors in qryVendorHlp)
                {
                    szMsg = string.Format("{0} - {1}", itemAllVendors.vdca.CompanyName, itemAllVendors.vd.VendorNo);
                    listSelector.Add(new KeyValuePair<string, string>(itemAllVendors.vd.VendorNo, szMsg));
                }
            }
            SelectList vendornolist = new SelectList(listSelector, "Key", "Value");
            ViewBag.VendorNoList = vendornolist;

            if (vendoritem == null)
            {
                ViewBag.bVendorItemData = false;
            }
            else
            {
                ViewBag.bVendorItemData = true;
            }

            return PartialView(vendoritem);
        }

        private void DeleteUnusedVendorItem(int nVendoritemId, string szItemId)
        {
            TimelyDepotContext db01 = new TimelyDepotContext();

            VendorItem vendoritem = null;
            IQueryable<VendorItem> qryVendorItem = db.VendorItems.Where(vdit => vdit.ItemId == szItemId && vdit.Id != nVendoritemId);
            if (qryVendorItem.Count() > 0)
            {
                foreach (var item in qryVendorItem)
                {
                    vendoritem = db01.VendorItems.Find(item.Id);
                    if (vendoritem != null)
                    {
                        db01.VendorItems.Remove(vendoritem);
                    }
                }
                db01.SaveChanges();
            }
        }

        //
        // GetVendorName
        public static string GetVendorName(TimelyDepotContext db, string szVendorNo)
        {
            string szName = "";

            IQueryable<Vendors> qryVendor = null;
            IQueryable<VendorsContactAddress> qryVendorAddres = null;

            Vendors vendor = null;
            VendorsContactAddress vendoraddress = null;

            qryVendor = db.Vendors.Where(vdr => vdr.VendorNo == szVendorNo);
            if (qryVendor.Count() > 0)
            {
                vendor = qryVendor.FirstOrDefault<Vendors>();
                if (vendor != null)
                {
                    qryVendorAddres = db.VendorsContactAddresses.Where(vad => vad.VendorId == vendor.Id);
                    if (qryVendorAddres.Count() > 0)
                    {
                        vendoraddress = qryVendorAddres.FirstOrDefault<VendorsContactAddress>();
                        if (vendoraddress != null)
                        {
                            szName = vendoraddress.CompanyName;
                        }
                    }
                }
            }

            return szName;
        }

        //
        // POST: /Inventory/UpdateItemInfo
        [HttpPost]
        [NoCache]
        [ValidateInput(false)]
        public ActionResult UpdateItemInfo(ItemView itemview)
        {
            ITEM item = null;

            if (itemview.item != null)
            {
                if (!string.IsNullOrEmpty(itemview.item.ItemID))
                {
                    item = db.ITEMs.Find(itemview.item.ItemID);
                    if (item != null)
                    {
                        item.CaseDimensionH = itemview.item.CaseDimensionH;
                        item.CaseDimensionL = itemview.item.CaseDimensionL;
                        item.CaseDimensionW = itemview.item.CaseDimensionW;
                        item.CaseWeight = itemview.item.CaseWeight;
                        item.DimensionD = itemview.item.DimensionD;
                        item.DimensionH = itemview.item.DimensionH;
                        item.DimensionL = itemview.item.DimensionL;
                        item.UnitPerCase = itemview.item.UnitPerCase;
                        item.UnitWeight = itemview.item.UnitWeight;
                        item.Note = itemview.item.Note;
                        //item.ClassNo = itemview.item.ClassNo;
                        //item.CollectionID = itemview.item.CollectionID;
                        //item.DeptoNo = itemview.item.DeptoNo;
                        //item.DescA = itemview.item.DescA;
                        //item.DescB = itemview.item.DescB;
                        //item.DialType = itemview.item.DialType;
                        //item.Inactive = itemview.item.Inactive;
                        //item.Keywords = itemview.item.Keywords;
                        //item.Misc_ID = itemview.item.Misc_ID;
                        //item.New = itemview.item.New;

                        //item.Pic2ID = itemview.item.Pic2ID;
                        //item.Pic3ID = itemview.item.Pic3ID;
                        //item.PicID = itemview.item.PicID;
                        //item.Price_ID = itemview.item.Price_ID;
                        //item.Special = itemview.item.Special;
                        //item.Status = itemview.item.Status;
                        //item.title = itemview.item.title;
                        //item.UPCCode = itemview.item.UPCCode;
                        //item.YearProduct = itemview.item.YearProduct;

                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                }
            }

            TempData["ActiveTab"] = "3";
            return RedirectToAction("Edit", new { id = item.ItemID });
            //return RedirectToAction("Index");
        }

        //
        // POST: /Inventory/UpdateItem
        [HttpPost]
        [NoCache]
        [ValidateInput(false)]
        public ActionResult UpdateItem(ItemView itemview)
        {
            ITEM item = null;

            if (itemview.item != null)
            {
                if (!string.IsNullOrEmpty(itemview.item.ItemID))
                {
                    item = db.ITEMs.Find(itemview.item.ItemID);
                    if (item != null)
                    {
                        item.CaseDimensionH = itemview.item.CaseDimensionH;
                        item.CaseDimensionL = itemview.item.CaseDimensionL;
                        item.CaseDimensionW = itemview.item.CaseDimensionW;
                        item.CaseWeight = itemview.item.CaseWeight;
                        item.ClassNo = itemview.item.ClassNo;
                        item.CollectionID = itemview.item.CollectionID;
                        item.DeptoNo = itemview.item.DeptoNo;
                        item.DescA = itemview.item.DescA;
                        item.DescB = itemview.item.DescB;
                        item.DialType = itemview.item.DialType;
                        item.DimensionD = itemview.item.DimensionD;
                        item.DimensionH = itemview.item.DimensionH;
                        item.DimensionL = itemview.item.DimensionL;
                        item.Inactive = itemview.item.Inactive;
                        item.Keywords = itemview.item.Keywords;
                        item.Misc_ID = itemview.item.Misc_ID;
                        item.New = itemview.item.New;
                        item.Note = itemview.item.Note;
                        item.Pic2ID = itemview.item.Pic2ID;
                        item.Pic3ID = itemview.item.Pic3ID;
                        item.PicID = itemview.item.PicID;
                        item.Price_ID = itemview.item.Price_ID;
                        item.Special = itemview.item.Special;
                        item.Status = itemview.item.Status;
                        item.title = itemview.item.title;
                        item.UnitPerCase = itemview.item.UnitPerCase;
                        item.UnitWeight = itemview.item.UnitWeight;
                        item.UPCCode = itemview.item.UPCCode;
                        item.YearProduct = itemview.item.YearProduct;

                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                }
            }

            return RedirectToAction("Edit", new { id = item.ItemID });
            //return RedirectToAction("Index");
        }

        //
        // GET: /Inventory/

        public ActionResult Index(int? page, string searchItem, string ckActive, string ckCriteria)
        {
            int nPos = -1;
            int pageIndex = 0;
            int pageSize = PageSize;
            ITEM itempart = null;
            IQueryable<ITEM> qryItem = null;
            IQueryable<SUB_ITEM> qrySubItem = null;
            List<string> listitems = new List<string>();
            List<ITEM> ITEMList = new List<ITEM>();

            if (string.IsNullOrEmpty(searchItem) || searchItem == "0")
            {
                //qryItem = db.ITEMs.OrderBy(it => it.ItemID);
                ViewBag.ckActiveHlp = "true";
                ViewBag.ckCriteriaHlp = "item";
                if (searchItem == "0")
                {
                    ViewBag.SearchItem = searchItem;

                    if (ckCriteria == "item")
                    {
                        if (ckActive == "true")
                        {
                            qryItem = db.ITEMs.Where(it => it.Status == false).OrderBy(it => it.ItemID);
                        }
                        else
                        {
                            qryItem = db.ITEMs.Where(it => it.Status == true).OrderBy(it => it.ItemID);
                        }

                        //Display the data
                    }

                }
            }
            else
            {
                ViewBag.SearchItem = searchItem;
                ViewBag.ckActiveHlp = ckActive;
                ViewBag.ckCriteriaHlp = ckCriteria;
                if (ckCriteria == "item")
                {
                    if (ckActive == "true")
                    {
                        qryItem = db.ITEMs.Where(it => it.ItemID.StartsWith(searchItem) && it.Status == false).OrderBy(it => it.ItemID);
                    }
                    else
                    {
                        qryItem = db.ITEMs.Where(it => it.ItemID.StartsWith(searchItem) && it.Status == true).OrderBy(it => it.ItemID);
                    }
                }
                if (ckCriteria == "depart")
                {
                    if (ckActive == "true")
                    {
                        qryItem = db.ITEMs.Where(it => it.DeptoNo.StartsWith(searchItem) && it.Status == false).OrderBy(it => it.ItemID);
                    }
                    else
                    {
                        qryItem = db.ITEMs.Where(it => it.DeptoNo.StartsWith(searchItem) && it.Status == true).OrderBy(it => it.ItemID);
                    }
                }
                if (ckCriteria == "classno")
                {
                    if (ckActive == "true")
                    {
                        qryItem = db.ITEMs.Where(it => it.ClassNo.StartsWith(searchItem) && it.Status == false).OrderBy(it => it.ItemID);
                    }
                    else
                    {
                        qryItem = db.ITEMs.Where(it => it.ClassNo.StartsWith(searchItem) && it.Status == true).OrderBy(it => it.ItemID);
                    }
                }
                if (ckCriteria == "upccode")
                {
                    if (ckActive == "true")
                    {
                        qryItem = db.ITEMs.Where(it => it.UPCCode.StartsWith(searchItem) && it.Status == false).OrderBy(it => it.ItemID);
                    }
                    else
                    {
                        qryItem = db.ITEMs.Where(it => it.UPCCode.StartsWith(searchItem) && it.Status == true).OrderBy(it => it.ItemID);
                    }
                }
                if (ckCriteria == "part")
                {
                    if (ckActive == "true")
                    {
                        qrySubItem = db.SUB_ITEM.Where(sbit => sbit.PartNo.StartsWith(searchItem)).OrderBy(sbit => sbit.PartNo);
                        if (qrySubItem.Count() > 0)
                        {
                            foreach (var item in qrySubItem)
                            {
                                nPos = -1;
                                nPos = listitems.IndexOf(item.ItemID);
                                if (nPos == -1)
                                {
                                    listitems.Add(item.ItemID);
                                }

                            }
                        }

                        if (listitems.Count > 0)
                        {
                            foreach (var item in listitems)
                            {
                                itempart = db.ITEMs.Find(item);
                                if (itempart != null)
                                {
                                    if (itempart.Status == false)
                                    {
                                        ITEMList.Add(itempart);                                        
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        qrySubItem = db.SUB_ITEM.Where(sbit => sbit.PartNo.StartsWith(searchItem)).OrderBy(sbit => sbit.PartNo);
                        if (qrySubItem.Count() > 0)
                        {
                            foreach (var item in qrySubItem)
                            {
                                nPos = -1;
                                nPos = listitems.IndexOf(item.ItemID);
                                if (nPos == -1)
                                {
                                    listitems.Add(item.ItemID);
                                }

                            }
                        }

                        if (listitems.Count > 0)
                        {
                            foreach (var item in listitems)
                            {
                                itempart = db.ITEMs.Find(item);
                                if (itempart != null)
                                {
                                    if (itempart.Status == true)
                                    {
                                        ITEMList.Add(itempart);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (qryItem != null)
            {
                if (qryItem.Count() > 0)
                {
                    foreach (var item in qryItem)
                    {
                        ITEMList.Add(item);
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


            var onePageOfData = ITEMList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            //return View(ITEMList.ToPagedList(pageIndex, pageSize));
            //return View(db.ITEMs.ToList());

            //Authorize user
            if (User.IsInRole("Owner"))
            {
                return View(ITEMList.ToPagedList(pageIndex, pageSize));
            }
            if (User.IsInRole("Admin"))
            {
                return View(ITEMList.ToPagedList(pageIndex, pageSize));
            }


            //return View(ITEMList.ToPagedList(pageIndex, pageSize));
            return RedirectToAction("LogOn", "Account");

        }

        //
        // GET: /Inventory/Details/5

        public ActionResult Details(string id = null)
        {
            ITEM item = db.ITEMs.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        //
        // GET: /Inventory/Create

        public PartialViewResult Create()
        {
            ITEM item = new ITEM();

            return PartialView(item);
        }

        //
        // POST: /Inventory/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(ITEM item)
        {
            if (ModelState.IsValid)
            {
                db.ITEMs.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(item);
        }

        //
        // GET: /Inventory/Edit/5
        [NoCache]
        public ActionResult Edit(string modomultiple, int? page, string id = null)
        {
            int pageIndex = 0;
            int pageSize = 50;
            int PageVendor = 0;
            int pageSubItem = 0;
            int nYear = 0;
            int nMonth = 0;
            int nDay = 0;
            string szPicPath = "";
            string szMsg = "";
            DateTime dDate = DateTime.Now;

            TimelyDepotContext db01 = new TimelyDepotContext();

            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            IQueryable<VendorItem> qryvendoritem = null;
            IQueryable<SUB_ITEM> qrySubItem = null;
            IQueryable<PRICE> qryForPrice = null;
            IQueryable<DiscountManage> qryDisc = null;
            IQueryable<Deptos> qryDepto = null;
            IQueryable<ClssNos> qryClas = null;
            IQueryable<YearProducts> qryYearProduct = null;

            VendorsContactAddress vendorcontactadress = null;
            VendorItem vendoritem = null;
            SUB_ITEM subitem = null;
            PRICE forprice = null;

            List<VendorItem> vendoritemList = new List<VendorItem>();
            List<SUB_ITEM> subitemList = new List<SUB_ITEM>();
            List<PRICE> forpriceList = new List<PRICE>();


            ITEM item = db.ITEMs.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            //Replace new line
            item.DescA = item.DescA.Replace("\r\n", "<br />");
            item.DescB = item.DescB.Replace("\r\n", "<br />");

            //Set the image path
            szPicPath = string.Format("~/Images/timely/Small_Pic/{0}.gif", item.ItemID);
            ViewBag.PicPath = Url.Content(szPicPath);

            listSelector = new List<KeyValuePair<string, string>>();
            //qryvendoritem = db.VendorItems.Where(cutadd => cutadd.ItemId == item.ItemID).OrderBy(cutadd => cutadd.VendorNo);
            qryvendoritem = db.VendorItems.Where(cutadd => cutadd.ItemId == item.ItemID && cutadd.VendorNo != null).OrderBy(cutadd => cutadd.VendorNo);
            if (qryvendoritem.Count() > 0)
            {
                //custSalesContact = qryCusSal.FirstOrDefault<CustomersSalesContact>();
                foreach (var vendoritemHlp in qryvendoritem)
                {
                    vendoritemList.Add(vendoritemHlp);

                    var qryVendor = db01.VendorsContactAddresses.Join(db01.Vendors, vdca => vdca.VendorId, vd => vd.Id, (vdca, vd)
                        => new { vdca, vd }).Where(Nvdca => Nvdca.vd.VendorNo == vendoritemHlp.VendorNo).OrderBy(Nvdca => Nvdca.vdca.CompanyName);
                    if (qryVendor.Count() > 0)
                    {
                        foreach (var itemVendor in qryVendor)
                        {
                            szMsg = string.Format("{0} - {1} -({2})", itemVendor.vdca.CompanyName, itemVendor.vd.VendorNo, vendoritemHlp.Id.ToString());
                            listSelector.Add(new KeyValuePair<string, string>(itemVendor.vd.VendorNo, szMsg));
                            //listSelector.Add(new KeyValuePair<string, string>(vendoritemHlp.Id.ToString(), vendoritemHlp.VendorNo));
                            if (vendoritem == null)
                            {
                                vendoritem = vendoritemHlp;
                            }

                        }
                    }

                }
            }
            else
            {
                //nYear = dDate.Year;
                //nMonth = dDate.Month;
                //nDay = dDate.Day;
                //dDate = new DateTime(nYear, nMonth, nDay, 0, 0, 0);
                //vendoritem = new VendorItem();
                //vendoritem.ItemId = item.ItemID;
                //db.VendorItems.Add(vendoritem);
                //db.SaveChanges();
                //vendoritemList.Add(vendoritem);
            }
            SelectList vendorItemlist = new SelectList(listSelector, "Key", "Value");
            ViewBag.VendorItemList = vendorItemlist;
            ViewBag.VendorItemListHas = listSelector.Count;



            qrySubItem = db.SUB_ITEM.Where(cutadd => cutadd.ItemID == item.ItemID).OrderBy(cutadd => cutadd.Description);
            if (qrySubItem.Count() > 0)
            {
                //custSalesContact = qryCusSal.FirstOrDefault<CustomersSalesContact>();
                foreach (var vendoritemHlp in qrySubItem)
                {
                    subitemList.Add(vendoritemHlp);
                }
            }
            else
            {
                subitem = new SUB_ITEM();
                subitem.ItemID = item.ItemID;
                db.SUB_ITEM.Add(subitem);
                db.SaveChanges();
                subitemList.Add(subitem);
            }

            //Get the setupforprice data 
            qryForPrice = db.PRICEs.Where(FrPr => FrPr.Item == item.ItemID).OrderBy(FrPr => FrPr.Qty);
            if (qryForPrice.Count() > 0)
            {
                foreach (var itemForPrice in qryForPrice)
                {
                    forpriceList.Add(itemForPrice);
                }

            }
            else
            {
                forprice = new PRICE();
                forprice.Item = item.ItemID;
                forprice.Discount_Code = "R";
                db.PRICEs.Add(forprice);
                db.SaveChanges();
                forpriceList.Add(forprice);
            }



            //Get the setupcagrdedetail
            SetupChargeDetail setupchargedetail = db.SetupChargeDetails.Where(stchd => stchd.itemid == item.ItemID).FirstOrDefault<SetupChargeDetail>();
            if (setupchargedetail == null)
            {
                setupchargedetail = new SetupChargeDetail();
                setupchargedetail.itemid = item.ItemID;
                setupchargedetail.ReSetupCharge = 0;
                setupchargedetail.ReSetupChargeDiscountCode = "V";
                setupchargedetail.SetUpCharge = 0;
                setupchargedetail.SetupChargeDiscountCode = "V";
                setupchargedetail.RunCharge = 0;
                setupchargedetail.RunChargeDiscountCode = "V";
                db.SetupChargeDetails.Add(setupchargedetail);
                db.SaveChanges();
            }

            //Get the VendorNo select list
            listSelector = new List<KeyValuePair<string, string>>();
            var qryVendorHlp = db.VendorsContactAddresses.Join(db.Vendors, vdca => vdca.VendorId, vd => vd.Id, (vdca, vd)
                => new { vdca, vd }).OrderBy(Nvdca => Nvdca.vdca.CompanyName);
            if (qryVendorHlp.Count() > 0)
            {
                foreach (var itemAllVendors in qryVendorHlp)
                {
                    szMsg = string.Format("{0} - {1}", itemAllVendors.vdca.CompanyName, itemAllVendors.vd.VendorNo);
                    listSelector.Add(new KeyValuePair<string, string>(itemAllVendors.vd.VendorNo, szMsg));
                }
            }
            SelectList vendornolist = new SelectList(listSelector, "Key", "Value");
            ViewBag.VendorNoList = vendornolist;

            //Get theActive/Inactive list
            listSelector = new List<KeyValuePair<string, string>>();
            listSelector.Add(new KeyValuePair<string, string>("Y", "Active"));
            listSelector.Add(new KeyValuePair<string, string>("N", "Inactive"));
            SelectList activeinactivelist = new SelectList(listSelector, "Key", "Value");
            ViewBag.ActiveInactivelist = activeinactivelist;

            //Get the discount code
            listSelector = new List<KeyValuePair<string, string>>();
            qryDisc = db.DiscountManages.OrderBy(dimg => dimg.DiscountName);
            if (qryDisc.Count() > 0)
            {
                foreach (var itemdiscount in qryDisc)
                {
                    listSelector.Add(new KeyValuePair<string, string>(itemdiscount.DiscountName, itemdiscount.DiscountName));
                }
            }
            SelectList discountlist = new SelectList(listSelector, "Key", "Value");
            ViewBag.DiscountList = discountlist;

            //Get the 1st set up
            listSelector = new List<KeyValuePair<string, string>>();
            szMsg = "Yes";
            listSelector.Add(new KeyValuePair<string, string>("1", szMsg));
            szMsg = "No";
            listSelector.Add(new KeyValuePair<string, string>("0", szMsg));
            SelectList yesnolist = new SelectList(listSelector, "Key", "Value");
            ViewBag.YesNoList = yesnolist;

            //if (vendoritem == null)
            //{
            //    vendoritem = new VendorItem();
            //    vendoritem.UpdateDate = DateTime.Now;
            //    vendoritem.LeadTimeHrs = 0;
            //    vendoritem.LeadTimeMin = 0;
            //    vendoritem.LeadTimeSec = 0;
            //}
            //if (vendoritem.UpdateDate == null)
            //{
            //    vendoritem.UpdateDate = DateTime.Now;
            //}
            //if (vendoritem.LeadTimeHrs == null)
            //{
            //    vendoritem.LeadTimeHrs = 0;
            //}
            //if (vendoritem.LeadTimeMin == null)
            //{
            //    vendoritem.LeadTimeMin = 0;
            //}
            //if (vendoritem.LeadTimeSec == null)
            //{
            //    vendoritem.LeadTimeSec = 0;
            //}



            ItemView itemview = new ItemView();
            itemview.item = item;
            itemview.vendoritem = vendoritem;
            itemview.vendorcontactaddress = vendorcontactadress;
            itemview.setupchragedetail = setupchargedetail;

            if (vendoritem == null)
            {
                ViewBag.VendorItemID = 0;
            }
            else
            {
                ViewBag.VendorItemID = vendoritem.Id;
            }

            //Get the Depto No List
            listSelector = new List<KeyValuePair<string, string>>();
            qryDepto = db.Deptos.OrderBy(dimg => dimg.DeptoNo);
            if (qryDepto.Count() > 0)
            {
                foreach (var itemdepto in qryDepto)
                {
                    listSelector.Add(new KeyValuePair<string, string>(itemdepto.DeptoNo, itemdepto.DeptoNo));
                }
            }
            SelectList deptolist = new SelectList(listSelector, "Key", "Value");
            ViewBag.DeptoList = deptolist;

            //Get the Class No List
            listSelector = new List<KeyValuePair<string, string>>();
            qryClas = db.ClssNos.OrderBy(dimg => dimg.ClssNo);
            if (qryClas.Count() > 0)
            {
                foreach (var itemdepto in qryClas)
                {
                    listSelector.Add(new KeyValuePair<string, string>(itemdepto.ClssNo, itemdepto.ClssNo));
                }
            }
            SelectList classlist = new SelectList(listSelector, "Key", "Value");
            ViewBag.ClassList = classlist;

            //Get the Year Product List
            listSelector = new List<KeyValuePair<string, string>>();
            qryYearProduct = db.YearProducts.OrderBy(dimg => dimg.YearofProducts);
            if (qryYearProduct.Count() > 0)
            {
                foreach (var itemdepto in qryYearProduct)
                {
                    listSelector.Add(new KeyValuePair<string, string>(itemdepto.YearofProducts, itemdepto.YearofProducts));
                }
            }
            SelectList yearproductlist = new SelectList(listSelector, "Key", "Value");
            ViewBag.YearProductList = yearproductlist;

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
                PageVendor = pageIndex;
                pageSubItem = pageIndex;
                //pageSales = pageIndex;
                //pageShipp = pageIndex;
            }
            else
            {
                switch (modomultiple)
                {
                    case "VendorItem":
                        PageVendor = pageIndex;
                        break;
                    case "SubItem":
                        pageSubItem = pageIndex;
                        break;
                    default:
                        break;
                }
            }


            var vendoritemListHlp = vendoritemList.ToPagedList(PageVendor, 1);
            ViewBag.OnePageOfvendoritemData = vendoritemListHlp;
            itemview.vendorsitemList = vendoritemListHlp;


            var subitemListHlp = subitemList.ToPagedList(pageSubItem, pageSize);
            ViewBag.OnePageOfsubitemData = vendoritemListHlp;
            itemview.subitemsList = subitemListHlp;

            var forpriceListHlp = forpriceList.ToPagedList(pageSubItem, pageSize);
            ViewBag.onePageofPriceData = forpriceListHlp;
            itemview.setupforpriceList = forpriceListHlp;

            //Set the active tab
            if (TempData["ActiveTab"] != null)
            {
                ViewBag.ActiveTab = TempData["ActiveTab"].ToString();
            }

            //Set the Edit Vendor action
            if (TempData["EditVendor"] != null)
            {
                if (TempData["EditVendor"].ToString() == "EditVendor")
                {
                    ViewBag.EditVendor = "EditVendor";
                }
            }

            return View(itemview);
        }

        ////
        //// POST: /Inventory/Edit/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[ValidateInput(false)]
        //public ActionResult Edit(ITEM item)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(item).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(item);
        //}

        //
        // GET: /Inventory/Delete/5

        public ActionResult Delete(string id = null)
        {
            ITEM item = db.ITEMs.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        //
        // POST: /Inventory/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ITEM item = db.ITEMs.Find(id);
            db.ITEMs.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public static string GetFirstVendorPartNo(TimelyDepotContext db01, string szItemId)
        {
            string szVendorPartNo = "";

            VendorItem vendoritem = db01.VendorItems.Where(vdit => vdit.ItemId == szItemId).FirstOrDefault<VendorItem>();
            if (vendoritem != null)
            {
                if (!string.IsNullOrEmpty(vendoritem.VendorPartNo))
                {
                    szVendorPartNo = vendoritem.VendorPartNo;

                }
            }

            return szVendorPartNo;
        }

        public static SelectList GetListVendorPartNo(TimelyDepotContext db01, string szItemId, ref string szVendorPartNo02)
        {
            IQueryable<SUB_ITEM> qrySubitem = db01.SUB_ITEM.Where(sbit => sbit.ItemID == szItemId).OrderBy(sbit => sbit.PartNo);

            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            if (qrySubitem.Count() > 0)
            {
                foreach (var item in qrySubitem)
                {
                    if (!string.IsNullOrEmpty(item.PartNo))
                    {
                        if (string.IsNullOrEmpty(szVendorPartNo02))
                        {
                            szVendorPartNo02 = item.PartNo;
                        }
                        else
                        {
                            szVendorPartNo02 = string.Format("{0} {1}", szVendorPartNo02, item.PartNo);
                        }
                        //listSelector.Add(new KeyValuePair<string, string>(item.PartNo, item.PartNo));                        
                    }
                }
            }

            SelectList listVendorPartNo = new SelectList(listSelector, "Key", "Value");

            return listVendorPartNo;
        }

        public static double GetTotalSOOutstanding(TimelyDepotContext db01, string szSubItemId)
        {
            double dSO = 0;

            dSO = Convert.ToDouble(db01.SalesOrderDetails.Where(sodt => sodt.Sub_ItemID == szSubItemId).Sum(sodt => sodt.Quantity));

            return dSO;
        }

        public static double GetTotalPOOutstanding(TimelyDepotContext db01, string szSubItemId)
        {
            double dPO = 0;

            dPO = Convert.ToDouble(db01.PurchasOrderDetails.Where(podt => podt.Sub_ItemID == szSubItemId).Sum(podt => podt.Quantity));

            return dPO;
        }
    }
}
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
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;

namespace TimelyDepotMVC.Controllers
{
    public class VendorsController : Controller
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
        // GET: /Vendors/PreviousItem
        public ActionResult PreviousVendor(string customerNo, string opcion, int id)
        {
            string szNextId = "";


            szNextId = GetPreviousVendor(customerNo);
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

        private string GetPreviousVendor(string customerNo)
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
                szConnString = connSettings["TimelyDepotContext"].ToString();
                sqlds.ConnectionString = szConnString;

                szSql = string.Format("SELECT TOP (100) PERCENT Id, VendorNo FROM Vendors " +
                    "WHERE (VendorNo < N'{0}') ORDER BY VendorNo DESC", customerNo);
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
                szNext = "";
                szError = exc.Message;
            }

            return szNext;
        }

        //
        // GET: /Vendors/NextItem
        public ActionResult NextVendor(string customerNo, string opcion, int id)
        {
            string szNextId = "";

            szNextId = GetNextVendor(customerNo);
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

        private string GetNextVendor(string id)
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
                //szNext = id;
                szConnString = connSettings["TimelyDepotContext"].ToString();
                sqlds.ConnectionString = szConnString;

                szSql = string.Format("SELECT TOP (100) PERCENT Id, VendorNo FROM Vendors " +
                    "WHERE (VendorNo > N'{0}') ORDER BY VendorNo", id);
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
                szNext = "";
                szError = exc.Message;
            }

            return szNext;
        }


        //
        // GET:/Vendors/ReceivedPurchaseOrderbyVendor
        [NoCache]
        public ActionResult ReceivedPurchaseOrderbyVendor(int? page, string id)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            string szMsg = "";
            string szVendorId = "";
            DateTime dDate = DateTime.Now;

            Vendors vendor = db.Vendors.Where(vd => vd.VendorNo == id).FirstOrDefault<Vendors>();
            if (vendor != null)
            {
                szVendorId = vendor.Id.ToString();
            }
            string szUrl = string.Format("~/Vendors/Edit/{0}", szVendorId);
            szUrl = Url.Content(szUrl);
            ViewBag.Quit = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, szUrl);

            PurchaseOrdersbyVendor purchaseorderbyvendor = null;
            List<PurchaseOrdersbyVendor> purchaseorderList = new List<PurchaseOrdersbyVendor>();

            //qryPOs = db.PurchaseOrders.Where(vd => vd.VendorId == id ).OrderByDescending(vd => vd.PurchaseOrderNo);

            var qryPOs = db.PurchasOrderDetails.Join(db.PurchaseOrders, podet => podet.PurchaseOrderId, po => po.PurchaseOrderId, (podet, po)
                => new { podet, po }).Where(prod => prod.po.VendorId == id && prod.podet.Sub_ItemID != null && prod.po.ReceiveStatus != null).OrderByDescending(prod => prod.po.PurchaseOrderId);
            if (qryPOs.Count() > 0)
            {
                foreach (var item in qryPOs)
                {
                    purchaseorderbyvendor = new PurchaseOrdersbyVendor();
                    if (!string.IsNullOrEmpty(item.po.ReceiveStatus))
                    {
                        szMsg = item.po.ReceiveStatus;
                        szMsg = szMsg.Replace("Received on ", "");
                        szMsg = szMsg.Replace(".", "");
                        dDate = Convert.ToDateTime(szMsg);
                        purchaseorderbyvendor.SODate = dDate;
                    }
                    else
                    {
                        szMsg = string.Empty;
                        purchaseorderbyvendor.SODate = null;
                    }
                    purchaseorderbyvendor.PurchaseOrderId = item.po.PurchaseOrderId;
                    purchaseorderbyvendor.PurchaseOrderNo = item.po.PurchaseOrderNo;
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

            ViewBag.ReceivedPO = true;

            var onePageOfData = purchaseorderList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View("OpenPurchaseOrderbyVendor", purchaseorderList.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /Vendors/OpenPurchaseOrderExcel
        public ActionResult ReceivedPurchaseOrderExcel(string id)
        {
            //DataTable hlpTbl = GetCustomerListTable();

            ExportCSV(GetReceivedPurchaseOrderTable(id), "ReceivedPurchaseOrderList");

            return RedirectToAction("Index", "ReportsExcel");
        }

        private DataTable GetReceivedPurchaseOrderTable(string szVendorNo)
        {
            long telHlp = 0;
            long faxHlp = 0;
            string telfmt = "000-000-0000";
            string szTel = "";
            string szMsg = "";
            DateTime dDate = DateTime.Now;
            DataTable table = null;
            DataRow row = null;

            PurchaseOrdersbyVendor purchaseorderbyvendor = null;
            List<PurchaseOrdersbyVendor> purchaseorderList = new List<PurchaseOrdersbyVendor>();

            //qryPOs = db.PurchaseOrders.Where(vd => vd.VendorId == id ).OrderByDescending(vd => vd.PurchaseOrderNo);

            var qryPOs = db.PurchasOrderDetails.Join(db.PurchaseOrders, podet => podet.PurchaseOrderId, po => po.PurchaseOrderId, (podet, po)
                => new { podet, po }).Where(prod => prod.po.VendorId == szVendorNo && prod.podet.Sub_ItemID != null && prod.po.ReceiveStatus != null).OrderByDescending(prod => prod.po.PurchaseOrderId);
            if (qryPOs.Count() > 0)
            {
                foreach (var item in qryPOs)
                {

                    purchaseorderbyvendor = new PurchaseOrdersbyVendor();
                    if (!string.IsNullOrEmpty(item.po.ReceiveStatus))
                    {
                        szMsg = item.po.ReceiveStatus;
                        szMsg = szMsg.Replace("Received on ", "");
                        szMsg = szMsg.Replace(".", "");
                        dDate = Convert.ToDateTime(szMsg);
                        purchaseorderbyvendor.SODate = dDate;
                    }
                    else
                    {
                        szMsg = string.Empty;
                        purchaseorderbyvendor.SODate = null;
                    }
                    purchaseorderbyvendor.PurchaseOrderId = item.po.PurchaseOrderId;
                    purchaseorderbyvendor.PurchaseOrderNo = item.po.PurchaseOrderNo;
                    //purchaseorderbyvendor.SODate = item.po.PODate;
                    purchaseorderbyvendor.Sub_ItemID = item.podet.Sub_ItemID;
                    purchaseorderbyvendor.Description = item.podet.Description;
                    purchaseorderbyvendor.Quantity = item.podet.Quantity;
                    purchaseorderbyvendor.UnitPrice = item.podet.UnitPrice;

                    purchaseorderList.Add(purchaseorderbyvendor);
                }
            }

            table = new DataTable("OReceivedPurchaseOrder");

            // Set the header
            DataColumn col01 = new DataColumn("PurchaseOrderNo", System.Type.GetType("System.String"));
            DataColumn col02 = new DataColumn("Date", System.Type.GetType("System.String"));
            DataColumn col03 = new DataColumn("ItemID", System.Type.GetType("System.String"));
            DataColumn col04 = new DataColumn("Description", System.Type.GetType("System.String"));
            DataColumn col05 = new DataColumn("Quantity", System.Type.GetType("System.String"));
            DataColumn col06 = new DataColumn("UnitPrice", System.Type.GetType("System.String"));
            table.Columns.Add(col01);
            table.Columns.Add(col02);
            table.Columns.Add(col03);
            table.Columns.Add(col04);
            table.Columns.Add(col05);
            table.Columns.Add(col06);

            //Set the data row
            foreach (var item in purchaseorderList)
            {
                row = table.NewRow();
                row["PurchaseOrderNo"] = item.PurchaseOrderNo;
                row["Date"] = item.SODate;
                row["ItemID"] = item.Sub_ItemID;
                row["Description"] = item.Description;
                row["Quantity"] = item.Quantity;
                row["UnitPrice"] = item.UnitPrice;
                table.Rows.Add(row);
            }

            return table;
        }

        //
        // GET: /Vendors/OpenPurchaseOrderExcel
        public ActionResult OpenPurchaseOrderExcel(string id)
        {
            //DataTable hlpTbl = GetCustomerListTable();

            ExportCSV(GetOpenPurchaseOrderTable(id), "OpenPurchaseOrderList");

            return RedirectToAction("Index", "ReportsExcel");
        }


        private DataTable GetOpenPurchaseOrderTable(string szVendorNo)
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
                => new { podet, po }).Where(prod => prod.po.VendorId == szVendorNo && prod.podet.Sub_ItemID != null && prod.po.ReceiveStatus == null).OrderByDescending(prod => prod.po.PurchaseOrderId);
            if (qryPOs.Count() > 0)
            {
                foreach (var item in qryPOs)
                {
                    szMsg = item.po.PurchaseOrderNo;

                    purchaseorderbyvendor = new PurchaseOrdersbyVendor();
                    purchaseorderbyvendor.PurchaseOrderId = item.po.PurchaseOrderId;
                    purchaseorderbyvendor.PurchaseOrderNo = item.po.PurchaseOrderNo;
                    purchaseorderbyvendor.SODate = item.po.PODate;
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
            DataColumn col03 = new DataColumn("ItemID", System.Type.GetType("System.String"));
            DataColumn col04 = new DataColumn("Description", System.Type.GetType("System.String"));
            DataColumn col05 = new DataColumn("Quantity", System.Type.GetType("System.String"));
            DataColumn col06 = new DataColumn("UnitPrice", System.Type.GetType("System.String"));
            table.Columns.Add(col01);
            table.Columns.Add(col02);
            table.Columns.Add(col03);
            table.Columns.Add(col04);
            table.Columns.Add(col05);
            table.Columns.Add(col06);

            //Set the data row
            foreach (var item in purchaseorderList)
            {
                row = table.NewRow();
                row["PurchaseOrderNo"] = item.PurchaseOrderNo;
                row["Date"] = item.SODate;
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
        public ActionResult OpenPurchaseOrderbyVendor(int? page, string id)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            string szMsg = "";
            string szVendorId = "";

            Vendors vendor = db.Vendors.Where(vd => vd.VendorNo == id).FirstOrDefault<Vendors>();
            if (vendor != null)
            {
                szVendorId = vendor.Id.ToString();
            }

            PurchaseOrdersbyVendor purchaseorderbyvendor = null;
            List<PurchaseOrdersbyVendor> purchaseorderList = new List<PurchaseOrdersbyVendor>();

            //qryPOs = db.PurchaseOrders.Where(vd => vd.VendorId == id ).OrderByDescending(vd => vd.PurchaseOrderNo);

            var qryPOs = db.PurchasOrderDetails.Join(db.PurchaseOrders, podet => podet.PurchaseOrderId, po => po.PurchaseOrderId, (podet, po)
                => new { podet, po }).Where(prod => prod.po.VendorId == id && prod.podet.Sub_ItemID != null && prod.po.ReceiveStatus == null).OrderByDescending(prod => prod.po.PurchaseOrderId);
            if (qryPOs.Count() > 0)
            {
                foreach (var item in qryPOs)
                {

                    purchaseorderbyvendor = new PurchaseOrdersbyVendor();
                    purchaseorderbyvendor.PurchaseOrderId = item.po.PurchaseOrderId;
                    purchaseorderbyvendor.PurchaseOrderNo = item.po.PurchaseOrderNo;
                    purchaseorderbyvendor.SODate = item.po.PODate;
                    purchaseorderbyvendor.Sub_ItemID = item.podet.Sub_ItemID;
                    purchaseorderbyvendor.Description = item.podet.Description;
                    purchaseorderbyvendor.Quantity = item.podet.Quantity;
                    purchaseorderbyvendor.UnitPrice = item.podet.UnitPrice;

                    purchaseorderList.Add(purchaseorderbyvendor);
                }
            }

            ViewBag.VendorNo = id;
            string szUrl = string.Format("~/Vendors/Edit/{0}", szVendorId);
            szUrl = Url.Content(szUrl);
            ViewBag.Quit = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, szUrl);

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
            return View(purchaseorderList.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /Vendors/CustomerListExcel
        public ActionResult VendorListExcel()
        {
            //DataTable hlpTbl = GetCustomerListTable();

            ExportCSV(GetVendorListTable(), "VendorList");

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

        private DataTable GetVendorListTable()
        {
            long telHlp = 0;
            long faxHlp = 0;
            string telfmt = "000-000-0000";
            string szTel = "";

            DataTable table = null;
            DataRow row = null;

            VendorList thevendorlist = null;
            List<VendorList> vendorList = new List<VendorList>();

            var qryVendors = db.VendorsContactAddresses.Join(db.Vendors, ctad => ctad.VendorId, cst => cst.Id, (ctad, cst)
                => new { ctad, cst }).OrderBy(cact => cact.cst.VendorNo);
            if (qryVendors.Count() > 0)
            {
                foreach (var item in qryVendors)
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

                    thevendorlist = new VendorList();
                    thevendorlist.Id = item.cst.Id;
                    thevendorlist.VendorNo = item.cst.VendorNo;
                    thevendorlist.CompanyName = item.ctad.CompanyName;
                    thevendorlist.FirstName = item.ctad.FirstName;
                    thevendorlist.LastName = item.ctad.LastName;
                    thevendorlist.State = item.ctad.State;
                    thevendorlist.Country = item.ctad.Country;
                    thevendorlist.Tel = szTel;

                    vendorList.Add(thevendorlist);
                }
            }

            table = new DataTable("VendorList");

            // Set the header
            DataColumn col01 = new DataColumn("VendorNo", System.Type.GetType("System.String"));
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
            foreach (var item in vendorList)
            {
                row = table.NewRow();
                row["VendorNo"] = item.VendorNo;
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
        // GET: /Vendors/VendorList
        [NoCache]
        public PartialViewResult VendorList(int? page)
        {

            int pageIndex = 0;
            int pageSize = PageSize;

            VendorList thevendorlist = null;
            List<VendorList> vendorList = new List<VendorList>();

            var qryVendors = db.VendorsContactAddresses.Join(db.Vendors, ctad => ctad.VendorId, cst => cst.Id, (ctad, cst)
                => new { ctad, cst }).OrderBy(cact => cact.cst.VendorNo);
            if (qryVendors.Count() > 0)
            {
                foreach (var item in qryVendors)
                {
                    thevendorlist = new VendorList();
                    thevendorlist.Id = item.cst.Id;
                    thevendorlist.VendorNo = item.cst.VendorNo;
                    thevendorlist.CompanyName = item.ctad.CompanyName;
                    thevendorlist.FirstName = item.ctad.FirstName;
                    thevendorlist.LastName = item.ctad.LastName;
                    thevendorlist.State = item.ctad.State;
                    thevendorlist.Country = item.ctad.Country;
                    thevendorlist.Tel = item.ctad.Tel;

                    vendorList.Add(thevendorlist);
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


            var onePageOfData = vendorList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return PartialView(vendorList.ToPagedList(pageIndex, pageSize));
        }

        //
        // /Vendors/GetCompanyName
        public static string GetCompanyName(TimelyDepotContext db01, string szVendorID)
        {
            int nVendorID = Convert.ToInt32(szVendorID);
            string szCompanyname = "";

            VendorsContactAddress vendoraddres = db01.VendorsContactAddresses.Where(vdad => vdad.VendorId == nVendorID).FirstOrDefault<VendorsContactAddress>();
            if (vendoraddres != null)
            {
                szCompanyname = vendoraddres.CompanyName;
            }

            return szCompanyname;
        }

        public static string GetCompanyName01(TimelyDepotContext db01, string szVendorID, ref string szCity, ref string szZip, ref string szPhone)
        {
            int nVendorID = Convert.ToInt32(szVendorID);
            string szCompanyname = "";

            VendorsContactAddress vendoraddres = db01.VendorsContactAddresses.Where(vdad => vdad.VendorId == nVendorID).FirstOrDefault<VendorsContactAddress>();
            if (vendoraddres != null)
            {
                szCompanyname = vendoraddres.CompanyName;
                szCity = vendoraddres.City;
                szZip = vendoraddres.Zip;
                szPhone = vendoraddres.Tel1;
            }

            return szCompanyname;
        }

        public static string GetCompanyName02(TimelyDepotContext db01, string szVendorID, ref string szCity, ref string szZip, ref string szPhone)
        {
            long telHlp = 0;
            long faxHlp = 0;
            string telfmt = "000-000-0000";
            string szBussinesType = "";

            int nVendorID = Convert.ToInt32(szVendorID);
            string szCompanyname = "";

            VendorsContactAddress vendoraddres = db01.VendorsContactAddresses.Where(vdad => vdad.VendorId == nVendorID).FirstOrDefault<VendorsContactAddress>();
            if (vendoraddres != null)
            {
                telHlp = Convert.ToInt64(vendoraddres.Tel1);
                faxHlp = Convert.ToInt64(vendoraddres.Fax);

                szCompanyname = vendoraddres.CompanyName;
                szCity = vendoraddres.City;
                szZip = vendoraddres.Zip;
                //szPhone = vendoraddres.Tel1;
                szPhone = telHlp.ToString(telfmt);

                szBussinesType = GetBussinesType(db01, Convert.ToInt32(vendoraddres.VendorId), "");

                if (!string.IsNullOrEmpty(szBussinesType))
                {
                    if (szBussinesType.ToUpper() != "DOMESTIC")
                    {
                        int nLen = 13;
                        if (!string.IsNullOrEmpty(vendoraddres.Tel1))
                        {
                            nLen = vendoraddres.Tel1.Length;
                        }
                        switch (nLen)
                        {
                            case 8:
                                telfmt = "000-00000";
                                break;
                            case 9:
                                telfmt = "000-000000";
                                break;
                            case 10:
                                telfmt = "000-0000000";
                                break;
                            case 11:
                                telfmt = "000-00000000";
                                break;
                            case 12:
                                telfmt = "000-000000000";
                                break;
                            case 13:
                                telfmt = "000-0000000000";
                                break;
                            default:
                                break;
                        }

                        telHlp = Convert.ToInt64(vendoraddres.Tel1);
                        szPhone = telHlp.ToString(telfmt);
                    }
                }
            }

            return szCompanyname;
        }

        public static string GetBussinesType(TimelyDepotContext db01, int nVendorid, string szVendorNo)
        {
            string szBussinesType = "Domestic";
            Vendors vendor = null;

            if (nVendorid > 0)
            {
                vendor = db01.Vendors.Find(nVendorid);
                if (vendor != null)
                {
                    szBussinesType = vendor.BussinesType;
                }
            }
            else
            {
                vendor = db01.Vendors.Where(vd => vd.VendorNo == szVendorNo).FirstOrDefault<Vendors>();
                if (vendor != null)
                {
                    szBussinesType = vendor.BussinesType;
                }
            }

            return szBussinesType;
        }

        //
        // GET: /Vendors/DeleteNote
        public ActionResult DeleteNote(int id)
        {
            VendorsSpecialNotes customersspecialnotes = db.VendorsSpecialNotes.Find(id);
            db.VendorsSpecialNotes.Remove(customersspecialnotes);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // POST: /Vendors/UpdateCustomerNote
        [HttpPost]
        public ActionResult UpdateCustomerNote(VendorsSpecialNotes customenote)
        {
            int nVendorID = 0;

            if (customenote != null)
            {
                if (ModelState.IsValid)
                {
                    if (customenote.Id == 0)
                    {
                        db.VendorsSpecialNotes.Add(customenote);
                        db.SaveChanges();
                    }
                    else
                    {
                        nVendorID = Convert.ToInt32(customenote.VendorId);
                        db.Entry(customenote).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Edit", new { id = nVendorID });
                }

            }

            return RedirectToAction("Index");
        }

        //
        // GET: /Vendors/CreateEditNote
        [NoCache]
        public PartialViewResult CreateEditNote(string customerid, int id = 0)
        {
            int nCustomerId = Convert.ToInt32(customerid);
            string szCustomerInfo = "";
            IQueryable<VendorsContactAddress> qryAddress = null;

            VendorsSpecialNotes customernote = null;
            Vendors customer = null;
            VendorsContactAddress customeraddress = null;

            if (id == 0)
            {
                customernote = new VendorsSpecialNotes();
                customernote.VendorId = nCustomerId;
            }
            else
            {
                customernote = db.VendorsSpecialNotes.Find(id);
            }

            //Get Customer info
            if (customernote != null)
            {
                customer = db.Vendors.Find(customernote.VendorId);
                if (customer != null)
                {
                    szCustomerInfo = string.Format("{0}", customer.VendorNo);

                    qryAddress = db.VendorsContactAddresses.Where(cust => cust.VendorId == customer.Id);
                    if (qryAddress.Count() > 0)
                    {
                        customeraddress = qryAddress.FirstOrDefault<VendorsContactAddress>();
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
        // GET: /Vendors/DeleteSales
        public ActionResult DeleteSales(int id, int vendordefaultid = 0)
        {
            int nVendorID = 0;
            VendorDefaults customerDefaults = db.VendorDefaults.Find(vendordefaultid);
            if (customerDefaults != null)
            {
                if (customerDefaults.SalesContactId == id)
                {
                    customerDefaults.SalesContactId = null;
                    customerDefaults.SalesName = null;
                }
            }

            VendorsSalesContact customersspecialnotes = db.VendorsSalesContacts.Find(id);
            nVendorID = Convert.ToInt32(customersspecialnotes.VendorId);
            db.VendorsSalesContacts.Remove(customersspecialnotes);
            db.SaveChanges();
            return RedirectToAction("Edit", new { id = nVendorID });
            //return RedirectToAction("Index");
        }

        //
        // POST: /Customer/UpdateVendorSales02
        [HttpPost]
        public ActionResult UpdateVendorSales02(VendorsSalesContact vendorsalescontact, string vendordefault)
        {
            int nIdHlp = 0;
            int nVendorDefault = Convert.ToInt32(vendordefault);

            VendorDefaults custdefault = null;

            if (vendorsalescontact != null)
            {
                if (ModelState.IsValid)
                {

                    if (vendorsalescontact.Id == 0)
                    {
                        if (!string.IsNullOrEmpty(vendorsalescontact.Tel))
                        {
                            vendorsalescontact.Tel = vendorsalescontact.Tel.Replace("-", "");
                        }
                        else
                        {
                            vendorsalescontact.Tel = "0";
                        }
                        if (!string.IsNullOrEmpty(vendorsalescontact.Fax))
                        {
                            vendorsalescontact.Fax = vendorsalescontact.Fax.Replace("-", "");
                        }
                        else
                        {
                            vendorsalescontact.Fax = "0";
                        }
                        db.VendorsSalesContacts.Add(vendorsalescontact);
                        db.SaveChanges();

                        //Set the default vendor value
                        custdefault = db.VendorDefaults.Find(nVendorDefault);
                        if (custdefault != null)
                        {
                            custdefault.SalesContactId = vendorsalescontact.Id;
                            custdefault.SalesName = string.Format("{0} {1}", vendorsalescontact.FirstName, vendorsalescontact.LastName);
                            db.Entry(custdefault).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        //**********************//
                        nIdHlp = vendorsalescontact.Id;
                        //vendorsalescontact.Id = Convert.ToInt32(vendorsalescontact.VendorId);
                        //customenote.VendorId = nIdHlp;
                        //**********************//

                        //Set the default customer value
                        custdefault = db.VendorDefaults.Find(nVendorDefault);
                        if (custdefault != null)
                        {
                            custdefault.SalesContactId = vendorsalescontact.Id;
                            custdefault.SalesName = string.Format("{0} {1}", vendorsalescontact.FirstName, vendorsalescontact.LastName);
                            db.Entry(custdefault).State = EntityState.Modified;
                        }

                        vendorsalescontact.Tel = vendorsalescontact.Tel.Replace("-", "");
                        vendorsalescontact.Fax = vendorsalescontact.Fax.Replace("-", "");
                        db.Entry(vendorsalescontact).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Edit", new { id = Convert.ToInt32(vendorsalescontact.VendorId) });
                }

            }

            return RedirectToAction("Index");
        }

        //
        // POST: /Customer/UpdateCustomerNote
        [HttpPost]
        public ActionResult UpdateVendorSales(VendorsSalesContact customenote, string vendordefault, string Id02hlp, string vendorid02hlp)
        {
            int nId02Hlp = 0;
            int nvendorid02hlp = 0;
            int nVendorDefault = Convert.ToInt32(vendordefault);

            VendorDefaults custdefault = null;

            if (customenote != null)
            {
                if (ModelState.IsValid)
                {

                    //Adjust the id values
                    nId02Hlp = Convert.ToInt32(Id02hlp);
                    nvendorid02hlp = Convert.ToInt32(vendorid02hlp);

                    if (customenote.Id != 0)
                    {
                        customenote.Id = nId02Hlp;
                    }
                    customenote.VendorId = nvendorid02hlp;

                    if (customenote.Id == 0)
                    {
                        if (!string.IsNullOrEmpty(customenote.Tel))
                        {
                            customenote.Tel = customenote.Tel.Replace("-", "");
                        }
                        else
                        {
                            customenote.Tel = "0";
                        }
                        if (!string.IsNullOrEmpty(customenote.Fax))
                        {
                            customenote.Fax = customenote.Fax.Replace("-", "");
                        }
                        else
                        {
                            customenote.Fax = "0";
                        }
                        db.VendorsSalesContacts.Add(customenote);
                        db.SaveChanges();

                        //Set the default vendor value
                        custdefault = db.VendorDefaults.Find(nVendorDefault);
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
                        //**********************//
                        //nIdHlp = customenote.Id;
                        //customenote.Id = Convert.ToInt32(customenote.VendorId);
                        //customenote.VendorId = nIdHlp;
                        //**********************//

                        //Set the default customer value
                        custdefault = db.VendorDefaults.Find(nVendorDefault);
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

                        customenote.Tel = customenote.Tel.Replace("-", "");
                        customenote.Fax = customenote.Fax.Replace("-", "");
                        db.Entry(customenote).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Edit", new { id = Convert.ToInt32(customenote.VendorId) });
                }

            }

            return RedirectToAction("Index");
        }
        //
        // GET: /Vendors/CreateEditSales02
        [NoCache]
        public PartialViewResult CreateEditSales02(string vendorid, string vendordefaultid)
        {
            int nVendorSalesid = 0;
            int nVendorId = Convert.ToInt32(vendorid);
            int nDefaultid = 0;

            VendorsSalesContact vendorsalescontact = null;
            VendorDefaults vendorDefault = null;

            //Get vendor sales contact
            vendorsalescontact = db.VendorsSalesContacts.Find(nVendorId);
            if (vendorsalescontact == null)
            {
                //Get  vendor default
                if (!string.IsNullOrEmpty(vendordefaultid))
                {
                    nDefaultid = Convert.ToInt32(vendordefaultid);
                }
                vendorDefault = db.VendorDefaults.Find(nDefaultid);
                if (vendorDefault != null)
                {
                    nVendorSalesid = Convert.ToInt32(vendorDefault.SalesContactId);
                }
                else
                {
                    nVendorSalesid = 0;
                }

                if (nVendorSalesid > 0)
                {
                    vendorsalescontact = db.VendorsSalesContacts.Find(nVendorSalesid);

                    //create new sales contact
                    if (vendorsalescontact == null && Convert.ToInt32(vendorDefault.VendorId) > 0)
                    {
                        vendorsalescontact = new VendorsSalesContact();
                        vendorsalescontact.VendorId = Convert.ToInt32(vendorDefault.VendorId);
                        db.VendorsSalesContacts.Add(vendorsalescontact);
                        db.SaveChanges();
                    }
                }
            }

            ViewBag.Id02 = vendorsalescontact.Id;
            ViewBag.VendorId02 = vendorsalescontact.VendorId;

            return PartialView(vendorsalescontact);
        }

        //
        // GET: /Vendors/CreateEditSales
        [NoCache]
        public PartialViewResult CreateEditSales(string vendorid, string vendoridHlp, int id = 0)
        {
            int nCustomerId = Convert.ToInt32(vendorid);
            string szCustomerInfo = "";
            //IQueryable<CustomersSalesContact> qryAddress = null;

            VendorsSalesContact customernote = null;
            Vendors customer = null;
            VendorDefaults customerDefault = null;
            //CustomersSubsidiaryAddress customeraddress = null;

            //if (id == 0)
            if (string.IsNullOrEmpty(vendorid))
            {
                customernote = new VendorsSalesContact();
                //customernote.VendorId = nCustomerId;
                //customernote.VendorId = Convert.ToInt32(vendoridHlp);
                customernote.VendorId = Convert.ToInt32(id);
                db.VendorsSalesContacts.Add(customernote);
                db.SaveChanges();
            }
            else
            {
                customernote = db.VendorsSalesContacts.Find(nCustomerId);
            }

            //Get Customer info
            if (customernote != null)
            {
                customer = db.Vendors.Find(customernote.VendorId);
                if (customer != null)
                {
                    szCustomerInfo = string.Format("{0}", customer.VendorNo);

                }
            }
            ViewBag.CustomerInfo = szCustomerInfo;
            ViewBag.VendorId02 = customernote.VendorId;
            ViewBag.Id02 = customernote.Id;

            //Get the customer default id
            ViewBag.CustomerDefaultId = 0;
            customerDefault = db.VendorDefaults.Where(ctdf => ctdf.VendorId == customernote.VendorId).FirstOrDefault<VendorDefaults>();
            if (customerDefault != null)
            {
                ViewBag.CustomerDefaultId = customerDefault.Id;
            }


            return PartialView(customernote);
        }

        //
        // POST: /Vendors/UpdateVendorhistory
        [HttpPost]
        public ActionResult UpdateVendorhistory(VendorView vendorview)
        {
            VendorsHistory customeraddress = null;

            if (vendorview.vendorhistory != null)
            {
                if (vendorview.vendorhistory.Id == 0)
                {
                    customeraddress = new VendorsHistory();
                }
                else
                {
                    customeraddress = db.VendorsHistories.Find(vendorview.vendorhistory.Id);
                }

                if (customeraddress != null)
                {
                    customeraddress.OpenPurchaseOrder = vendorview.vendorhistory.OpenPurchaseOrder;
                    customeraddress.OutstandingBalance = vendorview.vendorhistory.OutstandingBalance;
                    customeraddress.VendorId = vendorview.vendorhistory.VendorId;

                    db.Entry(customeraddress).State = EntityState.Modified;
                    db.SaveChanges();

                }
            }

            return RedirectToAction("Edit", new { id = Convert.ToInt32(customeraddress.VendorId) });
            //return RedirectToAction("Index");
        }

        //
        // POST: /Customers/UpdateCustomerBilling
        [HttpPost]
        public ActionResult UpdateVendorBilling(VendorView vendorview)
        {
            VendorsBillingDept customeraddress = null;

            if (vendorview.vendorbilling != null)
            {
                if (vendorview.vendorbilling.Id == 0)
                {
                    customeraddress = new VendorsBillingDept();
                }
                else
                {
                    customeraddress = db.VendorsBillingDepts.Find(vendorview.vendorbilling.Id);
                }

                if (customeraddress != null)
                {
                    customeraddress.Beneficiary = vendorview.vendorbilling.Beneficiary;
                    customeraddress.BeneficiaryAccountNo = vendorview.vendorbilling.BeneficiaryAccountNo;
                    customeraddress.SWIFT = vendorview.vendorbilling.SWIFT;
                    customeraddress.BankName = vendorview.vendorbilling.BankName;
                    customeraddress.BankAddress = vendorview.vendorbilling.BankAddress;

                    db.Entry(customeraddress).State = EntityState.Modified;
                    db.SaveChanges();

                }
            }

            return RedirectToAction("Edit", new { id = Convert.ToInt32(customeraddress.VendorId) });
            //return RedirectToAction("Index");
        }

        //
        // POST: /Customers/UpdateCustomerAddress
        [HttpPost]
        public ActionResult UpdateVendorAddress(VendorView vendorview)
        {
            VendorsContactAddress customeraddress = null;

            if (vendorview.vendoraddress != null)
            {
                if (vendorview.vendoraddress.Id == 0)
                {
                    customeraddress = new VendorsContactAddress();
                }
                else
                {
                    customeraddress = db.VendorsContactAddresses.Find(vendorview.vendoraddress.Id);
                }

                if (customeraddress != null)
                {
                    customeraddress.Address = vendorview.vendoraddress.Address;
                    customeraddress.City = vendorview.vendoraddress.City;
                    customeraddress.CompanyName = vendorview.vendoraddress.CompanyName;
                    customeraddress.Country = vendorview.vendoraddress.Country;
                    customeraddress.VendorId = vendorview.vendoraddress.VendorId;
                    customeraddress.Email = vendorview.vendoraddress.Email;
                    customeraddress.Fax = vendorview.vendoraddress.Fax;
                    customeraddress.FirstName = vendorview.vendoraddress.FirstName;
                    customeraddress.LastName = vendorview.vendoraddress.LastName;
                    customeraddress.Note = vendorview.vendoraddress.Note;
                    customeraddress.State = vendorview.vendoraddress.State;
                    customeraddress.Tel = vendorview.vendoraddress.Tel;
                    customeraddress.Title = vendorview.vendoraddress.Title;
                    customeraddress.Website = vendorview.vendoraddress.Website;
                    customeraddress.Zip = vendorview.vendoraddress.Zip;

                    db.Entry(customeraddress).State = EntityState.Modified;
                    db.SaveChanges();

                }
            }

            return RedirectToAction("Index");
        }

        //
        // POST: /Customers/UpdateCustomer
        [HttpPost]
        public ActionResult UpdateVendorandAddress(VendorView vendorview)
        {
            Vendors vendor = null;
            VendorsContactAddress customeraddress = null;

            //Update Customer General Information
            if (Convert.ToInt32(vendorview.vendor.Id) != 0)
            {
                vendor = db.Vendors.Find(vendorview.vendor.Id);
                if (vendor != null)
                {
                    vendor.BussinesType = vendorview.vendor.BussinesType;
                    vendor.CreditLimit = vendorview.vendor.CreditLimit;
                    vendor.VendorNo = vendorview.vendor.VendorNo;
                    vendor.Origin = vendorview.vendor.Origin;
                    vendor.PaymentTerms = vendorview.vendor.PaymentTerms;
                    vendor.Status = vendorview.vendor.Status;

                    db.Entry(vendor).State = EntityState.Modified;
                    //db.SaveChanges();

                }
            }

            if (vendorview.vendoraddress != null)
            {
                if (vendorview.vendoraddress.Id == 0)
                {
                    customeraddress = new VendorsContactAddress();
                }
                else
                {
                    customeraddress = db.VendorsContactAddresses.Find(vendorview.vendoraddress.Id);
                }

                if (customeraddress != null)
                {
                    customeraddress.Address = vendorview.vendoraddress.Address;
                    customeraddress.Address3 = vendorview.vendoraddress.Address3;
                    customeraddress.City = vendorview.vendoraddress.City;
                    customeraddress.CompanyName = vendorview.vendoraddress.CompanyName;
                    customeraddress.Country = vendorview.vendoraddress.Country;
                    customeraddress.VendorId = vendorview.vendoraddress.VendorId;
                    customeraddress.Email = vendorview.vendoraddress.Email;
                    customeraddress.FirstName = vendorview.vendoraddress.FirstName;
                    customeraddress.LastName = vendorview.vendoraddress.LastName;
                    customeraddress.Note = vendorview.vendoraddress.Note;
                    customeraddress.State = vendorview.vendoraddress.State;
                    if (!string.IsNullOrEmpty(vendorview.vendoraddress.Tel))
                    {
                        customeraddress.Tel = vendorview.vendoraddress.Tel.Replace("-", "");
                    }
                    if (!string.IsNullOrEmpty(vendorview.vendoraddress.Tel1))
                    {
                        customeraddress.Tel1 = vendorview.vendoraddress.Tel1.Replace("-", "");
                    }
                    if (!string.IsNullOrEmpty(vendorview.vendoraddress.Tel2))
                    {
                        customeraddress.Tel2 = vendorview.vendoraddress.Tel2.Replace("-", "");

                    }
                    if (!string.IsNullOrEmpty(vendorview.vendoraddress.Fax))
                    {
                        customeraddress.Fax = vendorview.vendoraddress.Fax.Replace("-", "");
                    }
                    customeraddress.Title = vendorview.vendoraddress.Title;
                    customeraddress.Website = vendorview.vendoraddress.Website;
                    customeraddress.Zip = vendorview.vendoraddress.Zip;

                    db.Entry(customeraddress).State = EntityState.Modified;
                }
            }
            db.SaveChanges();


            return RedirectToAction("Edit", new { id = Convert.ToInt32(vendorview.vendor.Id) });
            //return RedirectToAction("Index");
        }

        //
        // POST: /Customers/UpdateCustomer
        [HttpPost]
        public ActionResult UpdateVendor(VendorView vendorview)
        {
            Vendors vendor = null;

            //Update Customer General Information
            if (Convert.ToInt32(vendorview.vendor.Id) != 0)
            {
                vendor = db.Vendors.Find(vendorview.vendor.Id);
                if (vendor != null)
                {
                    vendor.BussinesType = vendorview.vendor.BussinesType;
                    vendor.CreditLimit = vendorview.vendor.CreditLimit;
                    //customers.CustomerNo = cusview.customer.CustomerNo;
                    vendor.Origin = vendorview.vendor.Origin;
                    vendor.PaymentTerms = vendorview.vendor.PaymentTerms;
                    vendor.Status = vendorview.vendor.Status;

                    db.Entry(vendor).State = EntityState.Modified;
                    db.SaveChanges();

                }
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Vendors/
        [NoCache]
        public ActionResult Index(int? page, string searchItem, string ckActive, string ckCriteria)
        {
            bool bHasVendor = false;
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<Vendors> qryVendors = null;

            List<Vendors> VendorsList = new List<Vendors>();

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
                            qryVendors = db.Vendors.Where(vd => vd.Status == true).OrderBy(vd => vd.VendorNo);
                        }
                        else
                        {
                            qryVendors = db.Vendors.Where(vd => vd.Status == false).OrderBy(vd => vd.VendorNo);
                        }

                        //Display the data
                        if (qryVendors != null)
                        {
                            if (qryVendors.Count() > 0)
                            {
                                foreach (var item in qryVendors)
                                {
                                    VendorsList.Add(item);
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
                        qryVendors = db.Vendors.Where(vd => vd.VendorNo.StartsWith(searchItem) && vd.Status == true).OrderBy(vd => vd.VendorNo);
                    }
                    else
                    {
                        qryVendors = db.Vendors.Where(vd => vd.VendorNo.StartsWith(searchItem) && vd.Status == false).OrderBy(vd => vd.VendorNo);
                    }
                    bHasVendor = true;
                }
                if (ckCriteria == "state")
                {
                    if (ckActive == "true")
                    {
                        qryVendors = db.Vendors.Where(vd => vd.BussinesType.StartsWith(searchItem) && vd.Status == true).OrderBy(vd => vd.VendorNo);
                    }
                    else
                    {
                        qryVendors = db.Vendors.Where(vd => vd.BussinesType.StartsWith(searchItem) && vd.Status == false).OrderBy(vd => vd.VendorNo);
                    }
                    bHasVendor = true;
                }
                if (ckCriteria == "company")
                {
                    if (ckActive == "true")
                    {
                        var qryMainContact = db.Vendors.Join(db.VendorsContactAddresses, ctc => ctc.Id, cus => cus.VendorId, (ctc, cus)
                            => new { ctc, cus }).Where(Nctcs => Nctcs.cus.CompanyName.StartsWith(searchItem) && Nctcs.ctc.Status == true).OrderBy(Nctcs => Nctcs.cus.CompanyName);
                        if (qryMainContact.Count() > 0)
                        {
                            foreach (var item in qryMainContact)
                            {
                                VendorsList.Add(item.ctc);
                            }
                        }
                    }
                    else
                    {
                        var qryMainContact = db.Vendors.Join(db.VendorsContactAddresses, ctc => ctc.Id, cus => cus.VendorId, (ctc, cus)
                            => new { ctc, cus }).Where(Nctcs => Nctcs.cus.CompanyName.StartsWith(searchItem) && Nctcs.ctc.Status == false).OrderBy(Nctcs => Nctcs.cus.CompanyName);
                        if (qryMainContact.Count() > 0)
                        {
                            foreach (var item in qryMainContact)
                            {
                                VendorsList.Add(item.ctc);
                            }
                        }
                    }
                }
                if (ckCriteria == "phone")
                {
                    if (ckActive == "true")
                    {
                        var qryMainContact = db.Vendors.Join(db.VendorsContactAddresses, ctc => ctc.Id, cus => cus.VendorId, (ctc, cus)
                            => new { ctc, cus }).Where(Nctcs => Nctcs.cus.Tel1.StartsWith(searchItem) && Nctcs.ctc.Status == true).OrderBy(Nctcs => Nctcs.cus.CompanyName);
                        if (qryMainContact.Count() > 0)
                        {
                            foreach (var item in qryMainContact)
                            {
                                VendorsList.Add(item.ctc);
                            }
                        }
                    }
                    else
                    {
                        var qryMainContact = db.Vendors.Join(db.VendorsContactAddresses, ctc => ctc.Id, cus => cus.VendorId, (ctc, cus)
                            => new { ctc, cus }).Where(Nctcs => Nctcs.cus.Tel1.StartsWith(searchItem) && Nctcs.ctc.Status == false).OrderBy(Nctcs => Nctcs.cus.CompanyName);
                        if (qryMainContact.Count() > 0)
                        {
                            foreach (var item in qryMainContact)
                            {
                                VendorsList.Add(item.ctc);
                            }
                        }
                    }
                }

            }

            if (qryVendors != null)
            {
                if (bHasVendor)
                {
                    if (qryVendors.Count() > 0)
                    {
                        foreach (var item in qryVendors)
                        {
                            VendorsList.Add(item);
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


            var onePageOfData = VendorsList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;

            //Authorize user
            if (User.IsInRole("Owner"))
            {
                return View(VendorsList.ToPagedList(pageIndex, pageSize));
            }
            if (User.IsInRole("Admin"))
            {
                return View(VendorsList.ToPagedList(pageIndex, pageSize));
            }


            //return View(VendorsList.ToPagedList(pageIndex, pageSize));
            return RedirectToAction("LogOn", "Account");

        }

        //
        // GET: /Vendors/Details/5

        public ActionResult Details(int id = 0)
        {
            Vendors vendors = db.Vendors.Find(id);
            if (vendors == null)
            {
                return HttpNotFound();
            }
            return View(vendors);
        }

        //
        // GET: /Vendors/Create01
        public ActionResult Create01()
        {
            int nCustomerId = 0;

            Vendors vendor = new Vendors();
            vendor.BussinesSice = DateTime.Now;
            vendor.VendorNo = GetNextVendorNo();
            vendor.Status = false;
            db.Vendors.Add(vendor);
            db.SaveChanges();

            nCustomerId = vendor.Id;
            TempData["AddCustomer"] = "AddCustomer";

            return RedirectToAction("Edit", new { id = nCustomerId });
        }

        private string GetNextVendorNo()
        {
            string szCustomerNo = "0";

            int qryHlp = db.Vendors.Max(cst => cst.Id);
            if (qryHlp > 0)
            {
                qryHlp++;
                szCustomerNo = qryHlp.ToString();
            }

            return szCustomerNo;
        }

        //
        // GET: /Vendors/Create

        public PartialViewResult Create()
        {
            Vendors vendors = new Vendors();
            vendors.BussinesSice = DateTime.Now;
            vendors.VendorNo = "0";

            return PartialView(vendors);
        }

        //
        // POST: /Vendors/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Vendors vendors)
        {
            if (ModelState.IsValid)
            {
                //AQUI  crear origin and terms
                db.Vendors.Add(vendors);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vendors);
        }

        //
        // GET: /Vendors/Edit/5
        [NoCache]
        public ActionResult Edit(string modomultiple, int? page, int id = 0)
        {
            int pageNote = 0;
            int pageSubsidiary = 0;
            int pageSales = 0;
            int pageShipp = 0;
            int pageIndex = 0;
            int pageSize = 10;
            string szMsg = "";
            string szVendorInfo = "";
            string szVendorId = "";

            List<KeyValuePair<string, string>> listSelector = new List<KeyValuePair<string, string>>();

            IQueryable<VendorsContactAddress> qryVenAdd = null;
            IQueryable<VendorsBillingDept> qryVenBill = null;
            IQueryable<VendorsSalesContact> qryVenSale = null;
            IQueryable<VendorsHistory> qryVenHis = null;
            IQueryable<VendorsSpecialNotes> qryCusNote = null;
            IQueryable<VendorTypes> qryBussines = null;
            IQueryable<Origin> qryOrigin = null;
            IQueryable<Terms> qryTerms = null;
            IQueryable<Bussines> qryBussines01 = null;

            VendorsContactAddress venAddress = null;
            VendorsBillingDept venBillin = null;
            VendorsSalesContact venSale = null;
            VendorsHistory venHistory = null;
            VendorsSpecialNotes CustNotes = null;
            VendorDefaults vendordefaults = null;

            List<VendorsSpecialNotes> notesList = new List<VendorsSpecialNotes>();
            List<VendorsSalesContact> salesList = new List<VendorsSalesContact>();

            //Get the vendor's data
            Vendors vendor = db.Vendors.Find(id);
            if (vendor != null)
            {
                //Use the vendors defaults 
                vendordefaults = db.VendorDefaults.Where(cudf => cudf.VendorId == id).FirstOrDefault<VendorDefaults>();
                if (vendordefaults == null)
                {
                    vendordefaults = new VendorDefaults();
                    vendordefaults.VendorId = id;
                    db.VendorDefaults.Add(vendordefaults);
                    db.SaveChanges();
                }

                szVendorInfo = vendor.VendorNo;

                qryVenAdd = db.VendorsContactAddresses.Where(cutadd => cutadd.VendorId == vendor.Id);
                if (qryVenAdd.Count() > 0)
                {
                    venAddress = qryVenAdd.FirstOrDefault<VendorsContactAddress>();
                    szVendorInfo = string.Format("{0} {1}", szVendorInfo, venAddress.CompanyName);
                    szVendorId = vendor.Id.ToString();
                }
                else
                {
                    venAddress = new VendorsContactAddress();
                    venAddress.VendorId = vendor.Id;
                    db.VendorsContactAddresses.Add(venAddress);
                    szVendorId = vendor.Id.ToString();
                }

                qryVenBill = db.VendorsBillingDepts.Where(cutadd => cutadd.VendorId == vendor.Id);
                if (qryVenBill.Count() > 0)
                {
                    venBillin = qryVenBill.FirstOrDefault<VendorsBillingDept>();
                }
                else
                {
                    venBillin = new VendorsBillingDept();
                    venBillin.VendorId = vendor.Id;
                    db.VendorsBillingDepts.Add(venBillin);
                }

                qryVenHis = db.VendorsHistories.Where(cutadd => cutadd.VendorId == vendor.Id);
                if (qryVenHis.Count() > 0)
                {
                    venHistory = qryVenHis.FirstOrDefault<VendorsHistory>();
                }
                else
                {
                    venHistory = new VendorsHistory();
                    venHistory.VendorId = vendor.Id;
                    db.VendorsHistories.Add(venHistory);
                }


                qryVenSale = db.VendorsSalesContacts.Where(cutadd => cutadd.VendorId == vendor.Id).OrderBy(cutadd => cutadd.FirstName).ThenBy(cutadd => cutadd.LastName);
                if (qryVenSale.Count() > 0)
                {
                    //custSalesContact = qryCusSal.FirstOrDefault<CustomersSalesContact>();
                    foreach (var item in qryVenSale)
                    {
                        salesList.Add(item);
                        if (venSale == null)
                        {
                            venSale = item;
                            if (vendordefaults.SalesContactId == null)
                            {
                                vendordefaults.SalesContactId = item.Id;
                                vendordefaults.SalesName = string.Format("{0} {1}", item.FirstName, item.LastName);
                                //custSalesContact = db.CustomersSalesContacts.Find(item.Id);
                            }
                        }
                    }
                }
                else
                {
                    venSale = new VendorsSalesContact();
                    venSale.VendorId = vendor.Id;
                    venSale.Address = string.Empty;
                    venSale.City = string.Empty;
                    venSale.Country = string.Empty;
                    venSale.Email = string.Empty;
                    venSale.Fax = "0";
                    venSale.FirstName = string.Empty;
                    venSale.LastName = string.Empty;
                    venSale.Note = string.Empty;
                    venSale.State = string.Empty;
                    venSale.Tel = "0";
                    venSale.Title = string.Empty;
                    venSale.Zip = string.Empty;
                    db.VendorsSalesContacts.Add(venSale);
                    db.SaveChanges();
                    salesList.Add(venSale);
                    if (vendordefaults.SalesContactId == null)
                    {
                        vendordefaults.SalesContactId = venSale.Id;
                        vendordefaults.SalesName = string.Format("{0} {1}", venSale.FirstName, venSale.LastName);
                        //custSalesContact = db.CustomersSalesContacts.Find(item.Id);
                    }
                }


                qryCusNote = db.VendorsSpecialNotes.Where(cutadd => cutadd.VendorId == vendor.Id).OrderByDescending(cutadd => cutadd.Id);
                if (qryCusNote.Count() > 0)
                {
                    //CustNotes = qryCusNote.FirstOrDefault<CustomersSpecialNotes>();
                    foreach (var item in qryCusNote)
                    {
                        notesList.Add(item);
                        if (CustNotes == null)
                        {
                            CustNotes = item;
                        }
                    }
                }
                else
                {
                    CustNotes = new VendorsSpecialNotes();
                    CustNotes.VendorId = vendor.Id;
                    db.VendorsSpecialNotes.Add(CustNotes);
                    notesList.Add(CustNotes);
                }
            }

            db.SaveChanges();

            ViewBag.VendorInfo = szVendorId;
            ViewBag.VendorId = szVendorId;

            //Get the dropdown data
            listSelector = new List<KeyValuePair<string, string>>();
            //qryBussines = db.VendorTypes.OrderBy(bss => bss.VendorType);
            //if (qryBussines.Count() > 0)
            //{
            //    foreach (var item in qryBussines)
            //    {
            //        listSelector.Add(new KeyValuePair<string, string>(item.VendorType, item.VendorType));
            //    }
            //}
            qryBussines01 = db.Bussines.OrderBy(bss => bss.BussinesType);
            if (qryBussines01.Count() > 0)
            {
                foreach (var item in qryBussines01)
                {
                    listSelector.Add(new KeyValuePair<string, string>(item.BussinesType, item.BussinesType));
                }
            }

            SelectList bussineslist = new SelectList(listSelector, "Key", "Value");
            ViewBag.VendorTypeList = bussineslist;

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
            qryVenSale = db.VendorsSalesContacts.Where(vdad => vdad.VendorId == vendor.Id).OrderBy(vdad => vdad.FirstName).ThenBy(vdad => vdad.LastName);
            if (qryVenSale.Count() > 0)
            {
                foreach (var item in qryVenSale)
                {
                    szMsg = string.Format("{0} {1}", item.FirstName, item.LastName);
                    listSelector.Add(new KeyValuePair<string, string>(item.Id.ToString(), szMsg));
                }
            }
            SelectList SalesContactList = new SelectList(listSelector, "Key", "Value");
            ViewBag.SalesContactList = SalesContactList;

            //Fix null values
            if (venSale == null)
            {
                venSale = new VendorsSalesContact();
                venSale.VendorId = vendor.Id;
                venSale.Tel = string.Empty;
                venSale.Fax = string.Empty;
                db.VendorsSalesContacts.Add(venSale);
                db.SaveChanges();

            }
            else
            {
                if (string.IsNullOrEmpty(venSale.Tel))
                {
                    venSale.Tel = string.Empty;
                }
                if (string.IsNullOrEmpty(venSale.Fax))
                {
                    venSale.Fax = string.Empty;
                }
            }

            VendorView vendorview = new VendorView();
            vendorview.vendor = vendor;
            vendorview.vendoraddress = venAddress;
            vendorview.vendorbilling = venBillin;
            vendorview.vendorhistory = venHistory;
            vendorview.vendorsalescontact = venSale;
            vendorview.vendornote = CustNotes;
            vendorview.vendordefaults = vendordefaults;

            //Get theActive/Inactive list
            listSelector = new List<KeyValuePair<string, string>>();
            listSelector.Add(new KeyValuePair<string, string>("Y", "Active"));
            listSelector.Add(new KeyValuePair<string, string>("N", "Inactive"));
            SelectList activeinactivelist = new SelectList(listSelector, "Key", "Value");
            ViewBag.ActiveInactivelist = activeinactivelist;

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
                    case "Sales":
                        pageSales = pageIndex;
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

            if (pageSales == 0)
            {
                pageSales = 1;
            }

            var salesListHlp = salesList.ToPagedList(pageSales, pageSize);
            ViewBag.OnePageOfsalesData = salesListHlp;
            vendorview.vendorsalesList = salesListHlp;


            var notesListHlp = notesList.ToPagedList(pageSales, pageSize);
            ViewBag.OnePageOfnoteData = notesListHlp;
            vendorview.vendornotesList = notesListHlp;

            return View(vendorview);
        }

        //
        // POST: /Vendors/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vendors vendors)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendors).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendors);
        }

        //
        // GET: /Vendors/Delete/5

        public ActionResult Delete(int id = 0)
        {
            IQueryable<VendorsContactAddress> qryAddress = null;
            IQueryable<VendorsSpecialNotes> qryNote = null;
            IQueryable<VendorsBillingDept> qryBilling = null;
            IQueryable<VendorsSalesContact> qrySales = null;
            IQueryable<VendorsHistory> qryHistory = null;
            IQueryable<VendorDefaults> qryVenDefault = null;

            VendorsContactAddress customerAddress = null;
            VendorsSpecialNotes customerNote = null;
            VendorsBillingDept customerBilling = null;
            VendorsSalesContact customerSales = null;
            VendorsHistory customerHistory = null;
            VendorDefaults vendordefault = null;

            Vendors vendors = db.Vendors.Find(id);
            if (vendors != null)
            {

                qryNote = db.VendorsSpecialNotes.Where(cust => cust.VendorId == vendors.Id);
                if (qryNote.Count() > 0)
                {
                    foreach (var item in qryNote)
                    {
                        db.VendorsSpecialNotes.Remove(item);
                    }
                }

                qryAddress = db.VendorsContactAddresses.Where(cust => cust.VendorId == vendors.Id);
                if (qryAddress.Count() > 0)
                {
                    customerAddress = qryAddress.FirstOrDefault<VendorsContactAddress>();
                    db.VendorsContactAddresses.Remove(customerAddress);
                }


                qryBilling = db.VendorsBillingDepts.Where(cust => cust.VendorId == vendors.Id);
                if (qryBilling.Count() > 0)
                {
                    customerBilling = qryBilling.FirstOrDefault<VendorsBillingDept>();
                    db.VendorsBillingDepts.Remove(customerBilling);
                }


                qrySales = db.VendorsSalesContacts.Where(cust => cust.VendorId == vendors.Id);
                if (qrySales.Count() > 0)
                {
                    foreach (var item in qrySales)
                    {
                        db.VendorsSalesContacts.Remove(item);
                    }
                }


                qryHistory = db.VendorsHistories.Where(cust => cust.VendorId == vendors.Id);
                if (qryHistory.Count() > 0)
                {
                    customerHistory = qryHistory.FirstOrDefault<VendorsHistory>();
                    db.VendorsHistories.Remove(customerHistory);
                }

                qryVenDefault = db.VendorDefaults.Where(cust => cust.VendorId == vendors.Id);
                if (qryVenDefault.Count() > 0)
                {
                    vendordefault = qryVenDefault.FirstOrDefault<VendorDefaults>();
                    if (vendordefault != null)
                    {
                        db.VendorDefaults.Remove(vendordefault);
                    }
                }

                db.Vendors.Remove(vendors);
                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        //
        // POST: /Vendors/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vendors vendors = db.Vendors.Find(id);
            db.Vendors.Remove(vendors);
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
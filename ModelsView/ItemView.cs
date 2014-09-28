using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using PagedList;
using TimelyDepotMVC.Models;
using TimelyDepotMVC.Models.Admin;
namespace TimelyDepotMVC.ModelsView
{
    public class ItemView
    {
        public ITEM item { get; set; }

        public VendorsContactAddress vendorcontactaddress { get; set; }

        public VendorItem vendoritem { get; set; }

        public SetupChargeDetail setupchragedetail { get; set; }

        public PagedList.IPagedList<TimelyDepotMVC.Models.Admin.VendorItem> vendorsitemList { get; set; }

        public PagedList.IPagedList<TimelyDepotMVC.Models.Admin.SUB_ITEM> subitemsList { get; set; }

        //public PagedList.IPagedList<TimelyDepotMVC.Models.Admin.Setup_for_Price> setupforpriceList { get; set; }
        public PagedList.IPagedList<TimelyDepotMVC.Models.Admin.PRICE> setupforpriceList { get; set; }


    }
}
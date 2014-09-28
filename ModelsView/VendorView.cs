using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PagedList;
using TimelyDepotMVC.Models;
using TimelyDepotMVC.Models.Admin;
namespace TimelyDepotMVC.ModelsView
{
    public class VendorView
    {
        public Vendors vendor { get; set; }

        public VendorsContactAddress vendoraddress { get; set; }

        public VendorsBillingDept vendorbilling { get; set; }

        public VendorsHistory vendorhistory { get; set; }

        public VendorsSalesContact vendorsalescontact { get; set; }

        public VendorsSpecialNotes vendornote { get; set; }

        public VendorDefaults vendordefaults { get; set; }

        public PagedList.IPagedList<TimelyDepotMVC.Models.Admin.VendorsSpecialNotes> vendornotesList { get; set; }

        public PagedList.IPagedList<TimelyDepotMVC.Models.Admin.VendorsSalesContact> vendorsalesList { get; set; }
    }
}
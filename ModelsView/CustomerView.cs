using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PagedList;
using TimelyDepotMVC.Models;
using TimelyDepotMVC.Models.Admin;

namespace TimelyDepotMVC.ModelsView
{
    public class CustomerView
    {
        public Customers customer { get; set; }

        public CustomersContactAddress customeraddress { get; set; }

        public CustomersBillingDept customerbilling { get; set; }

        public CustomersHistory customerhistory { get; set; }

        public CustomersCreditCardShipping customercredit { get; set; }

        public CustomerDefaults customerdefaults { get; set; }

        public CustomersSubsidiaryAddress custmersubsidiary { get; set; }

        public CustomersShipAddress customershipaddress { get; set; }

        public CustomersSalesContact customersalescontact { get; set; }

        public CustomersSpecialNotes customernote { get; set; }

        public CustomersCreditCardShipping customerscardshipping { get; set; }

        public PagedList.IPagedList<TimelyDepotMVC.Models.Admin.CustomersSpecialNotes> customernotesList { get; set; }

        public PagedList.IPagedList<TimelyDepotMVC.Models.Admin.CustomersSubsidiaryAddress> customersubsidiaryList { get; set; }

        public PagedList.IPagedList<TimelyDepotMVC.Models.Admin.CustomersSalesContact> customersalesList { get; set; }

        public PagedList.IPagedList<TimelyDepotMVC.Models.Admin.CustomersShipAddress> customershipList { get; set; }
    }
}
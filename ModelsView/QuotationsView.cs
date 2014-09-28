using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using PagedList;
using TimelyDepotMVC.Models;
using TimelyDepotMVC.Models.Admin;

namespace TimelyDepotMVC.ModelsView
{
    public class QuotationsView
    {
        public UserQuotation quotation { get; set; }

        public PagedList.IPagedList<TimelyDepotMVC.Models.Admin.UserQuotationDetail> quotationdetailsList { get; set; }
    }
}
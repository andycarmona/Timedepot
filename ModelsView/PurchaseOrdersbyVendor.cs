using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.ModelsView
{
    public class PurchaseOrdersbyVendor
    {
        public int PurchaseOrderId { get; set; }

        [Display(Name = "Purchase Order No")]
        public string PurchaseOrderNo { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? SODate { get; set; }

        public int? CustomerId { get; set; }

        [Display(Name = "Vendor No")]
        public string VendorNo { get; set; }

        [Display(Name = "Sub ItemID")]
        public string Sub_ItemID { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Qty")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        public double? Quantity { get; set; }

        [Display(Name = "Unit Price")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal? UnitPrice { get; set; }
    }
}
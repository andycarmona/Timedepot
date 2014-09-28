using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.ModelsView
{
    public class PurchaseOrderListHlp
    {
        public int PurchaseOrderId { get; set; }

        [Display(Name = "Vendor No")]
        public string VendorId { get; set; }

        [Display(Name = "Purchase Order No")]
        public string PurchaseOrderNo { get; set; }

        [Display(Name = "Sales Order No")]
        public string SalesOrderNo { get; set; }

        [Display(Name = "Required Date")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RequiredDate { get; set; }

        [Display(Name = "Ship Date")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ShipDate { get; set; }

        [Display(Name = "P/O Date")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PODate { get; set; }

        [Display(Name = "Amount")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? PaymentAmount { get; set; }

        public string ItemNo { get; set; }

        public int? TradeId { get; set; }

        public string ReceiveStatus { get; set; }

    }
}
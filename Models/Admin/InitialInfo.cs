using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class InitialInfo
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Sales Order No")]
        public int SalesOrderNo { get; set; }

        [Display(Name = "Invoice No")]
        public int InvoiceNo { get; set; }

        [Display(Name = "Payment No")]
        public int PaymentNo { get; set; }

        [Display(Name = "Purchase Order No")]
        public int PurchaseOrderNo { get; set; }

        [Display(Name = "Tax Rate")]
        public double TaxRate { get; set; }

        [Display(Name = "Payment Account")]
        public string PaymentAccount { get; set; }

        [Display(Name = "Shipper Info")]
        public string ShipperInfo { get; set; }

        [Display(Name = "Tracking No")]
        public int? TrackingNo { get; set; }
    }
}
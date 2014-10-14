using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.ModelsView
{
    public class PurchaseOrderList
    {
        public int PurchaseOrderId { get; set; }

        public int SalesOrderId { get; set; }

        [Display(Name = "Purchase Order No")]
        public string PurchaseOrderNo { get; set; }

        [Display(Name = "Sales Order Date")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? SODate { get; set; }

        public int CustomerId { get; set; }

        [Display(Name = "Vendor No")]
        public string VendorNo { get; set; }

        [Display(Name = "Vendor Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Sales Order No")]
        public string SalesOrderNo { get; set; }

        [Display(Name = "Ship Date")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ShipDate { get; set; }

        [Display(Name = "Payment")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? PaymentAmount { get; set; }

        [Display(Name = "Total Amount")]
        public double? TotalAmount { get; set; }

        [Display(Name = "Sales Order Amount")]
        public double SalesAmount { get; set; }

        [Display(Name = "Balance Due")]
        public double? BalanceDue { get; set; }
        
        [Display(Name = "Invoice #")]
        public string InvoiceNo { get; set; }

        public int InvoiceId { get; set; }

        [Display(Name = "InvoiceDate")]
        public DateTime? InvoiceDate { get; set; }


    }
}
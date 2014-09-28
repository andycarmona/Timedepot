using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class SalesOrderTES
    {
        [Key]
        public int SalesOrderId { get; set; }

        [Display(Name = "Sales Order No")]
        public string SalesOrderNo { get; set; }

        //[Display(Name = "S/O Date")]
        //[DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? SODate { get; set; }

        //[Display(Name = "Ship Via")]
        //public string ShipVia { get; set; }

        //[Display(Name = "Ship Date")]
        //[DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? ShipDate { get; set; }

        //public int? TradeId { get; set; }

        //[Required]
        //public int? CustomerId { get; set; }

        //public int? CustomerShiptoId { get; set; }

        //public string CustomerShipLocation { get; set; }

        //public int? VendorId { get; set; }

        //[Display(Name = "Bussines Type")]
        //public string BussinesType { get; set; }

        //[Display(Name = "Vendor Address")]
        //public string VendorAddress { get; set; }

        //[Display(Name = "Purchase Order No")]
        //public string PurchaseOrderNo { get; set; }

        //[Display(Name = "Payment Terms")]
        //public string PaymentTerms { get; set; }

        //[Display(Name = "Payment Date")]
        //[DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? PaymentDate { get; set; }

        //[Display(Name = "Payment Amount")]
        //[DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        //public decimal? PaymentAmount { get; set; }

        //[Display(Name = "Shipping & Handling")]
        //[DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        //public decimal? ShippingHandling { get; set; }

        //[Display(Name = "C/C#")]
        //public string CreaditCardNo { get; set; }

        //[Display(Name = "Note")]
        //public string Note { get; set; }

        //public bool IsBlindShip { get; set; }

        //[Display(Name = "Sales Rep.")]
        //public string SalesRep { get; set; }

        //[Display(Name = "Tax Rate")]
        //public double? Tax_rate { get; set; }

        //[Display(Name = "Invs Tax")]
        //public double? Invs_Tax { get; set; }

        //[Display(Name = "Approved by")]
        //public string Approvedby { get; set; }

        //[Display(Name = "Aproved Date")]
        //[DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? AprovedDate { get; set; }

        //[Display(Name = "Require Date")]
        //[DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? Requiredate { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }

        [Display(Name = "Invoice No")]
        public string InvoiceNo { get; set; }

        public int? SalesOrderId { get; set; }

        [Display(Name = "Sales Order No")]
        public string SalesOrderNo { get; set; }

        [Display(Name = "Invoice Date")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? InvoiceDate { get; set; }

        [Display(Name = "Ship Via")]
        public string ShipVia { get; set; }

        [Display(Name = "Ship Date")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ShipDate { get; set; }

        public int? TradeId { get; set; }

        [Required]
        public int? CustomerId { get; set; }

        public int? CustomerShiptoId { get; set; }

        public string CustomerShipLocation { get; set; }

        public int? VendorId { get; set; }

        [Display(Name = "Bussines Type")]
        public string BussinesType { get; set; }

        [Display(Name = "Vendor Address")]
        public string VendorAddress { get; set; }

        [Display(Name = "Purchase Order No")]
        public string PurchaseOrderNo { get; set; }

        [Display(Name = "Payment Terms")]
        public string PaymentTerms { get; set; }

        [Display(Name = "Payment Date")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PaymentDate { get; set; }

        [Display(Name = "Payment Amount")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? PaymentAmount { get; set; }

        [Display(Name = "Shipping & Handling")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? ShippingHandling { get; set; }

        [Display(Name = "C/C#")]
        public string CreaditCardNo { get; set; }

        [Display(Name = "Note")]
        public string Note { get; set; }

        public bool IsBlindShip { get; set; }

        [Display(Name = "Sales Rep.")]
        public string SalesRep { get; set; }

        [Display(Name = "Tracking No.")]
        public string TrackingNo { get; set; }

        [Display(Name = "Tax Rate")]
        public decimal? Tax_rate { get; set; }

        [Display(Name = "Invs Tax")]
        public decimal? Invs_Tax { get; set; }

        [Display(Name = "Warehouse")]
        public string Warehouse { get; set; }

        [Display(Name = "Name")]
        public string FromName { get; set; }

        [Display(Name = "Title")]
        public string FromTitle { get; set; }

        [Display(Name = "Company")]
        public string FromCompany { get; set; }

        [Display(Name = "Address1")]
        public string FromAddress1 { get; set; }

        [Display(Name = "Address2")]
        public string FromAddress2 { get; set; }

        [Display(Name = "City")]
        public string FromCity { get; set; }

        [Display(Name = "State")]
        public string FromState { get; set; }

        [Display(Name = "Zip")]
        public string FromZip { get; set; }

        [Display(Name = "Country")]
        public string FromCountry { get; set; }

        [Display(Name = "Email")]
        public string FromEmail { get; set; }

        [Display(Name = "Tel")]
        public string FromTel { get; set; }

        [Display(Name = "Fax")]
        public string FromFax { get; set; }

        [Display(Name = "Name")]
        public string ToName { get; set; }

        [Display(Name = "Title")]
        public string ToTitle { get; set; }

        [Display(Name = "Company")]
        public string ToCompany { get; set; }

        [Display(Name = "Address1")]
        public string ToAddress1 { get; set; }

        [Display(Name = "Address2")]
        public string ToAddress2 { get; set; }

        [Display(Name = "City")]
        public string ToCity { get; set; }

        [Display(Name = "State")]
        public string ToState { get; set; }

        [Display(Name = "Zip")]
        public string ToZip { get; set; }

        [Display(Name = "Country")]
        public string ToCountry { get; set; }

        [Display(Name = "Email")]
        public string ToEmail { get; set; }

        [Display(Name = "Tel")]
        public string ToTel { get; set; }

        [Display(Name = "Fax")]
        public string ToFax { get; set; }
    }
}
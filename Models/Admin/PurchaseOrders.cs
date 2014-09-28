using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class PurchaseOrders
    {
        [Key]
        public int PurchaseOrderId { get; set; }

        [Display(Name = "Purchase Order No")]
        public string PurchaseOrderNo { get; set; }

        [Display(Name = "Sales Order No")]
        public string SalesOrderNo { get; set; }

        public int? TradeId { get; set; }

        [Display(Name = "P/O Date")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PODate { get; set; }

        [Display(Name = "Vendor No")]
        public string VendorId { get; set; }

        [Display(Name = "Ship Date")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ShipDate { get; set; }

        [Display(Name = "Paid By")]
        public string PaidBy { get; set; }

        [Display(Name = "Blind Drop")]
        public string BlindDrop { get; set; }

        [Display(Name = "Logo")]
        public string Logo { get; set; }

        [Display(Name = "Imprint Color")]
        public string ImprintColor { get; set; }

        [Display(Name = "Reference")]
        public string PurchaseOrderReference { get; set; }

        public bool IsBlindShip { get; set; }

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

        [Display(Name = "Receive Status")]
        public string ReceiveStatus { get; set; }

        [Display(Name = "Required Date")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RequiredDate { get; set; }

        [Display(Name = "Invoice")]
        public string Invoice { get; set; }

        [Display(Name = "Tracking No")]
        public string TrackingNo { get; set; }

        [Display(Name = "Terms")]
        public string Terms { get; set; }

        [Display(Name = "Order By")]
        public string OrderBy { get; set; }

        [Display(Name = "Warehouse")]
        public string Warehouse { get; set; }

        [Display(Name = "Bill to")]
        public string Billto { get; set; }

        [Display(Name = "Ship Via")]
        public string ShipVia { get; set; }
    }
}
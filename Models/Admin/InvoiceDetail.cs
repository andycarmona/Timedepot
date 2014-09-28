using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class InvoiceDetail
    {
        [Key]
        public int Id { get; set; }

        public int? InvoiceId { get; set; }

        [Display(Name = "Req.Qty")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        public double? Quantity { get; set; }

        [Display(Name = "Ship Qty")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        public double? ShipQuantity { get; set; }

        [Display(Name = "B.O.Qty")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        public double? BackOrderQuantity { get; set; }

        [Display(Name = "ItemID")]
        public string ItemID { get; set; }

        [Display(Name = "Sub ItemID")]
        public string Sub_ItemID { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Tax")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        public double? Tax { get; set; }

        [Display(Name = "Unit Price")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal? UnitPrice { get; set; }

        [Display(Name = "Item Position")]
        public int? ItemPosition { get; set; }

        [Display(Name = "Item Order")]
        //[DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        public double? ItemOrder { get; set; }

        [Display(Name = "Logo")]
        public string Logo { get; set; }

        [Display(Name = "Imprint Method")]
        public string ImprintMethod { get; set; }
    }
}
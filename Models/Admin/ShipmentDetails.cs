using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class ShipmentDetails
    {
        [Display(Name = "ShipmentId")]
        public int? ShipmentId { get; set; }

        [Key]
        [Display(Name = "ShipmentId")]
        public int ShipmentDetailID { get; set; }

        [Display(Name = "Invoice DetailId")]
        public int? DetailId { get; set; }

        [Display(Name = "Box #")]
        public string BoxNo { get; set; }

        //[Display(Name = "Sub_ItemID")]
        [Display(Name = "Item")]
        public string Sub_ItemID { get; set; }

        [Display(Name = "Qty")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        public double? Quantity { get; set; }

        [Display(Name = "Unit Price")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal? UnitPrice { get; set; }

        [Display(Name = "Weight")]
        public int? UnitWeight { get; set; }

        [Display(Name = "Height")]
        public int? DimensionH { get; set; }

        [Display(Name = "Length")]
        public int? DimensionL { get; set; }

        [Display(Name = "Width")]
        public int? DimensionD { get; set; }

        [Display(Name = "Reference # 1")]
        public string Reference1 { get; set; }

        [Display(Name = "Reference # 2")]
        public string Reference2 { get; set; }
    }
}
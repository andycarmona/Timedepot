using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class SUB_ITEM
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string ItemID { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }

        [MaxLength(50)]
        public string Field1 { get; set; }

        [MaxLength(50)]
        public string Sub_ItemID { get; set; }

        [MaxLength(50)]
        public string Sub_GroupID { get; set; }

        [Display(Name = "On Hand Db")]
        public double? OnHand_Db { get; set; }

        [Display(Name = "On Hand Cr")]
        public double? OnHand_Cr { get; set; }

        [Display(Name = "Open/SO Db")]
        public double? OpenSO_Db { get; set; }

        [Display(Name = "Open/SO Cr")]
        public double? OpenSO_Cr { get; set; }

        [Display(Name = "Open/PO _Db")]
        public double? OpenPO_Db { get; set; }

        [Display(Name = "Open/PO Cr")]
        public double? OpenPO_Cr { get; set; }

        [Display(Name = "Quantity Db")]
        public double? Qty_Db { get; set; }

        [Display(Name = "Quantity Cr")]
        public double? Qty_Cr { get; set; }

        [Display(Name = "Gross Price")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? GrossPrice { get; set; }

        [Display(Name = "Net Price")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? NetPrice { get; set; }

        [Display(Name = "Part No")]
        public string PartNo { get; set; }

        [Display(Name = "Vendor Stock")]
        public double? VendorStock { get; set; }
    }
}
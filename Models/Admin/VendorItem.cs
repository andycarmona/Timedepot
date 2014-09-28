using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class VendorItem
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "ItemId")]
        public string ItemId { get; set; }

        [Display(Name = "Vendor No")]
        public string VendorNo { get; set; }

        [Display(Name = "Vendor Part No")]
        public string VendorPartNo { get; set; }

        [Display(Name = "Unit Cost (1C)")]
        public decimal? Cost { get; set; }

        [Display(Name = "Unit Cost (Blind)")]
        public decimal? CostBlind { get; set; }

        [Display(Name = "Run Charge")]
        public decimal? RunCharge { get; set; }

        [Display(Name = "Setup Charge")]
        public decimal? SetupCharge { get; set; }

        [Display(Name = "Re Setup Charge")]
        public decimal? ReSetupCharge { get; set; }

        //[Display(Name = "Lead Time")]
        //[DisplayFormat(DataFormatString = "{0: yyyy-MMM-dd}", ApplyFormatInEditMode = false)]
        //[DisplayFormat(DataFormatString = "{0: hh:mm:ss}", ApplyFormatInEditMode = true)]
        //[DisplayFormat(DataFormatString = "{0: mm:ss}", ApplyFormatInEditMode = true)]
        //public DateTime? LeadTime { get; set; }

        [Display(Name = "Lead Time Hours")]
        public int? LeadTimeHrs { get; set; }

        [Display(Name = "Lead Time Minutes")]
        public int? LeadTimeMin { get; set; }

        [Display(Name = "Lead Time Seconds")]
        public int? LeadTimeSec { get; set; }

        [Display(Name = "Lead Time")]
        public TimeSpan? LeadTime { get; set; }

        [Display(Name = "Update Date")]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[DisplayFormat(DataFormatString = "{0: hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? UpdateDate { get; set; }
    }
}
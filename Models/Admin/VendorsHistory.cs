using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class VendorsHistory
    {
        [Key]
        public int Id { get; set; }

        public int? VendorId { get; set; }

        [Display(Name = "Outstanding Balance")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? OutstandingBalance { get; set; }

        [Display(Name = "Open Purchase Order")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? OpenPurchaseOrder { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class Vendors
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Vendor No")]
        public string VendorNo { get; set; }

        public bool Status { get; set; }

        [Display(Name = "Vendor Type")]
        public string BussinesType { get; set; }

        public string Origin { get; set; }

        [Display(Name = "Credit Limit")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? CreditLimit { get; set; }

        [Display(Name = "Payment Terms")]
        public string PaymentTerms { get; set; }

        [Display(Name = "Bussines Since")]
        //[DisplayFormat(DataFormatString = "{0: yyyy-MMM-dd}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? BussinesSice { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class Customers
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Customer No")]
        [Required]
        public string CustomerNo { get; set; }

        public bool Status { get; set; }

        [Display(Name = "Type")]
        public string BussinesType { get; set; }

        [Display(Name = "Sales Person")]
        public string SalesPerson { get; set; }

        [Display(Name = "Dept No")]
        public string DeptoNo { get; set; }

        [Display(Name = "Seller Permit No")]
        public string SellerPermintNo { get; set; }

        [Display(Name = "ASI #")]
        public string ASINo { get; set; }

        [Display(Name = "PPAI #")]
        public string PPAINo { get; set; }

        [Display(Name = "Sage #")]
        public string SageNo { get; set; }

        [Display(Name = "Original")]
        public string Origin { get; set; }

        [Display(Name = "Credit Limit")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? CreditLimit { get; set; }

        [Display(Name = "Terms")]
        public string PaymentTerms { get; set; }

        [Display(Name = "Bussines Since")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BussinesSice { get; set; }
    }
}
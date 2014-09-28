using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class VendorsBillingDept
    {
        [Key]
        public int Id { get; set; }

        public int? VendorId { get; set; }

        [Display(Name = "Beneficiary")]
        public string Beneficiary { get; set; }

        [Display(Name = "Beneficiary Account No")]
        public string BeneficiaryAccountNo { get; set; }

        [Display(Name = "S.W.I.F.T.")]
        public string SWIFT { get; set; }

        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Display(Name = "Bank Address")]
        public string BankAddress { get; set; }


    }
}
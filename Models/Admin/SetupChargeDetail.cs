using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimelyDepotMVC.Models.Admin
{
    public class SetupChargeDetail
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "SetUp Charge")]
        public decimal SetUpCharge { get; set; }

        [Display(Name = "Run Charge")]
        public decimal RunCharge { get; set; }

        [MaxLength(40)]
        public string itemid { get; set; }

        [Display(Name = "Re Setup Charge")]
        public decimal ReSetupCharge { get; set; }

        [Display(Name = "Re Setup Code")]
        public string ReSetupChargeDiscountCode { get; set; }

        [Display(Name = "Run Code")]
        public string RunChargeDiscountCode { get; set; }

        [Display(Name = "Setup Code")]
        public string SetupChargeDiscountCode { get; set; }

        [Display(Name = "1st Setup Free")]
        public bool FirstSetupFree { get; set; }
    }
}
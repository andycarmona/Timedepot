using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimelyDepotMVC.Models.Admin
{
    public class UserQuotationDetail
    {
        [Key]
        public int Id { get; set; }

        public int? DetailId { get; set; }

        [MaxLength(100)]
        public string ProductType { get; set; }

        [MaxLength(20)]
        public string Quantity { get; set; }

        public decimal Amount { get; set; }

        public string ItemID { get; set; }

        public int? Status { get; set; }

        [Display(Name = "Shipped Quantity")]
        public double? ShippedQuantity { get; set; }

        [Display(Name = "B.O. Quantity")]
        public double? BOQuantity { get; set; }
    }
}
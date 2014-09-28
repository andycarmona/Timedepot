using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimelyDepotMVC.Models.Admin
{
    public class PRICE
    {
        [MaxLength(50)]
        public string Item { get; set; }

        public Int16 Qty { get; set; }

        [MaxLength(50)]
        public string PriceType { get; set; }

        [Display(Name="Price")]
        public decimal thePrice { get; set; }

        [MaxLength(50)]
        public string Discount_Code { get; set; }

        [MaxLength(20)]
        public string Description { get; set; }

        [Key]
        public int Id { get; set; }
    }
}
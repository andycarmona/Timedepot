using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class Setup_for_Price
    {
        //[Key, Column(Order = 0)]
        [MaxLength(50)]
        public string Item { get; set; }

        //[Key, Column(Order = 1)]
        public Int16? Qty { get; set; }

        public decimal Price { get; set; }

        [MaxLength(50)]
        public string Discount_Code { get; set; }

        [MaxLength(20)]
        public string Description { get; set; }

        [Key]
        public int Id { get; set; }
    }
}
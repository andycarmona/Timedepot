using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class DiscountManage
    {
        [MaxLength(60)]
        [Display(Name = "DiscountName")]
        public string DiscountName { get; set; }

        [MaxLength(60)]
        public string DiscountPercentage { get; set; }

        [Key]
        public int id { get; set; }
    }
}
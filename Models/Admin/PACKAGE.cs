using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class PACKAGE
    {
        [Key]
        [MaxLength(50)]
        public string PackageID { get; set; }

        [MaxLength(50)]
        public string Weight { get; set; }

        public decimal? Price { get; set; }

        public string Description { get; set; }

        [MaxLength(500)]
        public string ImagePath { get; set; }

        [MaxLength(500)]
        public string DiscountCode { get; set; }
    }
}
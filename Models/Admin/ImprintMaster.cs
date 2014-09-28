using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class ImprintMaster
    {
        [Key]
        public int ImprintId { get; set; }

        [MaxLength(100)]
        public string ImprintName { get; set; }

        public decimal? SetUpCharge { get; set; }

        public decimal? SetUpCharge2 { get; set; }

        public decimal? RunCharge { get; set; }

        public decimal? ColorCharge { get; set; }

        [MaxLength(100)]
        public string Displayname { get; set; }

        [MaxLength(100)]
        public string DiscountCode { get; set; }

        [MaxLength(100)]
        public string ImagePath { get; set; }

        public int? NumberColor { get; set; }

        [MaxLength(500)]
        public string Information { get; set; }

        [MaxLength(60)]
        public string RunChargeInclude { get; set; }

    }
}
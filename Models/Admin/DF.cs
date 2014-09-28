using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class DF
    {
        [Key]
        [MaxLength(255)]
        public string ItemID { get; set; }

        [MaxLength(50)]
        public string DialType { get; set; }

        [MaxLength(255)]
        public string DescA { get; set; }

        [MaxLength(50)]
        public string CollectionID { get; set; }

        [MaxLength(50)]
        public string Start_Qty { get; set; }

        [MaxLength(255)]
        public string Field11 { get; set; }

        [MaxLength(255)]
        public string Field12 { get; set; }

        [MaxLength(255)]
        public string Field13 { get; set; }

        [MaxLength(255)]
        public string Field14 { get; set; }

        [MaxLength(255)]
        public string Field15 { get; set; }

        public decimal? Field16 { get; set; }

        public decimal? Field17 { get; set; }

        public decimal? Field18 { get; set; }

        public decimal? Field19 { get; set; }

        public decimal? Field20 { get; set; }
    }
}
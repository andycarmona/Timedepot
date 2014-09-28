using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimelyDepotMVC.Models.Admin
{
    public class PackagingDetail
    {
        [Key]
        public int Id { get; set; }

        public int? PackId { get; set; }
        
        [MaxLength(100)]
        public string ItemNo { get; set; }

        [MaxLength(100)]
        public string DisplayName { get; set; }

        public decimal? Price { get; set; }

        [MaxLength(60)]
        public string ImagePath { get; set; }
    }
}
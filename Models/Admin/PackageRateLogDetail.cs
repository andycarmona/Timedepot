using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class PackageRateLogDetails
    {
        [Key]
        public int Id { get; set; }

        public int? IdRateLog { get; set; }

        [Display(Name = "Box No")]
        public string BoxNo { get; set; }

        [Display(Name = "Quantity")]
        public string Quantity { get; set; }

        [Display(Name = "Dimensions (WIxHIxLEN")]
        public string Dimensions { get; set; }

        [Display(Name = "Dimensions Units")]
        public string DimensionsUnits { get; set; }

        [Display(Name = "Weigth Units")]
        public string WeigthUnits { get; set; }

        [Display(Name = "Declared Value")]
        public string DeclaredValue { get; set; }

        [Display(Name = "Request Code")]
        public string RequestCode { get; set; }

        [Display(Name = "Package Type Code")]
        public string PackageTypeCode { get; set; }
    }
}
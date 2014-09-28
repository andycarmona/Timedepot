using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class ITEM
    {
        [Key]
        [MaxLength(50)]
        public string ItemID { get; set; }

        [MaxLength(50)]
        public string PicID { get; set; }

        [MaxLength(50)]
        public string DialType { get; set; }

        [Display(Name = "Description")]
        public string DescA { get; set; }

        public string DescB { get; set; }

        [MaxLength(50)]
        public string CollectionID { get; set; }

        [MaxLength(50)]
        public string Price_ID { get; set; }

        [MaxLength(50)]
        public string Misc_ID { get; set; }

        [MaxLength(1)]
        public string Inactive { get; set; }

        public string Keywords { get; set; }

        [MaxLength(1)]
        public string Special { get; set; }

        [MaxLength(1)]
        public string New { get; set; }

        [Display(Name = "Title")]
        [MaxLength(600)]
        public string title { get; set; }

        public string UnitPerCase { get; set; }

        public string UnitWeight { get; set; }

        public string CaseWeight { get; set; }

        public int? DimensionH { get; set; }

        public int? DimensionL { get; set; }

        public int? DimensionD { get; set; }

        public bool Status { get; set; }

        public string Pic2ID { get; set; }

        public string Pic3ID { get; set; }

        [Display(Name = "Dept No")]
        public string DeptoNo { get; set; }

        [Display(Name = "Year of Product")]
        public string YearProduct { get; set; }

        [Display(Name = "Class No")]
        public string ClassNo { get; set; }

        [Display(Name = "UPC Code")]
        public string UPCCode { get; set; }

        [Display(Name = "Case Dimension L")]
        public string CaseDimensionL { get; set; }

        [Display(Name = "Case Dimension W")]
        public string CaseDimensionW { get; set; }

        [Display(Name = "Case Dimension H")]
        public string CaseDimensionH { get; set; }

        public string Note { get; set; }
    }
}
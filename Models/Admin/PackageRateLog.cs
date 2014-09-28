using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class PackageRateLog
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Date Submit")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateSubmit { get; set; }

        [Display(Name = "ItemId")]
        public string ItemId { get; set; }

        [Display(Name = "Quantity")]
        public string Quantity { get; set; }

        [Display(Name = "Postal Zip Code")]
        public string PostalZipCode { get; set; }

        [Display(Name = "Boxes")]
        public string Boxes { get; set; }

        [Display(Name = "Items Last Box")]
        public string ItemsLastBox { get; set; }

        [Display(Name = "Full Box Weight")]
        public string FullBoxWeight { get; set; }

        [Display(Name = "Partial Box Weight")]
        public string PartialBoxWeight { get; set; }

        [Display(Name = "Value per Full Box")]
        public string ValueperFullBox { get; set; }

        [Display(Name = "Value per Partial Box")]
        public string ValueperPartialBox { get; set; }

        [Display(Name = "Case Height")]
        public string CaseHeight { get; set; }

        [Display(Name = "Case Lenght")]
        public string CaseLenght { get; set; }

        [Display(Name = "Case Width")]
        public string CaseWidth { get; set; }

        [Display(Name = "Case Weight")]
        public string CaseWeight { get; set; }

        [Display(Name = "Unit Price")]
        public string UnitPrice { get; set; }
    }
}
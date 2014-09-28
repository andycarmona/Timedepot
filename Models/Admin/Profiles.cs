using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class Profiles
    {
        [Key]
        public Guid UserId { get; set; }

        [Display(Name = "Property Names")]
        [MaxLength(4000)]
        [Required]
        public string PropertyNames { get; set; }

        [Display(Name = "Property Value Strings")]
        [MaxLength(4000)]
        [Required]
        public string PropertyValueStrings { get; set; }

        [Display(Name = "Property Value Binary")]
        //[MaxLength(4000)]
        [Required]
        public string PropertyValueBinary { get; set; }

        [Display(Name = "Last Updated Date")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime LastUpdatedDate { get; set; }
    }
}
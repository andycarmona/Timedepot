using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class VendorsSpecialNotes
    {
        [Key]
        public int Id { get; set; }

        public int? VendorId { get; set; }

        [Display(Name = "Special Note")]
        public string SpecialNote { get; set; }
    }
}
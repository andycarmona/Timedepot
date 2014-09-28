using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class CustomersSpecialNotes
    {
        [Key]
        public int Id { get; set; }

        public int? CustomerId { get; set; }

        [Display(Name = "Note")]
        public string SpecialNote { get; set; }
    }
}
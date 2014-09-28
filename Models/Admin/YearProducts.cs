using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class YearProducts
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Year of Product")]
        public string YearofProducts { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
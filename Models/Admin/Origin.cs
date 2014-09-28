using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimelyDepotMVC.Models.Admin
{
    public class Origin
    {
        [Key]
        public int Id { get; set; }

        [Display(Name="Origin")]
        public string Name { get; set; }
    }
}
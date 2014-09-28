using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimelyDepotMVC.Models.Admin
{
    public class Deptos
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Depto No")]
        public string DeptoNo { get; set; }

        public string Name { get; set; }
    }
}
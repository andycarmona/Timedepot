using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class Warehouses
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Warehouse")]
        public string Warehouse { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class Operator
    {
        [Key]
        [MaxLength(50)]
        public string Code { get; set; }

        [MaxLength(50)]
        public string Password { get; set; }
    }
}
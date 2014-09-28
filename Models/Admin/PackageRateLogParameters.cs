using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class PackageRateLogParameters
    {
        [Key]
        public int Id { get; set; }

        public int? IdRateLog { get; set; }

        [Display(Name = "Parameter")]
        public string Parameter { get; set; }

        [Display(Name = "Value")]
        public string ParameterValue { get; set; }
    }
}
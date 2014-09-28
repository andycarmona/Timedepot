using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class aspnet_Applications
    {
        [Display(Name = "Application Name")]
        [MaxLength(256)]
        public string ApplicationName { get; set; }

        [Display(Name = "Lowered Application Name")]
        [MaxLength(256)]
        public string LoweredApplicationName { get; set; }

        [Key]
        public Guid ApplicationId { get; set; }

        [Display(Name = "Description")]
        [MaxLength(256)]
        public string Description { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class Applications
    {
        [Display(Name = "Application Name")]
        [MaxLength(235)]
        public string ApplicationName { get; set; }

        [Key]
        public Guid ApplicationId { get; set; }

        [Display(Name = "Description")]
        [MaxLength(256)]
        public string Description { get; set; }
    }
}
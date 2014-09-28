using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class Roles
    {
        public Guid ApplicationId { get; set; }

        [Key]
        public Guid RoleId { get; set; }

        [Display(Name = "Role Name")]
        [MaxLength(256)]
        public string RoleName { get; set; }

        [Display(Name = "Description")]
        [MaxLength(256)]
        public string Description { get; set; }
    }
}
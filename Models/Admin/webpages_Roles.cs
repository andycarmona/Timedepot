using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimelyDepotMVC.Models.Admin
{
    public class webpages_Roles
    {
        [Key]
        public int RoleId { get; set; }

        [MaxLength(256)]
        public string RoleName { get; set; }
    }
}
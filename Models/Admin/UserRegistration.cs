using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimelyDepotMVC.Models.Admin
{
    public class UserRegistration
    {
        [Key]
        public int RId { get; set; }

        [MaxLength(200)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [MaxLength(200)]
        public string UserPassword { get; set; }

        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

    }
}
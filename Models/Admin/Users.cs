using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class Users
    {
        public Guid ApplicationId { get; set; }

        [Key]
        public Guid UserId { get; set; }

        [Display(Name = "User Name")]
        [MaxLength(50)]
        public string UserName { get; set; }

        public bool IsAnonymous { get; set; }

        [Display(Name = "Last Activity Date")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastActivityDate { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimelyDepotMVC.Models.Admin
{
    public class webpages_Membership
    {
        [Key]
        public int UserId { get; set; }

        public DateTime CreateDate { get; set; }

        [MaxLength(128)]
        public string ConfirmationToken { get; set; }

        public bool IsConfirmed { get; set; }

        public DateTime? LastPasswordFailureDate { get; set; }

        public int PasswordFailuresSinceLastSuccess { get; set; }

        [MaxLength(128)]
        public string Password { get; set; }

        public DateTime? PasswordChangedDate { get; set; }

        [MaxLength(128)]
        public string PasswordSalt { get; set; }

        [MaxLength(128)]
        public string PasswordVerificationToken { get; set; }

        public DateTime? PasswordVerificationTokenExpirationDate { get; set; }
    }
}
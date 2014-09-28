using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class Memberships
    {
        public Guid ApplicationId { get; set; }

        [Key]
        public Guid UserId { get; set; }

        [Display(Name = "Password")]
        [MaxLength(128)]
        public string Password { get; set; }

        public int PasswordFormat { get; set; }

        [Display(Name = "Password Salt")]
        [MaxLength(128)]
        public string PasswordSalt { get; set; }

        [Display(Name = "Email")]
        [MaxLength(256)]
        public string Email { get; set; }

        [Display(Name = "Password Question")]
        [MaxLength(256)]
        public string PasswordQuestion { get; set; }


        [Display(Name = "Password Answer")]
        [MaxLength(128)]
        public string PasswordAnswer { get; set; }

        public bool IsApproved { get; set; }

        public bool IsLockedOut { get; set; }

        [Display(Name = "Create Date")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Last Login Date")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastLoginDate { get; set; }

        [Display(Name = "Last Password Changed Date")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastPasswordChangedDate { get; set; }

        [Display(Name = "Last Lockout Date")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastLockoutDate { get; set; }

        public int FailedPasswordAttemptCount { get; set; }

        [Display(Name = "Failed Password Attempt Window Start")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FailedPasswordAttemptWindowStart { get; set; }

        public int FailedPasswordAnswerAttemptCount { get; set; }

        [Display(Name = "Failed Password Answer Attempt Windows Start")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FailedPasswordAnswerAttemptWindowsStart { get; set; }

        [Display(Name = "Comment")]
        [MaxLength(256)]
        public string Comment { get; set; }
    }
}
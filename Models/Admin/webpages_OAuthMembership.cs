using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimelyDepotMVC.Models.Admin
{
    public class webpages_OAuthMembership
    {
        [Key, Column(Order=0)]
        [MaxLength(30)]
        public string Provider { get; set; }

        [Key, Column(Order = 1)]
        [MaxLength(100)]
        public string ProviderUserId { get; set; }

        public int UserId { get; set; }
    }
}
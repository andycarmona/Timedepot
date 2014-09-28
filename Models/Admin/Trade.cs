using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class Trade
    {
        [Key]
        public int TradeId { get; set; }

        [Display(Name = "Trade Name")]
        public string TradeName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        [Display(Name = "Zip")]
        public string PostCode { get; set; }

        public string Country { get; set; }

        public string Tel { get; set; }

        public string Fax { get; set; }

        [Display(Name = "Web Site")]
        public string WebSite { get; set; }

        public string Email { get; set; }

        [Display(Name = "ASI No")]
        public string ASINo { get; set; }

        [Display(Name = "Sage No")]
        public string SageNo { get; set; }

        [Display(Name = "PPAI No")]
        public string PPAINo { get; set; }
    }
}
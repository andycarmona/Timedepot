using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class CustomersCreditCardShipping
    {
        [Key]
        public int Id { get; set; }

        public int? CustomerId { get; set; }

        [Display(Name = "Card Name")]
        public string Name { get; set; }

        [Display(Name = "Credit Number")]
        public string CreditNumber { get; set; }

        [Display(Name = "Card Type")]
        public string CardType { get; set; }

        [Display(Name = "Secure Code")]
        public string SecureCode { get; set; }

        [Display(Name = "Expiration Date")]
        //[DisplayFormat(DataFormatString = "{0: yyyy-MMM-dd}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ExpirationDate { get; set; }

        [Display(Name = "Phone")]
        public string Tel { get; set; }

        [Display(Name = "Address1")]
        public string Address1 { get; set; }

        [Display(Name = "Address2")]
        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string Country { get; set; }

    }
}
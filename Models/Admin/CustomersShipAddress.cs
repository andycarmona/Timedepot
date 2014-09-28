using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class CustomersShipAddress
    {
        [Key]
        public int Id { get; set; }

        public int? CustomerId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        public string Address1 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string Country { get; set; }

        public string Email { get; set; }

        public string Address2 { get; set; }

        [Display(Name = "Shipper Account")]
        public string ShipperAccount { get; set; }

        [Display(Name = "Shipping Preference")]
        public string ShippingPreference { get; set; }

        [Display(Name = "Phone")]
        public string Tel { get; set; }

        public string Fax { get; set; }
    }
}
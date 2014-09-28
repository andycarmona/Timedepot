using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class VendorsContactAddress
    {
        [Key]
        public int Id { get; set; }

        public int? VendorId { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        [Display(Name = "Postal Code")]
        public string Zip { get; set; }

        public string Country { get; set; }

        [Display(Name = "Phone 3")]
        public string Tel { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string Website { get; set; }

        [Display(Name = "Note")]
        public string Note { get; set; }

        [Display(Name = "Address3")]
        public string Address3 { get; set; }

        [Display(Name = "Phone 1")]
        public string Tel1 { get; set; }

        [Display(Name = "Phone 2")]
        public string Tel2 { get; set; }
    }
}
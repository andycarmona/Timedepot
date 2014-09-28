using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.ModelsView
{
    public class VendorList
    {
        public int Id { get; set; }

        [Display(Name = "Vendor No")]
        public string VendorNo { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Phone")]
        public string Tel { get; set; }

        public string State { get; set; }

        public string Country { get; set; }
    }
}
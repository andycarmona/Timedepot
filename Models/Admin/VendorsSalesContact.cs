﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class VendorsSalesContact
    {
        [Key]
        public int Id { get; set; }

        public int? VendorId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string Country { get; set; }

        [Display(Name = "Phone")]
        public string Tel { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string Note { get; set; }
    }
}
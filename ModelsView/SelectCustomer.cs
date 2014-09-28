using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.ModelsView
{
    public class SelectCustomer
    {
        [Display(Name = "Customer No")]
        public string CustomerNo { get; set; }

        [Display(Name = "Company Name")]
        public string Companyname { get; set; }

        public string Email { get; set; }
    }
}
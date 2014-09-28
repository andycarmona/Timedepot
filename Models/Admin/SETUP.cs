using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimelyDepotMVC.Models.Admin
{
    public class SETUP
    {
        [Key]
        [MaxLength(50)]
        public string SetupID { get; set; }

        public int? Run_charge { get; set; }

        public int? SetupCost { get; set; }

        [MaxLength(50)]
        public string Discount_Code { get; set; }

        public int? Min { get; set; }

        public string Description { get; set; }

        public bool Dial { get; set; }
    }
}
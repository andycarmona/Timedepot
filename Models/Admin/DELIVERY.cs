using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class DELIVERY
    {
        [Key]
        [MaxLength(50)]
        public string DeliveryID { get; set; }

        public string Description { get; set; }
    }
}
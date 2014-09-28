using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class InformationId
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        [Display(Name = "InformationId")]
        public string InformationId1 { get; set; }

        [MaxLength(800)]
        public string Description { get; set; }

        [MaxLength(200)]
        public string Priceinformation { get; set; }

        public string ProductionTime { get; set; }

        [MaxLength(200)]
        public string Artwork { get; set; }
    }
}
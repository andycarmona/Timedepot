using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class InformationDetail
    {
        [Key]
        public int Id { get; set; }

        public int? InformationId { get; set; }

        [MaxLength(20)]
        public string ItemId { get; set; }
    }
}
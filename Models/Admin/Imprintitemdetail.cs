using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class Imprintitemdetail
    {
        [Key]
        public int ID { get; set; }

        public int? printId { get; set; }

        [MaxLength(30)]
        public string itemId { get; set; }

        public int? RowNo { get; set; }

    }
}
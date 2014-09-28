using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimelyDepotMVC.Models.Admin
{
    public class packingitemdetail
    {
        [MaxLength(100)]
        public string PackId { get; set; }

        [MaxLength(50)]
        public string itemId { get; set; }

        [Key]
        public int Id { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class ITEM_PACKAGE
    {
        [MaxLength(50)]
        public string Item { get; set; }

        [MaxLength(50)]
        public string Package { get; set; }

        [Key]
        public int Id { get; set; }
    }
}
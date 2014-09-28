using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class New_Items
    {
        [Key]
        [MaxLength(255)]
        public string ITEMID { get; set; }

        [MaxLength(255)]
        public string SUB_ITEMID { get; set; }

        [MaxLength(255)]
        public string Desca { get; set; }

        [MaxLength(255)]
        public string DescB { get; set; }

        [MaxLength(255)]
        public string Colors_available { get; set; }
    }
}
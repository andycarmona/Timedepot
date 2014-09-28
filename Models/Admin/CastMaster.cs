using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class CastMaster
    {
        [Key]
        public int CastId { get; set; }

        [MaxLength(50)]
        public string CastName { get; set; }
    }
}
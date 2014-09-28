using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class Group_Master
    {
        [Key]
        public int Group_Id { get; set; }

        [MaxLength(50)]
        public string Group_Name { get; set; }
    }
}
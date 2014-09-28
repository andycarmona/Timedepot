using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimelyDepotMVC.Models.Admin
{
    public class WorkerInfluentialPersons
    {
        [Key]
        public int Id { get; set; }

        public int? categoryID { get; set; }

        public int? influentailPersonID { get; set; }
    }
}
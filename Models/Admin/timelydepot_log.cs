using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimelyDepotMVC.Models.Admin
{
    public class timelydepot_log
    {
        [Key]
        public int logID { get; set; }

        public DateTime? logTime { get; set; }

        [Display(Name="event")]
        [MaxLength(255)]
        public string eventdata { get; set; }

        public string note { get; set; }
    }
}
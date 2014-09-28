using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class EventLog
    {
        [Key]
        public int id { get; set; }

        [MaxLength(50)]
        [Display(Name = "EventLog")]
        public string EventLog1 { get; set; }

        [MaxLength(50)]
        public string IP { get; set; }

        [MaxLength(50)]
        public string User { get; set; }

        public DateTime? EventTime { get; set; }

    }
}
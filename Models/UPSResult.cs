using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimelyDepotMVC.Models
{
    public class UPSResult
    {
        public string code { get; set; }
        public string service { get; set; }
        public string cost { get; set; }
        public string time { get; set; }
    }
}
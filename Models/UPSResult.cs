using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimelyDepotMVC.Models
{
    public class UPSResult
    {
        //public string code { get; set; }
        //public string service { get; set; }
        //public string cost { get; set; }
        //public string time { get; set; }

        public string Code { get; set; }
        public string Service { get; set; }
        public string Cost { get; set; }
        public string NegociatedCost { get; set; }
        public string Time { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimelyDepotMVC.UPSWrappers
{
    public class ResultData
    {
        public string code { get; set; }
        public string service { get; set; }
        public decimal Publishedcost { get; set; }
        public decimal cost { get; set; }
        public decimal Negcost { get; set; }
        public string time { get; set; }
        public string errorMessage { get; set; }
    }
}
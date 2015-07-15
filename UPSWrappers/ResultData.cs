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
        public string Publishedcost { get; set; }
        public string cost { get; set; }
        public string Negcost { get; set; }
        public string time { get; set; }
    }
}
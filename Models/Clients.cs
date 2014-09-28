using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models
{
    public class Clients
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string Place { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimelyDepotMVC.Models.Admin
{
    public class CustomerDefaults
    {
        [Key]
        public int Id { get; set; }

        public int? CustomerId { get; set; }

        public int? SalesContactId { get; set; }

        [Display(Name = "Sales Contact")]
        public string SalesName { get; set; }

        public int? SubsidiaryId { get; set; }

        [Display(Name = "Subsidiary Name")]
        public string SubsidiaryName { get; set; }

        public int? ShiptoAddressId { get; set; }

        [Display(Name = "Ship to Name")]
        public string ShiptoName { get; set; }

        public int? NoteId { get; set; }

        [Display(Name = "Note")]
        public string NoteName { get; set; }
    }
}
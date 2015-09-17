using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TimelyDepotMVC.ModelsView
{
    public class ShipmentLogView
    {
  
        public int ShipmentId { get; set; }

        [Display(Name = "Shipment Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = false)]
        //[DisplayFormat(DataFormatString = "{0: g}", ApplyFormatInEditMode = true)]
        public DateTime? ShipmentDate { get; set; }

        [Display(Name = "InvoiceId")]
        public int? InvoiceId { get; set; }

        [Display(Name ="Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Invoice No")]
        public string InvoiceNo { get; set; }

        [Display(Name = "Ups Number")]
        public string UpsNumber { get; set; }

        public bool Shipped { get; set; }
    }
}
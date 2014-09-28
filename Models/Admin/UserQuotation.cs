using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimelyDepotMVC.Models.Admin
{
    public class UserQuotation
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        public string ProductId { get; set; }

        public int? UserId { get; set; }

        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        [MaxLength(8)]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [MaxLength(10)]
        [Display(Name = "Postal Type")]
        public string PostalType { get; set; }

        [MaxLength(12)]
        [Display(Name = "Imprint Type")]
        public string ImprintType { get; set; }

        public int? Status { get; set; }

        [Display(Name = "Invoice Status")]
        public int? Invoicestatus { get; set; }
    }
}
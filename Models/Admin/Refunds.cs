using System;

namespace TimelyDepotMVC.Models.Admin
{
    using System.ComponentModel.DataAnnotations;

    public class Refunds
    {
        [Key]
        public int RefundId { get; set; }

        public int CustomerId { get; set; }

        public double RefundAmount { get; set; }

        public DateTime Refunddate { get; set; }


    }
}
using System;

namespace TimelyDepotMVC.Models.Admin
{
    using System.ComponentModel.DataAnnotations;

    public class Refunds
    {
        [Key]
        public int RefundId { get; set; }

        public string RefundNo { get; set; }

        public int TransactionId { get; set; }

        public decimal RefundAmount { get; set; }

        public DateTime Refunddate { get; set; }

        public string SalesOrderNo { get; set; }

        public string CustomerNo { get; set; }

        public string PayLog { get; set; }

        public string ReferenceNo { get; set; }

        public string CreditCardNo { get; set; }
    }
}

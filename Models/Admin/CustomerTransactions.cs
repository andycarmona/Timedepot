namespace TimelyDepotMVC.Models.Admin
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CustomerTransactions
    {
       [Key]
        public int TransactionId { get; set; }

       public DateTime TransactionDate { get; set; }

        public string CustomerNo { get; set; }

        public string SalesorderNo { get; set; }

        public int RefundId { get; set; }

        public int PaymentId { get; set; }

        public int TransactionCode { get; set; }
    }
}
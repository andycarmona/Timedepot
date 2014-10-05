namespace TimelyDepotMVC.Models.Admin
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CustomerTransactions
    {
       [Key]
        public int TransactionId { get; set; }

        public int TransactionCode { get; set; }
    }
}
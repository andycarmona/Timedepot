namespace TimelyDepotMVC.Models.Admin
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Payments
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Payment No")]
        public string PaymentNo { get; set; }

        [Display(Name = "Customer No")]
        public string CustomerNo { get; set; }

        [Display(Name = "Sales Order No")]
        public string SalesOrderNo { get; set; }

        [Display(Name = "Payment Type")]
        public string PaymentType { get; set; }

        [Display(Name = "Credit Card Number")]
        public string CreditCardNumber { get; set; }

        [Display(Name = "Reference No")]
        public string ReferenceNo { get; set; }

        [Display(Name = "Check number")]
        public string CheckNo { get; set; }

        [Display(Name = "Amount")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? Amount { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PaymentDate { get; set; }

        [Display(Name = "Payment Log")]
        public string PayLog { get; set; }

        [Display(Name = "Invoice Payment")]
        public string InvoicePayment { get; set; }

        public int TransactionCode { get; set; }
    }
}
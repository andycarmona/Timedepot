using System;

namespace TimelyDepotMVC.ModelsView
{
    using System.ComponentModel.DataAnnotations;

    public class PaymentTransactionList
    {
        public string CustomerId { get; set; }

        [Display(Name = "Transaction No")]
        public string TransactionId { get; set; }

        public int SalesOrderId { get; set; }

        public int TransactionCode { get; set; }

        [Display(Name = "Customer No")]
        public string CustomerNo { get; set; }

        [Display(Name = "Salesorder No")]
        public string SalesOrderNo { get; set; }

        public decimal SalesAmount { get; set; }

        public string PaymentNo { get; set; }

        [Display(Name = "Payment type")]
        public string PaymentType { get; set; }

         [Display(Name = "Payment date")]
        public DateTime? PaymentDate { get; set; }

        [Display(Name = "Payment amount")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? PaymentAmount { get; set; }

        [Display(Name = "Refund amount")]
        public decimal? RefundAmount { get; set; }

        [Display(Name = "Credit card number")]
        public string CreditCardNumber { get; set; }

        [Display(Name = "Company name")]
        public string CompanyName { get; set; }

        [Display(Name = "Reference no")]
        public string ReferenceNo { get; set; }

        public decimal? BalanceDue { get; set; }

        public decimal? Amount { get; set; }

        [Display(Name = "Transaction date")]
        public DateTime? TransactionDate { get; set; }

        public string PayLog { get; set; }

        public string InvoiceNo { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public string InvoicePayment { get; set; }
    }
}
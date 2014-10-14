// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CashPayment.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TimelyDepotMVC.ModelsView
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CashPayment : IValidatableObject
    {
        public int? CustomerId { get; set; }

        public int SalesOrderId { get; set; }

        [Display(Name = "Salesorder No")]
        public string SalesOrderNo { get; set; }

        public double SalesAmount { get; set; }

        public string PaymentNo { get; set; }

        [Display(Name = "Payment type")]
        public string PaymentType { get; set; }

        [Display(Name = "Payment date")]
        public DateTime? PaymentDate { get; set; }

        public int TransactionCode { get; set; }

        [Display(Name = "Due balance")]
        public decimal BalanceDue { get; set; }
      

        public string CreditCardNumber { get; set; }

        public string ReferenceNo { get; set; }


        public double PaymentAmount { get; set; }

        public string CheckNumber { get; set; }

        public string InvoiceNo { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PaymentAmount > (double)this.BalanceDue)
            {
                yield return new ValidationResult("Payment amount should'n be bigger than the balance due");
            }

            if (PaymentAmount < 1)
            {
                yield return new ValidationResult("Please! Write an amount bigger than zero");
            }
        }


    }
}
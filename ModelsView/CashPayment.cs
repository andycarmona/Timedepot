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
    using System.Reflection.Emit;

    public class CashPayment : IValidatableObject
    {
        public int? CustomerId { get; set; }

        public int SalesOrderId { get; set; }

        [Display(Name = "Salesorder No")]
        public string SalesOrderNo { get; set; }

        public decimal SalesAmount { get; set; }

        public string PaymentNo { get; set; }

        [Display(Name = "Payment type")]
        public string PaymentType { get; set; }

        [Display(Name = "Payment date")]
        public DateTime? PaymentDate { get; set; }

        public int TransactionCode { get; set; }

        [Display(Name = "Due balance")]
        public decimal? BalanceDue { get; set; }


        public string CreditCardNumber { get; set; }

        public string ReferenceNo { get; set; }

        [Display(Name = "Payment amount")]
        [DisplayFormat(DataFormatString = "{0:0,0.0}")]
        public double PaymentAmount { get; set; }

        public string CheckNumber { get; set; }

        public int InvoiceId { get; set; }

        public string InvoiceNo { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public bool InvoicePayment { get; set; }

        public string ActualEnvironment { get; set; }

        [Display(Name = "Payment Log")]
        public string PayLog { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var balanceDue = this.BalanceDue;
            if (balanceDue != null)
            {
                var roundedBalanceDue = Math.Round((double)balanceDue,2,MidpointRounding.AwayFromZero);
                if (this.PaymentAmount > roundedBalanceDue)
                {
                    yield return new ValidationResult("Payment amount should'n be bigger than the balance due");
                }
            }

            if (PaymentAmount < 0.1)
            {
                yield return new ValidationResult("Please! Write an amount bigger than zero");
            }
        }


    }
}
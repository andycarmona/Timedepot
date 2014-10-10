// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CashPayment.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace TimelyDepotMVC.ModelsView
{
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

      
        public string CheckNumber { get; set; }

        public string CreditCardNumber { get; set; }

        public string ReferenceNo { get; set; }

        [Display(Name = "Payment amount")]
        [Required]
        public double PaymentAmount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PaymentAmount > SalesAmount)
            {
                yield return new ValidationResult("You should not pay more than the salesorder amount.");
            }
        }


    }
}
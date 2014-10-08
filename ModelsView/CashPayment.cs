using System;

namespace TimelyDepotMVC.ModelsView
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using TimelyDepotMVC.Helpers;

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

        [Display(Name = "Payment amount")]
        [SalesOrderAmountValidator(ErrorMessage = "Start date should be a future date")]
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
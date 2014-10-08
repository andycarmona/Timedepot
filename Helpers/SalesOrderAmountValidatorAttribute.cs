using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimelyDepotMVC.Helpers
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class SalesOrderAmountValidatorAttribute:ValidationAttribute,IClientValidatable
    {
        public override bool IsValid(object value)
        {
            return value != null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessage,
                ValidationType = "futuredate"
            };
        }
    }
}
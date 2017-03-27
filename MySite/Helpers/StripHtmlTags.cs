using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace MySite.Helpers
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    sealed public class StripHtmlTags : ValidationAttribute
    {
        private readonly string _tags;

        public StripHtmlTags(string tags)
            : base("{0} contains invalid character, please don't use Html markup ('<' and '>' tags)")
        {
            _tags = tags;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {

                var valueAsString = value.ToString();
                if (Regex.IsMatch(valueAsString, this._tags))
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }

            }
            return ValidationResult.Success;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(ErrorMessageString, name);
        }
    }
}
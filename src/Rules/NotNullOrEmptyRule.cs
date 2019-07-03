using System;
using System.Collections.Generic;
using System.Text;

namespace M5.Xamarin.Forms.Form.Rules
{
    /// <summary>
    /// Rule that determine if a string is null or empty
    /// </summary>
    public class NotNullOrEmptyRule : IValidationRule<string>
    {
        public NotNullOrEmptyRule(string ErrorMessage = null)
        {
            this.ErrorMessage = ErrorMessage;
        }

        /// <summary>
        /// The message that the user see if the rule was violated
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Call to see if the value violate this rule
        /// </summary>
        /// <param name="value">the value to be checked</param>
        /// <returns>true if value is validate</returns>
        public bool Validate(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            return true;
        }
    }
}

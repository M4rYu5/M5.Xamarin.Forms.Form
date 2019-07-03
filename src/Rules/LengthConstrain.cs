using System;
using System.Collections.Generic;
using System.Text;

namespace M5.Xamarin.Forms.Form.Rules
{
    /// <summary>
    /// Constrain the lenght of a string
    /// </summary>
    public class LengthConstrain : IValidationRule<string>
    {
        public LengthConstrain(string ErrorMessage = null, int min = 0, int max = int.MaxValue)
        {
            this.ErrorMessage = ErrorMessage;
            MinLenght = min;
            MaxLenght = max;

        }

        /// <summary>
        /// The value must have a length equal or greater than this
        /// </summary>
        public int MinLenght { get; set; }

        /// <summary>
        /// The value must have a length equal or less than this
        /// </summary>
        public int MaxLenght { get; set; }

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
            if (value == null)
            {
                if (MinLenght > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            if(value.Length >= MinLenght && value.Length <= MaxLenght)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace M5.Xamarin.Forms.Form.Rules
{
    /// <summary>
    /// Create rules with custom valiudation and message
    /// </summary>
    /// <typeparam name="T">Type of value that whill be validate</typeparam>
    public class DynamicRule<T> : IValidationRule<T>
    {

        private readonly Func<T, Tuple<bool, string>> rule;

        /// <summary>
        /// Create a new dynamic rule
        /// </summary>
        /// <param name="Rule">A function that take a param and return a Tuple that contain:  Item1 = value of truth, Item2 = ErrorMessage</param>
        public DynamicRule(Func<T, Tuple<bool, string>> Rule)
        {
            rule = Rule;
        }

        /// <summary>
        /// Error Message
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Validate your value
        /// </summary>
        /// <param name="value">Value that need to be checked</param>
        /// <returns></returns>
        public bool Validate(T value)
        {
            var val = rule?.Invoke(value) ?? Tuple.Create(true, "");
            ErrorMessage = val.Item2;
            return val.Item1;
        }
    }
}

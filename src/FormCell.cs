using M5.Xamarin.Forms.Form.KeyList;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace M5.Xamarin.Forms.Form
{
    /// <summary>
    /// Form Input Class. Provide basic communication between form and it's input
    /// </summary>
    public abstract class FormCell : ContentView, IKey<string>
    {
        /// <summary>
        /// Manage Cells by their keys.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Check if input is valid.
        /// </summary>
        /// <returns></returns>
        public abstract bool Check();

        /// <summary>
        /// Get cell(input) value
        /// </summary>
        /// <returns>the value of the current cell</returns>
        public abstract object GetValue();

        /// <summary>
        /// Last form that this cell was added in
        /// </summary>
        public Form ParentForm { get; set; }
    }

    /// <summary>
    /// A FormCell that keep a T type value
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public abstract class FormCell<T> : FormCell
    {
        #region Events
        /// <summary>
        /// Triggered when the input is invalid
        /// </summary>
        public event Action<string> InvalidInput;
        /// <summary>
        /// Triggered when the input is valid
        /// </summary>
        public event Action ValidInput;
        #endregion

        #region Propertys
        /// <summary>
        /// Rules that the value cannot violate
        /// </summary>
        public List<IValidationRule<T>> Rules { get; set; } = new List<IValidationRule<T>>();

        /// <summary>
        /// Value of current cell(input)
        /// </summary>
        public virtual T Value { get; set; }

        #endregion

        #region (override) Functions

        /// <summary>
        /// Check if Value violate the Rules. If the cell is not visible return true
        /// </summary>
        /// <returns></returns>
        public override bool Check()
        {
            if(IsVisible == false)
            {
                return true;
            }
            foreach (var rule in Rules)
            {
                if (!rule.Validate(Value))
                {
                    OnInvalidInput(rule.ErrorMessage);
                    return false;
                }
            }
            OnValidInput();
            return true;
        }

        /// <summary>
        /// Return the cell value (as object)
        /// </summary>
        /// <returns></returns>
        public override object GetValue()
        {
            return Value;
        }

        /// <summary>
        /// Called when Check find a rule that is violated
        /// </summary>
        /// <param name="ErrorMessage">The error message of the rule that was violated</param>
        protected virtual void OnInvalidInput(string ErrorMessage)
        {
            InvalidInput?.Invoke(ErrorMessage);
        }
        /// <summary>
        /// Called when "Check" finds that the value is valid
        /// </summary>
        protected virtual void OnValidInput()
        {
            ValidInput?.Invoke();
        }

        #endregion
    }
}

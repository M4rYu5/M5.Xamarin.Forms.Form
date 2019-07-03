namespace M5.Xamarin.Forms.Form
{
    /// <summary>
    /// Used to validate the form cell value
    /// </summary>
    /// <typeparam name="T">Cell value type</typeparam>
    public interface IValidationRule<T>
    {
        /// <summary>
        /// Error message displayed to user when the input was invalidate
        /// </summary>
        string ErrorMessage { get; set; }
        /// <summary>
        /// Validate the input with a constraint
        /// </summary>
        /// <param name="value">The value that this rule needs to check</param>
        /// <returns>true if the constraint is not violated</returns>
        bool Validate(T value);
    }
}

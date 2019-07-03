namespace M5.Xamarin.Forms.Form
{
    /// <summary>
    /// Used when an object required focus (usually by keyboard, using the "Next" commmand)
    /// </summary>
    public interface IFocusRequest
    {
        /// <summary>
        /// The action that is needed to do when an focus is required
        /// </summary>
        void OnFocusRequest();
    }
}

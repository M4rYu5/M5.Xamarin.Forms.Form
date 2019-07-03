using System;
using Xamarin.Forms;

namespace M5.Xamarin.Forms.Form
{
    /// <summary>
    /// Footer form interface. Help the Form to link the cancel and submit function with it's footer.
    /// </summary>
    public abstract class FormFooter : ContentView
    {
        /// <summary>
        /// Called when it is added to a new form. By default set the FormFooter.(Submit and Cancel) to call the Form.(Submit and Cancel)
        /// </summary>
        /// <param name="form">New Form</param>
        internal virtual void OnFormAdded(Form form)
        {
            Submit += form.Submit;
            Cancel += form.Cancel;
        }

        /// <summary>
        /// Called when it is removed from a form. By default remove the FormFooter.(Submit and Cancel) calls to Form.(Submit and Cancel)
        /// </summary>
        /// <param name="form">Old Form</param>
        internal virtual void OnFormRemoved(Form form)
        {
            Submit -= form.Submit;
            Cancel -= form.Cancel;
        }

        /// <summary>
        /// Call when a submit was required (by footer)
        /// </summary>
        public virtual event Action Submit;
        /// <summary>
        /// Call when a cancel was required (by footer)
        /// </summary>
        public virtual event Action Cancel;
    }
}

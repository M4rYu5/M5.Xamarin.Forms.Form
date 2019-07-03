using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace M5.Xamarin.Forms.Form
{
    /// <summary>
    /// Keeps the default form commands
    /// </summary>
    public static class Commands
    {
        /// <summary>
        /// Call OnFocusRequest() if parameter is IFocusRequest or(else) Focus() if parameter is VisualElement
        /// </summary>
        public static ICommand GoToNextCommand = new Command(
            (parameter) =>
            {
                if (parameter is IFocusRequest focusable)
                {
                    focusable.OnFocusRequest();
                }
                else if (parameter is VisualElement element)
                {
                    element.Focus();
                }
            },
            (parameter) =>
            {
                if (parameter != null && (parameter is IFocusRequest || parameter is VisualElement))
                    return true;
                return false;
            });
        /// <summary>
        /// Call Submit() if the parameter is a Form
        /// </summary>
        public static ICommand SubmitFormCommand = new Command(
            (parameter) =>
            {
                if (parameter is Form form)
                {
                    form.Submit();
                }
            },
            (parameter) =>
            {
                if (parameter != null && parameter is Form)
                    return true;
                return false;
            });
    }
}

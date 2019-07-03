using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace M5.Xamarin.Forms.Form.BasicViews
{
    /// <summary>
    /// Form footer with submit and cancel action.
    /// </summary>
    public class BasicFooter : FormFooter
    {
        protected Grid GridLayout = new Grid();

        /// <summary>
        /// Submit event
        /// </summary>
        public event Action Submit;
        /// <summary>
        /// Cancel event
        /// </summary>
        public event Action Cancel;

        public BasicFooter()
        {
            GridLayout.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            GridLayout.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            GridLayout.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50, GridUnitType.Absolute) });

            CancelButton = new Button() { Text = "Cancel", BackgroundColor = Color.FromHex("#cbd0df"), CornerRadius = 5, FontAttributes = FontAttributes.Bold };
            SubmitButton = new Button() { Text = "Submit", BackgroundColor = Color.FromHex("#1a86c6"), CornerRadius = 5, FontAttributes = FontAttributes.Bold };

            GridLayout.Children.Add(CancelButton, 0, 0);
            GridLayout.Children.Add(SubmitButton, 1, 0);

            Content = GridLayout;
        }

        #region private Cancel & Submit
        private Button cancel;
        private Button submit;

        private void cancelAction(object obj, EventArgs arg) => Cancel?.Invoke();
        private void submitAction(object obj, EventArgs arg) => Submit?.Invoke();
        #endregion

        /// <summary>
        /// Expose the cancel button
        /// </summary>
        public Button CancelButton
        {
            get { return cancel; }
            set
            {
                if (cancel != null)
                {
                    cancel.Clicked -= cancelAction;
                }
                cancel = value;
                cancel.Clicked += cancelAction;
            }
        }

        /// <summary>
        /// Get or set the cancel button text
        /// </summary>
        public string CancelButtonText
        {
            get { return CancelButton.Text; }
            set { CancelButton.Text = value; }
        }
        /// <summary>
        /// Expose the submit button
        /// </summary>
        public Button SubmitButton
        {
            get { return submit; }
            set
            {
                if (submit != null)
                {
                    submit.Clicked -= submitAction;
                }
                submit = value;
                submit.Clicked += submitAction;
            }
        }

        /// <summary>
        /// Get or set the submit button text
        /// </summary>
        public string SubmitButtonText
        {
            get { return SubmitButton.Text; }
            set { SubmitButton.Text = value; }
        }

    }
}

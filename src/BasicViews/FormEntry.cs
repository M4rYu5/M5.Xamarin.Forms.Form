using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace M5.Xamarin.Forms.Form.BasicViews
{
    /// <summary>
    /// Entry cell used by form.
    /// </summary>
    public class FormEntry : FormCell<string>, IFocusRequest
    {
        public bool KeepEntryCommandsWhenReplace { get; set; } = false;

        //Content Containers
        public StackLayout StackContainer { get; private set; }
        public Label TitleContainer { get; protected set; }
        private Entry _entry;
        public Entry EntryContainer
        {
            get { return _entry; }
            set
            {
                if(_entry != null && value != null && KeepEntryCommandsWhenReplace)
                {
                    value.Keyboard = _entry.Keyboard;
                    value.ReturnType = _entry.ReturnType;
                    value.ReturnCommandParameter = _entry.ReturnCommandParameter;
                    value.ReturnCommand = _entry.ReturnCommand;
                }

                if (_entry != null)
                {
                    StackContainer.Children.Remove(_entry);
                }
                _entry = value;
                if (value == null)
                {
                    return;
                }
                StackContainer.Children.Insert(1, _entry);
            }
        }
        public Label ErrorContainer { get; protected set; }

        /// <summary>
        /// Call when the entry need Focus
        /// </summary>
        public virtual void OnFocusRequest()
        {
            EntryContainer.Focus();
        }

        //EXPOSE ENTRY COMMANDS
        /// <summary>
        /// The keyboard return type. If it is set to "Next" or "Send" then the ReturnCommand whill be set to Commands.GoToNextCommand or Commands.SubmitFormCommand, respectivley 
        /// </summary>
        public ReturnType ReturnType
        {
            get { return EntryContainer.ReturnType; }
            set
            {
                EntryContainer.ReturnType = value;
                if (value == ReturnType.Next && ReturnCommand == null)
                {
                    ReturnCommand = Commands.GoToNextCommand;
                }
                else if (value == ReturnType.Send)
                {
                    ReturnCommand = Commands.SubmitFormCommand;
                }
            }
        }
        /// <summary>
        /// Command parameter (used for keyboard return type)
        /// </summary>
        public object ReturnCommandParameter
        {
            get { return EntryContainer.ReturnCommandParameter; }
            set { EntryContainer.ReturnCommandParameter = value; }
        }
        /// <summary>
        /// Set the return command. 
        /// </summary>
        public ICommand ReturnCommand
        {
            get { return EntryContainer.ReturnCommand; }
            set { EntryContainer.ReturnCommand = value; }
        }

        /// <summary>
        /// Set the Keyboard type
        /// </summary>
        public Keyboard Keyboard
        {
            get { return EntryContainer.Keyboard; }
            set { EntryContainer.Keyboard = value; }
        }


        public FormEntry()
        {
            _entry = new Entry() { PlaceholderColor = Color.Gray };
            StackContainer = new StackLayout() { Spacing = 0, Padding = 0, Margin = 0 };
            TitleContainer = new Label() { FontSize = 16d, TextColor = Color.Black};
            ErrorContainer = new Label() { TranslationY = -5, Margin = new Thickness(5, 0, 0, 0), FontSize = 12d, TextColor = Color.Red, IsVisible = false};

            StackContainer.Children.Add(TitleContainer);
            StackContainer.Children.Add(EntryContainer);
            StackContainer.Children.Add(ErrorContainer);
            Content = StackContainer;

            EntryContainer.TextChanged += (o, e) =>
            {
                Check();
            };
        }


        /// <summary>
        /// The title of this CellEntry (it is shown above the entry)
        /// </summary>
        public string Title
        {
            get { return TitleContainer.Text; }
            set { TitleContainer.Text = value; }
        }

        /// <summary>
        /// The Entry Placeholder
        /// </summary>
        public string Placeholder
        {
            get { return EntryContainer.Placeholder; }
            set { EntryContainer.Placeholder = value; }
        }

        /// <summary>
        /// The Entry Placeholder Color
        /// </summary>
        public Color PlaceholderColor
        {
            get { return EntryContainer.PlaceholderColor; }
            set { EntryContainer.PlaceholderColor = value; }
        }

        /// <summary>
        /// Set the value of this FormCell
        /// </summary>
        public override string Value { get => EntryContainer.Text; set => EntryContainer.Text = value; }

        /// <summary>
        /// Manage the OnInvalidInput. (show the error to user below entry)
        /// </summary>
        /// <param name="ErrorMessage"></param>
        protected override void OnInvalidInput(string ErrorMessage)
        {
            base.OnInvalidInput(ErrorMessage);
            ErrorContainer.Text = ErrorMessage;
            ErrorContainer.IsVisible = true;
            ParentForm?.ScrollToItem(this, ScrollToPosition.MakeVisible, true);
        }

        /// <summary>
        /// Manage the OnValidInput. (remove the error)
        /// </summary>
        protected override void OnValidInput()
        {
            base.OnValidInput();
            ErrorContainer.Text = "";
            ErrorContainer.IsVisible = false;
        }
    }
}

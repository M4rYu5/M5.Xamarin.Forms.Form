using M5.Xamarin.Forms.Form.KeyList;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace M5.Xamarin.Forms.Form
{
    [ContentProperty(nameof(FormChildren))]
    public class Form : StackLayout
    {
        //CONTENT CONTAINER REGION
        public ContentView HeaderContainer { get; protected set; }
        public ScrollView FormBodyScrollContainer { get; protected set; }
        public StackLayout FormBodyContainer { get; protected set; }
        public ContentView FooterContainer { get; protected set; }

        /// <summary>
        /// The form require a cancel action
        /// </summary>
        public event Action Canceling;
        /// <summary>
        /// Handle the submit action. (at this point all the input are valid)
        /// </summary>
        public Func<FormKeyList<string, View, FormCell>, bool> SubmitAction { get; set; }
        /// <summary>
        /// One or more rules (of the cells) are violated
        /// </summary>
        public event Action InvalidSubmitValues;
        /// <summary>
        /// The submit have just been started (but the checks (or submit actions) have not been called.)
        /// </summary>
        public event Action SubmitStarting;
        /// <summary>
        /// The submit process is done. If it was succeeded then the parameter will be true.
        /// </summary>
        public event Action<bool> SubmitEnded;


        public Form()
        {
            //Initialize Content Containers
            HeaderContainer = new ContentView() { Padding = 0, Margin = 0 };
            FormBodyScrollContainer = new ScrollView() { Padding = 0, Margin = 0 };
            FormBodyContainer = new StackLayout() { Padding = 0, Margin = 0 };
            FooterContainer = new ContentView() { Padding = 0, Margin = 0 };

            VerticalOptions = LayoutOptions.Start;

            //cannot press the element behind this view
            InputTransparent = true;
            CascadeInputTransparent = false;

            //Add header
            this.Children.Add(HeaderContainer);

            //Add body (_useScroll == true)
            this.FormChildren = new FormKeyList<string, View, FormCell>(FormBodyContainer.Children);
            FormBodyContainer.ChildAdded += FormChildAdded;
            FormBodyScrollContainer.Content = FormBodyContainer;
            this.Children.Add(FormBodyScrollContainer);

            //Add footer
            Footer = new BasicViews.BasicFooter();
            FooterContainer.Content = Footer;
            this.Children.Add(FooterContainer);
        }

        private void FormChildAdded(object sender, ElementEventArgs e)
        {
            if (e.Element is FormCell cell)
            {
                cell.ParentForm = this;
            }
        }

        /// <summary>
        /// Return the FormCell that has the given key
        /// </summary>
        /// <param name="key">Key of the FormCell</param>
        /// <returns>FormCell that has the Key</returns>
        public FormCell this[string key] => FormChildren[key];

        /// <summary>
        /// Scroll to first cell of the form that violate its rules
        /// </summary>
        private void ScrollToFirstInvalidValue()
        {
            if (!UseScrollView)
            {
                return;
            }

            foreach (var item in FormChildren.DictionaryElements)
            {
                if (!item.Check())
                {
                    FormBodyScrollContainer.ScrollToAsync(item, ScrollToPosition.Start, true);
                    break;
                }
            }
        }

        public Task ScrollToItem(View item, ScrollToPosition position, bool animated)
        {
            return FormBodyScrollContainer?.ScrollToAsync(item, position, animated);
        }


        #region Propertys
        private bool _useScroll = true;
        /// <summary>
        /// If it is set to false, then the content will not be scrollable. (default: true)
        /// </summary>
        public bool UseScrollView
        {
            get
            {
                return _useScroll;
            }
            set
            {
                if (value != _useScroll)
                {
                    _useScroll = value;
                    if (_useScroll)
                    {
                        base.Children.Remove(FormBodyContainer);
                        FormBodyScrollContainer.Content = FormBodyContainer;
                        base.Children.Insert(1, FormBodyScrollContainer);
                        InvalidateLayout();
                    }
                    else
                    {
                        base.Children.Remove(FormBodyScrollContainer);
                        FormBodyScrollContainer.Content = null;
                        base.Children.Insert(1, FormBodyContainer);
                        InvalidateLayout();
                    }
                }
            }
        }

        /// <summary>
        /// The header of this form
        /// </summary>
        public View Header
        {
            get { return HeaderContainer.Content; }
            set { HeaderContainer.Content = value; }
        }

        /// <summary>
        /// The content of the form (views, including cells)
        /// </summary>
        public FormKeyList<string, View, FormCell> FormChildren { get; set; }

        private FormFooter _footer;
        /// <summary>
        /// The footer of the form
        /// </summary>
        public FormFooter Footer
        {
            get
            {
                return _footer;
            }
            set
            {
                _footer?.OnFormRemoved(this);
                _footer = value;
                FooterContainer.Content = Footer;
                _footer?.OnFormAdded(this);
            }
        }

        #endregion


        #region (virtual) Functions

        /// <summary>
        /// Call submit. (it might fail)
        /// </summary>
        /// <returns></returns>
        public void Submit()
        {
            OnSubmit();
        }

        /// <summary>
        /// Manage the submit process. Don't override unless you need to change the entire submit process.
        /// Call: OnSubmitStarted; CheckAll; OnSubmitrequested; OnSubmitEnded; OnInvalidSubmitValues
        /// </summary>
        protected virtual void OnSubmit()
        {
            OnSubmitStarted();
            if (CheckAll())
            {
                if (OnSubmitRequest(FormChildren))
                {
                    OnSubmitEnded(true);
                }
                else
                {
                    OnSubmitEnded(false);
                }
            }
            else
            {
                OnInvalidSubmitValues();
                OnSubmitEnded(false);
            }
        }

        /// <summary>
        /// The submit have just been started (but the checks (or submit actions) have not been called.).
        /// Call: SubmitStarting (event)
        /// </summary>
        protected virtual void OnSubmitStarted()
        {
            SubmitStarting?.Invoke();
        }

        /// <summary>
        /// The submit process is done. If it was succeeded then the parameter will be true.
        /// Call: SubmitEnded (event)
        /// </summary>
        /// <param name="Successfully">true if succeed</param>
        protected virtual void OnSubmitEnded(bool Successfully)
        {
            SubmitEnded?.Invoke(Successfully);
        }

        /// <summary>
        /// Handle the submit action. (at this point all the input are valid)
        /// Call: SubmitAction (event)
        /// </summary>
        /// <param name="views">All the chilldren</param>
        /// <returns>true if succeed</returns>
        protected virtual bool OnSubmitRequest(FormKeyList<string, View, FormCell> views)
        {
            //return true if submitrequest is null
            return SubmitAction?.Invoke(FormChildren) ?? true;
        }

        /// <summary>
        /// One of the form cells violated its rules.
        /// Call: InvalidSubmitValues (event)
        /// </summary>
        protected virtual void OnInvalidSubmitValues()
        {
            InvalidSubmitValues?.Invoke();
            ScrollToFirstInvalidValue();
        }

        /// <summary>
        /// Cancel the actual form. OnCancelRequested whill be called.
        /// </summary>
        public void Cancel()
        {
            OnCancel();
        }

        /// <summary>
        /// Run when the Cancel method is called.
        /// Call: Canceling (event)
        /// </summary>
        protected virtual void OnCancel()
        {
            Canceling?.Invoke();
        }

        /// <summary>
        /// Check if the form have any value that violate its rules
        /// </summary>
        /// <returns></returns>
        public bool CheckAll()
        {
            bool validAll = true;
            //have to check evry one (to be able to display their message)
            foreach (var child in FormChildren.DictionaryElements)
            {
                bool temp = child.Check();
                if (validAll && !temp)
                {
                    validAll = temp;
                }
            }
            return validAll;
        }

        #endregion
    }


}
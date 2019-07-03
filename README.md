# M5.Xamarin.Forms.Form
A quick way to create forms in Xamarin.Forms. (Based on [this](https://devblogs.microsoft.com/xamarin/validation-xamarin-forms-enterprise-apps/) post about how to validate an entry.)
# Usage:
## Create a form (XAML)
  ```xaml
   xmlns:form="clr-namespace:M5.Xamarin.Forms.Form;assembly=M5.Xamarin.Forms.Form"
   xmlns:cell="clr-namespace:M5.Xamarin.Forms.Form.BasicViews;assembly=M5.Xamarin.Forms.Form"
   ........
    <form:Form x:Name="_Form">
        <form:Form.Header>
                <Label x:Name="_Title"
                       HorizontalTextAlignment="Center"
                       HorizontalOptions="Fill"
                       FontSize="24"
                       TextColor="Black"/>
        </form:Form.Header>
        <form:Form.Footer>
            <basicviews:BasicFooter x:Name="_Footer"
                                    Padding="10, 0, 10, 10"/>
        </form:Form.Footer>
        <cell:FormEntry x:Name="_Cell1"
              Title="Cell1:"
              Key="Cell1"
              Placeholder="Type the Cell1 name"
              Keyboard="Text"
              ReturnType="Next"
              ReturnCommandParameter="{Reference _Cell2}"/>
        <!-- The return command is automatically set in the controller -->
        <Label Text="------------------------------"
               FontSize="8"/>
        <!-- You can add any view here. -->
        <cell:FormEntry x:Name="_Cell2"
                        Title="Cell2: (numeric)"
                        Key="Cell2"
                        Placeholder="Type a number"
                        Keyboard="Numeric"
                        ReturnType="Send"
                        ReturnCommandParameter="{Reference _Form}"/>
        <!-- Pressing Send on keyboard whill automatically call the Form.Submit() method -->
    </form:Form>
  ```
## Add rules (to cells)
  ```csharp
    _Cell1.Rules.Add(Rules.NotNullEmptyOrWhiteSpace("A name is required."));
    _Cell2.Rules.Add(Rules.NotNullEmptyOrWhiteSpace("Required field."));
    _Cell2.Rules.Add(Rules.IsInteger("Must be a number."));
    _Cell2.Rules.Add(Rules.IsPositiveInteger("Must be a positive number."));
  ```
    , where
  ```csharp
        // in class Rules I have:
        public static DynamicRule<string> NotNullEmptyOrWhiteSpace(string ErrorMessage)
        {
            return new DynamicRule<string>((value) =>
            {
                if (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value))
                {
                    return Tuple.Create(false, ErrorMessage);
                }
                else
                {
                    return Tuple.Create(true, ErrorMessage);
                }
            });
        }
        public static DynamicRule<string> IsInteger(string ErrorMessage)
        {
            return new DynamicRule<string>((value) => Tuple.Create(int.TryParse(value, out int val), ErrorMessage));
        }
        .......
  ```
    , and DynamicRule<T> can be found [here](https://github.com/M4rYu5/M5.Xamarin.Forms.Form/blob/master/src/Rules/DynamicRule.cs).
## Set the Form Action
  The form action must return true if it had succeeded or false otherwise.
  ```csharp
       //set the form action to a local funcion
       _Form.SubmitAction = _Form_SubmitAction;
       ........
       //the local funcion:
       protected bool _Form_SubmitAction(FormKeyList<string, View, FormCell> cells)
       {
           //the slow way is to get the cells from _Form object and convert the object value to the actual type:
               //you can get the value (as object) by cells["cellKey"] or _Form["cellKey"]
               if(int.TryParse((string)cells[_Cell2.Key].GetValue(), out int result))
               {
                   //do something when the _Cell2 is 1
               }
               return false;
           //the fast way is to get the value directly from View
               if(int.TryParse(_Cell2.Key.Value, out int result))
               {
                   //do something when the _Cell2 is 1
               }
               return false;
       }
  ```
# More:
## Creating a FormCell
  You can create a FormCell by inheite the [FormCell<T>](https://github.com/M4rYu5/M5.Xamarin.Forms.Form/blob/master/src/FormCell.cs) class. Also you can implement the [IFocusRequest](https://github.com/M4rYu5/M5.Xamarin.Forms.Form/blob/master/src/IFocusRequest.cs) interface to run a specific action when the user moves (by pressing "next" on keyboard) from other cell to this one. See the [FormEntry](https://github.com/M4rYu5/M5.Xamarin.Forms.Form/blob/master/src/BasicViews/FormEntry.cs) class for more resources.
## Form Events
  The Form class has next events:
  * InvalidSubmitValues: The one of the cells values violate their rules
  * SubmitStarting: The submit began
  * SubmitEnded(bool succeeded): The submit ended, with succeeded parameter






using System;
using System.Collections.Generic;
using System.Text;

namespace M5.Xamarin.Forms.Form.KeyList
{
    public interface IKey<T>
    {
        T Key { get; }
    }
}

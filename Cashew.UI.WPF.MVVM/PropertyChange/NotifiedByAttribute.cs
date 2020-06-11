using System;

namespace Cashew.UI.WPF.MVVM.PropertyChange
{
    public class NotifiedByAttribute : Attribute
    {
        public NotifiedByAttribute(params string[] properties)
        {
            Properties = properties;
        }

        public string[] Properties { get; }
    }
}
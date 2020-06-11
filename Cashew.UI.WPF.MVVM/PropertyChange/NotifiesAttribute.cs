using System;

namespace Cashew.UI.WPF.MVVM.PropertyChange
{
    public class NotifiesAttribute : Attribute
    {
        public NotifiesAttribute(params string[] properties)
        {
            Properties = properties;
        }

        public string[] Properties { get; }
    }
}

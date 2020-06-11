using System.ComponentModel;

namespace Cashew.UI.WPF.MVVM.PropertyChange
{
    public interface IExternalNotifyPropertyChange : INotifyPropertyChanged
    {
        void TriggerProperty(string propertyName);
    }
}
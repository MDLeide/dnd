using System.ComponentModel;

namespace MVVM.PropertyChange
{
    public interface IExternalNotifyPropertyChange : INotifyPropertyChanged
    {
        void TriggerProperty(string propertyName);
    }
}
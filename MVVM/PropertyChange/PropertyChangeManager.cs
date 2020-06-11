using System.Collections.Generic;

namespace MVVM.PropertyChange
{
    public class PropertyChangeManager
    {
        readonly IExternalNotifyPropertyChange _obj;
        readonly Dictionary<string, List<string>> _senderToListener;

        public PropertyChangeManager(IExternalNotifyPropertyChange obj, Dictionary<string, List<string>> senderToListenerDictionary)
        {
            _obj = obj;
            _senderToListener = senderToListenerDictionary;
        }

        public void OnPropertyChanged(string propertyName)
        {
            _obj.TriggerProperty(propertyName);
            foreach (var listener in _senderToListener[propertyName])
                _obj.TriggerProperty(listener);
        }
    }
}
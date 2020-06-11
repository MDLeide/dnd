using System.Collections.Generic;
using System.Linq;

namespace MVVM.PropertyChange
{
    class PropertyToListenerBuilder
    {
        readonly Dictionary<string, List<string>> _propToListeners = new Dictionary<string, List<string>>();
        
        public void RegisterProperty(string propertyName)
        {
            _propToListeners.Add(propertyName, new List<string>());
        }

        public void RegisterListener(string sender, string listener)
        {
            if (!_propToListeners[sender].Contains(listener))
                _propToListeners[sender].Add(listener);
        }

        public Dictionary<string, List<string>> GetPropertyToListeners()
        {
            var propToListener = _propToListeners.ToDictionary(p => p.Key, p => new List<string>());

            foreach (var propToName in _propToListeners)
            {
                var queue = new Queue<string>();
                foreach (var listener in propToName.Value)
                {
                    propToListener[propToName.Key].Add(listener);
                    queue.Enqueue(listener);
                }

                while (queue.Any())
                {
                    var sender = queue.Dequeue();
                    foreach (var listener in _propToListeners[sender])
                    {
                        if (!propToListener[sender].Contains(listener))
                        {
                            propToListener[sender].Add(listener);
                            queue.Enqueue(listener);
                        }
                    }
                }
            }

            return propToListener;
        }
    }
}
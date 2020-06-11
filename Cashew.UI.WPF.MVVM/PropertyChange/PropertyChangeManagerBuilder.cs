using System;
using System.Collections.Generic;
using System.Linq;

namespace Cashew.UI.WPF.MVVM.PropertyChange
{
    public static class PropertyChangeManagerBuilder
    {
        static readonly Dictionary<Type, Dictionary<string, List<string>>> TypeToSenderToListener =
            new Dictionary<Type, Dictionary<string, List<string>>>();

        public static PropertyChangeManager GetPropertyChangeManager<T>(T obj)
            where T : IExternalNotifyPropertyChange
        {
            if (!TypeToSenderToListener.ContainsKey(obj.GetType()))
                TypeToSenderToListener.Add(obj.GetType(), GetPropertyToListeners(obj.GetType()));

            return new PropertyChangeManager(obj, TypeToSenderToListener[obj.GetType()]);
        }

        static Dictionary<string, List<string>> GetPropertyToListeners(Type t)
        {
            var builder = new PropertyToListenerBuilder();
            var properties = t.GetProperties();
            var propertyNames = properties.Select(p => p.Name).ToArray();

            foreach (var name in propertyNames)
                builder.RegisterProperty(name);

            foreach (var property in properties)
            {
                var notifies = property.GetCustomAttributes(typeof(NotifiesAttribute), false).Cast<NotifiesAttribute>();
                foreach (var notifiesAttribute in notifies)
                foreach (var listenerName in notifiesAttribute.Properties)
                    builder.RegisterListener(property.Name, listenerName);

                var notifiedBy = property.GetCustomAttributes(typeof(NotifiedByAttribute), false).Cast<NotifiedByAttribute>();
                foreach (var notifiedByAttribute in notifiedBy)
                foreach (var senderName in notifiedByAttribute.Properties)
                    builder.RegisterListener(senderName, property.Name);

                var notifiesAll = property.GetCustomAttributes(typeof(NotifiesAllAttribute), false).Any();
                if (notifiesAll)
                    foreach (var listener in propertyNames)
                        builder.RegisterListener(property.Name, listener);

                var notifiedByAll = property.GetCustomAttributes(typeof(NotifiedByAllAttribute), false).Any();
                if (notifiedByAll)
                    foreach (var sender in propertyNames)
                        builder.RegisterListener(sender, property.Name);
            }

            return builder.GetPropertyToListeners();
        }
    }
}
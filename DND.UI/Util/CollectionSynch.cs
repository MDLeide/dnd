using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using DND.Domain;
using DND.UI.Components;
using DND.UI.Domain;

namespace DND.UI.Util
{
    class CollectionSynch
    {
        public static void Link<TDomainObject, TViewModel>(ObservableCollection<TViewModel> obsCollection, IList<TDomainObject> backingCollection)
            where TDomainObject : DomainObject
            where TViewModel : DomainViewModel<TDomainObject>
        {
            Link(obsCollection, backingCollection, t => t.Object);
        }

        public static void Link<T>(ObservableCollection<T> obsCollection, IList<T> backingCollection)
        {
            Link(obsCollection, backingCollection, t => t);
        }

        public static void Link<T1, T2>(ObservableCollection<T1> obsCollection, IList<T2> backingCollection, Func<T1, T2> get)
        {
            obsCollection.CollectionChanged += (s, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var item in e.NewItems)
                            backingCollection.Add(get((T1)item));
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var item in e.OldItems)
                            backingCollection.Remove(get((T1)item));
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        foreach (var item in e.NewItems)
                            backingCollection.Add(get((T1)item));
                        foreach (var item in e.OldItems)
                            backingCollection.Remove(get((T1)item));
                        break;
                    case NotifyCollectionChangedAction.Move:
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        backingCollection.Clear();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };
        }
    }
}
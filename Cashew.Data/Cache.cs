using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Cashew.Data
{
    public class Cache<TObject, TId>
        where TObject : IIdentifiable<TId>
    {
        readonly ConcurrentDictionary<TId, TObject> _cache = new ConcurrentDictionary<TId, TObject>();

        /// <summary>
        /// Gets all the objects in the cache.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TObject> GetAll()
        {
            return _cache.Select(p => p.Value);
        }

        /// <summary>
        /// Gets objects from the cache whose <see cref="DomainObject.Id"/> is equal to one of the
        /// <see cref="DomainObject.Id"/>s in <paramref name="obj"/>. Any missing objects are added
        /// to the cache. Returns a collection of items either supplied in the original collection, or
        /// found in the cache.
        /// </summary>
        /// <param name="obj">A collection of items to add to the cache.</param>
        /// <returns>A collection of items which are now all stored in the cache.</returns>
        public IEnumerable<TObject> GetOrAdd(IEnumerable<TObject> obj)
        {
            return obj.Select(GetOrAdd);
        }

        /// <summary>
        /// Removes an item from the cache if it is present, returns true if the item was removed.
        /// </summary>
        /// <param name="obj">The object to remove.</param>
        /// <returns>True if the object was removed.</returns>
        public bool Remove(TObject obj)
        {
            if (_cache.ContainsKey(obj.Id))
            {
                _cache.TryRemove(obj.Id, out var removed);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds an object to the cache, if it is not already present. Otherwise, replaces it.
        /// </summary>
        /// <param name="obj">The object to add.</param>
        /// <returns>Returns <paramref name="obj"/>.</returns>
        public TObject Add(TObject obj)
        {
            if (_cache.ContainsKey(obj.Id))
                _cache[obj.Id] = obj;
            else
                _cache.TryAdd(obj.Id, obj);

            return obj;
        }

        /// <summary>
        /// Returns an object with a <see cref="DomainObject.Id"/>Id matching <paramref name="obj"/>
        /// from the cache, or, if no match is found, adds <paramref name="obj"/> to the cache and
        /// returns <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The object to get or add.</param>
        /// <returns>The matching object in the cache, or <paramref name="obj"/>.</returns>
        public TObject GetOrAdd(TObject obj)
        {
            if (!_cache.ContainsKey(obj.Id))
                _cache.TryAdd(obj.Id, obj);

            return _cache[obj.Id];
        }

        /// <summary>
        /// Gets an object from the cache whose <see cref="DomainObject.Id"/>Id matches <paramref name="id"/>,
        /// otherwise calls <paramref name="get"/>, adds its result to the cache, then return the result.
        /// </summary>
        /// <param name="id">Id of the object to get.</param>
        /// <param name="get">A function to retrieve an object if it is not present in the cache.</param>
        /// <returns>The object in the cache, or the newly created object.</returns>
        public TObject GetOrAdd(TId id, Func<TObject> get)
        {
            if (!_cache.ContainsKey(id))
                _cache.TryAdd(id, get());
            return _cache[id];
        }

        /// <summary>
        /// Gets objects from the cache which have an <see cref="DomainObject.Id"/>Id in <paramref name="ids"/>.
        /// If there are any missing items, <paramref name="get"/> is called. Results from <paramref name="get"/>
        /// that have an <see cref="DomainObject.Id"/>Id in <paramref name="ids"/> are then added to the cache.
        /// Returns a collection of all objects with <see cref="DomainObject.Id"/>s in <paramref name="ids"/>.
        /// </summary>
        /// <param name="id">Id of the object to get.</param>
        /// <param name="get">A function to retrieve an object if it is not present in the cache.</param>
        /// <returns>The object in the cache, or the newly created object.</returns>
        public IEnumerable<TObject> GetOrAdd(IEnumerable<TId> ids, Func<IEnumerable<TObject>> get)
        {
            var elements = new List<TObject>();
            ids = ids.ToArray();
            foreach (var id in ids)
                if (_cache.ContainsKey(id))
                    elements.Add(_cache[id]);
            if (elements.Count == ids.Count())
                return elements;

            var newElements = get().ToArray();
            foreach (var element in newElements)
                if (!_cache.ContainsKey(element.Id))
                    _cache.TryAdd(element.Id, element);

            return newElements;
        }
    }
}
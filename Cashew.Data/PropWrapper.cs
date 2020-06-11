using System;

namespace Cashew.Data
{
    /// <summary>
    /// Used a lazy-loading proxy.
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    public class PropWrapper<TProperty>
    {
        readonly Func<object, TProperty> _getWithContext;
        readonly object _context;
        bool _fetched;
        TProperty _obj;

        public PropWrapper(Func<TProperty> get)
            : this(null, o => get()) { }

        public PropWrapper(object context, Func<object, TProperty> get)
        {
            _getWithContext = get;
            _context = context;
        }

        public TProperty Value
        {
            get => _fetched ? _obj : Fetch();
            set
            {
                _obj = value;
                _fetched = true;
            }
        }

        TProperty Fetch()
        {
            _obj = _getWithContext(_context);
            _fetched = true;
            return _obj;
        }
    }
}
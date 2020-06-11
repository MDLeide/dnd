using System.Collections.Generic;
using System.Windows.Input;

namespace DND.UI.Components.IO
{
    class KeyBinding
    {
        public KeyBinding(params Key[] keys)
        {
            Keys = new List<Key>(keys);
        }

        public KeyBinding() { }

        public List<Key> Keys { get; } = new List<Key>();

        public bool KeyTriggers(Key key)
        {
            return Keys.Contains(key);
        }
    }
}
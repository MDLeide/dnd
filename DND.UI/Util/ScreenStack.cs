using System;
using System.Collections.Generic;
using System.Linq;
using Cashew.UI.WPF.MVVM;
using DND.UI.Components;
using DND.UI.Screens;

namespace DND.UI.Util
{
    class ScreenStack
    {
        static readonly ScreenStack Instance = new ScreenStack();

        readonly Stack<Screen> _screens = new Stack<Screen>();
        public Screen TopScreen => _screens.Any() ? _screens.Peek() : null;
        public IEnumerable<Screen> AllScreens => _screens.AsEnumerable();
        public bool IsEmpty => TopScreen == null;

        public static event EventHandler<ScreenChangedEventArgs> TopScreenChanged;
        public static void Push(Screen screen) => Instance.PushImpl(screen);
        public static Screen Pop() => Instance.PopImpl();
        public static void Remove(Screen screen) => Instance.RemoveImpl(screen);
        public static void PushOrFloat(Screen screen) => Instance.PushOrFloatImpl(screen);
        public static void PushOrFloat<T>(T obj) where T : Screen => Instance.PushOrFloatImpl(obj);

        public static CardTableViewModel ActiveCardSpace { get; private set; }

        void PushImpl(Screen screen)
        {
            var old = TopScreen;
            _screens.Push(screen);
            screen.Deactivated += (s, e) => RemoveImpl(s as Screen);
            screen.RequestClose += (s, e) => RemoveImpl(s as Screen);
            OnTopScreenChanged(old, TopScreen);
        }

        Screen PopImpl()
        {
            var old = _screens.Pop();
            OnTopScreenChanged(old, TopScreen);
            return old;
        }

        void RemoveImpl(Screen screen)
        {
            if (screen == TopScreen)
            {
                _screens.Pop();
                OnTopScreenChanged(screen, TopScreen);
            }

            RemoveScreen(screen);
        }

        void PushOrFloatImpl(Screen screen)
        {
            if (TopScreen == screen || FloatImpl(screen))
                return;
            PushImpl(screen);
        }

        void PushOrFloatImpl<T>(T obj)
            where T : Screen
        {
            var screens = _screens.Where(p => p.GetType() == typeof(T));
            var cnt = screens.Count();
            if (cnt > 1 )
                throw new InvalidOperationException("Cannot use PushOrFloat by Type when more than one view model of the given type is in the stack.");
            if (cnt == 1)
                FloatImpl(screens.FirstOrDefault());
            else
                PushImpl(obj);
        }

        bool FloatImpl(Screen screen)
        {
            if (TopScreen == screen || !RemoveScreen(screen))
                return false;
            PushImpl(screen);
            return true;
        }

        bool RemoveScreen(Screen screen)
        {
            if (!_screens.Contains(screen))
                return false;

            var removed = new Stack<Screen>();
            Screen current;
            while ((current = _screens.Pop()) != screen)
                removed.Push(current);

            Screen r;
            while ((r = removed.Pop()) != null)
                _screens.Push(r);
            return true;
        }

        void OnTopScreenChanged(Screen oldScreen, Screen newScreen)
        {
            if (newScreen is CardTableViewModel vm)
                ActiveCardSpace = vm;
            else
                ActiveCardSpace = null;

            TopScreenChanged?.Invoke(this, new ScreenChangedEventArgs(oldScreen, newScreen));
        }
    }
}
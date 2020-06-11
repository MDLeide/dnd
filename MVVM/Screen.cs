using System;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using MVVM.PropertyChange;
using Action = System.Action;

namespace MVVM
{
    public abstract class Screen : Caliburn.Micro.Screen, IExternalNotifyPropertyChange
    {
        readonly PropertyChangeManager _propChangeManager;

        protected Screen(bool load = true, object loadContext = null)
        {
            _propChangeManager = PropertyChangeManagerBuilder.GetPropertyChangeManager(this);
            if (load)
                Load(loadContext);
        }

        public override void NotifyOfPropertyChange(string propertyName = null)
        {
            _propChangeManager.OnPropertyChanged(propertyName);
        }

        void IExternalNotifyPropertyChange.TriggerProperty(string propertyName)
        {
            base.NotifyOfPropertyChange(propertyName);
        }

        bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (Equals(value, _isLoading)) return;
                _isLoading = value;
                NotifyOfPropertyChange(nameof(IsLoading));
            }
        }

        bool _isLoaded;
        public bool IsLoaded
        {
            get => _isLoaded;
            set
            {
                if (Equals(value, _isLoaded)) return;
                _isLoaded = value;
                NotifyOfPropertyChange(nameof(IsLoaded));
            }
        }

        public EventHandler RequestClose;

        public virtual void Close()
        {
            RequestClose?.Invoke(this, new EventArgs());
        }

        public void Load(object context = null)
        {
            if (IsLoaded)
                return;

            IsLoading = true;
            Task.Run(() =>
            {
                var loadContext = OnLoad(context);
                RunOnUiThread(() =>
                {
                    PostLoad(context, loadContext);
                    IsLoaded = true;
                    IsLoading = false;
                });
            });
        }

        protected bool ShowDialog(Screen screen)
        {
            var wm = new WindowManager();
            return wm.ShowDialog(screen) ?? false;
        }

        protected void RunOnUiThread(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }

        /// <summary>
        /// Perform loading operations and return a context object.
        /// </summary>
        /// <returns></returns>
        protected virtual object OnLoad(object context) => null;

        /// <summary>
        /// Perform post-load operations on the UI thread.
        /// </summary>
        /// <param name="context">Context object provided in constructor.</param>
        /// <param name="loadContext">A context object returned from OnLoad.</param>
        protected virtual void PostLoad(object context, object loadContext) { }
    }
}
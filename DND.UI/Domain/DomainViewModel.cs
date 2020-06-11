using System;
using Cashew.UI.WPF.MVVM;
using DND.DataAccess;
using DND.Domain;

namespace DND.UI.Domain
{
    public abstract class DomainViewModel<T> : Screen
        where T : DomainObject
    {
        protected DomainViewModel(T cardSpaceCard, IDataAccess<T> dataAccess)
        {
            DataAccess = dataAccess;
            Object = cardSpaceCard;
        }
        
        public int Id => Object.Id ?? -1;
        public IDataAccess<T> DataAccess { get; }
        public T Object { get; }

        #region SaveCommand

        RelayCommand _saveCommand;

        public RelayCommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(o => Save(), o => CanSave())); }
        }

        public virtual void Save()
        {
            PreSave();
            DataAccess.Save(Object);
            PostSave();
        }

        public virtual bool CanSave() => true;

        #endregion SaveCommand

        #region DeleteCommand

        RelayCommand _deleteCommand;

        public RelayCommand DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand = new RelayCommand(o => Delete(), o => CanDelete())); }
        }

        public virtual void Delete() => DataAccess.Delete(Object);

        public virtual bool CanDelete() => true;


        #endregion DeleteCommand

        #region CancelCommand

        RelayCommand _cancelCommand;

        public RelayCommand CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new RelayCommand(o => Cancel(), o => CanCancel())); }
        }
        
        public virtual void Cancel() => TryClose(false);

        public virtual bool CanCancel() => true;

        #endregion CancelCommand

        public virtual void SaveAndClose()
        {
            Save();
            TryClose(true);
        }
        
        public override object GetView(object context = null)
        {
            return null;
        }

        protected virtual void PreSave() { }

        protected virtual void PostSave() { }

        protected override void OnDeactivate(bool close)
        {
            // todo: query user if object is changed
            //if (IsChanged && !Interaction.DiscardUnsavedChangesQuery())
            //    base.OnDeactivate(false);
            //else
                base.OnDeactivate(true);
        }

        public override string ToString()
        {
            return Object == null ? $"VM - {typeof(T).Name}: null" : $"VM - {Object}";
        }
    }
}
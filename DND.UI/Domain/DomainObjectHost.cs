using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using DND.Domain;
using DND.UI.Components.IO;

namespace DND.UI.Domain
{
    interface IComponent : IHaveDisplayName, INotifyPropertyChangedEx, IHasToolbar
    {
    }

    interface IDomainComponent<out TDomain> : IComponent
        where TDomain : DomainObject
    {
        int Id { get; set; }
        bool IsTransient { get; }
        TDomain DomainObject { get; }
        ICommand Save { get; }
        ICommand Delete { get; }
    }

    interface IComponentHost<out TComponent> : IScreen
        where TComponent : IComponent
    {
        TComponent Component { get; }
    }

    interface IDomainComponentHost<out TDomain, out TDomainComponent> : IComponentHost<TDomainComponent>
        where TDomain : DomainObject
        where TDomainComponent : IDomainComponent<TDomain>
    {
        TDomain DomainObject { get; }
        IDomainComponent<TDomain> DomainComponent { get; }
    }

    interface IHasToolbar
    {
        ToolbarComponent Toolbar { get; }
        bool HasToolbar { get; }
    }

    class DomainHost<TDomain, TDomainViewModel> 
        where TDomain : DomainObject
        where TDomainViewModel : DomainViewModel<TDomain>
    {
    }
}

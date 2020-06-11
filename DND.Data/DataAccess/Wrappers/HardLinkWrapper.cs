using System;
using Cashew.Data;
using DND.DataAccess;
using DND.Domain;

namespace DND.Data.DataAccess.Wrappers
{
    class HardLinkWrapper : HardLink
    {

        public HardLinkWrapper(ICardDataAccess cardDataAccess, int originId, int targetId)
        {
            CardDataAccess = cardDataAccess;
            _origin = new PropWrapper<Card>(() => cardDataAccess.Get(originId));
            _target = new PropWrapper<Card>(() => cardDataAccess.Get(targetId));
        }

        public ICardDataAccess CardDataAccess { get; }

        readonly PropWrapper<Card> _origin;
        public override Card Origin
        {
            get => _origin.Value;
            set => _origin.Value = value;
        }

        readonly PropWrapper<Card> _target;

        public override Card Target
        {
            get => _target.Value;
            set => _target.Value = value;
        }
    }
}
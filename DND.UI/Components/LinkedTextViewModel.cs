using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cashew.UI.WPF.MVVM;
using DND.Domain;
using DND.Parser;

namespace DND.UI.Components
{
    public class LinkedTextViewModel : Screen
    {
        readonly ILiveParser _parser;
        readonly ICardLinker _linker;
        readonly object _pendingLock = new object();
        bool _pendingParse;
        

        public LinkedTextViewModel(ICardLinker linker, ILiveParser parser)
        {
            _linker = linker;
            _parser = parser;
        }

        public event EventHandler LinksUpdated;


        public Dictionary<Token, SoftLink> Links { get; } = new Dictionary<Token, SoftLink>();
        public int ParseDelayMs { get; set; } = 0;
        
        string _text;
        public string Text
        {
            get => _text;
            set => SetText(value);
        }

        public override object GetView(object context = null)
        {
            return null;
        }

        void SetText(string val)
        {
            if (Equals(val, _text)) return;

            _text = val;

            var doParse = false;
            lock (_pendingLock)
            {
                if (!_pendingParse)
                {
                    _pendingParse = true;
                    doParse = true;
                }
            }

            if (doParse)
                Parse();

            NotifyOfPropertyChange(nameof(Text));
        }
        
        void Parse()
        {
            void DoParse()
            {
                var collectionChanges = _parser.SetText(Text);

                foreach (var t in collectionChanges.Removed)
                    Links.Remove(t);

                foreach (var t in collectionChanges.PositionChanged)
                    Links[t].Position = t.Position;
                
                foreach (var t in collectionChanges.Added)
                {
                    SoftLink link = null;
                    if (!t.IsOpen)
                        link = _linker.Link(t);
                    Links.Add(t, link);
                }

                LinksUpdated?.Invoke(this, new EventArgs());

                lock (_pendingLock)
                {
                    _pendingParse = false;
                }
            }
            
            if (ParseDelayMs <= 0)
                DoParse();
            else
            {
                lock (_pendingLock)
                {
                    if (_pendingParse)
                        return;
                    _pendingParse = true;
                }

                Task.Run(() =>
                {
                    Thread.Sleep(ParseDelayMs);
                    DoParse();
                });
            }
        }
    }
}

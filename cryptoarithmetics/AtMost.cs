using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptoarithmetics
{
    internal sealed class AtMost
    {
        public readonly bool All;
        public readonly int Maximum;
        private int _current;
        public int Current => _current;
        public bool CanContinue => CanContinueInternal();

        public AtMost(bool all)
        {
            All = all;
            Maximum = -1;
            _current = 0;
        }

        public AtMost(int maximum)
        {
            All = false;
            Maximum = maximum;
            _current = 0;
        }

        public void Increase()
        {
            _current++;
        }

        private bool CanContinueInternal()
        {
            if (All)
            {
                return true;
            }
            else
            {
                return _current < Maximum;
            }
        }

        public void Reset()
        {
            _current = 0;
        }

    }
}

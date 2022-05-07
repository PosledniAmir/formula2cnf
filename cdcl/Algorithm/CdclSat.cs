using dpll.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using watched.Algorithm;

namespace cdcl.Algorithm
{
    internal class CdclSat
    {
        private readonly ClauseChecker _checker;
        private readonly LockedStack _stack;
        private int _decisions;
        private int _restarts;
        private int _learned;
        private int _resolutions;
    }
}

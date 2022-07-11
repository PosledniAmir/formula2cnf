using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Runner
{
    public interface ISatStatPrinter
    {
        public TimeSpan Elapsed { get; }
        public string ToString();
    }
}

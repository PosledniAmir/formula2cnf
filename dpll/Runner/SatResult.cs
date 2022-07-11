using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Runner
{
    public sealed class SatResult
    {
        public readonly ErrorResult Error;
        public readonly ISatStatPrinter Stats;
        public readonly ModelResult? Model;

        public SatResult(ISatStatPrinter stats, ErrorResult error, ModelResult? model)
        {
            Stats = stats;
            Error = error;
            Model = model;
        }

        public bool HadError()
        {
            return Error.ErrorOccured;
        }
    }
}

using dpll.Reader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Runner
{
    public sealed class SatRunner
    {
        private Stream _source;
        private ISatFactory _factory;
        public readonly FormulaType Type;

        public SatRunner(Stream source, FormulaType type, ISatFactory factory)
        {
            _source = source;
            Type = type;
            _factory = factory;
        }

        public SatResult Run()
        {
            var watch = Stopwatch.StartNew();

            if (!CnfStreamReader.TryParse(_source, Type, out var cnf, out var comments))
            {
                var error = new ErrorResult(true, "Formula could not be parsed.");
                var stats = new SatStats(watch.Elapsed);
                watch.Stop();
                return new SatResult(stats, error, null);
            }

            try
            {
                var sat = _factory.Create(cnf);
                var result = sat.IsSatisfiable();
                var model = sat.GetModels().FirstOrDefault();
                var stats = sat.GetStats(watch.Elapsed);
                watch.Stop();
                return new SatResult(stats, new ErrorResult(false, ""), new ModelResult(result, model, comments));
            }
            catch(Exception e)
            {
                var error = new ErrorResult(true, e.Message);
                var stats = new SatStats(watch.Elapsed);
                watch.Stop();
                return new SatResult(stats, error, null);
            }
        }
    }
}

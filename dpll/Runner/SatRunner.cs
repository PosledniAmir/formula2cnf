﻿using dpll.Reader;
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

        public SatRunner(Stream source, FormulaType type, ISatFactory _factory)
        {
            _source = source;
            Type = type;
        }

        public SatResult Run()
        {
            var watch = Stopwatch.StartNew();

            if (!CnfStreamReader.TryParse(_source, Type, out var cnf, out var comments))
            {
                var error = GetInputError();
                var stats = new ErrorStats(watch.Elapsed);
                watch.Stop();
                return new SatResult(stats, error, null);
            }

            try
            {
                var sat = _factory.Create(cnf);
                var result = sat.IsSatisfiable();
            }
            catch(Exception e)
            {
                var error = new ErrorResult(true, e.Message);
                var stats = new ErrorStats(watch.Elapsed);
                watch.Stop();
                return new SatResult(stats, error, null);
            }

        }

        private ErrorResult GetInputError()
        {
            return new ErrorResult(true, "Formula could not be parsed.");
        }
    }
}

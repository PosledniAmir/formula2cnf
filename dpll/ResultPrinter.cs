using dpll.Algorithm;
using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll
{
    internal sealed class ResultPrinter
    {
        private readonly CnfFormula _formula;
        private readonly VariableDescriptor? _variables;
        private readonly Stopwatch _watch;

        public ResultPrinter(CnfFormula formula, Stopwatch watch)
        {
            _formula = formula;
            _watch = watch;
            _variables = null;
        }

        public ResultPrinter(CnfFormula formula, VariableDescriptor variables, Stopwatch watch) : this(formula, watch)
        {
            _variables = variables;
        }

        public void Print()
        {
            var parsingTime = _watch.Elapsed;
            var sat = new DpllSat(_formula);
            var result = sat.IsSatisfiable();
            var totalTime = _watch.Elapsed;
            var builder = new StringBuilder()
            .AppendLine($"Solved formula in {totalTime.TotalMilliseconds} ms")
            .AppendLine($"Time spend parsing formula: {parsingTime.TotalMilliseconds} ms")
            .AppendLine($"Time spend solving formula: {(totalTime - parsingTime).TotalMilliseconds} ms")
            .AppendLine($"Performed decisions: {sat.Decisions}")
            .AppendLine($"Performed unit propagations: {sat.Resolutions}")
            .AppendLine($"Result: {result}");
            if (result)
            {
                builder.AppendLine("Model:").Append(GetModel(sat));
            }

            Console.WriteLine(builder.ToString());
        }

        private string GetModel(DpllSat sat)
        {
            var builder = new StringBuilder();
            var model = sat.GetModel();
            if (_variables == null)
            {
                foreach (var item in model)
                {
                    var output = item > 0 ? "true" : "false";
                    builder.AppendLine($"{Math.Abs(item)} = {output}");

                }
            }
            else
            {
                foreach (var item in model)
                {
                    var output = item > 0 ? "true" : "false";
                    if (_variables.TryTranslate(Math.Abs(item), out var name))
                    {
                        builder.AppendLine($"{name} = {output}");
                    }
                }
            }

            return builder.ToString();
        }
    }
}

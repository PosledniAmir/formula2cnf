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
    public sealed class ResultPrinter
    {
        private readonly AbstractSat _sat;
        private readonly VariableDescriptor? _variables;
        private readonly Stopwatch _watch;

        public ResultPrinter(AbstractSat sat, Stopwatch watch)
        {
            _sat = sat;
            _watch = watch;
            _variables = null;
        }

        public ResultPrinter(AbstractSat sat, VariableDescriptor variables, Stopwatch watch) : this(sat, watch)
        {
            _variables = variables;
        }

        public void Print()
        {
            var parsingTime = _watch.Elapsed;
            var result = _sat.IsSatisfiable();
            var totalTime = _watch.Elapsed;
            var builder = new StringBuilder()
            .AppendLine($"Solved formula in {totalTime.TotalMilliseconds} ms")
            .AppendLine($"Time spend parsing formula: {parsingTime.TotalMilliseconds} ms")
            .AppendLine($"Time spend solving formula: {(totalTime - parsingTime).TotalMilliseconds} ms")
            .AppendLine($"Performed decisions: {_sat.Decisions}")
            .AppendLine($"Performed unit propagations: {_sat.Resolutions}")
            .AppendLine($"Result: {result}");
            if (result)
            {
                builder.AppendLine("Model:").Append(GetModel(_sat));
            }

            Console.WriteLine(builder.ToString());
        }

        private string GetModel(AbstractSat sat)
        {
            var builder = new StringBuilder();
            var model = sat.GetModels().First();
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

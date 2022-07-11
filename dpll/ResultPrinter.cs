using dpll.Algorithm;
using dpll.Runner;
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
        public readonly int ExitCode;
        public readonly SatResult Result;

        public ResultPrinter(SatResult result)
        {
            Result = result;
            ExitCode = result.HadError() ? 1 : 0;
        }

        private string ErrorString()
        {
            return Result.Error.ToString();
        }

        private string SatString(bool printLearned)
        {
            var builder = new StringBuilder()
                .AppendLine($"Solved formula in {Result.Stats.Elapsed.TotalMilliseconds} ms")
                .AppendLine($"Performed decisions: {Result.Stats.Decisions}")
                .AppendLine($"Performed unit propagations: {Result.Stats.Resolutions}");
            if (printLearned)
            {
                builder.AppendLine($"Learned clauses: {Result.Stats.LearnedClauses}");
            }

            builder.AppendLine($"Result: {Result.Model.IsSatisfiable}");

            if (Result.Model.IsSatisfiable)
            {
                builder.AppendLine("Model:").Append(GetModel());
            }

            return builder.ToString();
        }

        public void Print(bool printLearned)
        {
            var result = Result.HadError() ? ErrorString() : SatString(printLearned);
            Console.WriteLine(result);
        }

        private string GetModel()
        {
            var builder = new StringBuilder();
            var model = Result.Model.Model;
            var variables = Result.Model.Description;
            if (variables == null)
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
                    if (variables.TryTranslate(Math.Abs(item), out var name))
                    {
                        builder.AppendLine($"{name} = {output}");
                    }
                }
            }

            return builder.ToString();
        }
    }
}

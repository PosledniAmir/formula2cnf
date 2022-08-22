using dpll.Reader;
using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace backbones
{
    internal readonly struct BackboneResult
    {
        public readonly string Filename;
        public readonly int Calls;
        public readonly IReadOnlySet<int> Backbones;

        public BackboneResult(string filename, int calls, IReadOnlySet<int> backbones)
        {
            Filename = filename;
            Calls = calls;
            Backbones = backbones;
        }
    }

    internal sealed class DirectoryRunner
    {
        public readonly string Dir;

        public DirectoryRunner(string directory)
        {
            Dir = directory;
        }

        private BackboneResult RunFile(string filepath)
        {
            var file = File.OpenRead(filepath);
            var reader = new DimacsReader(file);

            if (reader.TryRead(out var formula))
            {
                var score = JeroslowWang.Compute(formula);
                using var context = new Context();
                var builder = new FormulaBuilder(context);
                var variables = builder.GetVariables(formula);
                var formulaExpr = builder.GetFormula(formula, variables);

                var solver = new SolutionManager(context.MkSolver(), formulaExpr, variables);
                var candidates = solver.Solve();

                var order = score
                    .OrderByDescending(pair => pair.Value)
                    .Select(pair => pair.Key)
                    .ToList();

                var calls = 1;
                foreach (var test in order)
                {
                    if (candidates.Count == 0)
                    {
                        break;
                    }
                    var literal = 0;
                    if (candidates.Contains(test))
                    {
                        literal = test;
                    }
                    else if (candidates.Contains(-test))
                    {
                        literal = -test;
                    }

                    if (literal != 0)
                    {
                        var temp = solver.SolveWith(builder.Forbid(literal, variables[test]));
                        calls++;
                        if (temp.Count > 0)
                        {
                            candidates.IntersectWith(temp);
                        }
                    }
                }
                return new BackboneResult(Path.GetFileName(filepath), calls, candidates);
            }
            return new BackboneResult(Path.GetFileName(filepath), 0, new HashSet<int>());
        }

        public IEnumerable<BackboneResult> Run()
        {
            var results = Directory.GetFiles(Dir).Select(f => RunFile(f));
            return results;
        }
    }
}

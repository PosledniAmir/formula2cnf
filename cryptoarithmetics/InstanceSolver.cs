using cryptoarithmetics.Parsing;
using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptoarithmetics
{
    internal sealed class InstanceSolver : IDisposable
    {
        public readonly string Instance;
        public readonly int Base;
        public readonly bool Unique;
        private readonly Context _context;
        private ConditionGenerator? _generator;
        private Solver? _solver;

        public InstanceSolver(string instance, int k, bool unique)
        {
            Instance = instance;
            Base = k;
            Unique = unique;
            _context = new Context();
            _generator = null;
            _solver = null;
        }

        private ConditionGenerator CreateGenerator()
        {
            var builder = new FormulaBuilder(_context, Base);

            var tokens = Tokenizer
                .TokenizeInstance(Instance)
                .ToList();

            var parsed = Parser.Parse(tokens);
            return new ConditionGenerator(builder, parsed);
        }

        private Solver PepareSolver(ConditionGenerator generator)
        {
            var ranges = generator.CreateRangeConditions();
            var main = generator.CreateInstanceCondition(Unique);

            var solver = _context.MkSolver();
            solver.Assert(ranges.ToArray());
            solver.Assert(main);

            return solver;
        }

        private StringBuilder AppendModel(StringBuilder builder, Model model, ConditionGenerator generator)
        {
            var map = new Dictionary<char, char>();
            foreach (var (name, constant) in generator.Constants)
            {
                var solution = Convert.ToString(int.Parse(model.Evaluate(constant).ToString()), Base);
                map.Add(name.First(), solution.ToString().First());
                builder.AppendLine($"{name} = {solution}");
            }

            var solved = Instance.Select(c =>
            {
                if (map.TryGetValue(c, out var r))
                {
                    return r;
                }
                else
                {
                    return c;
                }
            }).Aggregate(new StringBuilder(), (builder, c) => builder.Append(c))
            .ToString();

            return builder.AppendLine(solved);
        }

        private Tuple<int, string> SolveInternal()
        {
            _generator ??= CreateGenerator();
            _solver ??= PepareSolver(_generator);

            var status = _solver.Check();
            var builder = new StringBuilder().AppendLine($"SAT status: {status}");

            if (status == Status.SATISFIABLE)
            {
                var model = _solver.Model;
                builder = AppendModel(builder, model, _generator);
                return Tuple.Create(0, builder.ToString());
            }
            else if (status == Status.UNSATISFIABLE)
            {
                return Tuple.Create(0, builder.ToString());
            }
            else
            {
                return Tuple.Create(1, builder.AppendLine(_solver.ReasonUnknown).ToString());
            }
        }

        public Tuple<int, string> Solve()
        {
            try
            {
                return SolveInternal();
            }
            catch (Exception ex)
            {
                return Tuple.Create(1, $"Error: {ex}");
            }
        }

        public void Dispose()
        {
            ((IDisposable)_context).Dispose();
        }
    }
}

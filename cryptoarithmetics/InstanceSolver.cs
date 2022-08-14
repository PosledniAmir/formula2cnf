using cryptoarithmetics.Parsing;
using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
        private bool _canContinue;
        private readonly List<BoolExpr> _forbidden;
        public bool CanContinue => _canContinue;

        public InstanceSolver(string instance, int k, bool unique)
        {
            Instance = instance;
            Base = k;
            Unique = unique;
            _canContinue = true;
            _context = new Context();
            _forbidden = new List<BoolExpr>();
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
            var ranges = generator.ConditionRange();
            var main = generator.GetInstance(Unique);

            var solver = _context.MkSolver();
            solver.Assert(ranges.ToArray());
            foreach (var nope in _forbidden)
            {
                solver.Assert(nope);
            }
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

        private static IEnumerable<Tuple<IntExpr, int>> ForbidModel(Model model, ConditionGenerator generator)
        {
            foreach (var (name, constant) in generator.Constants)
            {
                var solution = int.Parse(model.Evaluate(constant).ToString());
                yield return Tuple.Create(constant, solution);
            }
        }

        private Tuple<int, string> SolveInternal()
        {
            _canContinue = false;
            var generator = CreateGenerator();
            var solver = PepareSolver(generator);

            var status = solver.Check();
            var builder = new StringBuilder().AppendLine($"SAT status: {status}");

            if (status == Status.SATISFIABLE)
            {
                var model = solver.Model;
                builder = AppendModel(builder, model, generator);

                _canContinue = true;
                _forbidden.Add(generator.ForbidSolution(ForbidModel(model, generator)));

                return Tuple.Create(0, builder.ToString());
            }
            else if (status == Status.UNSATISFIABLE)
            {
                return Tuple.Create(0, builder.ToString());
            }
            else
            {
                return Tuple.Create(1, builder.AppendLine(solver.ReasonUnknown).ToString());
            }
        }

        public Tuple<int, string> Solve()
        {
            return SolveInternal();
            //try
            //{
            //    return SolveInternal();
            //}
            //catch (Exception ex)
            //{
            //    return Tuple.Create(1, $"Error: {ex}");
            //}
        }

        public void Dispose()
        {
            ((IDisposable)_context).Dispose();
        }
    }
}

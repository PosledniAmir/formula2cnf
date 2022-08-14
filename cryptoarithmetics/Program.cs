using cryptoarithmetics;
using cryptoarithmetics.Parsing;
using Microsoft.Z3;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("cryptoarithmetics.test")]

var instance = "(SEND + MORE = MONEY) || (SQUARE - DANCE = DANCER)";
using var context = new Context();
var builder = new FormulaBuilder(context, 10);

var tokens = Tokenizer
    .TokenizeInstance(instance)
    .ToList();

var parsed = Parser.Parse(tokens);
var generator = new ConditionGenerator(builder, parsed);

var ranges = generator.CreateRangeConditions();
var main = generator.CreateInstanceCondition(true);
var solver = context.MkSolver();
solver.Assert(ranges.ToArray());
solver.Assert(main);
var status = solver.Check();
Console.WriteLine($"SAT status: {status}");
if (status == Status.SATISFIABLE)
{
    var map = new Dictionary<char, char>();
    var model = solver.Model;
    foreach (var (name, constant) in generator.Constants)
    {
        var solution = model.Evaluate(constant);
        map.Add(name.First(), solution.ToString().First());
        Console.WriteLine($"{name} = {solution}");
    }

    var solved = instance.Select(c =>
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

    Console.WriteLine(solved);

    return 0;
}
else if (status == Status.UNSATISFIABLE)
{
    return 0;
}
else
{
    return 1;
}
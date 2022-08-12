using cryptoarithmetics;
using cryptoarithmetics.Parsing;
using Microsoft.Z3;

using var context = new Context();
var builder = new FormulaBuilder(context, 10);

var tokens = Tokenizer
    .ParseInstance("(SEND + MORE = MONEY) || (SQUARE - DANCE = DANCER)")
    .ToList();

var parsed = Parser.Parse(tokens);
var generator = new ConditionGenerator(builder, parsed);

var uniqueness = generator.CreateUniqueCondition();
var ranges = generator.CreateRangeConditions();
var main = generator.CreateInstanceCondition();
var solver = context.MkSolver();
solver.Assert(uniqueness);
solver.Assert(ranges.ToArray());
solver.Assert(main);
var status = solver.Check();
Console.WriteLine($"SAT status: {status}");
if (status == Status.SATISFIABLE)
{
    var model = solver.Model;
    foreach (var (name, constant) in generator.Constants)
    {
        var solution = model.Evaluate(constant);
        Console.WriteLine($"{name} = {solution}");
    }
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
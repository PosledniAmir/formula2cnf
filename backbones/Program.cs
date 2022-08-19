// See https://aka.ms/new-console-template for more information
using backbones;
using dpll.Reader;
using Microsoft.Z3;

var path = Path.Combine("..\\..\\", "CBS_k3_n100_m403_b10_0.cnf");
var file = File.OpenRead(path);
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

    Console.WriteLine($"Solved with {calls} SAT calls.");
    Console.WriteLine($"Number of backbones {candidates.Count}");
    var prependix = "";
    foreach (var result in candidates)
    {
        Console.Write(prependix);
        Console.Write($"{result}");
        prependix = " ";
    }
}
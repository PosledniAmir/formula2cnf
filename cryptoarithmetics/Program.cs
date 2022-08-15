using cryptoarithmetics;
using cryptoarithmetics.Parsing;
using Microsoft.Z3;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("cryptoarithmetics.test")]

const string Help = @"Arguments could not be parsed.
Cryptoarithmetics usage: cryptoarithmetics [input] [--unique | -u] -k <base> -c <computeSize> -p <printSize>
[input] = text file with cryptoarithmetics instance
-u, -unique = specifies if solution should have unique digit per letter per satisfying clauses
<base> = base of the solution
<computeSize> = number of solutions computed
<printSize> = number of solutions printed";

var watch = new Stopwatch();
watch.Start();
var help = false;
var nextBase = false;
var nextCompute = false;
var nextPrint = false;
var computeSolutions = new AtMost(100);
var printSolutions = new AtMost(3);
var unique = true;
var k = 10;
var instance = "(SEND + MORE = MONEY) && (SQUARE - DANCE = DANCER)";

foreach (var arg in args)
{
    var lower = arg.ToLower();
    if (nextBase)
    {
        nextBase = false;
        if (!int.TryParse(arg, out k))
        {
            help = true;
            break;
        }
        continue;
    }
    else if (nextCompute)
    {
        nextCompute = false;
        if (lower == "all")
        {
            computeSolutions = new AtMost(true);
        }
        else if (int.TryParse(arg, out var computeInt))
        {
            computeSolutions = new AtMost(computeInt);
        }
        else
        {
            help = true;
            break;
        }
        
        continue;
    }
    else if (nextPrint)
    {
        nextCompute = false;
        if (lower == "all")
        {
            printSolutions = new AtMost(true);
        }
        else if (int.TryParse(arg, out var computeInt))
        {
            printSolutions = new AtMost(computeInt);
        }
        else
        {
            help = true;
            break;
        }

        continue;
    }
    else if  (lower == "--unique")
    {
        unique = true;
    }
    else if (lower == "-s")
    {
        unique = true;
    }
    else if (File.Exists(arg))
    {
        instance = File.ReadAllText(arg);
    }
    else if (lower == "-k")
    {
        nextBase = true;
        continue;
    }
    else if (lower == "-c")
    {
        nextCompute = true;
        continue;
    }
    else if (lower == "-p")
    {
        nextPrint = true;
        continue;
    }
    else
    {
        help = true;
    }
}

if (help)
{
    Console.WriteLine(Help);
    return 1;
}

using var solver = new InstanceSolver(instance, k, unique);
var returnValue = 0;

while (solver.CanContinue && printSolutions.CanContinue)
{
    var result = solver.Solve();
    computeSolutions.Increase();
    printSolutions.Increase();
    returnValue = result.Item1;
    Console.Write(result.Item2);
}

while (solver.CanContinue && computeSolutions.CanContinue)
{
    solver.Solve();
    computeSolutions.Increase();
    printSolutions.Increase();
}

Console.WriteLine($"Total found solutions: {solver.Solutions}");
Console.WriteLine($"Unique: {unique}; Base: {k}");
Console.Write($"Solved in: {watch.ElapsedMilliseconds} ms");

return returnValue;
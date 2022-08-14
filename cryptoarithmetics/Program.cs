using cryptoarithmetics;
using cryptoarithmetics.Parsing;
using Microsoft.Z3;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("cryptoarithmetics.test")]

var computeSolutions = new AtMost(1_000);
var printSolutions = new AtMost(3);
var instance = "(SEND + MORE = MONEY) || (SQUARE - DANCE = DANCER)";
using var solver = new InstanceSolver(instance, 3, false);
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

Console.Write($"Total found solutions: {solver.Solutions}");

return returnValue;
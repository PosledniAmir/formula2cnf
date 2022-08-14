using cryptoarithmetics;
using cryptoarithmetics.Parsing;
using Microsoft.Z3;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("cryptoarithmetics.test")]

var instance = "(SEND + MORE = MONEY) || (SQUARE - DANCE = DANCER)";
using var solver = new InstanceSolver(instance, 16, true);
var returnValue = 0;
while (solver.CanContinue)
{
    var result = solver.Solve();
    returnValue = result.Item1;
    Console.Write(result.Item2);
}

return returnValue;
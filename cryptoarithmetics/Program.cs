using cryptoarithmetics;
using cryptoarithmetics.Parsing;
using Microsoft.Z3;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("cryptoarithmetics.test")]

var instance = "(SEND + MORE = MONEY) || (SQUARE - DANCE = DANCER)";
using var solver = new InstanceSolver(instance);
var result = solver.Solve();
Console.Write(result.Item2);
return result.Item1;
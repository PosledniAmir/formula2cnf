// See https://aka.ms/new-console-template for more information
using backbones;
using dpll.Reader;
using Microsoft.Z3;
using System.Diagnostics;
using System.Text;

const string Help = @"Arguments could not be parsed.
Cryptoarithmetics usage: backbones [input]
[input] = path directory with CNF formulae encoded in DIMACS";

var watch = Stopwatch.StartNew();
var path = ".";
var help = false;

if (args.Length != 1 && args.Length != 0)
{
    help = true;
}

foreach (var arg in args)
{
    if (Directory.Exists(arg))
    {
        path = arg;
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

var runner = new DirectoryRunner(path);
var results = runner.Run().ToList();

Console.WriteLine($"Solved in {watch.ElapsedMilliseconds} ms.");
foreach (var result in results)
{
    var builder = new StringBuilder();
    foreach (var backbone in result.Backbones)
    {
        builder.Append($"{backbone} ");
    }
    Console.WriteLine($"| {result.Filename} | {result.Calls} | {result.Backbones.Count} | {builder}|");
}

return 0;
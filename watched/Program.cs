using dpll;
using dpll.Algorithm;
using dpll.Reader;
using formula2cnf;
using formula2cnf.Formulas;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using watched.Algorithm;

[assembly: InternalsVisibleTo("dpll.test")]

var watch = new Stopwatch();
watch.Start();
var formula = FormulaType.Error;
var file = "";
var help = false;

foreach (var arg in args)
{
    var lower = arg.ToLower();
    if (lower == "--sat")
    {
        formula = FormulaType.Smt;
    }
    else if (lower == "-s")
    {
        formula = FormulaType.Smt;
    }
    else if (lower == "--cnf")
    {
        formula = FormulaType.Dimacs;
    }
    else if (lower == "-c")
    {
        formula = FormulaType.Dimacs;
    }
    else if (File.Exists(arg))
    {
        file = arg;
    }
    else
    {
        help = true;
    }
}

var input = Console.OpenStandardInput();

if (file != "")
{
    input = File.Open(file, FileMode.Open, FileAccess.Read);
    if (formula == FormulaType.Error)
    {
        formula = CnfFileReader.DetermineType(file);
    }
}

if (help || formula == FormulaType.Error)
{
    Console.WriteLine("Arguments could not be parsed.");
    Console.WriteLine("watched usage: formula2cnf [input] [--sat | -s | --cnf | -c]");
    Console.WriteLine("if the format cannot be read from file extension you can specify the format using --sat | -s for smt-lib, --cnf | -c for dimacs");
    return 1;
}

if (!CnfStreamReader.TryParse(input, formula, out var cnf, out var comments))
{
    Console.WriteLine("Formula could not be parsed.");
    return 1;
}

var printer = new ResultPrinter(new DpllSat(new WatchedPruner(new WatchedFormula(cnf))), comments, watch);
printer.Print();
return 0;
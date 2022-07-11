using dpll;
using dpll.Algorithm;
using dpll.Reader;
using dpll.Runner;
using formula2cnf;
using formula2cnf.Formulas;
using System.Diagnostics;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("dpll.test")]

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
    Console.WriteLine("dpll usage: formula2cnf [input] [--sat | -s | --cnf | -c]");
    Console.WriteLine("if the format cannot be read from file extension you can specify the format using --sat | -s for smt-lib, --cnf | -c for dimacs");
    return 1;
}

var factory = new DpllFactory();
var runner = new SatRunner(input, formula, factory);
var result = runner.Run();
var printer = new ResultPrinter(result);
printer.Print(false);
return printer.ExitCode;

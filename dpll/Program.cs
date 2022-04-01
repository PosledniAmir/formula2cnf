using dpll.Algorithm;
using dpll.Reader;
using formula2cnf;
using formula2cnf.Formulas;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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
        if (formula == FormulaType.Error)
        {
            var info = new FileInfo(file);
            if (info.Extension == ".cnf")
            {
                formula = FormulaType.Dimacs;
            }
            else if (info.Extension == ".sat")
            {
                formula = FormulaType.Smt;
            }
        }
    }
    else
    {
        help = true;
    }
}

if (help || formula == FormulaType.Error)
{
    Console.WriteLine("Arguments could not be parsed.");
    Console.WriteLine("dpll usage: formula2cnf [input] [--sat | -s | --cnf | -c]");
    Console.WriteLine("if the format cannot be read from file extension you can specify the format using --sat | -s for smt-lib, --cnf | -c for dimacs");
    return 1;
}

var input = Console.OpenStandardInput();

if (file != "")
{
    input = File.Open(file, FileMode.Open, FileAccess.Read);
}

CnfFormula cnf;

if (formula == FormulaType.Smt)
{
    var reader = new Converter(input, false);
    if (!reader.TryConvert(out cnf, out var comments))
    {
        Console.WriteLine("Formula could not be parsed.");
        return 1;
    }
}
else if (formula == FormulaType.Dimacs)
{
    var reader = new DimacsReader(input);
    if (!reader.TryRead(out cnf))
    {
        Console.WriteLine("Formula could not be parsed.");
        return 1;
    }
}
else
{
    Console.WriteLine("Internal error");
    return 1;
}

var parsingTime = watch.Elapsed;
var sat = new DpllSat(cnf);
var result = sat.IsSatisfiable();
var totalTime = watch.Elapsed;
Console.WriteLine($"Solved formula in {totalTime.TotalMilliseconds} ms");
Console.WriteLine($"Time spend parsing formula: {parsingTime.TotalMilliseconds} ms");
Console.WriteLine($"Time spend solving formula: {(totalTime - parsingTime).TotalMilliseconds} ms");
Console.WriteLine($"Performed decisions: {sat.Decisions}");
Console.WriteLine($"Performed unit propagations: {sat.Resolutions}");
Console.WriteLine($"Result: {result}");
if (result)
{
    throw new NotImplementedException();
}
return 0;

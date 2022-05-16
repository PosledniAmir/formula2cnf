using cdcl.Algorithm;
using dpll;
using dpll.Reader;
using formula2cnf;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using watched.Algorithm;

[assembly: InternalsVisibleTo("dpll.test")]

var watch = new Stopwatch();
watch.Start();
var formula = FormulaType.Error;
var file = "";
var help = false;
var decisions = 1000;
float multiplier = 1.1f;
int cache = 1000;
bool nextDecisions = false;
bool nextMultiplier = false;
bool nextCache = false;

foreach (var arg in args)
{
    if (nextDecisions)
    {
        nextDecisions = false;
        if (!int.TryParse(arg, out decisions))
        {
            help = true;
            break;
        }
        continue;
    }
    else if (nextMultiplier)
    {
        nextMultiplier = false;
        if(!float.TryParse(arg, out multiplier))
        {
            help = true;
            break;
        }
        continue;
    }
    else if (nextCache)
    {
        nextCache = false;
        if (!int.TryParse(arg, out cache))
        {
            help = true;
            break;
        }
        continue;
    }
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
    else if (lower == "-d")
    {
        nextDecisions = true;
        continue;
    }
    else if (lower == "-m")
    {
        nextMultiplier = true;
        continue;
    }
    else if (lower == "-cache")
    {
        nextCache = true;
        continue;
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
    Console.WriteLine("Cdcl usage: formula2cnf [input] [--sat | -s | --cnf | -c] -d <decisions> -m <multiplier> -cache <cacheSize>");
    Console.WriteLine("if the format cannot be read from file extension you can specify the format using --sat | -s for smt-lib, --cnf | -c for dimacs");
    Console.WriteLine("<decisions> decsribe how many decisions are performed before the first restart");
    Console.WriteLine("<multiplier> each restart we update <decisions> = <decisions> * <multiplier>");
    Console.WriteLine("<cacheSize> size of learned clauses cache");
    return 1;
}

var input = Console.OpenStandardInput();

if (file != "")
{
    input = File.Open(file, FileMode.Open, FileAccess.Read);
}

ResultPrinter printer;

if (formula == FormulaType.Smt)
{
    var reader = new Converter(input, false);
    if (!reader.TryConvert(out var cnf, out var comments))
    {
        Console.WriteLine("Formula could not be parsed.");
        return 1;
    }
    printer = new ResultPrinter(new CdclSat(new WatchedPruner(new WatchedFormula(cnf)), decisions, multiplier, cache), comments, watch);
}
else if (formula == FormulaType.Dimacs)
{
    var reader = new DimacsReader(input);
    if (!reader.TryRead(out var cnf))
    {
        Console.WriteLine("Formula could not be parsed.");
        return 1;
    }
    printer = new ResultPrinter(new CdclSat(new WatchedPruner(new WatchedFormula(cnf)), decisions, multiplier, cache), watch);
}
else
{
    Console.WriteLine("Internal error");
    return 1;
}

printer.Print();
return 0;
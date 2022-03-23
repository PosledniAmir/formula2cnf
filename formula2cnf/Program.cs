using formula2cnf;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("formula2cnf.test")]

var implication = false;
var paths = new List<string>();
var help = false;

foreach(var arg in args)
{
    var lower = arg.ToLower();
    if (lower == "--implication")
    {
        implication = true;
    }
    else if (lower == "-i")
    {
        implication = true;
    }
    else if (File.Exists(arg))
    {
        paths.Add(arg);
    }
    else
    {
        help = true;
    }
}

if (help || paths.Count > 2)
{
    Console.WriteLine("Arguments could not be parsed.");
    Console.WriteLine("formula2cnf usage: formula2cnf [input [output]] [--implication | -i]");
    Console.WriteLine("--implication or -i option will use implications in the encoding instead of equivalence");
    return 1;
}

var input = Console.OpenStandardInput();
var output = Console.OpenStandardOutput();

if (paths.Count > 0)
{
    input = File.Open(paths[0], FileMode.Open, FileAccess.Read);
}

if (paths.Count > 1)
{
    output = File.Open(paths[1], FileMode.Create, FileAccess.Write);
}

var convertor = new Converter(input, implication);
if (!convertor.TryConvert(out var cnf))
{
    Console.WriteLine("Formula could not be parsed.");
    return 1;
}

using var writer = new StreamWriter(output);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
writer.Write(cnf.ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
return 0;

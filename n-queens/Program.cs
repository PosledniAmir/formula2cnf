using formula2cnf;
using n_queens;
using n_queens.Builders;
using System.Text;

var dirPath = Directory.GetCurrentDirectory();
if (args.Length > 0 )
{
    dirPath = args[0];
}

var start = 3;
if (args.Length > 1)
{
    start = int.Parse(args[1]);
}

var end = 1000;
if (args.Length > 2)
{
    end = int.Parse(args[2]);
}

Console.WriteLine("Welcome to CNF generator for the task N Queens Puzzle.");
Console.WriteLine("The program accepts these arguments: n-queens.exe [director] [start] [end]");
Console.WriteLine("Directory - path to directory where the n-queens files will be generated.");
Console.WriteLine("Start - starting n for n queens puzzle.");
Console.WriteLine("End - ending n for n queens puzzle.");
Console.WriteLine("--------------------------------------------------------------------------");
Console.WriteLine($"Directory = {dirPath}");
Console.WriteLine($"Start = {start}");
Console.WriteLine($"End = {end}");
Console.WriteLine("--------------------------------------------------------------------------");

for (int i = start; i <= end; i++)
{
    QueensFileGenerator.Generate(dirPath, i);
    Console.WriteLine($"{i} were generated");
}

return 0;
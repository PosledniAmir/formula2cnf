﻿using dimacs_result_reader;

if (args.Length != 2)
{
    Console.WriteLine("This program accepts exactly two arguments");
    Console.WriteLine("First argument is a path to dmc file generated by formula2cnf program");
    Console.WriteLine("Second argument is a path to a text file where the satisfiable assignment produced by CDCL is described");
    return 1;
}

var dmc = args[0];
var res = args[1];
var variables = VariableParser.Parse(dmc);
var result = ResultParser.Parse(res, variables);

foreach (var line in result)
{
    Console.WriteLine($"{line.Key} = {line.Value}");
}

var dimension = 0;
while (result.ContainsKey($"p_{dimension}_{0}"))
{
    dimension++;
}

for (var i = 0; i < dimension; i++)
{
    for (var j = 0; j < dimension; j++)
    {
        var value = result[$"p_{j}_{i}"] ? 'x' : ' ';
        Console.Write($"|{value}");
    }
    Console.WriteLine('|');
}

return 0;

using Benchmarker.Configs;
using Benchmarker.Runners;
using Benchmarker.Stats;


var dirPath = Directory.GetCurrentDirectory();
Console.WriteLine($"Starting benchmarking in {dirPath}");

ConfigProvider.GenerateConfig(dirPath);
var config = ConfigProvider.ReadConfiguration(dirPath);
var benchDir = config.BenchmarkDirectory;
var benchmarks = config.Configs.Select(c => new CnfBenchmarkRunner(c.GetFactory()));

foreach (var benchmark in benchmarks)
{
    var table = new StatTable(benchmark.SatFactoryName);
    Console.WriteLine($"Benchmark in {benchDir} of factory {table.FactoryName}");

    foreach(var row in benchmark.Run(benchDir))
    {
        Console.Write('.');
        table.Add(row.Item1, row.Item2.GetMinMeanMax());
    }
    Console.WriteLine();

    var printer = new TablePrinter(table.GetColumns());
    
    foreach(var line in printer.GetLines())
    {
        Console.WriteLine();
    }
}

using Benchmarker;
using Benchmarker.Runners;

var dirPath = Directory.GetCurrentDirectory();
ConfigProvider.GenerateConfig(dirPath);
var config = ConfigProvider.ReadConfiguration(dirPath);
var benchDir = config.BenchmarkDirectory;
var benchmarks = config.Configs.Select(c => new CnfBenchmarkRunner(c.GetFactory()));

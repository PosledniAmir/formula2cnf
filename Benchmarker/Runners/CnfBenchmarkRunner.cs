using Benchmarker.Stats;
using dpll.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker.Runners
{
    internal sealed class CnfBenchmarkRunner
    {
        public const string Ignore = "ignore";
        private readonly CnfDirectoryRunner _dirRunner;
        public string SatFactoryName => _dirRunner.SatFactoryName;

        public CnfBenchmarkRunner(CnfDirectoryRunner dirRunner)
        {
            _dirRunner = dirRunner;
        }

        public CnfBenchmarkRunner(ISatFactory factory)
        {
            _dirRunner = new CnfDirectoryRunner(new CnfFileRunner(factory));
        }

        public IEnumerable<Tuple<string, SatAggregator>> Run(string directoryPath)
        {
            var result = _dirRunner.Run(directoryPath);
            if (result.Item2.FailureCount == 0 && result.Item2.SuccessCount != 0)
            {
                yield return result;
            }

            var directory = new DirectoryInfo(directoryPath);
            foreach (var subDir in directory.GetDirectories().Where(d => !d.Name.StartsWith(Ignore)).OrderBy(d => d.Name))
            {
                result = _dirRunner.Run(subDir.FullName);
                if (result.Item2.FailureCount == 0 && result.Item2.SuccessCount != 0)
                {
                    yield return result;
                }
            }
        }
    }
}

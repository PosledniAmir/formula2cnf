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
            yield return _dirRunner.Run(directoryPath);

            var directory = new DirectoryInfo(directoryPath);
            foreach (var subDir in directory.GetDirectories())
            {
                yield return _dirRunner.Run(directoryPath);
            }
        }
    }
}

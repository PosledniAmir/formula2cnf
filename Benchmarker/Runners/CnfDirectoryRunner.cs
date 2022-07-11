using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker.Runners
{
    internal class CnfDirectoryRunner
    {
        private readonly CnfFileRunner _fileRunner;

        public CnfDirectoryRunner(CnfFileRunner fileRunner)
        {
            _fileRunner = fileRunner;
        }

        public Tuple<string, SatAggregator> Run(string directoryPath)
        {
            var aggregator = new SatAggregator();
            var directory = new DirectoryInfo(directoryPath);

            foreach (var file in directory.GetFiles())
            {
                aggregator.Add(file.FullName, _fileRunner.Run(file.FullName));
            }

            return Tuple.Create(directoryPath, aggregator);
        }
    }
}

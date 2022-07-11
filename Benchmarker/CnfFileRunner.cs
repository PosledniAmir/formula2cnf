using dpll.Reader;
using dpll.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker
{
    internal sealed class CnfFileRunner
    {
        private readonly ISatFactory _factory;

        public CnfFileRunner(ISatFactory factory)
        {
            _factory = factory;
        }

        public SatResult Run(string filePath)
        {
            var type = CnfFileReader.DetermineType(filePath);
            try
            {
                var input = File.Open(filePath, FileMode.Open, FileAccess.Read);
                var runner = new SatRunner(input, type, _factory);
                return runner.Run();
            }
            catch (Exception e)
            {
                return new SatResult(new SatStats(TimeSpan.Zero), new ErrorResult(true, e.Message), null);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker.Configs
{
    public sealed class Configuration
    {
        public static Configuration Default => new Configuration
        {
            BenchmarkDirectory = "..//..//..//..//Benchmarks",
            Configs = new List<SatConfig>
            {
                new CdclConfig
                {
                    Decisions = 1000,
                    Multiplier = 1.1F,
                    Cache = 1000
                },
                new WatchedConfig { },
                new DpllConfig { },
            }
        };

        public string BenchmarkDirectory { get; set; }
        public List<SatConfig> Configs { get; set; }
    }
}

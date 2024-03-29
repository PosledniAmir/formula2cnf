﻿using cdcl;
using dpll.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using watched;

namespace Benchmarker.Configs
{
    [XmlInclude(typeof(DpllConfig))]
    [XmlInclude(typeof(WatchedConfig))]
    [XmlInclude(typeof(CdclConfig))]
    public abstract class SatConfig
    {
        public abstract ISatFactory GetFactory();
    }
}

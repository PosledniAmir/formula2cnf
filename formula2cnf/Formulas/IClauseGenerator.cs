using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Formulas
{
    public interface IClauseGenerator
    {
        public int Variable { get; }
        public IEnumerable<IReadOnlyList<int>> Generate();
    }
}

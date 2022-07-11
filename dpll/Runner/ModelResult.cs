using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Runner
{
    public sealed class ModelResult
    {
        public readonly bool IsSatisfiable;
        public readonly IReadOnlyList<int> Model;
        public readonly VariableDescriptor? Description;

        public ModelResult(bool isSatisfiable, IReadOnlyList<int> model, VariableDescriptor? description)
        {
            IsSatisfiable = isSatisfiable;
            Model = model;
            Description = description;
        }
    }
}

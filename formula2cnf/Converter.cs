using formula2cnf.Formulas;
using formula2cnf.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf
{
    public sealed class Converter
    {
        private readonly Stream _input;
        private readonly bool _implication;

        public Converter(Stream input, bool implication)
        {
            _input = input;
            _implication = implication;
        }

        public bool TryConvert(out CnfFormula? cnf, out VariableDescriptor? descriptor)
        {
            cnf = null;
            descriptor = null;
            var grammar = new FormulaGrammar();
            if (grammar.TryParse(ReadInput(), out var tokens))
            {
                var builder = new FormulaTreeBuilder();
                foreach (var item in tokens)
                {
                    if (!builder.TryParse(item))
                    {
                        return false;
                    }
                }
                if (builder.Root != null)
                {
                    var generator = new ClauseGenerator(_implication);
                    var result = generator.Generate(builder.Root);
                    cnf = new CnfFormula(result);
                    descriptor = new VariableDescriptor(generator.First, generator.Count, generator.NamedVariables);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private string ReadInput()
        {
            using var reader = new StreamReader(_input);
            return reader.ReadToEnd();
        }
    }
}

using formula2cnf.Formulas;
using formula2cnf.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf
{
    internal sealed class Converter
    {
        private readonly Stream _input;
        private readonly Stream _output;
        private readonly bool _implication;

        public Converter(Stream input, Stream output, bool implication)
        {
            _input = input;
            _output = output;
            _implication = implication;
        }

        public void Convert()
        {
            var grammar = new FormulaGrammar();
            if (grammar.TryParse(ReadInput(), out var tokens))
            {
                var builder = new FormulaTreeBuilder();
                foreach (var item in tokens)
                {
                    if (!builder.TryParse(item))
                    {
                        throw new NotImplementedException();
                    }
                }
                if (builder.Root != null)
                {
                    var generator = new ClauseGenerator(_implication);
                    var result = generator.Generate(builder.Root);
                    var cnf = new CnfFormula(result);
                    PrintOutput(cnf.ToString());
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private string ReadInput()
        {
            using var reader = new StreamReader(_input);
            return reader.ReadToEnd();
        }

        private void PrintOutput(string output)
        {
            using var writer = new StreamWriter(_output);
            writer.Write(output);
        }
    }
}

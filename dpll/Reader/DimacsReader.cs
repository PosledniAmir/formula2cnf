using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Reader
{
    public class DimacsReader
    {
        private readonly Stream _input;
        private readonly List<List<int>> _clauses;
        private int _varNum;
        private int _claNum;

        public DimacsReader(Stream input)
        {
            _clauses = new List<List<int>>();
            _varNum = 0;
            _claNum = 0;
            _input = input;
        }

        public bool TryRead(out CnfFormula? cnf)
        {
            _claNum = 0;
            _varNum = 0;
            _clauses.Clear();

            using var reader = new StreamReader(_input);
            var line = reader.ReadLine();
            while (line != null)
            {

                if (!IsComment(line) && !string.IsNullOrWhiteSpace(line))
                {
                    if (IsDefinition(line))
                    {
                        if (!ParseDefinition(line))
                        {
                            cnf = null;
                            return false;
                        }
                    }
                    else if (!ParseNumbers(line))
                    {
                        cnf = null;
                        return false;
                    }
                }
                line = reader.ReadLine();
            }

            cnf = new CnfFormula(_clauses);
            return true;
        }

        private bool IsComment(string line)
        {
            return line.StartsWith('c');
        }

        private bool IsDefinition(string line)
        {
            return line.StartsWith('p');
        }

        private bool ParseDefinition(string line)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 4)
            {
                return false;
            }
            if (_varNum != 0 || _claNum != 0)
            {
                return false;
            }
            if (!int.TryParse(parts[2], out _varNum))
            {
                return false;
            }
            if (!int.TryParse(parts[2], out _claNum))
            {
                return false;
            }
            return true;
        }

        private bool ParseNumbers(string line)
        {
            var result = new List<int>();
            var index = 0;
            while (index < line.Length)
            {
                while (char.IsWhiteSpace(line[index]))
                {
                    index++;
                    if (index == line.Length)
                    {
                        break;
                    }
                }
                var start = index;

                while (!char.IsWhiteSpace(line[index]))
                {
                    index++;
                    if (index == line.Length)
                    {
                        break;
                    }
                }
                var end = index;

                if (int.TryParse(line[start..end], out var number) && number != 0)
                {
                    result.Add(number);
                }
                else if (number == 0)
                {
                    _clauses.Add(result);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}

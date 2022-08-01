using formula2cnf;
using n_queens.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n_queens
{
    internal static class QueensFileGenerator
    {
        public const string NamePrefix = "queens_";
        public const string NameSuffix = ".dmc";
        public static void Generate(string directory, int size)
        {
            var builder = new TableBuilder(size);
            var result = builder.Build();
            var bytes = Encoding.ASCII.GetBytes(result.ToString());
            using var stream = new MemoryStream(bytes);
            var converter = new Converter(stream, false);

            if (!converter.TryConvert(out var cnf, out var descriptor))
            {
                throw new Exception("Something went terribly wrong.");
            }
            else
            {
                var filePath = Path.Combine(directory, $"{NamePrefix}{size}{NameSuffix}");
                File.WriteAllText(filePath, $"{descriptor}{Environment.NewLine}{cnf}");
            }
        }
    }
}

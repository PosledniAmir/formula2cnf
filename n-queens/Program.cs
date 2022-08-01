using formula2cnf;
using n_queens;
using n_queens.Builders;
using System.Text;

var builder = new TableBuilder(4);
var result = builder.Build();
var bytes = Encoding.ASCII.GetBytes(result.ToString());
using var stream = new MemoryStream(bytes);
var converter = new Converter(stream, false);
if (converter.TryConvert(out var cnf, out var descriptor))
{
    Console.WriteLine(descriptor.ToString());
    Console.WriteLine();
    Console.WriteLine(cnf.ToString());
    return 0;
}

Console.WriteLine("Error, something went wrong.");

return 1;
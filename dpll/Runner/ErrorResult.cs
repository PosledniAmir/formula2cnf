using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Runner
{
    public sealed class ErrorResult
    {
        public readonly bool ErrorOccured;
        public readonly string? ErrorMessage;

        public ErrorResult(bool errorOccured, string? errorMessage)
        {
            ErrorOccured = errorOccured;
            ErrorMessage = errorMessage;
        }

        public override string ToString()
        {
            if (ErrorOccured)
            {
                return $"Error: {ErrorMessage}.";
            }
            else
            {
                return "No error occured.";
            }
        }
    }
}

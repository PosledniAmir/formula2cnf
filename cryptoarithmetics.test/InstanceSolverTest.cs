using cryptoarithmetics.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptoarithmetics.test
{
    public sealed class InstanceSolverTest
    {
        public struct TestData
        {
            public string Instance { get; set; }
            public int Base { get; set; }
            public bool Unique { get; set; }
            public int MaxSolutions { get; set; }
            public IReadOnlyList<string> Solutions { get; set; }
        }

        public static IEnumerable<object[]> GetData()
        {
            yield return new object[]
            {
                new TestData {
                    Instance = "SEND + MORE = MONEY",
                    Base = 2,
                    Unique = false,
                    MaxSolutions = 3,
                    Solutions = new List<string>
                    {
                        "1110 + 1001 = 10111",
                        "1010 + 1010 = 10100",
                        "1001 + 1000 = 10001",
                    },
                }
            };
            yield return new object[]
            {
                new TestData {
                    Instance = "SEND + MORE = MONEY",
                    Base = 10,
                    Unique = true,
                    MaxSolutions = 3,
                    Solutions = new List<string>
                    {
                        "9567 + 1085 = 10652",
                    },
                }
            };
            yield return new object[]
            {
                new TestData {
                    Instance = "(SEND + MORE = MONEY) && (SQUARE - DANCE = DANCER)",
                    Base = 16,
                    Unique = false,
                    MaxSolutions = 3,
                    Solutions = new List<string>
                    {
                        "(F00F + 1000 = 1000F) && (FF0000 - F0000 = F00000)",
                    },
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void BasicTest01(TestData input)
        {
            var solver = new InstanceSolver(input.Instance, input.Base, input.Unique);
            var solutions = new List<string>();
            var tries = input.MaxSolutions;
            while (solver.CanContinue && tries > 0)
            {
                var (good, result) = solver.Solve();
                if (good == 0)
                {
                    var splitted = result.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                    if (splitted.Length > 1)
                    {
                        solutions.Add(splitted[^1]);
                    }
                }
                tries--;
            }
            Assert.Equal(input.Solutions.Count, solutions.Count);
            for (var i = 0; i < solutions.Count; ++i)
            {
                Assert.Equal(input.Solutions[i], solutions[i]);
            }
        }
    }
}

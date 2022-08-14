using cryptoarithmetics.Parsing;
using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptoarithmetics.test
{
    public sealed class ConditionGeneratorTest
    {
        public struct TestData
        {
            public string Instance { get; set; }
            public bool Exception { get; set; }
        }

        public static IEnumerable<object[]> GetData()
        {
            yield return new object[]
            {
                new TestData {
                    Instance = "",
                    Exception = true,
                }
            };

            yield return new object[]
            {
                new TestData
                {
                    Instance = "SEND + ",
                    Exception = true,
                }
            };

            yield return new object[]
            {
                new TestData
                {
                    Instance = "SEND + MORE = MONEY",
                    Exception = false,
                }
            };

            yield return new object[]
            {
                new TestData
                {
                    Instance = "(SEND + MORE = MONEY)",
                    Exception = false,
                }
            };

            yield return new object[]
            {
                new TestData
                {
                    Instance = "(SEND + MORE = MONEY) || (SQUARE - DANCE = DANCER)",
                    Exception = false,
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void BasicTest01(TestData input)
        {
            var tokens = Tokenizer.TokenizeInstance(input.Instance).ToList();
            var result = Parser.Parse(tokens);
            using var context = new Context();
            var builder = new FormulaBuilder(context, 10);
            var generator = new ConditionGenerator(builder, result);
            var range = generator.CreateRangeConditions();
            if (input.Exception)
            {
                Assert.Throws<ArgumentException>(() => generator.CreateInstanceCondition(true));
            }
            else
            {
                var instance = generator.CreateInstanceCondition(true);
            }
        }
    }
}

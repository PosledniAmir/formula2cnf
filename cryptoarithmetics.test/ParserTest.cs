using cryptoarithmetics.Parsing;

namespace cryptoarithmetics.test
{
    public sealed class ParserTest
    {
        public struct TestData
        {
            public string Instance { get; set; }
            public HashSet<string> FirstLetters { get; set; }
            public List<string> Words { get; set; }
        }

        public static IEnumerable<object[]> GetData()
        {
            yield return new object[]
            {
                new TestData {
                    Instance = "",
                    FirstLetters = new HashSet<string>(),
                    Words = new List<string>(),
                }
            };

            yield return new object[]
            {
                new TestData
                {
                    Instance = "SEND + ",
                    FirstLetters = new HashSet<string> { "S" },
                    Words = new List<string> { "SEND" },
                }
            };

            yield return new object[]
            {
                new TestData
                {
                    Instance = "SEND + MORE = MONEY",
                    FirstLetters = new HashSet<string> { "S", "M" },
                    Words = new List<string> { "SEND", "MORE", "MONEY" },
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void BasicTest01(TestData input)
        {
            var tokens = Tokenizer.TokenizeInstance(input.Instance).ToList();
            var result = Parser.Parse(tokens);
            Assert.Equal(result.FirstLetters.Count, input.FirstLetters.Count);
            foreach (var item in input.FirstLetters)
            {
                Assert.Contains(item, result.FirstLetters);
            }
            Assert.Equal(result.FirstLetters, input.FirstLetters);
        }
    }
}
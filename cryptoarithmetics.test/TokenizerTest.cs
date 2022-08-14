using cryptoarithmetics.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptoarithmetics.test
{
    public sealed class TokenizerTest
    {
        public struct TestData
        {
            public string Instance { get; set; }
            public List<Tuple<Token, string>> Tokens { get; set; }
        }

        public static IEnumerable<object[]> GetData()
        {
            yield return new object[]
            {
                new TestData {
                    Instance = "",
                    Tokens = new List<Tuple<Token, string>> { }
                }
            };

            yield return new object[]
            {
                new TestData
                {
                    Instance = "SEND",
                    Tokens = new List<Tuple<Token, string>>
                    {
                        Tuple.Create(Token.Word, "SEND"),
                    }
                }
            };

            yield return new object[]
            {
                new TestData
                {
                    Instance = "SEND + ",
                    Tokens = new List<Tuple<Token, string>>
                    {
                        Tuple.Create(Token.Word, "SEND"),
                        Tuple.Create(Token.Plus, "+"),
                    }
                }
            };

            yield return new object[]
            {
                new TestData
                {
                    Instance = "SEND + MORE ",
                    Tokens = new List<Tuple<Token, string>>
                    {
                        Tuple.Create(Token.Word, "SEND"),
                        Tuple.Create(Token.Plus, "+"),
                        Tuple.Create(Token.Word, "MORE"),
                    }
                }
            };

            yield return new object[]
            {
                new TestData
                {
                    Instance = "SEND + MORE = MONEY",
                    Tokens = new List<Tuple<Token, string>>
                    {
                        Tuple.Create(Token.Word, "SEND"),
                        Tuple.Create(Token.Plus, "+"),
                        Tuple.Create(Token.Word, "MORE"),
                        Tuple.Create(Token.Equal, "="),
                        Tuple.Create(Token.Word, "MONEY"),
                    }
                }
            };

            yield return new object[]
            {
                new TestData
                {
                    Instance = "(SEND + MORE = MONEY) || (SQUARE - DANCE = DANCER)",
                    Tokens = new List<Tuple<Token, string>>
                    {
                        Tuple.Create(Token.LeftBracket, "("),
                        Tuple.Create(Token.Word, "SEND"),
                        Tuple.Create(Token.Plus, "+"),
                        Tuple.Create(Token.Word, "MORE"),
                        Tuple.Create(Token.Equal, "="),
                        Tuple.Create(Token.Word, "MONEY"),
                        Tuple.Create(Token.RightBracket, ")"),
                        Tuple.Create(Token.Or, "||"),
                        Tuple.Create(Token.LeftBracket, "("),
                        Tuple.Create(Token.Word, "SQUARE"),
                        Tuple.Create(Token.Plus, "+"),
                        Tuple.Create(Token.Word, "DANCE"),
                        Tuple.Create(Token.Equal, "="),
                        Tuple.Create(Token.Word, "DANCER"),
                        Tuple.Create(Token.RightBracket, ")"),
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void BasicTest01(TestData input)
        {
            var parsed = Tokenizer.TokenizeInstance(input.Instance).ToList();
            Assert.Equal(input.Tokens.Count, parsed.Count);
        }
    }
}

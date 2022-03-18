using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace dpll.test
{
    public class DeciderTest
    {
        [Fact]
        public void BasicTest01()
        {
            var decider = new Decider(2);
            var decided = new Stack<int>();

            var result = decider.TryDecide();
            Assert.NotEqual(0, result);
            decided.Push(result);

            result = decider.TryDecide();
            Assert.NotEqual(0, result);
            decided.Push(result);

            Assert.Equal(2, decided.Count);
            Assert.Contains(1, decided);
            Assert.Contains(2, decided);

            Assert.Equal(1, decider.Backtrack());
            decided.Pop();

            result = decider.TryDecide();
            Assert.NotEqual(0, result);
            decided.Push(result);

            Assert.Equal(2, decided.Count);
            Assert.Contains(1, decided);
            Assert.Contains(-2, decided);

            Assert.Equal(2, decider.Backtrack());
            decided.Pop();
            decided.Pop();

            result = decider.TryDecide();
            Assert.NotEqual(0, result);
            decided.Push(result);
            result = decider.TryDecide();
            Assert.NotEqual(0, result);
            decided.Push(result);

            Assert.Equal(2, decided.Count);
            Assert.Contains(-1, decided);
            Assert.Contains(2, decided);

            Assert.Equal(1, decider.Backtrack());
            decided.Pop();

            result = decider.TryDecide();
            Assert.NotEqual(0, result);
            decided.Push(result);

            Assert.Equal(2, decided.Count);
            Assert.Contains(-1, decided);
            Assert.Contains(-2, decided);
        }
    }
}
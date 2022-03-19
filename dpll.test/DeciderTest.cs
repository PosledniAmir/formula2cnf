using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace dpll.test
{
    public class DeciderTest
    {
        private IEnumerable<List<int>> Resolve(Decider decider)
        {
            var decided = new Stack<int>();
            var sat = true;
            while (sat)
            {
                var result = decider.TryDecide();
                while (result != 0)
                {
                    decided.Push(result);
                    result = decider.TryDecide();
                }

                yield return decided.ToList();

                var backtrack = decider.Backtrack();
                if (backtrack == 0)
                {
                    sat = false;
                }
                else
                {
                    for(int i = 0; i < backtrack; i++)
                    {
                        decided.Pop();
                    }
                }
            }
        }

        private void Next(List<int> values)
        {
            for(var i = values.Count - 1; i >= 0; i--)
            {
                if (values[i] > 0)
                {
                    values[i] = -values[i];
                    break;
                }
                else
                {
                    values[i] = -values[i];
                }
            }
        }

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

        [Fact]
        public void BasicTest02()
        {
            var decider = new Decider(5);
            var expected = new List<int> { 1, 2, 3, 4, 5 };
            var loops = 0;

            foreach(var resolved in Resolve(decider))
            {
                Assert.Equal(5, resolved.Count);
                foreach (var item in expected)
                {
                    Assert.Contains(item, resolved);
                }
                loops++;
                Next(expected);
            }

            Assert.Equal(32, loops);
        }

        [Fact]
        public void BasicTest03()
        {
            var decider = new Decider(6);
            var expected = new List<int> { 1, 2, 4, 5, 6 };
            var loops = 0;

            decider.TryDecide(3);
            foreach (var resolved in Resolve(decider))
            {
                Assert.Equal(5, resolved.Count);
                foreach (var item in expected)
                {
                    Assert.Contains(item, resolved);
                }
                loops++;
                Next(expected);
            }

            Assert.Equal(32, loops);
        }
    }
}
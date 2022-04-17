using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using watched.Algorithm;
using Xunit;

namespace dpll.test
{
    public sealed class WatchedClauseTest
    {
        [Fact]
        public void BasicTest01()
        {
            var watchedClause = new WatchedClause(0, new[] { 1 });
            var (first, second) = watchedClause.Exposed;
            Assert.Equal(1, first);
            Assert.Equal(0, second);
            Assert.True(watchedClause.Unit);
            var result = watchedClause.SetFalse(1, new HashSet<int>());
            Assert.Equal(0, result);
            Assert.True(watchedClause.Unsatisfiable);
        }

        [Fact]
        public void BasicTest02()
        {
            var watchedClause = new WatchedClause(0, new[] { 1, -2 });
            var (first, second) = watchedClause.Exposed;
            Assert.Equal(1, first);
            Assert.Equal(-2, second);
            Assert.False(watchedClause.Unit);
            var result = watchedClause.SetFalse(1, new HashSet<int>());
            Assert.Equal(0, result);
            Assert.True(watchedClause.Unit);
            Assert.False(watchedClause.Unsatisfiable);
        }

        [Fact]
        public void BasicTest03()
        {
            var watchedClause = new WatchedClause(0, new[] { 1, -2, 3 });
            var (first, second) = watchedClause.Exposed;
            Assert.Equal(1, first);
            Assert.Equal(-2, second);
            Assert.False(watchedClause.Unit);
            var result = watchedClause.SetFalse(1, new HashSet<int>());
            Assert.Equal(3, result);
            Assert.False(watchedClause.Unit);
            Assert.False(watchedClause.Unsatisfiable);
            result = watchedClause.SetFalse(3, new HashSet<int>(new[] { -1 }));
            Assert.Equal(0, result);
            Assert.True(watchedClause.Unit);
            Assert.False(watchedClause.Unsatisfiable);
        }
    }
}

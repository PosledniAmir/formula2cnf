using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using watched.Algorithm;
using Xunit;

namespace formula2cnf.test
{
    public sealed class WatchedArrayTest
    {
        [Fact]
        public void BasicTest()
        {
            var array = new WatchedArray<int>(new [] { 1, 2, 3, 4, 5 });
            Assert.Equal(1, array.GetValue(0));
            Assert.Equal(2, array.GetValue(1));
            Assert.Equal(3, array.GetValue(2));
            Assert.Equal(4, array.GetValue(3));
            Assert.Equal(5, array.GetValue(4));
            Assert.Equal(0, array.GetValue(5));
            Assert.Equal(0, array.GetValue(6));
        }
    }
}

using System;
using System.Linq;
using System.Threading;

namespace IEnumerableCorrelater.UnitTests.TestUtils
{
    /// <summary>
    /// Calling the Equal method for this class take a lot of time
    /// </summary>
    class SlowCompareElement
    {
        public static SlowCompareElement[] ToArrayOfSlowCompareElement(string s) =>
            s.Select(c => new SlowCompareElement(c)).ToArray();

        private readonly char c;

        public SlowCompareElement(char c)
        {
            this.c = c;
        }

        public override bool Equals(object obj)
        {
            Thread.Sleep(200);
            return obj is SlowCompareElement element &&
                   c == element.c;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(c);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using IEnumerableCorrelater.CollectionWrappers;
using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.UnitTests.TestUtils
{
    static class Utils
    {
        public static void SetToRetrunInputCollection<T>(this ICorrelater<T> correlater, int distance)
        {
            A.CallTo(() => correlater.Correlate(A<IEnumerable<T>>._, A<IEnumerable<T>>._))
                .ReturnsLazily((IEnumerable<T> c1, IEnumerable<T> c2) =>
                    new CorrelaterResult<T>(distance, c1.ToArray(), c2.ToArray()));
        }

        public static void SetToRetrun<T>(this ICorrelater<T> correlater, Func<IEnumerable<T>, IEnumerable<T>> func1 ,Func<IEnumerable<T>, IEnumerable<T>> func2, int distance = 0)
        {
            A.CallTo(() => correlater.Correlate(A<IEnumerable<T>>._, A<IEnumerable<T>>._))
                .ReturnsLazily((IEnumerable<T> c1, IEnumerable<T> c2) =>
                    new CorrelaterResult<T>(distance, func1(c1).ToArray(), func2(c2).ToArray()));
        }

        public static IEnumerable<char> RemoveNull(this IEnumerable<char> collection) =>
            collection.Where(c => c != '\0');
    }
}

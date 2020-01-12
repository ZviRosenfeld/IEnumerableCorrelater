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
            A.CallTo(() => correlater.Compare(A<ICollectionWrapper<T>>._, A<ICollectionWrapper<T>>._))
                .ReturnsLazily((ICollectionWrapper<T> c1, ICollectionWrapper<T> c2) =>
                    new CorrelaterResult<T>(distance, c1.ToArray(), c2.ToArray()));
        }

        public static void SetToRetrun<T>(this ICorrelater<T> correlater, Func<IEnumerable<T>, IEnumerable<T>> func1 ,Func<IEnumerable<T>, IEnumerable<T>> func2, int distance = 0)
        {
            A.CallTo(() => correlater.Compare(A<ICollectionWrapper<T>>._, A<ICollectionWrapper<T>>._))
                .ReturnsLazily((ICollectionWrapper<T> c1, ICollectionWrapper<T> c2) =>
                    new CorrelaterResult<T>(distance, func1(c1).ToArray(), func2(c2).ToArray()));
        }

        public static ICollectionWrapper<T> ToCollectionWrapper<T>(this IEnumerable<T> collection) =>
            new CollectionWrapperFactory().GetCollectionWrapper(collection);

        public static IEnumerable<char> RemoveNull(this IEnumerable<char> collection) =>
            collection.Where(c => c != '\0');
    }
}

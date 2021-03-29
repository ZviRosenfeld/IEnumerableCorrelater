using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEnumerableCorrelater.Exceptions;
using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.CollectionWrappers
{
    /// <summary>
    /// This is a decorator class for an ICollectionWrapper.
    /// It returns the area of the collection between start and end.
    /// </summary>
    class OffsetCollectionWrapper<T> : ICollectionWrapper<T>
    {
        private readonly ICollectionWrapper<T> innerCollection;
        private readonly int start;
        private readonly int end;

        public OffsetCollectionWrapper(ICollectionWrapper<T> innerCollection, int start, int end)
        {
            if (start > end)
                throw new InternalException($"Code 1000 (in {nameof(OffsetCollectionWrapper<T>)}: {nameof(start)} == {start} is smaller than {nameof(end)} == {end})");

            this.innerCollection = innerCollection;
            this.start = start;
            this.end = Math.Min(end, innerCollection.Length);
        }

        public T this[int index]
        {
            get
            {
                if (index >= end - start)
                    throw new InternalException($"Code 1001 ({nameof(index)} was {index}, while {nameof(start)} == {start} and {nameof(end)} == {end})");

                return innerCollection[index + start];
            }
        }

        public int Length => end - start;

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var element in innerCollection.Skip(start).Take(end - start))
                stringBuilder.Append(element + ", ");

            return stringBuilder.ToString();
        }

        public IEnumerator<T> GetEnumerator() =>
            innerCollection.Skip(start).Take(end - start).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

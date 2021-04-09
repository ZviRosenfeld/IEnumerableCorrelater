using IEnumerableCorrelater.CollectionWrappers;
using IEnumerableCorrelater.CorrelaterWrappers;
using IEnumerableCorrelater.Exceptions;
using IEnumerableCorrelater.Interfaces;
using IEnumerableCorrelater.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IEnumerableCorrelater.Correlaters
{
    /// <summary>
    /// PatienceDiffCorrelater is an algorithm that was developed specifically for comparing diffs in code. 
    /// It tends to do very well when comparing two files that started the same, but will perform poorly for almost every other case.
    /// </summary>
    public class PatienceDiffCorrelater<T> : AbstractCorrelater<T>, IContinuousCorrelater<T>
    {
        private readonly IContinuousCorrelater<T> innerCorrelater = 
            new IgnoreIdenticalBeginningAndEndCorrelaterWrapper<T>(new LazyFitMatchingElementsCorrelater());

        public PatienceDiffCorrelater()
        {
            if (default(T) != null && typeof(T) != typeof(char))
                throw new EnumerableCorrelaterException($"{nameof(T)} must be nullable or a char");

            innerCorrelater.OnProgressUpdate += (p, t) => OnProgressUpdate?.Invoke(p, t);
            innerCorrelater.OnResultUpdate += r => OnResultUpdate?.Invoke(r);
        }

        public override event Action<int, int> OnProgressUpdate;
        public event Action<CorrelaterResult<T>> OnResultUpdate;

        protected override CorrelaterResult<T> InternalCorrelate(ICollectionWrapper<T> collection1, ICollectionWrapper<T> collection2, CancellationToken cancellationToken = default)
        {
            return innerCorrelater.Correlate(collection1, collection2, cancellationToken);
        }

        private class LazyFitMatchingElementsCorrelater : ICorrelater<T>
        {
            private readonly PatienceSortingAlgorithm patienceSortingAlgorithm = new PatienceSortingAlgorithm();
            private readonly ICorrelater<T> innerCorrelater = new IgnoreIdenticalBeginningAndEndCorrelaterWrapper<T>(new NullCorrelator<T>());

            public event Action<int, int> OnProgressUpdate;

            public CorrelaterResult<T> Correlate(IEnumerable<T> collection1, IEnumerable<T> collection2, CancellationToken cancellationToken = default)
            {
                var collection1Wrapper = collection1.ToCollectionWrapper();
                var collection2Wrapper = collection2.ToCollectionWrapper();

                var (elementsUniqueInBothCollections, indexOfElementsInCollection1) = GetElementsUniqueInBothCollections(collection1Wrapper, collection2Wrapper);
                var longestSubsequence = patienceSortingAlgorithm.GetRisingIndexes(elementsUniqueInBothCollections.Select(v => v.LocationInCollection2).ToArray());
                var allResults = new List<CorrelaterResult<T>>();
                
                if (!longestSubsequence.Any())
                {
                    var bestMatch1 = collection1Wrapper.Concat(collection2Wrapper.Length.GetNNullElemenets<T>());
                    var bestMatch2 = collection1Wrapper.Length.GetNNullElemenets<T>().Concat(collection2Wrapper);

                    allResults.Add(new CorrelaterResult<T>(bestMatch1.Count(), bestMatch1.ToArray(), bestMatch2.ToArray()));
                }
                else
                {
                    int previousLocationInCollection1 = -1, previousLocationInCollection2 = -1;
                    for (var i = 0; i < longestSubsequence.Count(); i++)
                    {
                        var locationInCollection2 = longestSubsequence[i];
                        var matchingElement = collection2Wrapper[locationInCollection2];
                        var locationInCollection1 = indexOfElementsInCollection1[matchingElement].Location;
                        allResults.Add(innerCorrelater.Correlate(
                            new OffsetCollectionWrapper<T>(collection1Wrapper, previousLocationInCollection1 + 1, locationInCollection1),
                            new OffsetCollectionWrapper<T>(collection2Wrapper, previousLocationInCollection2 + 1, locationInCollection2),
                            cancellationToken));
                        allResults.Add(new CorrelaterResult<T>(0, new[] { matchingElement }, new[] { matchingElement }));
                        previousLocationInCollection1 = locationInCollection1;
                        previousLocationInCollection2 = locationInCollection2;
                    }
                    allResults.Add(innerCorrelater.Correlate(
                            new OffsetCollectionWrapper<T>(collection1Wrapper, previousLocationInCollection1 + 1, collection1Wrapper.Length),
                            new OffsetCollectionWrapper<T>(collection2Wrapper, previousLocationInCollection2 + 1, collection2Wrapper.Length),
                            cancellationToken));
                }

                return Merge(allResults);
            }

            public CorrelaterResult<T> Merge(List<CorrelaterResult<T>> results)
            {
                var bestMatch1 = new List<T>();
                var bestMatch2 = new List<T>();
                var distance = 0L;

                foreach (var result in results)
                {
                    bestMatch1.AddRange(result.BestMatch1);
                    bestMatch2.AddRange(result.BestMatch2);
                    distance += result.Distance;
                }

                return new CorrelaterResult<T>(distance, bestMatch1.ToArray(), bestMatch2.ToArray());
            }

            private (List<ElementLocation>, Dictionary<T, TimesAndLocation>) GetElementsUniqueInBothCollections(ICollectionWrapper<T> collection1, ICollectionWrapper<T> collection2)
            {
                var elementsAppearingInCollection1 = GetElemenetsInCollection(collection1);
                var elementsAppearingInCollection2 = GetElemenetsInCollection(collection2);

                var elementsUniqueInBothCollections = new List<ElementLocation>();
                foreach (var element in collection1)
                {
                    if (!elementsAppearingInCollection1.ContainsKey(element)) continue;
                    if (elementsAppearingInCollection1[element].Times > 1) continue;
                    if (!elementsAppearingInCollection2.ContainsKey(element)) continue;
                    if (elementsAppearingInCollection2[element].Times > 1) continue;

                    elementsUniqueInBothCollections.Add(new ElementLocation(element, elementsAppearingInCollection1[element].Location, elementsAppearingInCollection2[element].Location));
                }

                return (elementsUniqueInBothCollections, elementsAppearingInCollection1);
            }

            private Dictionary<T, TimesAndLocation> GetElemenetsInCollection(ICollectionWrapper<T> collection)
            {
                var elementsAppearingInCollection = new Dictionary<T, TimesAndLocation>();
                for (var i = 0; i < collection.Length; i++)
                {
                    var element = collection[i];
                    if (elementsAppearingInCollection.ContainsKey(element))
                        elementsAppearingInCollection[element].Times++;
                    else
                        elementsAppearingInCollection[element] = new TimesAndLocation(i, 1);
                }

                return elementsAppearingInCollection;
            }
        }

        private class ElementLocation
        {
            public ElementLocation(T element, int locationInCollection1, int locationInCollection2)
            {
                Element = element;
                LocationInCollection1 = locationInCollection1;
                LocationInCollection2 = locationInCollection2;
            }

            public T Element;

            public int LocationInCollection1 { get; }

            public int LocationInCollection2 { get; }

            public override string ToString() =>
                $"{Element} at ({LocationInCollection1}, {LocationInCollection2})";
        }

        private class TimesAndLocation
        {
            public TimesAndLocation(int location, int times)
            {
                Location = location;
                Times = times;
            }

            public int Location { get; }

            public int Times { get; set; }

            public override string ToString() =>
                $"{nameof(Location)}:  {Location}; {nameof(Times)}:  {Times}";
        }
    }
}

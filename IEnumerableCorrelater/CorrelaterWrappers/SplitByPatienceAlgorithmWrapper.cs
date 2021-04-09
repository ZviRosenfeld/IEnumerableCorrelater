using IEnumerableCorrelater.CollectionWrappers;
using IEnumerableCorrelater.Interfaces;
using IEnumerableCorrelater.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IEnumerableCorrelater.CorrelaterWrappers
{
    /// <summary>
    /// This wrapper splits the correlation problems into smaller problems by finding items that are appear exactly once in both collections.
    /// We then set as many of these as we can in place, and use an inner correlater to correlate the items in between.
    /// </summary>
    public class SplitByPatienceAlgorithmWrapper<T> : ICorrelater<T>
    {
        private readonly PatienceSortingAlgorithm patienceSortingAlgorithm = new PatienceSortingAlgorithm();
        private readonly ICorrelater<T> innerCorrelater;
        private readonly bool multiThread;

        public SplitByPatienceAlgorithmWrapper(ICorrelater<T> innerCorrelater, bool multiThread = true)
        {
            this.innerCorrelater = innerCorrelater;
            this.multiThread = multiThread;
        }

        public event Action<int, int> OnProgressUpdate;

        public CorrelaterResult<T> Correlate(IEnumerable<T> collection1, IEnumerable<T> collection2, CancellationToken cancellationToken = default)
        {
            var collection1Wrapper = collection1.ToCollectionWrapper();
            var collection2Wrapper = collection2.ToCollectionWrapper();

            var (elementsUniqueInBothCollections, indexOfElementsInCollection1) = GetElementsUniqueInBothCollections(collection1Wrapper, collection2Wrapper);
            var longestSubsequence = patienceSortingAlgorithm.GetRisingIndexes(elementsUniqueInBothCollections.Select(v => v.LocationInCollection2).ToArray());
            var allResults = new List<Task<CorrelaterResult<T>>>();

            if (!longestSubsequence.Any())
            {
                var bestMatch1 = collection1Wrapper.Concat(collection2Wrapper.Length.GetNNullElemenets<T>());
                var bestMatch2 = collection1Wrapper.Length.GetNNullElemenets<T>().Concat(collection2Wrapper);

                allResults.Add(Task.FromResult(new CorrelaterResult<T>(bestMatch1.Count(), bestMatch1.ToArray(), bestMatch2.ToArray())));
            }
            else
            {
                int previousLocationInCollection1 = -1, previousLocationInCollection2 = -1;
                for (var i = 0; i < longestSubsequence.Count(); i++)
                {
                    var locationInCollection2 = longestSubsequence[i];
                    var matchingElement = collection2Wrapper[locationInCollection2];
                    var locationInCollection1 = indexOfElementsInCollection1[matchingElement].Location;
                    var relevantCollection1 = new OffsetCollectionWrapper<T>(collection1Wrapper, previousLocationInCollection1 + 1, locationInCollection1);
                    var relevantCollection2 = new OffsetCollectionWrapper<T>(collection2Wrapper, previousLocationInCollection2 + 1, locationInCollection2);
                    allResults.Add(Task.Run(() => innerCorrelater.Correlate(relevantCollection1, relevantCollection2, cancellationToken)));
                    if (!multiThread) allResults.Last().Wait();
                    allResults.Add(Task.FromResult(new CorrelaterResult<T>(0, new[] { matchingElement }, new[] { matchingElement })));
                    previousLocationInCollection1 = locationInCollection1;
                    previousLocationInCollection2 = locationInCollection2;
                }
                allResults.Add(Task.Run(() =>
                    innerCorrelater.Correlate(
                        new OffsetCollectionWrapper<T>(collection1Wrapper, previousLocationInCollection1 + 1, collection1Wrapper.Length),
                        new OffsetCollectionWrapper<T>(collection2Wrapper, previousLocationInCollection2 + 1, collection2Wrapper.Length),
                        cancellationToken)));
            }

            return Merge(allResults);
        }

        public CorrelaterResult<T> Merge(List<Task<CorrelaterResult<T>>> results)
        {
            var bestMatch1 = new List<T>();
            var bestMatch2 = new List<T>();
            var distance = 0L;

            foreach (var result in results.Select(t => t.Result))
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
# IEnumerableCorrelater

EnumerableCorrelater knows to compare two IEnumerables or strings. It returns the distance between them, and returns two arrays which represent their "best match".
The following 2 examples should demonstrate what I mean by "best match":

```
Collection1 = { "A", "B", "C", "D"}
Collection2 = { "A", "B", "D"}

BestMatch1 = { "A", "B",  "C", "D"}
BestMatch2 = { "A", "B", null, "D"}
```

```
Collection1 = { "A", "B", "C", "D"}
Collection2 = { "A", "B", "I", "D"}

BestMatch1 = { "A", "B", "C", "D"}
BestMatch2 = { "A", "B", "I", "D"}
```

## Table of Constant

- [Usage](#usage)
  - [Impotent Notes](#impotent-notes)
- [Correlaters](#correlaters)
  - [LevenshteinCorrelater\<T>](#levenshteincorrelater)
  - [DamerauLevenshteinCorrelater\<T>](#dameraulevenshteincorrelater)
  - [DynamicLcsCorrelater\<T>](#dynamiclcscorrelater)
- [Optimizations](#optimizations)
  - [SplitToChunksCorrelaterWrapper\<T>](#splittochunkscorrelaterwrapper)
  - [IgnoreIdenticalBeginningAndEndCorrelaterWrapper\<T>](#ignoreidenticalbeginningandendcorrelaterwrapper)
- [IContinuousCorrelaters](#icontinuouscorrelaters)
- [OnProgressUpdate Event](#onprogressupdate-event)

## Usage

**Exsample1:** Comparing two collections. The example compares arrays of strings, but you can really compare collections of any type as longs as the type is nullable or a char.

```CSharp
uint removalCost = 1, insertionCost = 1;

// You'll need to implement your own IDistanceCalculator<T>. 
// IDistanceCalculator defines the "distance" between any two elements.
IDistanceCalculator<string> distanceCalculator = new MyDistanceCalculator<string>();

// The library contains a number of ICorrelaters. 
// LevenshteinCorrelater uses dynamic programing to find the Levenshtein-distance between the two collections.
ICorrelater<string> correlater = 
    new LevenshteinCorrelater<string>(distanceCalculator, removalCost, insertionCost);

string[] array1 = { "A", "D", "C" };
string[] array2 = { "A", "B", "C" };

// Compare the collections - you can compare any IEnumerable<T>.
CorrelaterResult<string> result = correlater.Correlate(array1, array2);

// Print some of the result
Console.WriteLine(result.Distance);
Console.WriteLine(result.BestMatch1);
Console.WriteLine(result.BestMatch2);
```

**Exsample2:** Comparing 2 strings. A string is actually an IEnumerable\<char>, and therefore we must use an ICorrelater\<char>.

```CSharp
uint removalCost = 1, insertionCost = 1;

// You'll need to implement your own IDistanceCalculator<char>. 
// IDistanceCalculator defines the "distance" between any two elements.
IDistanceCalculator<char> distanceCalculator = new MyDistanceCalculator<char>();

// The library contains a number of ICorrelaters. 
// LevenshteinCorrelater uses dynamic programing to find the Levenshtein-distance between the two collections.
// Since a string is actually an IEnumerable<char>, we need to use an ICorrelater<char>.
ICorrelater<char> correlater = 
    new LevenshteinCorrelater<char>(distanceCalculator, removalCost, insertionCost);

string string1 = "ABC";
string string2 = "ADC";

// Compare the strings.
CorrelaterResult<char> result = correlater.Correlate(string1, string2);

// Print some of the result
Console.WriteLine(result.Distance);
Console.WriteLine(result.BestMatch1);
Console.WriteLine(result.BestMatch2);
```

### Impotent Notes

- Correlaters are not thread safe.
- All costs must not be negative.
- The Equal method must be defined in a meaningful way for the elements of the collection you're correlating.
- The distance between equal elements must be zero.
- IEnumerableCorrelater doesn't support comparing collections with null elements in them. If you need null elements, consider using the "Null Object Pattern".
- By default, the correlaters will copy the collection to an array (unless the collection is a string, array or list). If you don't want this to happen, you'll need to implements the [ICollectionWrapper](IEnumerableCorrelater/Interfaces/ICollectionWrapper.cs) interface to wrap you're collection, and call the correlate method with that.

### A Sample Implementation of an IDistanceCalculator\<char>

```CSharp
/// <summary>
/// An implantation of an IDistanceCalculator&lt;char&gt;
/// </summary>
class CharDistanceCalculator : IDistanceCalculator<char>
{
    private const int DEFAULT_DISTANCE = 20;
    // We'll use a dictionary that will hold the distances between different pairs
    private readonly Dictionary<Tuple<char, char>, uint> distance = new Dictionary<Tuple<char, char>, uint>()
    {
        {new Tuple<char, char>('a', 'e'), 1 },
        {new Tuple<char, char>('a', 'i'), 2 },
        {new Tuple<char, char>('a', 'o'), 2 },
        {new Tuple<char, char>('a', 'u'), 3 },
        {new Tuple<char, char>('b', 'd'), 1 },
        {new Tuple<char, char>('c', 'e'), 2 },
        {new Tuple<char, char>('c', 's'), 2 },
        {new Tuple<char, char>('c', 'i'), 3 },
        ...
    }; 

    public uint Distance(char element1, char element2)
    {
	// If the elements are equal, they should return a distance of 0
	if (element1.Equals(element2))
            return 0;
	
        var tuple = new Tuple<char, char>(element1, element2);
        if (distance.ContainsKey(tuple))
            return distance[tuple];

        // For any pairs not in the dictionary, we'll return a default distance.
        return DEFAULT_DISTANCE;
    }
}
```

## Correlaters

### LevenshteinCorrelater

[LevenshteinCorrelater\<T>](IEnumerableCorrelater/Correlaters/LevenshteinCorrelater.cs) Finds the [LevenshteinDistance](https://en.wikipedia.org/wiki/Levenshtein_distance) and best correlation between two collections using dynamic programming.

### DamerauLevenshteinCorrelater

[DamerauLevenshteinCorrelater\<T>](IEnumerableCorrelater/Correlaters/DamerauLevenshteinCorrelater.cs) Finds the [DamerauLevenshteinDistance](https://en.wikipedia.org/wiki/Damerau%E2%80%93Levenshtein_distance) and best correlation between two collections using dynamic programming.

### DynamicLcsCorrelater

[DynamicLcsCorrelater\<T>](IEnumerableCorrelater/Correlaters/DynamicLcsCorrelater.cs) Finds the [LongestCommonSubsequence](https://en.wikipedia.org/wiki/Longest_common_subsequence_problem) and best correlation between two collections using dynamic programming. Available since version 1.1.2.

## Optimizations

Correlation of big collections can take a lot of time.
To solve this, there are a number of ICorrelater\<T> that can be wrapped around the [base correlaters](#correlaters).
These wrappers can greatly increase the base correlater's performance.

### [SplitToChunksCorrelaterWrapper](IEnumerableCorrelater/CorrelaterWrappers/SplitToChunksCorrelaterWrapper.cs)

This wrapper splits the collection into smaller chunks, and correlates each chunk individually, thus reducing correlation time and memory consumption.
Since the correlates typically have a time and memory complexity of O(n\*m), where n and m are the size of the collection being correlated,
reducing the collections size can have a big impact on performance.

Please note that using SplitToChunksCorrelaterWrapper will reduce your correlation's accuracy. 

```CSharp
uint removalCost = 7;
uint insertionCost = 7;
uint missmatchCost = 10;
int chunkSize = 200; // Bigger chunks will result in a slower, albeit more accurate, correlation
ICorrelater<char> innerCorrelater = 
    new LevenshteinCorrelater<char>(missmatchCost, removalCost, insertionCost);

// The SplitToChunksCorrelaterWrapper wraps an inner ICorrelater
ICorrelater<char> splitToChunksCorrelaterWrapper =
    new SplitToChunksCorrelaterWrapper<char>(innerCorrelater, chunkSize);

CorrelaterResult<char> result = splitToChunksCorrelaterWrapper.Correlate(collection1, collection2);
``` 

SplitToChunksCorrelaterWrapper is a [IContinuousCorrelater](#icontinuouscorrelaters).
This means that the OnResultUpdate event will be triggered for each new chunk that is correlated, 
so you can start displaying the results before the full calculation is completed.

### [IgnoreIdenticalBeginningAndEndCorrelaterWrapper](IEnumerableCorrelater/CorrelaterWrappers/IgnoreIdenticalBeginningAndEndCorrelaterWrapper.cs)

Available since version 1.1.2.

This wrapper improves the correlation's performance be removing the beginning and the end of the sequence if they are equal.
This can be useful in cases like source code correlation - where the changes are likely only a few lines in the middle of a file.

For instance, if we'd be comparing the following sequences, we'd be able to remove the "A" and "B" from the beginning and the "Y" and "Z" from the end.
As a result, all we'd need to compare is the "S" to the "T".

```
Collection1 = { "A", "B", "S", "Y", "Z" }
Collection2 = { "A", "B", "T", "Y", "Z" }
```

IgnoreIdenticalBeginningAndEndCorrelaterWrapper is a [IContinuousCorrelater](#icontinuouscorrelaters).
If the inner correlater is not continuous the "OnResultUpdate" will be raised twice - once for the equal part of the collection, and a second time for the rest of the result.
If, on the other hand, the inner correlater is continuous the "OnResultUpdate" will be raised every time the inner correlater raises the event, plus once at the before the inner correlater starts with the beginning part of the collections that's equal, and another time after the inner correlater finishes with the end part that's equal.

## IContinuousCorrelaters

Correlation of big collections can take a considerable amount of time.

ContinuousCorrelaters solve this problem by providing the caller with updates
on the correlation of the earlier segments of the collection while they continue working out the correlation of the later ones. 

The ContinuousCorrelaters will raise the OnResultUpdate for every segment it finishes correlating.
Please note that the OnResultUpdate will only contain the new segment (and no previously sent segments). 
Also there's no guarantee that the accumulated distance sent to the OnResultUpdate will equal the actual distance.

```CSharp
IContinuousCorrelater<char> continuousCorrelater =
    new SplitToChunksCorrelaterWrapper<char>(innerCorrelater, chunkSize);
continuousCorrelater.OnResultUpdate += (CorrelaterResult<char> partialResult) =>
{
    // Do something with the result here.
    // Please note that the OnResultUpdate will only contain the new segment (and not previously sent segments).

    myUi.Distance += partialResult.Distance; // Note that the accumulated distance may differ from the actual distance.
    myUi.BestMatch1.AddRange(partialResult.BestMatch1);
    myUi.BestMatch2.AddRange(partialResult.BestMatch2);
};

// Run the correlate in a new thread so that our UI doesn't freeze
Task.Run(() => continuousCorrelater.Correlate(collection1, collection2));
```

## OnProgressUpdate Event

The ICorrelater interface contains the OnProgressUpdate event, which is called to update the correlation's progress.
The OnProgressUpdate event is called with 2 parameters of type int, where the first is the current progress, and the second is the total progress.

```CSharp
ICorrelater<string> correlater = new LevenshteinCorrelater<string>(10, 7, 7);
correlater.OnProgressUpdate += (int currentProgress, int totalProgress) =>
{
    // Do something with the progress update here
};

CorrelaterResult<string> result = correlater.Correlate(collection1, collection2);
```
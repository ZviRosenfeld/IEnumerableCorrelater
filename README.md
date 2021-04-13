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
  - [MyersAlgorithmCorrelater\<T>](#myersalgorithmcorrelater)
  - [PatienceDiffCorrelater\<T>](#patiencediffcorrelater)
  - [HuntSzymanskiCorrelater\<T>](#HuntSzymanskiCorrelater)
  - [LevenshteinCorrelater\<T>](#levenshteincorrelater)
  - [DamerauLevenshteinCorrelater\<T>](#dameraulevenshteincorrelater)
  - [DynamicLcsCorrelater\<T>](#dynamiclcscorrelater)
- [Optimizations](#optimizations)
  - [SplitToChunksCorrelaterWrapper\<T>](#splittochunkscorrelaterwrapper)
  - [IgnoreIdenticalBeginningAndEndCorrelaterWrapper\<T>](#ignoreidenticalbeginningandendcorrelaterwrapper)
  - [SplitByPatienceAlgorithmWrapper\<T>](#splitbypatiencealgorithmwrapper)
- [IContinuousCorrelaters](#icontinuouscorrelaters)
- [OnProgressUpdate Event](#onprogressupdate-event)

## Usage

**Exsample1:** Comparing two collections using MyersAlgorithmCorrelater. The example compares arrays of strings, but you can really compare collections of any type as longs as the type is nullable or a char.
Note that MyersAlgorithmCorrelater is available since version 1.2.0.

```CSharp
// The library contains a number of ICorrelaters. 
// MyersAlgorithmCorrelater is particularity good for cases where we aren't expecting many changes (like diff tools for code changes). 
// Indeed, it is used as the default diff algorithm for git.
ICorrelater<string> correlater = new MyersAlgorithmCorrelater<string>();
            
string[] array1 = { "A", "D", "C" };
string[] array2 = { "A", "B", "C" };

// Correlate the collections - you can compare any IEnumerable<T>.
CorrelaterResult<string> result = correlater.Correlate(array1, array2);

// Print some of the result
Console.WriteLine(result.Distance); // Should be 2
Console.WriteLine(result.BestMatch1); // Should be { "A", "D", null, "C"}
Console.WriteLine(result.BestMatch2); // Should be { "A", null, "B", "C"}

return result;
```

**Exsample2:** Comparing 2 strings using the LevenshteinCorrelater. A string is actually an IEnumerable\<char>, and therefore we must use an ICorrelater\<char>.

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
- The Equal method must be defined in a meaningful way for the elements of the collection you're correlating.
- IEnumerableCorrelater doesn't support comparing collections with null elements in them. If you need null elements, consider using the "Null Object Pattern".
- By default, the correlaters will copy the collection to an array (unless the collection is a string, array or list). If you don't want this to happen, you'll need to implements the [ICollectionWrapper](IEnumerableCorrelater/Interfaces/ICollectionWrapper.cs) interface to wrap you're collection, and call the correlate method with that.

For LevenshteinCorrelater, DamerauLevenshteinCorrelater and DynamicLcsCorrelater:
- All costs must not be negative.
- The distance between equal elements must be zero.

## Correlaters

### MyersAlgorithmCorrelater

[MyersAlgorithmCorrelater\<T>](IEnumerableCorrelater/Correlaters/MyersAlgorithmCorrelater.cs) is an algorithm for calculating the [LongestCommonSubsequence](https://en.wikipedia.org/wiki/Longest_common_subsequence_problem) and best correlation between two collections.
This algorithm has a runtime of O(n \* d), where n is the size of the bigger collection, and d is the number of changed elements between the collections.
This makes the algorithm particularity good for cases where we aren't expecting many changes (like diff tools for code changes). Indeed, it is used as the default diff algorithm for git.

This correlater is Available since version 1.2.0.

### PatienceDiffCorrelater

[PatienceDiffCorrelater\<T>](IEnumerableCorrelater/Correlaters/PatienceDiffCorrelater.cs) is an algorithm that was developed specifically for comparing diffs in code. It does a very good job at creating human-readable diffs.
You can read more about it [here](https://bramcohen.livejournal.com/73318.html). 

This correlater is Available since version 1.2.1.

### HuntSzymanskiCorrelater

[HuntSzymanskiCorrelater\<T>](IEnumerableCorrelater/Correlaters/HuntSzymanskiCorrelater.cs) is another algorithm for calculating the [LongestCommonSubsequence](https://en.wikipedia.org/wiki/Longest_common_subsequence_problem) and best correlation between two collections.
This algorithm has a runtime of O((r + n) log n), where n is the size of the bigger collection, and r is the total number of matching pairs.
In the wost case r = n \* n, which would mean a runtime of O((n \* n) long n), but in practice O(n log n) is rather expected.

This correlater is Available since version 1.2.1.

### LevenshteinCorrelater

[LevenshteinCorrelater\<T>](IEnumerableCorrelater/Correlaters/LevenshteinCorrelater.cs) Finds the [LevenshteinDistance](https://en.wikipedia.org/wiki/Levenshtein_distance) and best correlation between two collections using dynamic programming.
The correlater's runtime is O(n \* m), where n and m are the length of the collections being compared.

### DamerauLevenshteinCorrelater

[DamerauLevenshteinCorrelater\<T>](IEnumerableCorrelater/Correlaters/DamerauLevenshteinCorrelater.cs) Finds the [DamerauLevenshteinDistance](https://en.wikipedia.org/wiki/Damerau%E2%80%93Levenshtein_distance) and best correlation between two collections using dynamic programming.
The correlater's runtime is O(n \* m), where n and m are the length of the collections being compared.

### DynamicLcsCorrelater

[DynamicLcsCorrelater\<T>](IEnumerableCorrelater/Correlaters/DynamicLcsCorrelater.cs) Finds the [LongestCommonSubsequence](https://en.wikipedia.org/wiki/Longest_common_subsequence_problem) and best correlation between two collections using dynamic programming. 
The dynamic algorithm for the LCS problem has a runtime of O(n \* m), where n and m are the length of the collections being compared.

This correlater is Available since version 1.2.0.

## Optimizations

Correlation of big collections can take a lot of time.
To solve this, there are a number of ICorrelater\<T> that can be wrapped around the [base correlaters](#correlaters).
These wrappers can greatly increase the base correlater's performance.

### [SplitToChunksCorrelaterWrapper](IEnumerableCorrelater/CorrelaterWrappers/SplitToChunksCorrelaterWrapper.cs)

This wrapper splits the collection into smaller chunks, and correlates each chunk individually, thus reducing correlation time and memory consumption.
Since the correlaters typically have a time and memory complexity of O(n\*m), where n and m are the size of the collection being correlated,
reducing the collections size can have a big impact on performance.

Please note that using SplitToChunksCorrelaterWrapper will reduce your correlation's accuracy. 

```CSharp
uint removalCost = 7;
uint insertionCost = 7;
uint missmatchCost = 10;
int chunkSize = 200; // Bigger chunks will result in a slower, albeit more accurate, correlations
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

Available since version 1.2.0.

This wrapper improves the correlation's performance be removing the beginning and the end of the sequence if they are equal.
This can be useful in cases like source code correlation - where the changes are likely only a few lines in the middle of a file.

For instance, if we'd be comparing the following sequences, we'd be able to remove the "A" and "B" from the beginning and the "Y" and "Z" from the end.
As a result, all we'd need to compare is the "S" to the "T".

```
Collection1 = { "A", "B", "S", "Y", "Z" }
Collection2 = { "A", "B", "T", "Y", "Z" }
```

IgnoreIdenticalBeginningAndEndCorrelaterWrapper is a [IContinuousCorrelater](#icontinuouscorrelaters).
If the inner correlater is not continuous, the "OnResultUpdate" will be raised twice - once for the equal part of the collection, and a second time for the rest of the result.
If, on the other hand, the inner correlater is continuous the "OnResultUpdate" will be raised every time the inner correlater raises the event, plus once at the before the inner correlater starts with the beginning part of the collections that's equal, and another time after the inner correlater finishes with the end part that's equal.

### [SplitByPatienceAlgorithmWrapper](IEnumerableCorrelater/CorrelaterWrappers/SplitByPatienceAlgorithmWrapper.cs)

Available since version 1.2.1.

This wrapper splits the correlation problems into smaller problems by finding items that are appear exactly once in both collections.
We then set as many of these as we can in place, and use an inner correlater to correlate the collections in between.

For instance, say we have the following 2 collections:

```
Collection1 = { "1", "A", "2", "3", "4" "B", "5" }
Collection2 = { "6", "A", "7", "8", "B", "9" }
```

We'd first set "A" and "B" in both collections as matching.
Then we'd used the inner correlater to correlate the collection before "A" (e.g. { "1" } and { "6" }), the collections between "A" to "B" (e.g. { "2", "3" , "4" } and { "7", "8" }), and the collections after "B" e.g. { "5" } and { "9" }).

Note that this is the tactic used in [PatienceDiffCorrelater\<T>](#patiencediffcorrelater). Indeed, out implementation of the PatienceDiffCorrelater utilizes this wrapper. 

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
Please note that not all the correlaters raise the OnProgressUpdate event.

```CSharp
ICorrelater<string> correlater = new LevenshteinCorrelater<string>(10, 7, 7);
correlater.OnProgressUpdate += (int currentProgress, int totalProgress) =>
{
    // Do something with the progress update here
};

CorrelaterResult<string> result = correlater.Correlate(collection1, collection2);
```
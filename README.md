# IEnumerableCorrelater

EnumerableCorrelater knows to compare two IEnumerables or strings. It returns the distance between them, and returns two arrays which represent their "best match".
The following 2 exsamples should deminstrate what I mean by "best match":

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
- [CorrelaterWrapper](#correlaterwrapper)
  - [SplitToChunksCorrelaterWrapper\<T>](#splittochunkscorrelaterwrapper)
- [OnProgressUpdate Event](#onprogressupdate-event)

## Usage

**Exsample1:** Comparing two collections. The example compares arrays of strings, but you can really compare collections of any type as longs as the type is nullable or a char.

```CSharp
int removalCost = 1, insertionCost = 1;

// You'll need to implement your own IDistanceCalculator<T>. 
// IDistanceCalculator defines the "distance" between any to elements.
IDistanceCalculator<string> distanceCalculator = new MyDistanceCalculator<string>();

// The library contains a number of ICorrelaters. 
// LevenshteinCorrelater uses dynamic programing to find the Levenshtein-distance between the two collections.
ICorrelater<string> correlater = 
    new LevenshteinCorrelater<string>(distanceCalculator, removalCost, insertionCost);

// Wrap the ICorrelater with an EnumerableCorrelater<T> to use it to compare collections
EnumerableCorrelater<string> enumerableCorrelater = new EnumerableCorrelater<string>(correlater);

string[] array1 = { "A", "D", "C" };
string[] array2 = { "A", "B", "C" };

// Compare the collections - you can compare any IEnumerable<T>.
CorrelaterResult<string> result = enumerableCorrelater.Correlate(array1, array2);

// Print some of the result
Console.WriteLine(result.Distance);
Console.WriteLine(result.BestMatch1);
Console.WriteLine(result.BestMatch2);
```

**Exsample2:** Comparing 2 strings. StringCorrelater treats the string as an array of chars, and therefore uses an ICalculator\<char>.

```CSharp
int removalCost = 1, insertionCost = 1;

// You'll need to implement your own IDistanceCalculator<char>. 
// IDistanceCalculator defines the "distance" between any to elements.
IDistanceCalculator<char> distanceCalculator = new MyDistanceCalculator<char>();

// The library contains a number of ICorrelaters. 
// LevenshteinCorrelater uses dynamic programing to find the Levenshtein-distance between the two collections.
ICorrelater<char> correlater = 
    new LevenshteinCorrelater<char>(distanceCalculator, removalCost, insertionCost);
	
// Wrap the ICorrelater with a StringCorrelater to use it to compare strings
StringCorrelater stringCorrelater = new StringCorrelater(correlater);

string string1 = "ABC";
string string2 = "ADC";

// Compare the strings.
CorrelaterResult<char> result = stringCorrelater.Correlate(string1, string2);

// Print some of the result
Console.WriteLine(result.Distance);
Console.WriteLine(result.BestMatch1);
Console.WriteLine(result.BestMatch2);
```

### Impotent Notes

- Correlaters are not thread safe.
- All costs must be positive. This won't be enforced by the library, but negative costs can results in odd and unexpected behavior.

### A Sample Implementation of an IDistanceCalculator\<char>

```CSharp
/// <summary>
/// An implantation of an IDistanceCalculator&lt;char&gt;
/// </summary>
class CharDistanceCalculator : IDistanceCalculator<char>
{
    private const int DEFAULT_DISTANCE = 20;
    // We'll use a dictionary that will hold the distances between different pairs
    private readonly Dictionary<Tuple<char, char>, int> distance = new Dictionary<Tuple<char, char>, int>()
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

    public int Distance(char element1, char element2)
    {
	// If the elements are equal, they should return a distance of 0
	if (element1.Equals(element2))
            return 0;
	
        var tuple = new Tuple<char, char>(element1, element2);
        if (distance.ContainsKey(tuple))
            return distance[tuple];

        // For any distances not in the dictionary, we'll return a default distance.
        return DEFAULT_DISTANCE;
    }
}
```

## Correlaters

### LevenshteinCorrelater

[LevenshteinCorrelater\<T>](IEnumerableCorrelater/Correlaters/LevenshteinCorrelater.cs) Finds the [LevenshteinDistance](https://en.wikipedia.org/wiki/Levenshtein_distance) and best correlation between two collections. 

### DamerauLevenshteinCorrelater

[DamerauLevenshteinCorrelater\<T>](IEnumerableCorrelater/Correlaters/DamerauLevenshteinCorrelater.cs) Finds the [DamerauLevenshteinDistance](https://en.wikipedia.org/wiki/Damerau%E2%80%93Levenshtein_distance) and best correlation between two collections. 

## CorrelaterWrappers

There are a number of ICorrelater\<T> that can be wrapped around the [base correlaters](#correlaters).
These wrappers can increase the base correlater's performance, or add other abilities.

### SplitToChunksCorrelaterWrapper

This wrapper splits the collection into smaller chunks, and correlates each chunk individually, thus reducing correlation time and memory consumption.
Since the correlates typically have a time and memory complexity of O(n\*m), where n and m are the size of the collection being correlated,
reducing the collections size can have a big impact on performance.

Please note that SplitToChunksCorrelaterWrapper will reduce your correlation's accuracy. 

```CSharp
int removalCost = 7;
int insertionCost = 7;
int missmatchCost = 10;
int chunkSize = 200; // Bigger chunks will result in a slower, but more acurate correlation
ICorrelater<char> innerCorrelater = 
    new LevenshteinCorrelater<char>(missmatchCost, removalCost, insertionCost);

// The SplitToChunksCorrelaterWrapper wrappes an inner ICorrelater
ICorrelater<char> optimizedCorrelater =
    new SplitToChunksCorrelaterWrapper<char>(innerCorrelater, chunkSize);

// Wrap the ICorrelater with an EnumerableCorrelater<T> to use it to compare collections
EnumerableCorrelater<char> enumerableCorrelater =
    new EnumerableCorrelater<char>(optimizedCorrelater);

CorrelaterResult<char> result = enumerableCorrelater.Correlate(collection1, collection2);
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

EnumerableCorrelater<string> enumerableCorrelater = new EnumerableCorrelater<string>(correlater);
CorrelaterResult<string> result = enumerableCorrelater.Correlate(collection1, collection2);
```
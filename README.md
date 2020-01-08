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

### A Sample Implemantation of an IDistanceCalculator\<char>

```CSharp
/// <summary>
/// An implimantation of an IDistanceCalculator&lt;char&gt;
/// </summary>
class CharDistanceCalculator : IDistanceCalculator<char>
{
    private const int DEFAULT_DISTANCE = 20;
    // We'll use a dictionary that will hold the distances between diffrent pairs
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
        var tuple = new Tuple<char, char>(element1, element2);
        if (distance.ContainsKey(tuple))
            return distance[tuple];

        // For any distances not in the dictinary, we'll return a default distance.
        return DEFAULT_DISTANCE;
    }
}
```
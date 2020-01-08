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

Exsample1: Comparing two collections. The example compares arrays of strings, but you can really compare collections of any type as longs as the type is nullable or a char.

```CSharp
int removalCost = 1, insertionCost = 1;
IDistanceCalculator<string> distanceCalculator = new MyDistanceCalculator<string>();
ICorrelater<string> correlater = 
    new LevenshteinCorrelater<string>(distanceCalculator, removalCost, insertionCost);
EnumerableCorrelater<string> enumerableCorrelater = new EnumerableCorrelater<string>(correlater);

string[] array1 = { "A", "D", "C" };
string[] array2 = { "A", "B", "C" };

CorrelaterResult<string> result = enumerableCorrelater.Correlate(array1, array2);

// Print some of the results
Console.WriteLine(result.Distance);
Console.WriteLine(result.BestMatch1);
Console.WriteLine(result.BestMatch2);
```

Exsample2: Comparing 2 strings. StringCorrelater treats the string as an array of chars, and therefore uses an ICalculator<char>.

```CSharp
int removalCost = 1, insertionCost = 1;
IDistanceCalculator<char> distanceCalculator = new MyDistanceCalculator<char>();
ICorrelater<char> correlater = 
    new LevenshteinCorrelater<char>(distanceCalculator, removalCost, insertionCost);
StringCorrelater stringCorrelater = new StringCorrelater(correlater);

string string1 = "ABC";
string string2 = "ADC";

CorrelaterResult<char> result = stringCorrelater.Correlate(string1, string2);

// Print some of the results
Console.WriteLine(result.Distance);
Console.WriteLine(result.BestMatch1);
Console.WriteLine(result.BestMatch2);
```
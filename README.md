# IEnumerableCompare

EnumerableCompare knows to compare two IEnumerables. It returns the distance between them, and returns two arrays which represent their "best match".
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

```CSharp
int removalCost = 1, insertionCost = 1;
IDistanceCalculator<string> distanceCalculator = new MyDistanceCalculator();
IEnumerableComparer<string> comparer =
    new LevenshteinEnumerableComparer<string>(distanceCalculator, removalCost, insertionCost);

string[] array1 = { "A", "D", "C" };
string[] array2 = { "A", "B", "C" };

CompareResult<string> result = comparer.Compare(array1, array2);

// Print some of the results
Console.WriteLine(result.Distance);
Console.WriteLine(result.BestMatch1);
Console.WriteLine(result.BestMatch1);
```
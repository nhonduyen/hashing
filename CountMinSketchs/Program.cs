using CountMinSketchs;
using Newtonsoft.Json;

Console.WriteLine("Count Min Sketch");
using var streamReader = new StreamReader("fruits.json");
string jsonFruits = streamReader.ReadToEnd();
var fruits = JsonConvert.DeserializeObject<List<string>>(jsonFruits);

double epsilon = 0.01; // Example: 1% error factor
double delta = 0.01; // Example: 99% confidence

var (width, depth) = CountMinSketchSizeCalculator.CalculateWidthAndDepth(epsilon, delta);

var countMinSketch = new CountMinSketch(width, depth);
foreach (var fruit in fruits)
{
    countMinSketch.Add(fruit);
}

foreach (var item in fruits)
{
    var count = countMinSketch.Count(item);
    Console.WriteLine($"Count for {item} : {count}");
}
countMinSketch.PrintMatrix();

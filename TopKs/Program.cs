using CountMinSketchs;
using TopKs;
using Newtonsoft.Json;

Console.WriteLine("Top K items");
using var streamReader = new StreamReader("fruits.json");
string jsonFruits = streamReader.ReadToEnd();
var fruits = JsonConvert.DeserializeObject<List<string>>(jsonFruits);

double epsilon = 0.01; // Example: 1% error factor
double delta = 0.01; // Example: 99% confidence
int k = 5; // top 5 frequently items

var (width, depth) = CountMinSketchSizeCalculator.CalculateWidthAndDepth(epsilon, delta);
var topK = new TopKFrequentElements(k, width, depth);

foreach (var fruit in fruits)
{
    topK.Add(fruit);
}

var topKFruits = topK.GetTopK();
Console.WriteLine($"Top {k} fruits: ");
foreach (var item in topKFruits)
{
    Console.WriteLine(item);
}


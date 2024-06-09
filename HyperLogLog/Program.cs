using HyperLogLog;
using Newtonsoft.Json;

Console.WriteLine("Hyper Log Log");
using var streamReader = new StreamReader("fruits.json");
string jsonFruits = streamReader.ReadToEnd();
var fruits = JsonConvert.DeserializeObject<List<string>>(jsonFruits);

var hll = new HLL(10); // Use 10 bits for indexing (2^10 = 1024 registers)

foreach (var fruit in fruits)
{
    hll.Add(fruit);
}

var estimate = hll.Estimate();
Console.WriteLine($"Estimate number of distinct elements: {estimate}");



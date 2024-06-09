using BloomFilter;
using Newtonsoft.Json;

Console.WriteLine("Bloom Filter");
Console.WriteLine("Read list of fruits");
using var streamReader = new StreamReader("fruits.json");
string jsonFruits = streamReader.ReadToEnd();
var fruits = JsonConvert.DeserializeObject<List<string>>(jsonFruits);

using var streamReader1 = new StreamReader("randomfruits.json");
string jsonRandomFruits = streamReader1.ReadToEnd();
var randomFruits = JsonConvert.DeserializeObject<List<string>>(jsonRandomFruits);

int expectedNumberOfElements = 1000; // Example: 1000 elements stored in bloom filer
double desiredFalsePositiveRate = 0.01; // Example: 1% false positive rate
var (bitArraySize, hashFunctionCount) = BloomFilterHelper.CalculateSizeAndHashCount(expectedNumberOfElements, desiredFalsePositiveRate);

var bloomFilter = new BloomFilters(hashFunctionCount, bitArraySize);
foreach (var fruit in fruits)
{
    Console.WriteLine(fruit);
    bloomFilter.Add(fruit);
}
var validFruits = fruits.Select(f => f.ToString()).Take(5).ToArray();
foreach (var item in validFruits)
{
    var mightInFruitList = bloomFilter.MightContains(item);
    Console.WriteLine($"{item} exists in the fruit list? {mightInFruitList}");
}

foreach (var item in randomFruits)
{
    var mightInFruitList = bloomFilter.MightContains(item);
    Console.WriteLine($"{item} exists in the fruit list? {mightInFruitList}");
}
bloomFilter.PrintBitArray();
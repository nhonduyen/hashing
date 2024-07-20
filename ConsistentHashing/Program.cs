// See https://aka.ms/new-console-template for more information
using ConsistentHashing;

Console.WriteLine("ConsistentHashing!");

ConsistentHash<string> ch = new ConsistentHash<string>(3);

Console.WriteLine("Adding Server1, Server2, Server3");
ch.AddNode("Server1");
ch.AddNode("Server2");
ch.AddNode("Server3");

Console.WriteLine("\nCurrent circle:");
ch.PrintCircle();

Console.WriteLine("\nDistribution of 1000 keys:");
PrintDistribution(ch.GetDistribution(1000));

Console.WriteLine("\nAdding Server4");
ch.AddNode("Server4");

Console.WriteLine("\nUpdated circle:");
ch.PrintCircle();

Console.WriteLine("\nUpdated distribution of 1000 keys:");
PrintDistribution(ch.GetDistribution(1000));

static void PrintDistribution(Dictionary<string, int> distribution)
{
    foreach (var kvp in distribution.OrderBy(x => x.Key))
    {
        Console.WriteLine($"{kvp.Key}: {kvp.Value} keys");
    }
}
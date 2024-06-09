using CountMinSketchs;

namespace TopKs
{
    public class TopKFrequentElements
    {
        private readonly int k;
        private readonly CountMinSketch cms;
        private readonly Dictionary<string, int> frequencyMap;
        private readonly SortedSet<(int count, string item)> minHeap;

        public TopKFrequentElements(int k, int width, int depth)
        {
            this.k = k;
            this.cms = new CountMinSketch(width, depth);
            this.frequencyMap = new Dictionary<string, int>();
            this.minHeap = new SortedSet<(int count, string item)>(Comparer<(int, string)>.Create((x, y) => x.Item1 == y.Item1 ? x.Item2.CompareTo(y.Item2) : x.Item1.CompareTo(y.Item1)));
        }

        public void Add(string item)
        {
            cms.Add(item);
            int count = cms.Count(item);

            if (!frequencyMap.ContainsKey(item))
            {
                frequencyMap[item] = count;
                minHeap.Add((count, item));
            }
            else
            {
                minHeap.Remove((frequencyMap[item], item));
                frequencyMap[item] = count;
                minHeap.Add((count, item));
            }

            if (minHeap.Count > k)
            {
                var minElement = minHeap.First();
                minHeap.Remove(minElement);
                frequencyMap.Remove(minElement.item);
            }
        }

        public List<string> GetTopK()
        {
            return minHeap.OrderByDescending(x => x.count).Select(x => x.item).ToList();
        }
    }
}

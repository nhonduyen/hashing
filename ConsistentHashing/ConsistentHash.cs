using System.Security.Cryptography;
using System.Text;

namespace ConsistentHashing
{
    public class ConsistentHash<T>
    {
        private SortedDictionary<int, T> circle = new SortedDictionary<int, T>();
        private int replicas;
        private HashSet<T> nodes = new HashSet<T>();

        public ConsistentHash(int replicas)
        {
            this.replicas = replicas;
        }

        public void AddNode(T node)
        {
            if (nodes.Add(node))
            {
                for (int i = 0; i < replicas; i++)
                {
                    int hash = GetHash($"{node}:{i}");
                    circle[hash] = node;
                }
            }
        }

        public void RemoveNode(T node)
        {
            if (nodes.Remove(node))
            {
                for (int i = 0; i < replicas; i++)
                {
                    int hash = GetHash($"{node}:{i}");
                    circle.Remove(hash);
                }
            }
        }

        public T GetNode(string key)
        {
            if (circle.Count == 0)
                return default(T);

            int hash = GetHash(key);

            if (!circle.ContainsKey(hash))
            {
                var keys = new int[circle.Keys.Count];
                circle.Keys.CopyTo(keys, 0);
                Array.Sort(keys);
                for (int i = 0; i < keys.Length; i++)
                {
                    if (keys[i] > hash)
                        return circle[keys[i]];
                }
                return circle[keys[0]];
            }

            return circle[hash];
        }

        private int GetHash(string key)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
                return BitConverter.ToInt32(hash, 0);
            }
        }

        public void PrintCircle()
        {
            foreach (var node in nodes)
            {
                Console.WriteLine($"Node: {node}");
                for (int i = 0; i < replicas; i++)
                {
                    int hash = GetHash($"{node}:{i}");
                    Console.WriteLine($"  Replica {i}: {hash}");
                }
            }
        }

        public Dictionary<T, int> GetDistribution(int numKeys)
        {
            var distribution = new Dictionary<T, int>();
            for (int i = 0; i < numKeys; i++)
            {
                T node = GetNode($"Key{i}");
                if (!distribution.ContainsKey(node))
                    distribution[node] = 0;
                distribution[node]++;
            }
            return distribution;
        }
    }
}

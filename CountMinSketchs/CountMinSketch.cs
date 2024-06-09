using System.Security.Cryptography;
using System.Text;

namespace CountMinSketchs
{
    public class CountMinSketch
    {
        private readonly int[,] _table;
        private readonly int _depth;
        private readonly int _width;
        private readonly HashAlgorithm[] _hashAlgorithms;

        public CountMinSketch(int width, int depth)
        {
            _width = width;
            _depth = depth;
            _table = new int[depth, width];
            _hashAlgorithms = new HashAlgorithm[depth];

            for (int i = 0; i < depth; i++)
            {
                _hashAlgorithms[i] = MD5.Create();
            }
        }

        private int ComputeHash(HashAlgorithm algorithm, string item, int seed)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(item + seed);
            byte[] hash = algorithm.ComputeHash(bytes);
            int result = BitConverter.ToInt32(hash, 0);
            return Math.Abs(result % _width);
        }

        public void Add(string item)
        {
            for (int i = 0; i < _depth; i++)
            {
                int hash = ComputeHash(_hashAlgorithms[i], item, i);
                _table[i, hash]++;
            }
        }

        public int Count(string item)
        {
            int minCount = int.MaxValue;
            for (int i = 0; i < _depth; i++)
            {
                int hash = ComputeHash(_hashAlgorithms[i], item, i);
                minCount = Math.Min(minCount, _table[i, hash]);
            }
            return minCount;
        }

        public void PrintMatrix()
        {
            for (int i = 0; i < _depth; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    Console.Write(_table[i, j] + " ");
                }
                Console.WriteLine(Environment.NewLine);
            }
        }
    }
}

using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace BloomFilter
{
    public class BloomFilters
    {
        private readonly BitArray _bitArray;
        private readonly int _hashCount;
        private readonly int _size;
        private readonly List<HashAlgorithm> _hashAlgorithms;

        public BloomFilters(int hashCount, int size)
        {
            _hashCount = hashCount;
            _size = size;
            _bitArray = new BitArray(size);
            _hashAlgorithms = new List<HashAlgorithm>();
            for (int i = 0; i < hashCount; i++)
            {
                _hashAlgorithms.Add(MD5.Create());
            }
        }

        private int ComputeHash(HashAlgorithm algo, string item, int seed)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(item + seed);
            byte[] hash = algo.ComputeHash(bytes);
            int result = BitConverter.ToInt32(hash, 0);
            return Math.Abs(result % _size);
        }

        public void Add(string item)
        {
            foreach (var algorithm in _hashAlgorithms)
            {
                int position = ComputeHash(algorithm, item, _hashAlgorithms.IndexOf(algorithm));
                _bitArray[position] = true;
            }
        }

        public bool MightContains(string item)
        {
            foreach (var algorithm in _hashAlgorithms)
            {
                int position = ComputeHash(algorithm, item, _hashAlgorithms.IndexOf(algorithm));
                if (!_bitArray[position])
                {
                    return false;
                }
            }
            return true;
        }

        public void PrintBitArray()
        {
            for (int i = 0; i < _size; i++)
            {
                Console.Write($"{_bitArray[i]} ");
            }
        }
    }
}

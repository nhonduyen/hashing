namespace BloomFilter
{
    public static class BloomFilterHelper
    {
        public static (int bitArraySize, int hashFunctionCount) CalculateSizeAndHashCount(int n, double p)
        {
            int m = (int)Math.Ceiling(-n * Math.Log(p) / Math.Pow(Math.Log(2), 2));
            int k = (int)Math.Ceiling((m / (double)n) * Math.Log(2));
            return (m, k);
        }
    }
}

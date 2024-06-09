using System.Security.Cryptography;
using System.Text;

namespace HyperLogLog
{
    public class HLL
    {
        private readonly int[] registers;
        private readonly int m;
        private readonly double alphaMM;

        public HLL(int b)
        {
            if (b < 4 || b > 16)
            {
                throw new ArgumentException("b must be between 4 and 16");
            }

            m = 1 << b; // m = 2^b, number of registers
            registers = new int[m];

            // Calculate alphaMM using the bias correction formula for HyperLogLog
            alphaMM = (0.7213 / (1 + 1.079 / m)) * m * m;
        }

        private int Hash(string item)
        {
            using (var hashAlgorithm = MD5.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(item);
                byte[] hash = hashAlgorithm.ComputeHash(bytes);
                return BitConverter.ToInt32(hash, 0);
            }
        }

        public void Add(string item)
        {
            int hash = Hash(item);
            int registerIndex = hash & (m - 1); // Determine the register index
            int rank = 1;
            int w = hash >> (32 - (int)Math.Log2(m)); // Extract the relevant bits for rank computation

            // Count the leading zeros in the remaining bits
            while ((w & 1) == 0 && rank <= 32)
            {
                rank++;
                w >>= 1;
            }

            // Update the register with the maximum rank
            registers[registerIndex] = Math.Max(registers[registerIndex], rank);
        }

        public double Estimate()
        {
            double sum = 0.0;

            // Calculate the harmonic mean of the register values
            foreach (int register in registers)
            {
                sum += 1.0 / (1 << register);
            }

            double estimate = alphaMM / sum;

            // Apply correction for small cardinalities
            if (estimate <= 2.5 * m)
            {
                int zeros = 0;
                foreach (int register in registers)
                {
                    if (register == 0)
                    {
                        zeros++;
                    }
                }
                if (zeros != 0)
                {
                    estimate = m * Math.Log((double)m / zeros);
                }
            }
            // Apply correction for large cardinalities
            else if (estimate > (1 << 32) / 30.0)
            {
                estimate = -(1 << 32) * Math.Log(1 - estimate / (1 << 32));
            }

            return estimate;
        }
    }
}

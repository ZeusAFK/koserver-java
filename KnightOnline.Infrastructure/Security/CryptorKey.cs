using System;
using System.Numerics; // For BigInteger
using System.Security.Cryptography; // For RandomNumberGenerator

namespace KnightOnline.Infrastructure.Security
{
    public static class CryptorKey
    {
        public static BigInteger GenerateKey()
        {
            // Java's BigInteger(64, new Random(System.nanoTime())) creates a positive BigInteger
            // that is probably prime with a specified bit length.
            // .NET's BigInteger doesn't have a direct constructor like that for probable primes or fixed bit length randoms.
            // We need a positive 64-bit number that results in a 20-digit decimal string.
            // A 64-bit number's decimal representation can be up to 20 digits.
            // Let's generate a random 8-byte array and convert to a positive BigInteger.

            byte[] bytes = new byte[8]; // 64 bits
            BigInteger n;

            do
            {
                RandomNumberGenerator.Fill(bytes);
                // Ensure it's positive: if the highest bit is 1, BigInteger interprets it as negative.
                // We can add a leading zero byte to ensure positivity if converting from big-endian byte array.
                // Or, ensure the last byte's highest bit is 0, or simply take Abs value.
                // For simplicity and to ensure it's always positive and using all 64 bits potentially for magnitude:
                n = new BigInteger(bytes, isUnsigned: true, isBigEndian: false); // Treat as unsigned, little-endian bytes
            }
            // Java code loops until n.toString().length() == 20.
            // This is a peculiar requirement. Max value for ulong (64-bit unsigned) is 18,446,744,073,709,551,615 (20 digits).
            // Min 20-digit number is 10^19.
            while (n.ToString().Length != 20);

            return n;
        }
    }
}

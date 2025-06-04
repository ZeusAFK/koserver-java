using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text; // For ToHexString, if not using HexDump class

namespace KnightOnline.Infrastructure.Shared.Utils
{
    public static class ByteUtils
    {
        public static byte[] CompressGZip(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            using (var outputStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    gzipStream.Write(data, 0, data.Length);
                }
                return outputStream.ToArray();
            }
        }

        public static byte[] DecompressGZip(byte[] compressedData)
        {
            if (compressedData == null) throw new ArgumentNullException(nameof(compressedData));
            using (var inputStream = new MemoryStream(compressedData))
            using (var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            using (var outputStream = new MemoryStream())
            {
                gzipStream.CopyTo(outputStream);
                return outputStream.ToArray();
            }
        }

        public static byte[] ComputeSHA1Hash(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            using (SHA1 sha1 = SHA1.Create())
            {
                return sha1.ComputeHash(data);
            }
        }

        // Optional: Helper to get SHA1 hash as a hex string, similar to Java's ArrayUtils.SHAsum
        public static string ComputeSHA1HashString(byte[] data)
        {
            byte[] hashBytes = ComputeSHA1Hash(data);
            // return Convert.ToHexString(hashBytes); // .NET 5+
            // Or use a similar method as HexDump if .NET Framework compatibility was a concern
            // For this project, Convert.ToHexString is fine.
            return Convert.ToHexString(hashBytes).ToLowerInvariant(); // Java version was uppercase, but often lowercase is preferred for SHA strings. Let's go with lowercase.
        }
    }
}

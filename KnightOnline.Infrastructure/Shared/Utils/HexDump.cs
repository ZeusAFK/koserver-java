using System;
using System.Text;
using System.Globalization; // For ParseHex

namespace KnightOnline.Infrastructure.Shared.Utils
{
    public static class HexDump
    {
        private static readonly char[] HexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        public static string DumpHexString(byte[] array)
        {
            if (array == null) return string.Empty;
            return DumpHexString(array, 0, array.Length);
        }

        public static string DumpHexString(byte[] array, int offset, int length)
        {
            if (array == null) return string.Empty;
            if (offset < 0 || offset > array.Length || length < 0 || (offset + length) > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset) + " or " + nameof(length));
            }

            StringBuilder result = new StringBuilder();
            byte[] line = new byte[16];
            int lineIndex = 0;

            result.Append(Environment.NewLine); // Start with a newline like the original
            result.Append("0x");
            result.Append(ToHexString(offset)); // Initial offset

            for (int i = 0; i < length; i++)
            {
                if (lineIndex == 16)
                {
                    result.Append("  "); // Space before ASCII representation

                    for (int j = 0; j < 16; j++)
                    {
                        if (line[j] > 32 && line[j] < 127) // Printable ASCII
                        {
                            result.Append((char)line[j]);
                        }
                        else
                        {
                            result.Append(".");
                        }
                    }
                    result.Append(Environment.NewLine); // Newline after ASCII
                    result.Append("0x");
                    result.Append(ToHexString(offset + i)); // Current offset
                    lineIndex = 0;
                }

                byte b = array[offset + i];
                result.Append(" ");
                result.Append(HexDigits[(b >> 4) & 0x0F]);
                result.Append(HexDigits[b & 0x0F]);

                line[lineIndex++] = b;
            }

            // Append remaining ASCII for the last line
            if (lineIndex != 0) // If there's anything in the current line
            {
                int count = (16 - lineIndex) * 3; // Spaces for formatting
                count++; // Extra space before ASCII
                for (int k = 0; k < count; k++)
                {
                    result.Append(" ");
                }

                for (int j = 0; j < lineIndex; j++)
                {
                    if (line[j] > 32 && line[j] < 127)
                    {
                        result.Append((char)line[j]);
                    }
                    else
                    {
                        result.Append(".");
                    }
                }
            }
            result.Append(Environment.NewLine); // Final newline
            return result.ToString();
        }

        public static string ToHexString(byte b)
        {
            return Convert.ToHexString(new[] { b });
        }

        public static string ToHexString(byte[] array)
        {
            if (array == null) return string.Empty;
            return Convert.ToHexString(array);
        }

        public static string ToHexString(byte[] array, int offset, int length)
        {
            if (array == null) return string.Empty;
             if (offset < 0 || offset > array.Length || length < 0 || (offset + length) > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset) + " or " + nameof(length));
            }
            return Convert.ToHexString(array, offset, length);
        }

        public static string ToHexString(int i)
        {
            // Original Java code converts int to big-endian byte array.
            // .NET's BitConverter is little-endian by default.
            // Convert.ToHexString expects a byte array.
            byte[] array = BitConverter.GetBytes(i);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(array); // Convert to big-endian to match Java
            }
            return Convert.ToHexString(array);
        }
        
        // The Java toByteArray(byte b) and toByteArray(int i) are not strictly needed
        // as Convert.ToHexString takes byte[] directly and BitConverter handles int to byte[].

        public static byte[] HexStringToByteArray(string hexString)
        {
            if (string.IsNullOrEmpty(hexString)) return Array.Empty<byte>();
            if (hexString.Length % 2 != 0) throw new ArgumentException("Hex string must have an even number of characters.", nameof(hexString));

            return Convert.FromHexString(hexString);
        }
    }
}

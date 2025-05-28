using System;
using System.Linq;
using System.Text;

namespace KnightOnline.Infrastructure.Shared.Utils
{
    public static class PrintUtils
    {
        public static void PrintSection(string sectionName)
        {
            if (sectionName == null) sectionName = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append("-[ ");
            sb.Append(sectionName);
            sb.Append(" ]");

            // In C#, string.PadLeft can achieve this more easily,
            // but the original Java code uses a loop with insert.
            // Let's try to match the original intent for length calculation,
            // then simplify if the padding logic is trivial.
            // Target length is 79.
            const int targetLength = 79;
            while (sb.Length < targetLength)
            {
                // Original Java prepends "=".
                // A more direct C# way for fixed length padding:
                // sb.Insert(0, "=");
                // However, if the goal is just to fill with '=', PadLeft is better.
                // For now, let's stick to the loop to be closer, then consider refactor.
                 if (sb.Length < targetLength) // Ensure not to over-pad if sectionName is very long
                 {
                    sb.Insert(0, "=");
                 } else {
                    break; // Should not happen if targetLength is reasonable
                 }

            }
             // If sectionName was too long, it might exceed 79. The Java code doesn't explicitly handle this.
             // The C# StringBuilder will grow.
             // A more robust C# way would be:
             // string title = $"-[ {sectionName} ]-";
             // Console.WriteLine(title.PadLeft(targetLength - title.Length + title.Length / 2 + sectionName.Length / 2, '=').PadRight(targetLength, '='));
             // But let's keep it simple and closer to original for now.
             // The original logic seems to ensure the '=-[ section ]' part is padded with '=' on the left up to 79 chars.

            Console.WriteLine(sb.ToString());
        }

        public static string ReverseHexString(string hexString)
        {
            if (string.IsNullOrEmpty(hexString))
            {
                return string.Empty;
            }
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException("Hex string must have an even number of characters.", nameof(hexString));
            }

            int chunkCount = hexString.Length / 2;
            string[] chunks = new string[chunkCount];
            for (int i = 0; i < chunkCount; i++)
            {
                chunks[i] = hexString.Substring(i * 2, 2);
            }

            Array.Reverse(chunks);
            return string.Join(string.Empty, chunks);
        }
    }
}

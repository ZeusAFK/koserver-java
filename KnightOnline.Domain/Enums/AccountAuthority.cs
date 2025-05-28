using System;

namespace KnightOnline.Domain.Enums
{
    public enum AccountAuthority : byte // Or int, if values can be larger or negative
    {
        BANNED = 0, // Original Java was BANNED(0)
        NORMAL = 1  // Original Java was NORMAL(1)
        // Add other authority levels here if they exist or are planned
    }

    public static class AccountAuthorityExtensions
    {
        // Optional: A helper method similar to Java's 'from(int value)'
        // This provides type safety over direct casting if desired.
        public static AccountAuthority FromValue(int value)
        {
            if (Enum.IsDefined(typeof(AccountAuthority), (byte)value))
            {
                return (AccountAuthority)value;
            }
            // Or throw an ArgumentOutOfRangeException, or return a default
            throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not defined for AccountAuthority.");
            // Alternatively, for a non-throwing version:
            // return Enum.IsDefined(typeof(AccountAuthority), (byte)value) ? (AccountAuthority)value : default(AccountAuthority); // e.g., BANNED
        }

        // Optional: A helper to get the integer value, though direct casting works (e.g., (int)authority)
        public static int GetValue(this AccountAuthority authority)
        {
            return (int)authority;
        }
    }
}

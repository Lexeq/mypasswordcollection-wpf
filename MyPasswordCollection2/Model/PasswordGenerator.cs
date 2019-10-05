using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPC.Model
{
    [Flags]
    public enum CharSet
    {
        UppercaseLetters = 0b0001,
        LowercaseLetters = 0b0010,
        AllLetters = 0b0011,
        Digits = 0b0100,
        Symbols = 0b1000,
        All = 0b1111
    }

    public class PasswordGenerator
    {
        private static readonly Random random = new Random();

        private readonly char[] digits = "0123456789".ToCharArray();
        private readonly char[] lowercaseLetters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        private readonly char[] uppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private readonly char[] symbols = @"!@#$%^&*()_+-=[]\{}|;':""/.,<>?".ToCharArray();

        public string Generate(CharSet set, int length)
        {
            var alphabet = new List<char>();
            if (set.HasFlag(CharSet.Digits))
            {
                alphabet.AddRange(digits);
            }

            if (set.HasFlag(CharSet.LowercaseLetters))
            {
                alphabet.AddRange(lowercaseLetters);
            }
            if (set.HasFlag(CharSet.UppercaseLetters))
            {
                alphabet.AddRange(uppercaseLetters);
            }
            if (set.HasFlag(CharSet.Symbols))
            {
                alphabet.AddRange(symbols);
            }

            var password = new char[length];
            for (int i = 0; i < length; i++)
            {
                password[i] = alphabet[random.Next(alphabet.Count)];
            }
            return new string(password);
        }
    }
}

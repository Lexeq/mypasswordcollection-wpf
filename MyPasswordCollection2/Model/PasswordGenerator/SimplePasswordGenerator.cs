using System;
using System.Collections.Generic;

namespace MPC.Model
{
    public class SimplePasswordGenerator : IPasswordGenerator
    {
        private static Random Random { get; } = new Random();

        public  string Generate(CharSets set, int length)
        {
            var alphabet = new List<char>();

            if (set.HasFlag(CharSets.Digits))
            {
                alphabet.AddRange(AlphabetSource.GetDigits());
            }
            if (set.HasFlag(CharSets.LowercaseLetters))
            {
                alphabet.AddRange(AlphabetSource.GetLowercaseLetters());
            }
            if (set.HasFlag(CharSets.UppercaseLetters))
            {
                alphabet.AddRange(AlphabetSource.GetUppercaseLetters());
            }
            if (set.HasFlag(CharSets.Symbols))
            {
                alphabet.AddRange(AlphabetSource.GetSymbols());
            }

            var password = new char[length];
            for (int i = 0; i < length; i++)
            {
                password[i] = alphabet[Random.Next(alphabet.Count)];
            }

            return new string(password);
        }
    }
}

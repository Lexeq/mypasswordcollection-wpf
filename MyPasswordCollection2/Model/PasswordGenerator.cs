using System;
using System.Collections.Generic;

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

    public abstract class PasswordGeneratorBase
    {
        protected static readonly Random random = new Random();

        protected readonly char[] digits = "0123456789".ToCharArray();
        protected readonly char[] lowercaseLetters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        protected readonly char[] uppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        protected readonly char[] symbols = @"!@#$%^&*()_+-=[]\{}|;':""/.,<>?".ToCharArray();

        public abstract string Generate(CharSet set, int length);
    }
    public class SimplePasswordGenerator : PasswordGeneratorBase
    {
        public override string Generate(CharSet set, int length)
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

    public class PasswordGenerator : PasswordGeneratorBase
    {
        public override string Generate(CharSet set, int length)
        {
            if (length < 1)
                throw new ArgumentException("Password length to small.", nameof(length));

            bool first = true;
            var password = new char[length];

            if (set.HasFlag(CharSet.LowercaseLetters))
            {
                var perc = first ? 100 : 50;
                first = false;
                Fill(password, lowercaseLetters, perc);
            }
            if (set.HasFlag(CharSet.UppercaseLetters))
            {
                var perc = first ? 100 : 50;
                first = false;
                Fill(password, uppercaseLetters, perc);
            }

            if (set.HasFlag(CharSet.Digits))
            {
                var perc = first ? 100 : 25;
                first = false;
                Fill(password, digits, perc);

            }

            if (set.HasFlag(CharSet.Symbols))
            {
                var perc = first ? 100 : 20;
                first = false;
                Fill(password, symbols, perc);
            }
            if (first)
                throw new ArgumentException("No char set choosen.", nameof(set));

            return new string(password);
        }

        private void Fill(char[] array, char[] alphabet, int chance)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (random.Next(0, 100) < chance)
                {
                    array[i] = alphabet[random.Next(alphabet.Length)];
                }
            }
        }

        
    }
}

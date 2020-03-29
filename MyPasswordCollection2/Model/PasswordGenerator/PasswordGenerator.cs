using System;
using System.Collections.Generic;
using System.Linq;

namespace MPC.Model
{

    public class PasswordGenerator : IPasswordGenerator
    {
        private static Random Random { get; } = new Random();

        public  string Generate(CharSets set, int length)
        {
            if (length < 1) 
                throw new ArgumentException("Password length to small.", nameof(length));

            bool startWithLetter = true;
            var password = new char[length];

            if (set.HasFlag(CharSets.LowercaseLetters))
            {
                var perc = startWithLetter ? 100 : 50;
                startWithLetter = false;
                Fill(password, AlphabetSource.GetLowercaseLetters(), perc);
            }
            if (set.HasFlag(CharSets.UppercaseLetters))
            {
                var perc = startWithLetter ? 100 : 50;
                startWithLetter = false;
                Fill(password, AlphabetSource.GetUppercaseLetters(), perc);
            }

            if (set.HasFlag(CharSets.Digits))
            {
                var perc = startWithLetter ? 100 : 25;
                startWithLetter = false;
                Fill(password, AlphabetSource.GetDigits(), perc);
            }

            if (set.HasFlag(CharSets.Symbols))
            {
                var perc = startWithLetter ? 100 : 20;
                startWithLetter = false;
                Fill(password, AlphabetSource.GetSymbols(), perc);
            }
            if (startWithLetter)
                throw new ArgumentException("No char set choosen.", nameof(set));

            return new string(password);
        }

        private void Fill(char[] array, char[] alphabet, int chance)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (Random.Next(0, 100) < chance)
                {
                    array[i] = alphabet[Random.Next(alphabet.Length)];
                }
            }
        }

        
    }
}

using System;
using System.Collections.Generic;

namespace MPC.Model
{
    internal static class AlphabetSource
    {
        public static char[] GetDigits() => "0123456789".ToCharArray();

        public static char[] GetLowercaseLetters() => "abcdefghijklmnopqrstuvwxyz".ToCharArray();

        public static char[] GetUppercaseLetters() => "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        public static char[] GetSymbols() => @"!@#$%^&*()_+-=[]\{}|;':""/.,<>?".ToCharArray();

    }
}

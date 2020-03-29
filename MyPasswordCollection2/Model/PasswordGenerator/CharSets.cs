using System;

namespace MPC.Model
{
    [Flags]
    public enum CharSets
    {
        UppercaseLetters = 0b0001,
        LowercaseLetters = 0b0010,
        AllLetters = 0b0011,
        Digits = 0b0100,
        Symbols = 0b1000,
        All = 0b1111
    }
}

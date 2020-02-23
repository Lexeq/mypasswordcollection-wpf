using MPC.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MPC.ViewModels
{
    class TextService : IStringsSource
    {
        private readonly IDictionary dictionary;

        public TextService(IDictionary dictionary)
        {
            this.dictionary = dictionary;
        }

        public string GetString(UIStrings key, IDictionary<string, string> variables)
        {
            if (variables == null)
                throw new ArgumentNullException(nameof(variables));

            return SetVariables(GetString(key), variables);
        }

        public string GetString(UIStrings key)
        {
            return Resolve(ConvertEnumToKey(key));
        }

        private string Resolve(string key)
        {
            return dictionary[key]?.ToString() ?? key;
        }

        private string ConvertEnumToKey(UIStrings value)
        {
            return "msg." + value.ToString();
        }

        private enum States { AddTextChar, SpecialChar, AddKeyChar }

        private string SetVariables(string str, IDictionary<string, string> variables)
        {
            var specialChar = '%';
            var text = new StringBuilder();
            var key = new StringBuilder();
            var state = States.AddTextChar;

            for (int i = 0; i < str.Length; i++)
            {
                switch (state)
                {
                    case States.AddTextChar:
                        if (str[i] == specialChar)
                        {
                            state = States.SpecialChar;
                        }
                        else
                        {
                            text.Append(str[i]);
                        }
                        break;
                    case States.SpecialChar:
                        if (str[i] == '%')
                        {
                            text.Append('%');
                            state = States.AddTextChar;
                        }
                        else
                        {
                            state = States.AddKeyChar;
                            key.Append(str[i]);
                        }
                        break;
                    case States.AddKeyChar:
                        if (str[i] == '%')
                        {
                            state = States.AddTextChar;
                            if (variables.TryGetValue(key.ToString(), out var value))
                            {
                                text.Append(value);
                            }
                            else
                            {
                                text.Append(key);
                            }
                            key.Clear();
                        }
                        else
                        {
                            key.Append(str[i]);
                        }
                        break;
                }
            }
            if (state != States.AddTextChar)
                throw new FormatException($"Bad string. Perhaps closing '{specialChar}' missed.");

            return text.ToString();
        }
    }
}

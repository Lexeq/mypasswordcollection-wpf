using MPC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MPC.ViewModels
{
    class PasswordGenerationViewModel : BaseViewModel
    {
        private bool error;

        public bool HasError
        {
            get { return error; }
            set
            {
                error = value;
                OnPropertyChanged();
            }
        }

        public bool UseDigits
        {
            get => useDigits;
            set
            {
                useDigits = value;
                OnPropertyChanged();
            }
        }

        public bool UseSymbols
        {
            get => useSymbols;
            set
            {
                useSymbols = value;
                OnPropertyChanged();
            }
        }

        public bool UseLetters
        {
            get => useLetters;
            set
            {
                useLetters = value;
                OnPropertyChanged();
            }
        }

        private string generatedPassword;

        public string GeneratedPassword
        {
            get => generatedPassword;
            set
            {
                generatedPassword = value;
                OnPropertyChanged();
            }
        }

        private int passwordLength;

        public int PasswordLength
        {
            get { return passwordLength; }
            set
            {
                passwordLength = value;
                OnPropertyChanged();
            }
        }

        private ICommand generateCommand;
        private bool useDigits;
        private bool useSymbols;
        private bool useLetters;

        public ICommand GenerateCommand
        {
            get { return generateCommand ?? (generateCommand = new Command(GenerateNew)); }
        }

        public PasswordGenerationViewModel()
        {
            UseDigits = true;
            UseLetters = true;
            UseSymbols = true;
            PasswordLength = 8;
        }

        private void GenerateNew()
        {
            CharSet set = 0;
            HasError = false;
            if (UseDigits)
                set |= CharSet.Digits;
            if (UseLetters)
                set |= CharSet.AllLetters;
            if (UseSymbols)
                set |= CharSet.Symbols;
            if (set == 0)
            {
                HasError = true;
                return;
            }
            var gen = new PasswordGenerator();
            GeneratedPassword = gen.Generate(set, PasswordLength);
        }
    }
}

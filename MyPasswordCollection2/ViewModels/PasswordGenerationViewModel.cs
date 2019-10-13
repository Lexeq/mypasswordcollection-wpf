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
        private readonly PasswordGenerator generator = new PasswordGenerator();
        private CharSet sets = 0;

        private bool hasError;

        public bool HasError
        {
            get => hasError;
            set
            {
                hasError = value;
                OnPropertyChanged();
            }
        }

        public bool UseDigits
        {
            get => sets.HasFlag(CharSet.Digits);
            set
            {
                SetFlag(CharSet.Digits, value);
                OnPropertyChanged();
            }
        }

        public bool UseSymbols
        {
            get => sets.HasFlag(CharSet.Symbols);
            set
            {
                SetFlag(CharSet.Symbols, value);
                OnPropertyChanged();
            }
        }

        public bool UseLetters
        {
            get => sets.HasFlag(CharSet.AllLetters);
            set
            {
                SetFlag(CharSet.AllLetters, value);
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

        private void SetFlag(CharSet set, bool isDefined)
        {
            sets = isDefined ? sets | set : sets & ~set;
        }

        private void GenerateNew()
        {
            if (HasError = (sets == 0))
                return;
            GeneratedPassword = generator.Generate(sets, PasswordLength);
        }
    }
}

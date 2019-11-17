using MPC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MPC.ViewModels
{
    class PasswordGenerationViewModel : DataErrorNotifyingViewModel
    {
        private readonly PasswordGenerator generator = new PasswordGenerator();
        private CharSet sets = 0;

        private string errorText;
        private int passwordLength;
        private string generatedPassword;
        private ICommand generateCommand;
        private ICommand copyCommand;

        public PasswordGenerationViewModel()
        {
            UseDigits = true;
            UseLetters = true;
            UseSymbols = true;
            PasswordLength = 8;
        }

        public string ErrorText
        {
            get => errorText;
            set
            {
                errorText = value;
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

        public string GeneratedPassword
        {
            get => generatedPassword;
            set
            {
                generatedPassword = value;
                OnPropertyChanged();
                Validate();
            }
        }

        public int PasswordLength
        {
            get { return passwordLength; }
            set
            {
                passwordLength = value;
                if (value < 1)
                {
                    AddPropertyError("To small");
                    OnPropertyChanged(nameof(ErrorText));
                }

                else
                {
                    ClearPropertyErrors();
                }
                OnPropertyChanged();
            }
        }

        public ICommand GenerateCommand
        {
            get { return generateCommand ?? (generateCommand = new Command(GenerateNew, () => !HasErrors)); }
        }

        public ICommand CopyCommand
        {
            get { return copyCommand ?? (copyCommand = new Command(CopyToClipBoard, () => !string.IsNullOrEmpty(GeneratedPassword))); }
        }

        public void CopyToClipBoard()
        {
            Clipboard.SetText(GeneratedPassword);
        }

        private void SetFlag(CharSet set, bool isDefined)
        {
            sets = isDefined ? sets | set : sets & ~set;
            Validate();
        }

        private void GenerateNew()
        {
            GeneratedPassword = generator.Generate(sets, PasswordLength);
        }

        private void Validate()
        {
            ClearAllErrors();
            if (sets == 0)
                AddPropertyError("No char set selected", nameof(sets));
            if (passwordLength < 1)
                AddPropertyError("Password length to small", nameof(PasswordLength));
            UpdateErrorText();
        }

        private void UpdateErrorText()
        {
            ErrorText = GetErrors(GetPropertiesWithErrors().FirstOrDefault()).Cast<string>().FirstOrDefault();
        }
    }
}

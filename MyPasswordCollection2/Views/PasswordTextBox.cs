using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace MPC.Views
{
    [TemplatePart(Name = "PART_CapsWarning", Type = typeof(UIElement))]
    public class PasswordTextBox : TextBox
    {
        private UIElement capsWarning;

        #region DependencyProperties
        public static readonly DependencyProperty UsePasswordCharProperty =
            DependencyProperty.Register(
                nameof(UsePasswordChar),
                typeof(bool),
                typeof(PasswordTextBox),
                new FrameworkPropertyMetadata(false, OnPropertyChanged)
        );

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                nameof(Password),
                typeof(string),
                typeof(PasswordTextBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));

        public static readonly DependencyProperty PasswordCharProperty =
            DependencyProperty.Register(
                nameof(PasswordChar),
                typeof(char),
                typeof(PasswordTextBox),
                new PropertyMetadata('*', OnPropertyChanged));

        public static readonly DependencyProperty ShowCapsLockWarningProperty =
            DependencyProperty.Register(
                nameof(ShowCapsLockWarning),
                typeof(bool),
                typeof(PasswordTextBox),
                new PropertyMetadata(true));

        public bool ShowCapsLockWarning
        {
            get { return (bool)GetValue(ShowCapsLockWarningProperty); }
            set { SetValue(ShowCapsLockWarningProperty, value); }
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as PasswordTextBox;
            obj?.Update();

        }

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public bool UsePasswordChar
        {
            get { return (bool)GetValue(UsePasswordCharProperty); }
            set { SetValue(UsePasswordCharProperty, value); }
        }

        public char PasswordChar
        {
            get { return (char)GetValue(PasswordCharProperty); }
            set { SetValue(PasswordCharProperty, value); }
        }

        #endregion

        #region Ctor

        static PasswordTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PasswordTextBox),
                new FrameworkPropertyMetadata(typeof(PasswordTextBox)));
        }
        public PasswordTextBox()
        {
            CommandManager.AddPreviewCanExecuteHandler(this, new CanExecuteRoutedEventHandler(CanExecutePreview));
            CommandManager.AddPreviewExecutedHandler(this, new ExecutedRoutedEventHandler(ExecutePreview));
            Loaded += (o, e) => UpdateCapsWarning();
            IsKeyboardFocusedChanged += (o, e) => UpdateCapsWarning();
        }

        private void ExecutePreview(object sender, ExecutedRoutedEventArgs e)
        {
            if(e.Command == ApplicationCommands.Cut)
            {
                Clipboard.SetText(SelectedText);
                RemoveText(true);
                e.Handled = true;
            }
            if(e.Command == ApplicationCommands.Paste)
            {
                AddText(Clipboard.GetText());
                e.Handled = true;
            }
        }

        #endregion

        private void CanExecutePreview(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy || e.Command == ApplicationCommands.Cut)
            {
                e.Handled = UsePasswordChar;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            e.Handled = true;
            if (e.Key == Key.Back)
                RemoveText(true);
            else if (e.Key == Key.Delete)
                RemoveText(false);
            else if (e.Key == Key.CapsLock)
                UpdateCapsWarning();
            else if (e.Key == Key.Space)
                AddText(" ");
            else 
                e.Handled = false;

            base.OnPreviewKeyDown(e);
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)        
        {
            if (e.Text == "\r" || e.Text == '\u001b'.ToString())  //u001b - ESC
                return;
            AddText(e.Text);
            e.Handled = true;
        }

        private void AddText(string text)
        {
            if (IsReadOnly || string.IsNullOrEmpty(text))
                return;

            if (SelectionLength != 0)
                RemoveText(true);
            var carretIndex = CaretIndex;
            Password = Password != null ? Password.Insert(CaretIndex, TakeSingleLine(text)) : text;
            Update(carretIndex + text.Length);
        }

        private string TakeSingleLine(string text)
        {
            int i = text.IndexOfAny(new[] { '\r', '\n' });
            return i < 0 ? text : text.Substring(0, i);
        }

        private void RemoveText(bool back)
        {
            if (IsReadOnly)
                return;

            if (Password == null || Password.Length == 0 ||
                (back == false && CaretIndex == Password.Length) || (back && CaretIndex == 0 && SelectionLength == 0))
                return;

            var carretIndex = CaretIndex;
            if (SelectionLength != 0)
            {
                Password = RemoveRange(Password, SelectionStart, SelectionLength);
                Update(carretIndex);
            }
            else if (back)
            {
                Password = RemoveRange(Password, CaretIndex - 1, 1);
                Update(carretIndex - 1);
            }
            else
            {
                Password = RemoveRange(Password, CaretIndex, 1);
                Update(carretIndex);
            }
        }

        private void Update(int newCaretIndex = -1)
        {
            Text = Password == null ? string.Empty : UsePasswordChar ? new string(PasswordChar, Password.Length) : Password;
            CaretIndex = newCaretIndex < 0 ? Text.Length : newCaretIndex;
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            e.Handled = true;
            UpdateCapsWarning();
            base.OnTextChanged(e);
        }

        private string RemoveRange(string str, int start, int length)
        {
            return str.Remove(start) + str.Substring(start + length);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            capsWarning = GetTemplateChild("PART_CapsWarning") as UIElement;
        }

        private void UpdateCapsWarning()
        {
            if (capsWarning != null)
                capsWarning.Visibility =
                    ShowCapsLockWarning
                    && IsKeyboardFocused
                    && IsEnabled
                    && !IsReadOnly
                    && UsePasswordChar
                    && Keyboard.GetKeyStates(Key.CapsLock).HasFlag(KeyStates.Toggled)
                    ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
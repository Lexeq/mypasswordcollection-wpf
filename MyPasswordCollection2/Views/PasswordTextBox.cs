using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace MPC.Views
{
    [TemplatePart(Name = "PART_CapsWarning", Type = typeof(UIElement))]
    public class PasswordTextBox : TextBox
    {
        #region Fields

        private UIElement capsWarning;

        #endregion

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
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged, OnCoercePassword));

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
            (d as PasswordTextBox)?.UpdateText();
        }

        private static object OnCoercePassword(DependencyObject d, object baseValue)
        {
            return baseValue ?? string.Empty;
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
            Loaded += (o, e) => UpdateCapsWarningVisibility();
        }

        #endregion

        #region Commands

        private void CanExecutePreview(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy || e.Command == ApplicationCommands.Cut)
            {
                e.CanExecute = !UsePasswordChar;
                e.Handled = true;
            }
            if (e.Command == ApplicationCommands.Undo || e.Command == ApplicationCommands.Redo)
            {
                e.CanExecute = false;
                e.Handled = true;
            }
        }

        private void ExecutePreview(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Cut)
            {
                Clipboard.SetText(SelectedText);
                Delete(true);
                e.Handled = true;
            }
            else if (e.Command == ApplicationCommands.Paste)
            {
                AddText(Clipboard.GetText());
                e.Handled = true;
            }
            else if (e.Command == EditingCommands.Delete)
            {
                Delete(false);
                e.Handled = true;
            }
            else if (e.Command == EditingCommands.Backspace)
            {
                Delete(true);
                e.Handled = true;
            }
            else if (e.Command is RoutedUICommand cmd)
            {
                if (cmd.Name == "Space" || cmd.Name == "ShiftSpace")
                {
                    AddText(" ");
                    e.Handled = true;
                }
            }
        }

        #endregion

        #region Overrides

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.CapsLock)
                UpdateCapsWarningVisibility();
            base.OnPreviewKeyDown(e);
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (e.Text == "\r" || e.Text == '\u001b'.ToString())  //u001b - ESC
                return;
            AddText(e.Text);
            e.Handled = true;
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            e.Handled = true;
            UpdateCapsWarningVisibility();
            base.OnTextChanged(e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            capsWarning = GetTemplateChild("PART_CapsWarning") as UIElement;
        }

        protected override void OnIsKeyboardFocusedChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsKeyboardFocusedChanged(e);
            UpdateCapsWarningVisibility();
        }

        #endregion

        #region Methods

        private void AddText(string text)
        {
            if (IsReadOnly || string.IsNullOrEmpty(text))
                return;

            if (SelectionLength != 0)
                Delete(true);

            var carretIndex = CaretIndex;
            Password = Password.Insert(CaretIndex, TakeSingleLine(text));
            CaretIndex = carretIndex + text.Length;
        }

        private string TakeSingleLine(string text)
        {
            int i = text.IndexOfAny(new[] { '\r', '\n' });
            return i < 0 ? text : text.Substring(0, i);
        }

        private void Delete(bool deleteBackward)
        {
            if (IsReadOnly
                || (!deleteBackward && CaretIndex == Password.Length)
                || (deleteBackward && CaretIndex == 0 && SelectionLength == 0))
                return;
            var caret = CaretIndex;
            if (SelectionLength != 0)
            {
                Password = Password.Remove(SelectionStart, SelectionLength);
            }
            else
            {
                if (deleteBackward)
                {
                    caret--;
                }
                Password = Password.Remove(caret, 1);
            }
            CaretIndex = caret;
        }

        private void UpdateText()
        {
            Text = UsePasswordChar ? new string(PasswordChar, Password.Length) : Password;
        }

        private void UpdateCapsWarningVisibility()
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

        #endregion
    }
}
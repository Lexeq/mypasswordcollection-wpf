using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MPC
{
    public class PasswordTextBox : TextBox
    {
        public static readonly DependencyProperty UsePasswordCharProperty =
            DependencyProperty.Register(
                nameof(UsePasswordChar),
                typeof(bool),
                typeof(PasswordTextBox),
                new FrameworkPropertyMetadata(false, OnPropertyChanged)
        );

        public static readonly DependencyProperty PlainTextProperty =
            DependencyProperty.Register(
                nameof(PlainText),
                typeof(string),
                typeof(PasswordTextBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));

        public static readonly DependencyProperty PasswordCharProperty =
            DependencyProperty.Register(
                nameof(PasswordChar),
                typeof(char),
                typeof(PasswordTextBox),
                new PropertyMetadata('*', OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as PasswordTextBox;
            obj?.Update();

        }

        public string PlainText
        {
            get { return (string)GetValue(PlainTextProperty); }
            set { SetValue(PlainTextProperty, value); }
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

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            e.Handled = true;
            if (IsReadOnly)
            { }
            else if (e.Key == Key.Back)
                RemoveText(true);
            else if (e.Key == Key.Delete)
                RemoveText(false);
            else if (e.Key == Key.V && e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control))
                AddText(Clipboard.GetText());
            //disable new line, space and cut/copy
            else if (e.Key == Key.Return || e.Key == Key.Space ||
                ((e.Key == Key.X || e.Key == Key.C) && e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control)))
            { }
            else
                e.Handled = false;

           base.OnPreviewKeyDown(e);
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            AddText(e.Text);
            e.Handled = true;
        }

        private void AddText(string text)
        {
            if (SelectionLength != 0)
                RemoveText(true);
            PlainText = PlainText != null ? PlainText.Insert(CaretIndex, text) : text;
            Update(CaretIndex + text.Length);
        }

        protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            e.Handled = true;
            base.OnContextMenuOpening(e);
        }

        private void RemoveText(bool back)
        {
            if (PlainText == null || PlainText.Length == 0 ||
                (back == false && CaretIndex == PlainText.Length) || (back && CaretIndex == 0))
                return;

            if (SelectionLength != 0)
            {
                PlainText = RemoveRange(PlainText, SelectionStart, SelectionLength);
                Update(SelectionStart);
            }
            else if (back)
            {
                PlainText = RemoveRange(PlainText, CaretIndex - 1, 1);
                Update(CaretIndex - 1);
            }
            else
            {
                PlainText = RemoveRange(PlainText, CaretIndex, 1);
                Update(CaretIndex);
            }
        }

        private void Update(int newCaretIndex = -1)
        {
            Text = UsePasswordChar ? new string(PasswordChar, PlainText.Length) : PlainText;
            CaretIndex = newCaretIndex < 0 ? Text.Length : newCaretIndex;
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            e.Handled = true;
            base.OnTextChanged(e);
        }

        private string RemoveRange(string str, int start, int length)
        {
            return str.Remove(start) + str.Substring(start + length);
        }
    }
}

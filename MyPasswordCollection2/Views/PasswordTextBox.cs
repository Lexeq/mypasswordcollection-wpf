using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace MPC.Views
{
    public class PasswordTextBox : TextBox
    {
        Popup capsWarningPopup;

        #region DependencyProperties
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

        #endregion

        #region Ctor
        public PasswordTextBox()
        {
            Loaded += (ol, el) =>
            {
                capsWarningPopup = new Popup()
                {
                    Child = new Viewbox() { Child = new Label() { Content = "CapsLock is ON", Background = Brushes.Yellow } },
                    PlacementTarget = this,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalOffset = ActualHeight,
                    Placement = PlacementMode.Relative
                };

                var owner = Window.GetWindow(this);
                if (owner != null)
                    owner.LocationChanged += (o, e) => { capsWarningPopup.HorizontalOffset++; capsWarningPopup.HorizontalOffset--; }; //force to redraw popup
                LostFocus += (o, e) => UpdateCapsWarning();
                GotFocus += (o, e) => UpdateCapsWarning();
            };
        }
        #endregion

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            e.Handled = true;
            if (e.Key == Key.Back)
                RemoveText(true);
            else if (e.Key == Key.Delete)
                RemoveText(false);
            else if (e.Key == Key.V && e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control))
                AddText(Clipboard.GetText());
            else if (e.Key == Key.CapsLock)
                UpdateCapsWarning();
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
            var carretIndex = CaretIndex;
            PlainText = PlainText != null ? PlainText.Insert(CaretIndex, text) : text;
            Update(carretIndex + text.Length);
        }

        protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            e.Handled = true;
            base.OnContextMenuOpening(e);
        }

        private void RemoveText(bool back)
        {
            if (PlainText == null || PlainText.Length == 0 ||
                (back == false && CaretIndex == PlainText.Length) || (back && CaretIndex == 0 && SelectionLength == 0))
                return;

            var carretIndex = CaretIndex;
            if (SelectionLength != 0)
            {
                PlainText = RemoveRange(PlainText, SelectionStart, SelectionLength);
                Update(carretIndex);
            }
            else if (back)
            {
                PlainText = RemoveRange(PlainText, CaretIndex - 1, 1);
                Update(carretIndex - 1);
            }
            else
            {
                PlainText = RemoveRange(PlainText, CaretIndex, 1);
                Update(carretIndex);
            }
        }

        private void Update(int newCaretIndex = -1)
        {

            Text = PlainText == null ? string.Empty : UsePasswordChar ? new string(PasswordChar, PlainText.Length) : PlainText;
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

        private void UpdateCapsWarning()
        {
            var KS = Keyboard.GetKeyStates(Key.CapsLock);
            capsWarningPopup.IsOpen = ShowCapsLockWarning && IsFocused && IsEnabled && !IsReadOnly && UsePasswordChar && Keyboard.GetKeyStates(Key.CapsLock).HasFlag(KeyStates.Toggled);
        }
    }
}

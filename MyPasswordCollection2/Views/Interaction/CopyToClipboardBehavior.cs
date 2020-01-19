using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace MPC.Views
{
    class CopyToClipboardBehavior : Behavior<Button>
    {

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(CopyToClipboardBehavior));


        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Click += AssociatedObject_Click;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.Click -= AssociatedObject_Click;
        }

        private void AssociatedObject_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Clipboard.SetText(Text);
        }
    }
}

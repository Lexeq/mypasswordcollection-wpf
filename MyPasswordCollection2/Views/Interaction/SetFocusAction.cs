using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace MPC.Views
{
    class SetFocusAction : TriggerAction<DependencyObject>
    {
        public Control FocusElement
        {
            get { return (Control)GetValue(FocusElementProperty); }
            set { SetValue(FocusElementProperty, value); }
        }

        public static readonly DependencyProperty FocusElementProperty =
            DependencyProperty.Register(nameof(FocusElement), typeof(Control), typeof(SetFocusAction));


        protected override void Invoke(object parameter)
        {
            FocusElement?.Focus();
        }
    }
}

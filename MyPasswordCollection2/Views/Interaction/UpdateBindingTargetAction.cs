using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace MPC.Views
{
    class UpdateBindingTargetAction : TriggerAction<Button>
    {
        public PasswordView View
        {
            get { return (PasswordView)GetValue(ViewProperty); }
            set { SetValue(ViewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for View.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewProperty =
            DependencyProperty.Register(nameof(View), typeof(PasswordView), typeof(UpdateBindingTargetAction));


        protected override void Invoke(object parameter)
        {
            View.tbSite.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            View.tbLogin.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            View.ptbPassword.GetBindingExpression(PasswordTextBox.PasswordProperty).UpdateTarget();
        }
    }
}

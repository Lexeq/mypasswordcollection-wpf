using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace MPC.Views
{
    class UpdateBindingSourceAction : TriggerAction<Button>
    {


        public PasswordView View
        {
            get { return (PasswordView)GetValue(ViewProperty); }
            set { SetValue(ViewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for View.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewProperty =
            DependencyProperty.Register(nameof(View), typeof(PasswordView), typeof(UpdateBindingSourceAction));


        protected override void Invoke(object parameter)
        {
            View.tbSite.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            View.tbLogin.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            View.ptbPassword.GetBindingExpression(PasswordTextBox.PasswordProperty).UpdateSource();
        }
    }
}

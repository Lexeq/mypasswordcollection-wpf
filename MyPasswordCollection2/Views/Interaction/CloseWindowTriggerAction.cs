using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Interop;

namespace MPC.Views
{
    public class CloseWindowTriggerAction : TriggerAction<Window>
    {
        public bool? DialogResult
        {
            get { return (bool?)GetValue(DialogResultProperty); }
            set { SetValue(DialogResultProperty, value); }
        }

        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.Register(nameof(DialogResult), typeof(bool?), typeof(CloseWindowTriggerAction));


        protected override void Invoke(object parameter)
        {
            if (ComponentDispatcher.IsThreadModal)
            {
                AssociatedObject.DialogResult = DialogResult;
            }
            AssociatedObject.Close();
        }
    }
}

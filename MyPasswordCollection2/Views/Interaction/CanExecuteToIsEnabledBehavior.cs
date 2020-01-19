using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace MPC.Views
{
    class CanExecuteToIsEnabledBehavior : Behavior<Button>
    {
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(CanExecuteToIsEnabledBehavior), new PropertyMetadata(CommandChangedCallback), Bar);

        private static bool Bar(object value)
        {
            return true;
        }

        private static void CommandChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (CanExecuteToIsEnabledBehavior)d;
            obj.OnCommandChanged((ICommand)e.OldValue, (ICommand)e.NewValue);
        }

        private void OnCommandChanged(ICommand oldCommand, ICommand newCommand)
        {
            if (oldCommand != null)
            {
                oldCommand.CanExecuteChanged -= CommnadCanExecuteChanged;
            }
            if (newCommand != null)
            {
                newCommand.CanExecuteChanged += CommnadCanExecuteChanged;
            }
        }

        private void CommnadCanExecuteChanged(object sender, EventArgs e)
        {
            AssociatedObject.IsEnabled = Command.CanExecute(null);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace MPC.Views
{
    public class CloseWindowTriggerAction : TriggerAction<Window>
    {
        protected override void Invoke(object parameter)
        {
            AssociatedObject.Close();
        }
    }
}

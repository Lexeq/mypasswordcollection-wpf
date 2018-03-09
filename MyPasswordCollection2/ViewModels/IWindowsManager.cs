using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPC.ViewModels
{
    public interface IWindowsManager
    {
        void Show<TViewModel>(TViewModel vm);

        void ShowDialog<TViewModel>(TViewModel vm);
    }
}

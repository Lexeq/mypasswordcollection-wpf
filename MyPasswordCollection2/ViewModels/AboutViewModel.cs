using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MPC.ViewModels
{
    class AboutViewModel : BaseViewModel
    {
        public string AboutText { get; set; }

        public AboutViewModel()
        {
            string version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;

            AboutText = "This app isn't ensure good protection for your passwords."
                + Environment.NewLine + Environment.NewLine +
                "MyPasswordCollection"
                + Environment.NewLine +
                "v." + version;
        }
    }
}

using System;
using System.Reflection;

namespace MPC.ViewModels
{
    class AboutViewModel : BaseViewModel
    {
        public string AboutText { get; }

        public AboutViewModel(string text = "")
        {
            string version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;

            AboutText = text
                + Environment.NewLine + Environment.NewLine +
                "MyPasswordCollection"
                + Environment.NewLine +
                "v." + version;
        }
    }
}

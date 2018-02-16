using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPC
{
    class IOService : IIOService
    {
        public string OpenFileDialog(string defaultPath)
        {
            return OpenFileDialog(defaultPath, string.Empty);
        }

        public string OpenFileDialog(string defaultPath, string filter)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = filter };
            ofd.ShowDialog();
            return ofd.FileName;
        }

        public string SaveFileDialog(string defaultPath)
        {
            return SaveFileDialog(defaultPath, string.Empty);
        }

        public string SaveFileDialog(string defaultPath, string filter)
        {
            SaveFileDialog sfd = new SaveFileDialog() { Filter = filter };
            sfd.ShowDialog();
            return sfd.FileName;
        }
    }
}

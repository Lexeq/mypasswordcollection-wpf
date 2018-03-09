using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPC.ViewModels
{
    public class FileDialogSettings
    {
        public string Filter { get; set; }

        public string FileName { get; set; }

        public string InitialDirectory { get; set; }
    }
}

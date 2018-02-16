using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPC
{
    public interface IIOService
    {
        string OpenFileDialog(string defaultPath);

        string OpenFileDialog(string defaultPath, string filter);

        string SaveFileDialog(string defaultPath);

        string SaveFileDialog(string defaultPath, string filter);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPC.Model
{
    public interface IPasswordGenerator
    {
        string Generate(CharSets sets, int length);
    }
}

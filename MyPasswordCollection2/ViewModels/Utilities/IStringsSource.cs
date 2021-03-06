﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPC.ViewModels
{
    public interface IStringsSource
    {
        string GetString(UIStrings key);

        string GetString(UIStrings key, IDictionary<string, string> variables);
    }
}

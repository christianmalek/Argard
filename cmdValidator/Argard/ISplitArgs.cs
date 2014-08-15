using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Argard
{
    public interface ISplitArgs
    {
        List<string> SplitArgs(string args);
    }
}

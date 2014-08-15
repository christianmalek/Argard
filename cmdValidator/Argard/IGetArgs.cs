using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Argard
{
    public interface IGetArgs
    {
        List<Argument> GetArgs(List<string> args);
    }
}
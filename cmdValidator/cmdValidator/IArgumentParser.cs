using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmdValidator
{
    public interface IArgumentParser
    {
        List<Argument> GetArgs(List<string> args);
    }
}
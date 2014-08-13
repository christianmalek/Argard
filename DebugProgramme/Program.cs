using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cmdValidator;

namespace DebugProgramme
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser validator = new Parser(false);
            //validator.AddArgumentSet("create:\"s,(dsc|description:\"s),gid:\"s,src|source[s]:\"l", dummyFunc);
            //validator.AddArgumentSet("mod[ify]:\"s,(dsc|description:\"s),(src|source[s]:\"l)", dummyFunc);
            //validator.AddArgumentSet("del[ete]:\"s", dummyFunc);
            //validator.AddArgumentSet("l[i]st:\"s,type:m|g", dummyFunc);
            //validator.AddArgumentSet("l[i]st", dummyFunc);
            //validator.AddArgumentSet("sync:\"l", dummyFunc);
            validator.AddArgumentSet("install:test , src:(blubb)", dummyFunc);

            validator.CheckArgs("install test -src blubb");
        }

        static void dummyFunc(ArgumentSetArgs args)
        {

        }
    }
}
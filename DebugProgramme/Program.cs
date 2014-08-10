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
            Validator validator = new Validator(true);
            //validator.AddArgumentSet("create:\"s,(dsc|description:\"s),gid:\"s,src|source[s]:\"l", dummyFunc);
            //validator.AddArgumentSet("mod[ify]:\"s,(dsc|description:\"s),(src|source[s]:\"l)", dummyFunc);
            //validator.AddArgumentSet("del[ete]:\"s", dummyFunc);
            //validator.AddArgumentSet("l[i]st:\"s,type:m|g", dummyFunc);
            //validator.AddArgumentSet("l[i]st", dummyFunc);
            validator.AddArgumentSet("sync[hronize]:\"s,sync", dummyFunc);

            validator.CheckArgs("sync, sync");
        }
        static void dummyFunc(ArgumentSetData data)
        {

        }
    }
}
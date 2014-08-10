using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmdValidator
{
    public class ArgumentSetData
    {
        public Dictionary<string, ArgumentScheme> CMD { get; private set; }
        public Dictionary<string, ArgumentScheme> Options { get; private set; }
        public string[] UnknownParameters { get; private set; }

        public ArgumentSetData(ArgumentSet argumentSet, string[] unknownOptions)
        {
            this.CMD = new Dictionary<string, ArgumentScheme>();
            this.Options = new Dictionary<string, ArgumentScheme>();

            //set unknownParameters
            this.UnknownParameters = unknownOptions;

            //add all possible identifiers as key for the cmd
            foreach (var identifier in argumentSet.ArgSchemes[0].Identifiers)
                CMD.Add(identifier, argumentSet.ArgSchemes[0]);

            //add all possible identifiers as key for the options
            for (int i = 1; i < argumentSet.ArgSchemes.Length; i++)
                foreach (var identifier in argumentSet.ArgSchemes[i].Identifiers)
                    Options.Add(identifier, argumentSet.ArgSchemes[i]);
        }
    }
}
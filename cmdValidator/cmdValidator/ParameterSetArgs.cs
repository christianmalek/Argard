using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmdValidator
{
    public class ParameterSetArgs
    {
        public Dictionary<string, Parameter> CMD { get; private set; }
        public Dictionary<string, Parameter> Options { get; private set; }
        public Argument[] UnknownArguments { get; private set; }

        public ParameterSetArgs(ParameterSet argumentSet, Argument[] unknownArguments)
        {
            this.CMD = new Dictionary<string, Parameter>();
            this.Options = new Dictionary<string, Parameter>();

            //set unknownParameters
            this.UnknownArguments = unknownArguments;

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
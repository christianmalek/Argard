using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmdValidator
{
    class ArgumentParser : IArgumentParser
    {
        private string[] _optionPrefixes;

        public ArgumentParser()
            : this(new string[] { "--", "-", "/" })
        { }

        public ArgumentParser(string[] optionPrefixes)
        {
            _optionPrefixes = optionPrefixes;
        }

        public List<Argument> GetArgs(List<string> args)
        {
            List<Argument> parsedArgs = new List<Argument>();

            foreach (var arg in args)
            {
                string optionWithoutPrefix = GetOptionWithoutPrefix(arg);

                if (optionWithoutPrefix != null)
                    parsedArgs.Add(new Argument(optionWithoutPrefix, true));
                else
                    parsedArgs.Add(new Argument(arg, false));
            }

            return parsedArgs;
        }

        private string GetOptionWithoutPrefix(string argument)
        {
            int optionPrefixLength = IsArgumentAnOption(argument);

            if (optionPrefixLength > -1)
                return argument.Substring(optionPrefixLength);
            else
                return null;
        }

        //returns the length of the option prefix
        //if there is none it returns -1
        private int IsArgumentAnOption(string argument)
        {
            for (int i = 0; i < this._optionPrefixes.Length; i++)
                if (argument.StartsWith(this._optionPrefixes[i]))
                    return this._optionPrefixes[i].Length;

            return -1;
        }
    }
}
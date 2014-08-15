using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Argard
{
    class ArgumentParser : IArgumentParser
    {
        private string[] _optionPrefixes;
        private const string _stringSplitPattern = "(\"([^\"]+)\"|[^(\\s|,)]+),?";

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

        public List<string> SplitArgs(string args)
        {
            List<string> splittedArgs = new List<string>();

            foreach (Match match in Regex.Matches(args, _stringSplitPattern))
            {
                string text = match.Groups[2].ToString();

                if (text != string.Empty)
                    splittedArgs.Add(text);
                else
                    splittedArgs.Add(match.Groups[1].ToString());
            }

            return splittedArgs;
        }
    }
}
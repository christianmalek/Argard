using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using cmdValidator.Exception;

namespace cmdValidator
{
	public class ParameterSetParser
	{
        //validation patterns
		private const string _stringSplitPattern = "(\"([^\"]+)\"|[^(\\s|,)]+),?";

        private ParameterParser _argumentSchemeParser;
        private IArgumentParser _argumentParser;

		private bool _ignoreUnknownParameters;
        private bool _ignoreCase;
		private List<ParameterSet> _argumentSets;

        public IArgumentParser ArgumentParser
        {
            get { return _argumentParser; }
            set { _argumentParser = value; }
        }
        public bool IgnoreUnknownParameters
        {
            get { return this._ignoreUnknownParameters; }
            set { this._ignoreUnknownParameters = value; }
        }
        public bool IgnoreCase
        {
            get { return this._ignoreCase; }
        }

        public ParameterSetParser(bool ignoreUnknownParameters)
            : this(ignoreUnknownParameters, false)
        { }

        public ParameterSetParser(bool ignoreUnknownParameters, bool ignoreCase)
            : this(ignoreUnknownParameters, ignoreCase, new ArgumentParser())
        { }

        public ParameterSetParser(bool ignoreUnknownParameters, bool ignoreCase, IArgumentParser argumentParser)
        {
            this._argumentSchemeParser = new ParameterParser();
            this._argumentParser = argumentParser;

            this._ignoreUnknownParameters = ignoreUnknownParameters;
            this._ignoreCase = ignoreCase;
            this._argumentSets = new List<ParameterSet>();
        }

        public void AddArgumentSet(string argumentSchemesString, OnTrigger getArgs)
        {
            if (this._ignoreCase)
                argumentSchemesString = argumentSchemesString.ToLower();

            IEnumerable<Parameter> argumentSchemes = _argumentSchemeParser.ParseArgumentSchemes(argumentSchemesString);

            ParameterSet newArgumentSet = new ParameterSet(argumentSchemes, getArgs);

            //checks if a cmd already exists,
            //if not the argSet is legal and will be added to the argumentSets,
            //otherwise a exception will be thrown
            foreach (var argumentSet in this._argumentSets)
                foreach (var newIdentifier in newArgumentSet.GetCmd().Identifiers)
                    if (argumentSet.GetCmd().Identifiers.Contains(newIdentifier))
                    {
                        string message = "Cmd already exists. It's only allowed to use every cmd one time.";
                        throw new InvalidArgumentSetException(string.Format("{0}\nduplicative cmd: {1}", message, newIdentifier)); //TODO: Exception type is wrong
                    }

            this._argumentSets.Add(new ParameterSet(argumentSchemes, getArgs));
        }

        public bool CheckArgs(string args)
        {
            return this.CheckArgs(new List<string>(SplitArgs(args)));
        }

		public bool CheckArgs(List<string> args)
		{
            if (this._ignoreCase)
                for (int i = 0; i < args.Count; i++)
                    args[i] = args[i].ToLower();

            List<Argument> parsedArgs = _argumentParser.GetArgs(args);

			bool result = false;
			for (int i = 0; i < this._argumentSets.Count; i++)
			{
				List<Argument> unknownArguments = new List<Argument>();
				ParameterSet parsedArgumentSet = this.GetParsedArgumentSet(this._argumentSets[i], parsedArgs, this._ignoreUnknownParameters, ref unknownArguments);

				if (parsedArgumentSet != null)
				{
					this._argumentSets[i] = parsedArgumentSet;

					if (this._ignoreUnknownParameters || (!this._ignoreUnknownParameters && unknownArguments.Count == 0))
						parsedArgumentSet.TriggerEvent(unknownArguments.ToArray());
					result = true;
				}
			}
			return result;
		}

		private ParameterSet GetParsedArgumentSet(ParameterSet argSet, List<Argument> args, bool ignoreUnknownParameters, ref List<Argument> unknownArguments)
		{
			List<Parameter> list = new List<Parameter>();

            args = GetFlagSplittedArgs(argSet, args);

			for (int i = 0; i < argSet.ArgSchemes.Length; i++)
			{
				Parameter parsedArgumentScheme = _argumentSchemeParser.GetParsedArgumentScheme(argSet.ArgSchemes[i], ref args);
				if (parsedArgumentScheme == null)
					return null;

				list.Add(parsedArgumentScheme);
			}
			unknownArguments = new List<Argument>(args);
			argSet.ArgSchemes = list.ToArray();
			return argSet;
		}

        private List<Argument> GetFlagSplittedArgs(ParameterSet argSet, List<Argument> args)
        {
            List<Argument> splittedArgs = new List<Argument>();
            List<string> identifiers = GetAllIdentifiers(argSet);

            foreach(var arg in args)
            {
                if(arg.IsOption)
                {
                    if (identifiers.Contains(arg.Value))
                        splittedArgs.Add(arg);
                    else if (ConsistsArgOfFlags(arg.Value, identifiers))
                        foreach (var flag in arg.Value.ToCharArray())
                            splittedArgs.Add(new Argument(Convert.ToString(flag), true));

                    else
                        splittedArgs.Add(arg);
                }
                else
                    splittedArgs.Add(arg);
            }

            return splittedArgs;
        }

        private bool ConsistsArgOfFlags(string arg, List<string> identifiers)
        {
            foreach (var letter in arg)
                if (identifiers.Contains(Convert.ToString(letter)) == false)
                    return false;

            return true;
        }

        private List<string> GetAllIdentifiers(ParameterSet argSet)
        {
            List<string> identifiers = new List<string>();

            foreach (var argScheme in argSet.ArgSchemes)
                foreach (var identifier in argScheme.Identifiers)
                    identifiers.Add(identifier);

            return identifiers;
        }

		private IEnumerable<string> SplitArgs(string args)
		{
			Regex regex = new Regex(ParameterSetParser._stringSplitPattern);
			MatchCollection matchCollection = regex.Matches(args);

			foreach (Match match in matchCollection)
			{
				string text = match.Groups[2].ToString();

				if (text != string.Empty)
					yield return text;
				else
					yield return match.Groups[1].ToString();
			}
		}
	}
}

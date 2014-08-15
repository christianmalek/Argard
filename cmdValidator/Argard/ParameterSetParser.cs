using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Argard.Exception;

namespace Argard
{
	public class ParameterSetParser
	{
        private ParameterParser _parameterParser;
        private IArgumentParser _argumentParser;

		private bool _ignoreUnknownParameters;
        private bool _ignoreCase;
		private List<ParameterSet> _parameterSets;

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
            this._parameterParser = new ParameterParser();
            this._argumentParser = argumentParser;

            this._ignoreUnknownParameters = ignoreUnknownParameters;
            this._ignoreCase = ignoreCase;
            this._parameterSets = new List<ParameterSet>();
        }

        public void AddParameterSet(string parameterSchemes, OnTrigger onTrigger)
        {
            if (this._ignoreCase)
                parameterSchemes = parameterSchemes.ToLower();

            List<Parameter> parameters = _parameterParser.ParseParameterSchemes(parameterSchemes);
            ParameterSet newArgumentSet = new ParameterSet(parameters, onTrigger);

            //checks if a cmd already exists,
            //if not the argSet is legal and will be added to the argumentSets,
            //otherwise a exception will be thrown
            foreach (var parameterSet in this._parameterSets)
                foreach (var newIdentifier in newArgumentSet.GetCmd().Identifiers)
                    if (parameterSet.GetCmd().Identifiers.Contains(newIdentifier))
                    {
                        string message = "Cmd already exists. It's only allowed to use every cmd one time.";
                        throw new InvalidParameterSetException(string.Format("{0}\nduplicative cmd: {1}", message, newIdentifier)); //TODO: Exception type is wrong
                    }

            this._parameterSets.Add(new ParameterSet(parameters, onTrigger));
        }

        public bool CheckArgs(string args)
        {
            return this.CheckArgs(_argumentParser.SplitArgs(args));
        }

		public bool CheckArgs(List<string> args)
		{
            if (this._ignoreCase)
                for (int i = 0; i < args.Count; i++)
                    args[i] = args[i].ToLower();

            List<Argument> parsedArgs = _argumentParser.GetArgs(args);

			bool result = false;
			for (int i = 0; i < this._parameterSets.Count; i++)
			{
				List<Argument> unknownArguments = new List<Argument>();
				ParameterSet parsedArgumentSet = this.GetParsedArgumentSet(this._parameterSets[i], parsedArgs, this._ignoreUnknownParameters, ref unknownArguments);

				if (parsedArgumentSet != null)
				{
					this._parameterSets[i] = parsedArgumentSet;

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
				Parameter parsedArgumentScheme = _parameterParser.GetParsedParameter(argSet.ArgSchemes[i], ref args);
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
	}
}

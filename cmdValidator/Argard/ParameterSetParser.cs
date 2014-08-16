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
            ParameterSet newParameterSet = new ParameterSet(parameters, onTrigger);

            //checks if a parameter set is clearly recognizable from others,
            //if not the argSet is legal and will be added to the argumentSets,
            //otherwise a exception will be thrown
            foreach (var parameterSet in this._parameterSets)
                if (AreArgumentSetsClearlyRecognizable(parameterSet, newParameterSet) == false)
                    throw new InvalidParameterSetException(string.Format("An already added parameter set couldn't be distinguished from another parameter set. The amount or name of non-optional parameters must be different."));

                //foreach (var newIdentifier in newParameterSet.GetCmd().Identifiers)
                //    if (parameterSet.GetCmd().Identifiers.Contains(newIdentifier))
                //    {

                //        //

                //        string message = "Cmd already exists. It's only allowed to use every cmd one time.";
                //        throw new InvalidParameterSetException(string.Format("{0}\nduplicative cmd: {1}", message, newIdentifier)); //TODO: Exception type is wrong
                //    }

            this._parameterSets.Add(new ParameterSet(parameters, onTrigger));
        }

        private bool AreArgumentSetsClearlyRecognizable(ParameterSet set1, ParameterSet set2)
        {
            bool cmdIdentifiersAreOverlapping = AreIdentifiersOverlapping(set1.GetCmd(), set2.GetCmd());

            if (cmdIdentifiersAreOverlapping == false)
                return true;
            else
            {
                if(set1.Parameters.Length < set2.Parameters.Length)
                {
                    ParameterSet swap = set1;
                    set1 = set2;
                    set2 = swap;
                }

                foreach (var param in set1.Parameters)
                {
                    //optional params are not relevant
                    if (param.IsOptional)
                        continue;

                    Parameter matchingParam = set2.ContainsParameter(param.Identifiers);

                    if(matchingParam == null)
                        return true;
                }
            }

            return false;
        }

        private bool AreIdentifiersOverlapping(Parameter param1, Parameter param2)
        {
            foreach (var identifier in param1.Identifiers)
                if (param2.Identifiers.Contains(identifier))
                    return true;

            return false;
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

			for (int i = 0; i < argSet.Parameters.Length; i++)
			{
				Parameter parsedArgumentScheme = _parameterParser.GetParsedParameter(argSet.Parameters[i], ref args);
				if (parsedArgumentScheme == null)
					return null;

				list.Add(parsedArgumentScheme);
			}
			unknownArguments = new List<Argument>(args);
			argSet.Parameters = list.ToArray();
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

            foreach (var argScheme in argSet.Parameters)
                foreach (var identifier in argScheme.Identifiers)
                    identifiers.Add(identifier);

            return identifiers;
        }
	}
}

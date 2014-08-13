using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
namespace cmdValidator
{
	public class Parser
	{
        //validation patterns
		private const string _identifierPattern= "(?:(?:(\\w+)(\\[\\w+\\])?(\\w*))|(?:(\\[\\w+\\])(\\w+)))";
		private const string _stringSplitPattern = "(\"([^\"]+)\"|[^(\\s|,)]+),?";

        private ArgumentSchemeValidator _validator;

        private string[] _optionPrefixes;
		private bool _ignoreUnknownParameters;
        private bool _ignoreCase;
		private List<ArgumentSet> _argumentSets;
        public bool IgnoreUnknownParameters
        {
            get { return this._ignoreUnknownParameters; }
            set { this._ignoreUnknownParameters = value; }
        }
        public bool IgnoreCase
        {
            get { return this._ignoreCase; }
        }

        public Parser(bool ignoreUnknownParameters)
            : this(ignoreUnknownParameters, false, null)
        { }

        public Parser(bool ignoreUnknownParameters, bool ignoreCase)
            : this(ignoreUnknownParameters, ignoreCase, null)
        { }

		private Parser(bool ignoreUnknownParameters, bool ignoreCase, string[] separators)
		{
            this._validator = new ArgumentSchemeValidator();

            this._ignoreUnknownParameters = ignoreUnknownParameters;
            this._ignoreCase = ignoreCase;
            this._argumentSets = new List<ArgumentSet>();

            if (separators == null)
				this._optionPrefixes = new string[]
				{
					"-",
					"--",
					"/"
				};
			else
				this._optionPrefixes = separators;

            //necessary, otherwise the StartsWith() method will not work properly
            this._optionPrefixes = this.SortByLengthDescending(this._optionPrefixes).Cast<string>().ToArray<string>();
		}

        private IEnumerable<string> SortByLengthDescending(IEnumerable<string> e)
        {
            return
                from s in e
                orderby s.Length descending
                select s;
        }

        public void AddArgumentSet(string argumentSchemesString, GetArguments getArgs)
        {
            if (this._ignoreCase)
                argumentSchemesString = argumentSchemesString.ToLower();

            IEnumerable<ArgumentScheme> argumentSchemes = this.ParseArgumentSchemes(argumentSchemesString);

            ArgumentSet newArgumentSet = new ArgumentSet(argumentSchemes, getArgs);

            //checks if a cmd already exists,
            //if not the argSet is legal and will be added to the argumentSets,
            //otherwise a exception will be thrown
            foreach (var argumentSet in this._argumentSets)
                foreach (var newIdentifier in newArgumentSet.GetCmd().Identifiers)
                    if (argumentSet.GetCmd().Identifiers.Contains(newIdentifier))
                    {
                        string message = "Cmd already exists. It's only allowed to use every cmd one time.";
                        throw new InvalidArgumentSetException(string.Format("{0}\nduplicative cmd: {1}", message, newIdentifier));
                    }

            this._argumentSets.Add(new ArgumentSet(argumentSchemes, getArgs));
        }

		public bool CheckArgs(string args)
		{
			return this.CheckArgs(this.SplitArgs(args).Cast<string>().ToList<string>());
		}

		public bool CheckArgs(List<string> args)
		{
            if (this._ignoreCase)
                for (int i = 0; i < args.Count; i++)
                    args[i] = args[i].ToLower();

			bool result = false;
			for (int i = 0; i < this._argumentSets.Count; i++)
			{
				List<string> unknownParameters = new List<string>();
				ArgumentSet parsedArgumentSet = this.GetParsedArgumentSet(this._argumentSets[i], args, this._ignoreUnknownParameters, ref unknownParameters);

				if (parsedArgumentSet != null)
				{
					this._argumentSets[i] = parsedArgumentSet;

					if (this._ignoreUnknownParameters || (!this._ignoreUnknownParameters && unknownParameters.Count == 0))
						parsedArgumentSet.TriggerEvent(unknownParameters.ToArray());
					result = true;
				}
			}
			return result;
		}

		private ArgumentSet GetParsedArgumentSet(ArgumentSet argSet, List<string> args, bool ignoreUnknownParameters, ref List<string> unknownOptions)
		{
			List<ArgumentScheme> list = new List<ArgumentScheme>();

			for (int i = 0; i < argSet.ArgSchemes.Length; i++)
			{
				ArgumentScheme parsedArgumentScheme = this.GetParsedArgumentScheme(argSet.ArgSchemes[i], ref args);
				if (parsedArgumentScheme == null)
					return null;

				list.Add(parsedArgumentScheme);
			}
			unknownOptions = new List<string>(args);
			argSet.ArgSchemes = list.ToArray();
			return argSet;
		}

        private ArgumentScheme GetParsedArgumentScheme(ArgumentScheme argScheme, ref List<string> args)
        {
            bool validCmdIdentifier = false;
            bool validOptionIdentifier = false;

            //check if identifier is existing and remove it from args
            //otherwise return null and stop it that way from parsing
            if (args.Count > 0)
            {
                validCmdIdentifier = CheckIfArgIsValidCmdIdentifier(argScheme, args[0]);
                validOptionIdentifier = CheckIfArgIsValidOptionIdentifier(argScheme, args[0]);

                if (validCmdIdentifier || validOptionIdentifier)
                    args.RemoveAt(0);
                else
                    return null;
            }
            else if (argScheme.IsOptional)
                return argScheme;
            else
                return null;

            switch (argScheme.ArgumentValues.ValueType)
            {
                case ValueType.None:
                    return argScheme;
                case ValueType.Single:
                    return ParseSingleValue(argScheme, ref args);
                case ValueType.List:
                    return ParseListValue(argScheme, ref args);
            }

            return null;
        }

        //checks if the passed argument is a cmd identifier of the passed argument scheme
        private bool CheckIfArgIsValidCmdIdentifier(ArgumentScheme argScheme, string arg)
        {
            int isOption = this.IsOption(arg);

            return argScheme.IsCmd && isOption == -1 && argScheme.Identifiers.Contains(arg);
        }

        //checks if the passed argument is a option identifier of the passed argument scheme
        private bool CheckIfArgIsValidOptionIdentifier(ArgumentScheme argScheme, string arg)
        {
            int isOption = this.IsOption(arg);

            if (isOption == -1)
                return false;

            string argWithoutOptionPrefix = arg.Substring(isOption);

            return argScheme.IsCmd == false && argScheme.Identifiers.Contains(argWithoutOptionPrefix);
        }

        private ArgumentScheme ParseSingleValue(ArgumentScheme argScheme, ref List<string> args)
        {
            if (args.Count > 0 && this.IsOption(args[0]) == -1)
            {
                bool specialValueRequired = argScheme.ArgumentValues.AllowedValues.Count > 0;
                bool requiredValueAvailable = argScheme.ArgumentValues.AllowedValues.Contains(args[0]);

                if (specialValueRequired && requiredValueAvailable || !specialValueRequired)
                {
                    argScheme.ArgumentValues.ParsedValues.Add(args[0]);
                    args.RemoveAt(0);
                    return argScheme;
                }
                else
                    return null;
            }

            if (argScheme.ArgumentValues.AreValuesOptional)
                return argScheme;
            else
                return null;
        }

        private ArgumentScheme ParseListValue(ArgumentScheme argScheme, ref List<string> args)
        {
            List<string> argsCopy = new List<string>();
            foreach (var arg in args)
                argsCopy.Add(arg);

            foreach (var arg in argsCopy)
            {
                if (this.IsOption(arg) != -1)
                    break;
                argScheme.ArgumentValues.ParsedValues.Add(arg);
                args.Remove(arg);
            }

            if (argScheme.ArgumentValues.ParsedValues.Count > 0 || argScheme.ArgumentValues.AreValuesOptional)
                return argScheme;
            else
                return null;
        }

        //returns the length of the option prefix
        //if there is none it returns -1
		private int IsOption(string argument)
		{
            for (int i = 0; i < this._optionPrefixes.Length; i++)
                if (argument.StartsWith(this._optionPrefixes[i]))
                    return this._optionPrefixes[i].Length;

            return -1;
		}

		private IEnumerable<string> SplitArgs(string args)
		{
			Regex regex = new Regex(Parser._stringSplitPattern);
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

		private IEnumerable<ArgumentScheme> ParseArgumentSchemes(string argumentSchemesString)
		{
			argumentSchemesString = argumentSchemesString.Trim();
			List<ArgumentScheme> list = new List<ArgumentScheme>();
			string[] array = argumentSchemesString.Split(new char[]
			{
				','
			});

            //if there are no argument schemes stop parsing and throw exception
            if (array.Length < 1)
                throw new InvalidArgumentSchemeException("The argument schemes are empty.");

            //first argumentScheme is always the cmd
            list.Add(ParseArgumentScheme(array[0], true));

            //every other argumentScheme cannot be the cmd
            for (int i = 1; i < array.Length; i++)
                list.Add(ParseArgumentScheme(array[i], false));

			return list;
		}

        private ArgumentScheme ParseArgumentScheme(string argumentSchemeString, bool isCmd)
        {
            ArgumentScheme argumentScheme = this.GetArgument(argumentSchemeString, isCmd);

            if (argumentScheme == null)
                throw new InvalidArgumentSchemeException(string.Format("Following argument scheme is corrupted:\n\"{0}\"", argumentSchemeString));

            return argumentScheme;
        }

		private ArgumentScheme GetArgument(string argumentSchemeString, bool isCmd)
		{
			bool isOptional = this.CheckFirstAndLastCharOfString('(', ')', argumentSchemeString);

			if (isOptional)
				argumentSchemeString = this.RemoveLastAndFirstChar(argumentSchemeString);

            //remove spaces at the beginning and end for the sake of validation
            argumentSchemeString = argumentSchemeString.Trim();

			if (this._validator.Validate(argumentSchemeString))
			{
				int num = argumentSchemeString.IndexOf(':');

				if (num == -1)
					num = argumentSchemeString.IndexOf('=');

				if (num > -1)
				{
					string valueScheme = argumentSchemeString.Substring(num + 1).Trim();

					//string[] values2 = this.GetValues(valueScheme, out areValuesOptional, out valueType).Cast<string>().ToArray<string>();
                    ArgumentValues argValueList = this.GetValues(valueScheme);
                    IEnumerable<string> identifiers = this.GetIdentifiers(argumentSchemeString.Substring(0, num));

                    return new ArgumentScheme(identifiers, isOptional, isCmd, argValueList);
				}
				else
				{
                    IEnumerable<string> identifiers = this.GetIdentifiers(argumentSchemeString);
                    ArgumentValues argumentValues = new ArgumentValues(ValueType.None, false, new List<string>());

                    return new ArgumentScheme(identifiers, isOptional, isCmd, argumentValues);
				}
			}

            return null;
		}

        private ArgumentValues GetValues(string valueScheme)
        {
            bool areValuesOptional = this.CheckFirstAndLastCharOfString('(', ')', valueScheme);

            if(areValuesOptional)
                valueScheme = this.RemoveLastAndFirstChar(valueScheme);

            valueScheme = valueScheme.ToLower();

            if (valueScheme == "^list" || valueScheme == "^l")
                return new ArgumentValues(ValueType.List, areValuesOptional, new List<string>());
            else if (valueScheme == "^single" || valueScheme == "^s")
                return new ArgumentValues(ValueType.Single, areValuesOptional, new List<string>());
            else
            {
                List<string> values = new List<string>(valueScheme.Split(new char[] { '|' }));

                //remove all spaces at the beginning and the end
                for (int i = 0; i < values.Count; i++)
                    values[i] = values[i].Trim();

                return new ArgumentValues(ValueType.Single, areValuesOptional, values);
            }
        }

		private IEnumerable<string> GetIdentifiers(string multipleIdentifierScheme)
		{
			List<string> list = new List<string>();
			string[] array = multipleIdentifierScheme.Split(new char[]
			{
				'|'
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string singleIdentifierScheme = array2[i];
				list.AddRange(this.SplitIdentifiers(singleIdentifierScheme));
			}
			return list;
		}
		private IEnumerable<string> SplitIdentifiers(string singleIdentifierScheme)
		{
			List<string> list = new List<string>();
			Regex regex = new Regex(Parser._identifierPattern);
			Match match = regex.Match(singleIdentifierScheme);
			string[] array = new string[5];
			for (int i = 0; i < 5; i++)
			{
				array[i] = match.Groups[i + 1].ToString();
			}
			if (array[0] != string.Empty)
			{
				if (array[1] != string.Empty)
				{
					if (array[2] != string.Empty)
					{
						list.Add(array[0] + array[2]);
						list.Add(array[0] + this.RemoveLastAndFirstChar(array[1]) + array[2]);
					}
					else
					{
						list.Add(array[0]);
						list.Add(array[0] + this.RemoveLastAndFirstChar(array[1]));
					}
				}
				else
				{
					list.Add(array[0]);
				}
			}
			else
			{
				list.Add(this.RemoveLastAndFirstChar(array[3]) + array[4]);
				list.Add(array[4]);
			}
			return list;
		}

		private string RemoveLastAndFirstChar(string text)
		{
			string result;
			if (text.Length >= 3)
			{
				result = text.Substring(1, text.Length - 2);
			}
			else
			{
				result = text;
			}
			return result;
		}

		private bool CheckFirstAndLastCharOfString(char firstChar, char lastChar, string text)
		{
			return text.Length > 1 && text[0] == firstChar && text[text.Length - 1] == lastChar;
		}
	}
}

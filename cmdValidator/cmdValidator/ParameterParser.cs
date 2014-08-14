using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using cmdValidator.Exception;

namespace cmdValidator
{
    class ParameterParser
    {
        private const string _identifierPattern = "(?:(?:(\\w+)(\\[\\w+\\])?(\\w*))|(?:(\\[\\w+\\])(\\w+)))";
        private ParameterValidator _validator;

        public ParameterParser()
        {
            _validator = new ParameterValidator();
        }

        public List<Parameter> ParseArgumentSchemes(string argumentSchemesString)
        {
            argumentSchemesString = argumentSchemesString.Trim();
            List<Parameter> list = new List<Parameter>();
            string[] array = argumentSchemesString.Split(new char[]
			{
				','
			});

            //if there are no argument schemes stop parsing and throw exception
            if (array.Length < 1)
                throw new InvalidArgumentSchemeException("The argument schemes are empty.");
            
            //first argumentScheme is always the cmd
            list.Add(GetArgumentScheme(array[0], true));

            //every other argumentScheme cannot be the cmd
            for (int i = 1; i < array.Length; i++)
                list.Add(GetArgumentScheme(array[i], false));

            return list;
        }

        private Parameter GetArgumentScheme(string argumentSchemeString, bool isCmd)
        {
            Parameter argumentScheme = this.ParseArgumentScheme(argumentSchemeString, isCmd);

            if (argumentScheme == null)
                throw new InvalidArgumentSchemeException(string.Format("Following argument scheme is corrupted:\n\"{0}\"", argumentSchemeString));

            return argumentScheme;
        }

        private Parameter ParseArgumentScheme(string argumentSchemeString, bool isCmd)
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

                    ParameterValues argValueList = this.GetValues(valueScheme);
                    IEnumerable<string> identifiers = this.GetIdentifiers(argumentSchemeString.Substring(0, num));

                    return new Parameter(identifiers, isOptional, isCmd, argValueList);
                }
                else
                {
                    IEnumerable<string> identifiers = this.GetIdentifiers(argumentSchemeString);
                    ParameterValues argumentValues = new ParameterValues(ValueType.None, false, new List<string>());

                    return new Parameter(identifiers, isOptional, isCmd, argumentValues);
                }
            }

            return null;
        }

        private ParameterValues GetValues(string valueScheme)
        {
            bool areValuesOptional = this.CheckFirstAndLastCharOfString('(', ')', valueScheme);

            if (areValuesOptional)
                valueScheme = this.RemoveLastAndFirstChar(valueScheme);

            valueScheme = valueScheme.ToLower();

            if (valueScheme == "^list" || valueScheme == "^l")
                return new ParameterValues(ValueType.List, areValuesOptional, new List<string>());
            else if (valueScheme == "^single" || valueScheme == "^s")
                return new ParameterValues(ValueType.Single, areValuesOptional, new List<string>());
            else
            {
                List<string> values = new List<string>(valueScheme.Split(new char[] { '|' }));

                //remove all spaces at the beginning and the end
                for (int i = 0; i < values.Count; i++)
                    values[i] = values[i].Trim();

                return new ParameterValues(ValueType.Single, areValuesOptional, values);
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
            Regex regex = new Regex(_identifierPattern);
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

        public Parameter GetParsedArgumentScheme(Parameter argScheme, ref List<Argument> args)
        {
            bool validCmdIdentifier = false;
            bool validOptionIdentifier = false;

            int argIndex = GetArgIndex(argScheme, args);

            if (argIndex < 0)
                return null;

            //check if identifier is existing and remove it from args
            //otherwise return null and stop it that way from parsing
            if (args.Count > 0)
            {
                validCmdIdentifier = CheckIfArgIsValidCmdIdentifier(argScheme, args[argIndex]);
                validOptionIdentifier = CheckIfArgIsValidOptionIdentifier(argScheme, args[argIndex]);

                if (validCmdIdentifier || validOptionIdentifier)
                    args.RemoveAt(argIndex);
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

        private int GetArgIndex(Parameter argScheme, List<Argument> args)
        {
            for (int i = 0; i < args.Count; i++)
                if (args[i].IsOption && argScheme.Identifiers.Contains(args[i].Value) ||
                        argScheme.IsCmd && argScheme.Identifiers.Contains(args[i].Value))
                    return i;

            return -1;
        }

        //checks if the passed argument is a cmd identifier of the passed argument scheme
        private bool CheckIfArgIsValidCmdIdentifier(Parameter argScheme, Argument arg)
        {
            return argScheme.IsCmd && arg.IsOption == false && argScheme.Identifiers.Contains(arg.Value);
        }

        //checks if the passed argument is a option identifier of the passed argument scheme
        private bool CheckIfArgIsValidOptionIdentifier(Parameter argScheme, Argument arg)
        {
            if (arg.IsOption == false)
                return false;

            return argScheme.IsCmd == false && argScheme.Identifiers.Contains(arg.Value);
        }

        private Parameter ParseSingleValue(Parameter argScheme, ref List<Argument> args)
        {
            if (args.Count > 0 && args[0].IsOption == false)
            {
                bool specialValueRequired = argScheme.ArgumentValues.AllowedValues.Count > 0;
                bool requiredValueAvailable = argScheme.ArgumentValues.AllowedValues.Contains(args[0].Value);

                if (specialValueRequired && requiredValueAvailable || !specialValueRequired)
                {
                    argScheme.ArgumentValues.ParsedValues.Add(args[0].Value);
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

        private Parameter ParseListValue(Parameter argScheme, ref List<Argument> args)
        {
            List<Argument> argsCopy = new List<Argument>();
            foreach (var arg in args)
                argsCopy.Add(arg);

            foreach (var arg in argsCopy)
            {
                if (arg.IsOption)
                    break;
                argScheme.ArgumentValues.ParsedValues.Add(arg.Value);
                args.Remove(arg);
            }

            if (argScheme.ArgumentValues.ParsedValues.Count > 0 || argScheme.ArgumentValues.AreValuesOptional)
                return argScheme;
            else
                return null;
        }
    }
}

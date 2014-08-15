using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Argard.Exception;

namespace Argard
{
    class ParameterParser
    {
        private const string _identifierPattern = "(?:(?:(\\w+)(\\[\\w+\\])?(\\w*))|(?:(\\[\\w+\\])(\\w+)))";
        private ParameterValidator _validator;
        private IdentifierParser _identifierParser;
        private ValueParser _valueParser;

        public ParameterParser()
        {
            _validator = new ParameterValidator();
            _identifierParser = new IdentifierParser();
            _valueParser = new ValueParser();
        }

        public List<Parameter> ParseParameterSchemes(string parameterSchemes)
        {
            parameterSchemes = parameterSchemes.Trim();
            List<Parameter> parameters = new List<Parameter>();
            string[] array = parameterSchemes.Split(new char[] { ',' });

            //if there are no argument schemes stop parsing and throw exception
            if (array.Length < 1)
                throw new InvalidParameterException("The argument schemes are empty.");
            
            //first argumentScheme is always the cmd
            parameters.Add(GetParameter(array[0], true));

            //every other argumentScheme cannot be the cmd
            for (int i = 1; i < array.Length; i++)
                parameters.Add(GetParameter(array[i], false));

            return parameters;
        }

        private Parameter GetParameter(string parameterScheme, bool isCmd)
        {
            Parameter parameter = this.ParseParameterScheme(parameterScheme, isCmd);

            if (parameter == null)
                throw new InvalidParameterException(string.Format("Following argument scheme is corrupted:\n\"{0}\"", parameterScheme));

            return parameter;
        }

        private Parameter ParseParameterScheme(string parameterScheme, bool isCmd)
        {
            bool isOptional = StringHelper.CheckFirstAndLastCharOfString('(', ')', parameterScheme);

            if (isOptional)
                parameterScheme = StringHelper.RemoveLastAndFirstChar(parameterScheme);

            //remove spaces at the beginning and end for the sake of validation
            parameterScheme = parameterScheme.Trim();

            if (this._validator.Validate(parameterScheme))
            {
                int num = parameterScheme.IndexOf(':');

                if (num == -1)
                    num = parameterScheme.IndexOf('=');

                if (num > -1)
                {
                    string valueScheme = parameterScheme.Substring(num + 1).Trim();

                    ParameterValues argValueList = _valueParser.GetValues(valueScheme);
                    IEnumerable<string> identifiers = _identifierParser.GetIdentifiers(parameterScheme.Substring(0, num));

                    return new Parameter(identifiers, isOptional, isCmd, argValueList);
                }
                else
                {
                    IEnumerable<string> identifiers = _identifierParser.GetIdentifiers(parameterScheme);
                    ParameterValues argumentValues = new ParameterValues(ValueType.None, false, new List<string>());

                    return new Parameter(identifiers, isOptional, isCmd, argumentValues);
                }
            }

            return null;
        }

        public Parameter GetParsedParameter(Parameter parameter, ref List<Argument> args)
        {
            bool validCmdIdentifier = false;
            bool validOptionIdentifier = false;

            int argIndex = GetArgIndex(parameter, args);

            if (argIndex < 0)
                return null;

            //check if identifier is existing and remove it from args
            //otherwise return null and stop it that way from parsing
            if (args.Count > 0)
            {
                validCmdIdentifier = CheckIfArgIsValidCmdIdentifier(parameter, args[argIndex]);
                validOptionIdentifier = CheckIfArgIsValidOptionIdentifier(parameter, args[argIndex]);

                if (validCmdIdentifier || validOptionIdentifier)
                    args.RemoveAt(argIndex);
                else
                    return null;
            }
            else if (parameter.IsOptional)
                return parameter;
            else
                return null;

            switch (parameter.ArgumentValues.ValueType)
            {
                case ValueType.None:
                    return parameter;
                case ValueType.Single:
                    return ParseSingleValue(parameter, ref args);
                case ValueType.List:
                    return ParseListValue(parameter, ref args);
            }

            return null;
        }

        private int GetArgIndex(Parameter parameter, List<Argument> args)
        {
            for (int i = 0; i < args.Count; i++)
                if (args[i].IsOption && parameter.Identifiers.Contains(args[i].Value) ||
                        parameter.IsCmd && parameter.Identifiers.Contains(args[i].Value))
                    return i;

            return -1;
        }

        //checks if the passed argument is a cmd identifier of the passed argument scheme
        private bool CheckIfArgIsValidCmdIdentifier(Parameter parameter, Argument arg)
        {
            return parameter.IsCmd && arg.IsOption == false && parameter.Identifiers.Contains(arg.Value);
        }

        //checks if the passed argument is a option identifier of the passed argument scheme
        private bool CheckIfArgIsValidOptionIdentifier(Parameter parameter, Argument arg)
        {
            if (arg.IsOption == false)
                return false;

            return parameter.IsCmd == false && parameter.Identifiers.Contains(arg.Value);
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

        private Parameter ParseListValue(Parameter parameter, ref List<Argument> args)
        {
            List<Argument> argsCopy = new List<Argument>();
            foreach (var arg in args)
                argsCopy.Add(arg);

            foreach (var arg in argsCopy)
            {
                if (arg.IsOption)
                    break;
                parameter.ArgumentValues.ParsedValues.Add(arg.Value);
                args.Remove(arg);
            }

            if (parameter.ArgumentValues.ParsedValues.Count > 0 || parameter.ArgumentValues.AreValuesOptional)
                return parameter;
            else
                return null;
        }
    }
}

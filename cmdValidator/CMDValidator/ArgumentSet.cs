using System;
using System.Collections.Generic;
using System.Linq;
namespace cmdValidator
{
    public class ArgumentSet
    {
        private ArgumentScheme[] _argSchemes;
        public event GetArguments GetArgs;

        public ArgumentScheme[] ArgSchemes
        {
            get { return this._argSchemes; }
            set { this._argSchemes = value; }
        }

        public ArgumentSet(IEnumerable<ArgumentScheme> argSchemes, GetArguments getArgs)
        {
            this._argSchemes = argSchemes.Cast<ArgumentScheme>().ToArray<ArgumentScheme>();
            this.Validate();
            this.GetArgs += getArgs;
        }

        public void TriggerEvent(string[] unknownOptions)
        {
            if (this.GetArgs != null)
                this.GetArgs(new ArgumentSetArgs(this, unknownOptions));
        }

        //validates the argument schemes on required argument schemes
        //and the cmd identifier on non-optionality
        private void Validate()
        {
            if(!HasRequiredArgumentScheme())
                throw new InvalidArgumentSetException("At least one argument scheme is required.");

            if (!IsCmdRequired())
                throw new InvalidArgumentSetException("The command argument scheme must not be optional.");
        }

        //checks if at least one argument scheme is required
        private bool HasRequiredArgumentScheme()
        {
            foreach (var argScheme in this._argSchemes)
                if (!argScheme.IsOptional)
                    return true;

            return false;
        }

        //checks if the cmd argument scheme is required
        private bool IsCmdRequired()
        {
            return this._argSchemes.Length > 0 && !this._argSchemes[0].IsOptional;
        }

        public ArgumentScheme GetCmd()
        {
            foreach (var argumentScheme in this._argSchemes)
                if (argumentScheme.IsCmd)
                    return argumentScheme;
            return null;
        }

        private Dictionary<string, ArgumentScheme> getArgSchemeDic()
        {
            Dictionary<string, ArgumentScheme> dictionary = new Dictionary<string, ArgumentScheme>();

            foreach (var argumentScheme in this._argSchemes)
                foreach (string identifier in argumentScheme.Identifiers)
                    dictionary.Add(identifier, argumentScheme);

            return dictionary;
        }
    }
}
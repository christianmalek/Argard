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

            if (!this.IsValid())
                throw new InvalidArgumentSetException("At least one argument scheme is required.");

            this.GetArgs += getArgs;
        }
        public void TriggerEvent(string[] unknownOptions)
        {
            if (this.GetArgs != null)
                this.GetArgs(new ArgumentSetArgs(this, unknownOptions));
        }

        private bool IsValid()
        {
            ArgumentScheme[] argSchemes = this._argSchemes;
            bool result;
            for (int i = 0; i < argSchemes.Length; i++)
            {
                ArgumentScheme argumentScheme = argSchemes[i];
                if (!argumentScheme.IsOptional)
                {
                    result = true;
                    return result;
                }
            }
            result = false;
            return result;
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
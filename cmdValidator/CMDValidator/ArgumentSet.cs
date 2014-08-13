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

            new ArgumentSetValidator().Validate(this._argSchemes);

            this.GetArgs += getArgs;
        }

        public void TriggerEvent(string[] unknownOptions)
        {
            if (this.GetArgs != null)
                this.GetArgs(new ArgumentSetArgs(this, unknownOptions));
        }

        public ArgumentScheme GetCmd()
        {
            foreach (var argumentScheme in this._argSchemes)
                if (argumentScheme.IsCmd)
                    return argumentScheme;
            return null;
        }
    }
}
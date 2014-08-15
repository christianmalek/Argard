using System;
using System.Collections.Generic;
using System.Linq;
namespace Argard
{
    public class ParameterSet
    {
        private Parameter[] _argSchemes;
        public event OnTrigger GetArgs;

        public Parameter[] ArgSchemes
        {
            get { return this._argSchemes; }
            set { this._argSchemes = value; }
        }

        public ParameterSet(IEnumerable<Parameter> argSchemes, OnTrigger getArgs)
        {
            this._argSchemes = argSchemes.Cast<Parameter>().ToArray<Parameter>();

            new ParameterSetValidator().Validate(this._argSchemes);

            this.GetArgs += getArgs;
        }

        public void TriggerEvent(Argument[] unknownArguments)
        {
            if (this.GetArgs != null)
                this.GetArgs(new ParameterSetArgs(this, unknownArguments));
        }

        public Parameter GetCmd()
        {
            foreach (var argumentScheme in this._argSchemes)
                if (argumentScheme.IsCmd)
                    return argumentScheme;
            return null;
        }
    }
}
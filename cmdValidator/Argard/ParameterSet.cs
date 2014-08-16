using System;
using System.Collections.Generic;
using System.Linq;
namespace Argard
{
    public class ParameterSet
    {
        private Parameter[] _parameters;
        public event OnTrigger OnTrigger;

        public Parameter[] Parameters
        {
            get { return this._parameters; }
            set { this._parameters = value; }
        }

        public ParameterSet(IEnumerable<Parameter> parameter, OnTrigger onTrigger)
        {
            this._parameters = parameter.ToArray<Parameter>();

            new ParameterSetValidator().Validate(this._parameters);

            this.OnTrigger += onTrigger;
        }

        public void TriggerEvent(Argument[] unknownArguments)
        {
            if (this.OnTrigger != null)
                this.OnTrigger(new ParameterSetArgs(this, unknownArguments));
        }

        public Parameter GetCmd()
        {
            foreach (var argumentScheme in this._parameters)
                if (argumentScheme.IsCmd)
                    return argumentScheme;
            return null;
        }

        public Parameter ContainsParameter(string identifier)
        {
            foreach (var parameter in _parameters)
                if (parameter.Identifiers.Contains(identifier))
                    return parameter;

            return null;
        }

        public Parameter ContainsParameter(List<string> identifiers)
        {
            foreach (var parameter in _parameters)
                foreach (var identifier in identifiers)
                    if (parameter.Identifiers.Contains(identifier))
                        return parameter;

            return null;
        }
    }
}
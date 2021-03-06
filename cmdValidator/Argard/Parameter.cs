using System;
using System.Collections.Generic;
namespace Argard
{
    public class Parameter
    {
        private List<string> _identifiers;
        private bool _isOptional;
        private bool _isCmd;
        ParameterValues _values;

        public List<string> Identifiers
        {
            get { return this._identifiers; }
        }
        public ParameterValues Values
        {
            get { return this._values; }
        }
        public bool IsOptional
        {
            get { return this._isOptional; }
        }
        public bool IsCmd
        {
            get { return this._isCmd; }
        
        }
        public bool IsFlag
        {
            get
            {
                if (this._isCmd == false && this._values.ValueType == ValueType.None)
                {
                    foreach (var identifier in this._identifiers)
                        if (identifier.Length > 1)
                            return false;
                    return true;
                }
                return false;
            }
        }

        public Parameter(IEnumerable<string> identifiers, bool isNotRequired, bool isCmd, ParameterValues argumentValues)
        {
            this.Initialize(identifiers, isNotRequired, isCmd, argumentValues);
        }

        private void Initialize(IEnumerable<string> identifiers, bool isNotRequired, bool isCmd, ParameterValues argumentValues)
        {
            this._identifiers = new List<string>(identifiers);
            this._isOptional = isNotRequired;
            this._isCmd = isCmd;
            this._values = argumentValues;
        }

        private string GetStringEntries(IEnumerable<string> strings)
        {
            string text = "";
            foreach (string current in strings)
                text = text + current + ", ";

            if (text == "")
                return "";
            else
                return text.Substring(0, text.Length - 2);
        }
    }
}
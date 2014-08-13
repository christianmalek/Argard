using System;
using System.Collections.Generic;
namespace cmdValidator
{
    public class ArgumentScheme
    {
        private List<string> _identifiers;
        private bool _isOptional;
        private bool _isCmd;
        ArgumentValues _argumentValues;

        public List<string> Identifiers
        {
            get { return this._identifiers; }
        }
        public ArgumentValues ArgumentValues
        {
            get { return this._argumentValues; }
        }
        public bool IsOptional
        {
            get { return this._isOptional; }
        }
        public bool IsCmd
        {
            get { return this._isCmd; }
        }

        public ArgumentScheme(IEnumerable<string> identifiers, ArgumentValues argumentValues, bool isNotRequired, bool isCmd)
        {
            this.Initialize(identifiers, argumentValues, isNotRequired, isCmd);
        }

        private void Initialize(IEnumerable<string> identifiers, ArgumentValues argumentValues, bool isNotRequired, bool isCmd)
        {
            this._identifiers = new List<string>(identifiers);
            this._isOptional = isNotRequired;
            this._isCmd = isCmd;
            this._argumentValues = argumentValues;
        }

        //public override string ToString()
        //{
        //    string str = "";
        //    str = str + "Identifiers: " + this.GetStringEntries(this._identifiers);
        //    str = str + ", AllowedValues: " + this.GetStringEntries(this._allowedValues);
        //    str = str + ", ParsedValues: " + this.GetStringEntries(this._parsedValues);
        //    str = str + ", ValueType: " + this._valueType.ToString();
        //    str = str + ", AreValuesOptional: " + this._areValuesOptional.ToString();
        //    str = str + ", IsCmd: " + this._isCmd.ToString();
        //    str = str + ", IsOptional: " + this._isOptional.ToString();
        //    return str;
        //}

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
using System;
using System.Collections.Generic;
namespace cmdValidator
{
    public class ArgumentScheme
    {
        private List<string> _identifiers;
        private List<string> _allowedValues;
        private List<string> _parsedValues;
        private ValueType _valueType;
        private bool _areValuesOptional;
        private bool _isOptional;
        private bool _isCmd;
        public List<string> Identifiers
        {
            get { return this._identifiers; }
        }
        public List<string> AllowedValues
        {
            get { return this._allowedValues; }
        }
        public List<string> ParsedValues
        {
            get { return this._parsedValues; }
            set { this._parsedValues = value; }
        }
        public string FirstValue
        {
            get
            {
                if (this._parsedValues.Count > 0)
                    return this._parsedValues[0];
                else
                    return null;
            }
        }
        public ValueType ValueType
        {
            get { return this._valueType; }
        }
        public bool AreValuesOptional
        {
            get { return this._areValuesOptional; }
        }
        public bool IsOptional
        {
            get { return this._isOptional; }
        }
        public bool IsCmd
        {
            get { return this._isCmd; }
        }

        public ArgumentScheme(IEnumerable<string> identifiers, IEnumerable<string> values, ValueType valueType, bool optionalValues, bool isNotRequired, bool isCmd)
        {
            this.Initialize(identifiers, values, valueType, optionalValues, isNotRequired, isCmd);
        }

        private void Initialize(IEnumerable<string> identifiers, IEnumerable<string> values, ValueType valueType, bool optionalValues, bool isNotRequired, bool isCmd)
        {
            this._identifiers = new List<string>(identifiers);
            this._allowedValues = new List<string>(values);
            this._parsedValues = new List<string>();
            this._valueType = valueType;
            this._areValuesOptional = optionalValues;
            this._isOptional = isNotRequired;
            this._isCmd = isCmd;
        }
        public void AddIdentifier(string identifier)
        {
            this._identifiers.Add(identifier);
        }
        public void AddValue(string value)
        {
            this._allowedValues.Add(value);
        }

        public override string ToString()
        {
            string str = "";
            str = str + "Identifiers: " + this.GetStringEntries(this._identifiers);
            str = str + ", AllowedValues: " + this.GetStringEntries(this._allowedValues);
            str = str + ", ParsedValues: " + this.GetStringEntries(this._parsedValues);
            str = str + ", ValueType: " + this._valueType.ToString();
            str = str + ", AreValuesOptional: " + this._areValuesOptional.ToString();
            str = str + ", IsCmd: " + this._isCmd.ToString();
            str = str + ", IsOptional: " + this._isOptional.ToString();
            return str;
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
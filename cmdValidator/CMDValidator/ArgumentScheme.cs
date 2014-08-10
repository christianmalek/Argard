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
        private bool _optionalValues;
        private bool _isNotRequired;
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
        public ValueType ValueType
        {
            get { return this._valueType; }
        }
        public bool OptionalValues
        {
            get { return this._optionalValues; }
        }
        public bool IsOptional
        {
            get { return this._isNotRequired; }
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
            this._optionalValues = optionalValues;
            this._isNotRequired = isNotRequired;
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
            str = str + ", PredefinedValues: " + this.GetStringEntries(this._allowedValues);
            str = str + ", ParsedValues: " + this.GetStringEntries(this._parsedValues);
            str = str + ", ValueType: " + this._valueType.ToString();
            str = str + ", Optional Values: " + this._optionalValues.ToString();
            str = str + ", IsCmd: " + this._isCmd.ToString();
            str = str + ", Is Not Required: " + this._isNotRequired.ToString();
            return str;
        }
        private string GetStringEntries(IEnumerable<string> strings)
        {
            string text = "";
            foreach (string current in strings)
            {
                text = text + current + ", ";
            }
            string result;
            if (text == "")
            {
                result = "None, ";
            }
            else
            {
                result = text;
            }
            return result;
        }
    }
}
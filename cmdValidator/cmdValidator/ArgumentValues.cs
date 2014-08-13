using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmdValidator
{
    public class ArgumentValues
    {
        ValueType _valueType;
        bool _areValuesOptional;
        List<string> _allowedValues;
        List<string> _parsedValues;

        public ValueType ValueType
        {
            get { return this._valueType; }
        }
        public bool AreValuesOptional
        {
            get { return this._areValuesOptional; }
        }
        public List<string> AllowedValues
        {
            get { return this._allowedValues; }
        }
        public string FirstValue
        {
            get
            {
                if (this._allowedValues.Count > 0)
                    return this._allowedValues[0];
                else
                    return null;
            }
        }
        public List<string> ParsedValues
        {
            get { return this._parsedValues; }
            set { this._parsedValues = value; }
        }

        public ArgumentValues(ValueType valueType, bool areValuesOptional, List<string> allowedValues)
        {
            this._valueType = valueType;
            this._areValuesOptional = areValuesOptional;
            this._allowedValues = allowedValues;
            this._parsedValues = new List<string>();
        }
    }
}

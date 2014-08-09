using System;
using System.Collections.Generic;
namespace cmdValidator
{
	public class ArgumentScheme
	{
		private List<string> _identifiers;
		private List<string> _predefinedValues;
		private List<string> _parsedValues;
		private ValueType _valueType;
		private bool _optionalValues;
		private bool _isOptional;
		public List<string> Identifiers
		{
			get
			{
				return this._identifiers;
			}
		}
		public List<string> PredefinedValues
		{
			get
			{
				return this._predefinedValues;
			}
		}
		public List<string> ParsedValues
		{
			get
			{
				return this._parsedValues;
			}
			set
			{
				this._parsedValues = value;
			}
		}
		public ValueType ValueType
		{
			get
			{
				return this._valueType;
			}
		}
		public bool OptionalValues
		{
			get
			{
				return this._optionalValues;
			}
		}
		public bool IsOptional
		{
			get
			{
				return this._isOptional;
			}
		}
		public ArgumentScheme(IEnumerable<string> identifiers, IEnumerable<string> values, ValueType valueType, bool optionalValues, bool isOptional)
		{
			this.Initialize(identifiers, values, valueType, optionalValues, isOptional);
		}
		public ArgumentScheme(IEnumerable<string> identifiers, bool isOptional)
		{
			this.Initialize(identifiers, new List<string>(), ValueType.None, false, isOptional);
		}
		private void Initialize(IEnumerable<string> identifiers, IEnumerable<string> values, ValueType valueType, bool optionalValues, bool isOptional)
		{
			this._identifiers = new List<string>(identifiers);
			this._predefinedValues = new List<string>(values);
			this._parsedValues = new List<string>();
			this._valueType = valueType;
			this._optionalValues = optionalValues;
			this._isOptional = isOptional;
		}
		public void AddIdentifier(string identifier)
		{
			this._identifiers.Add(identifier);
		}
		public void AddValue(string value)
		{
			this._predefinedValues.Add(value);
		}
		public override string ToString()
		{
			string str = "";
			str = str + "Identifiers: " + this.GetStringEntries(this._identifiers);
			str = str + "\nPredefinedValues: " + this.GetStringEntries(this._predefinedValues);
			str = str + "\nParsedValues: " + this.GetStringEntries(this._parsedValues);
			str = str + "\nValueType: " + this._valueType.ToString();
			str = str + "\nOptional Values: " + this._optionalValues.ToString();
			return str + "\nIs Optional: " + this._isOptional.ToString();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
namespace cmdValidator
{
	public class CMDValidator
	{
		private string _identifierPattern;
		private string _stringSplitPattern;
		private string _argumentPattern;
		private string[] _separators;
		private bool _ignoreUnknownParameters;
		private List<ArgumentSet> _argumentSets;
		public bool IgnoreUnknownParameters
		{
			get
			{
				return this._ignoreUnknownParameters;
			}
			set
			{
				this._ignoreUnknownParameters = value;
			}
		}
		public CMDValidator(bool ignoreUnknownParameters) : this(ignoreUnknownParameters, null)
		{
		}
		private CMDValidator(bool ignoreUnknownParameters, string[] separators)
		{
			this._identifierPattern = "(?:(?:(\\w+)(\\[\\w+\\])?(\\w*))|(?:(\\[\\w+\\])(\\w+)))";
			this._stringSplitPattern = "(\"([^\"]+)\"|[^(\\s|,)]+),?";
			this._argumentPattern = "\\A(?:(?:(?:\\w+)(?:\\[\\w+\\])?(?:\\w*))|(?:(?:\\[\\w+\\])(?:\\w+)))(?:\\|(?:(?:(?:\\w+)(?:\\[\\w+\\])?(?:\\w*))|(?:(?:\\[\\w+\\])(?:\\w+))))*(?:(?::\\([\"\\w ]+(?:\\|[\"\\w ]+)*\\))|(?::[\"\\w ]+(?:\\|[\"\\w ]+)*))?\\Z";
			if (separators == null)
			{
				this._separators = new string[]
				{
					"-",
					"--",
					"/"
				};
			}
			else
			{
				this._separators = separators;
			}
			this._separators = this.SortByLengthDescending(this._separators).Cast<string>().ToArray<string>();
			this._ignoreUnknownParameters = ignoreUnknownParameters;
			this._argumentSets = new List<ArgumentSet>();
		}
		private IEnumerable<string> SortByLengthDescending(IEnumerable<string> e)
		{
			return 
				from s in e
				orderby s.Length descending
				select s;
		}
		public void AddArgumentSet(string argumentSchemes, GetArguments getArgs)
		{
			IEnumerable<ArgumentScheme> argumentSchemes2 = this.GetArgumentSchemes(argumentSchemes);
			this._argumentSets.Add(new ArgumentSet(argumentSchemes2, getArgs));
		}
		public bool CheckArgs(string args)
		{
			return this.CheckArgs(this.SplitArgs(args).Cast<string>().ToArray<string>());
		}
		public bool CheckArgs(string[] args)
		{
			bool result = false;
			for (int i = 0; i < this._argumentSets.Count; i++)
			{
				List<string> list = new List<string>();
				ArgumentSet parsedArgumentSet = this.GetParsedArgumentSet(this._argumentSets[i], args, this._ignoreUnknownParameters, ref list);
				if (parsedArgumentSet != null)
				{
					this._argumentSets[i] = parsedArgumentSet;
					if (this._ignoreUnknownParameters || (!this._ignoreUnknownParameters && list.Count == 0))
					{
						parsedArgumentSet.TriggerEvent(list.ToArray());
					}
					result = true;
				}
			}
			return result;
		}
		private ArgumentSet GetParsedArgumentSet(ArgumentSet argSet, string[] args, bool ignoreUnknownParameters, ref List<string> unknownParameters)
		{
			List<ArgumentScheme> list = new List<ArgumentScheme>();
			ArgumentSet result;
			for (int i = 0; i < argSet.ArgSchemes.Length; i++)
			{
				ArgumentScheme parsedArgumentScheme = this.GetParsedArgumentScheme(argSet.ArgSchemes[i], ref args);
				if (parsedArgumentScheme == null)
				{
					result = null;
					return result;
				}
				list.Add(parsedArgumentScheme);
			}
			unknownParameters = new List<string>(args);
			argSet.ArgSchemes = list.ToArray();
			result = argSet;
			return result;
		}
		private ArgumentScheme GetParsedArgumentScheme(ArgumentScheme argScheme, ref string[] args)
		{
			ArgumentScheme result;
			for (int i = 0; i < args.Length; i++)
			{
				int num = -1;
				string identifier = this.GetIdentifier(argScheme.Identifiers, args, out num);
				if (identifier != "")
				{
					int num2 = num;
					i = num + 1;
					switch (argScheme.ValueType)
					{
					case ValueType.None:
					{
						int num3 = num2;
						args = this.RemoveItems(num2, num3, args);
						result = argScheme;
						return result;
					}
					case ValueType.Single:
						if (i < args.Length)
						{
							if (this.IsOption(args[i]) == -1)
							{
								if ((argScheme.PredefinedValues.Count > 0 && argScheme.PredefinedValues.Contains(args[i])) || argScheme.PredefinedValues.Count == 0)
								{
									argScheme.ParsedValues.Add(args[i]);
									int num3 = i;
									args = this.RemoveItems(num2, num3, args);
									result = argScheme;
									return result;
								}
								if (argScheme.OptionalValues)
								{
									result = argScheme;
									return result;
								}
								result = null;
								return result;
							}
						}
						else
						{
							if (argScheme.OptionalValues)
							{
								int num3 = num2;
								args = this.RemoveItems(num2, num3, args);
								result = argScheme;
								return result;
							}
						}
						result = null;
						return result;
					case ValueType.List:
					{
						List<string> list = new List<string>();
						int num3 = num2;
						for (int j = num3 + 1; j < args.Length; j++)
						{
							if (this.IsOption(args[j]) != -1)
							{
								break;
							}
							list.Add(args[j]);
							num3 = j;
						}
						args = this.RemoveItems(num2, num3, args);
						if (list.Count > 0)
						{
							argScheme.ParsedValues = list;
							result = argScheme;
							return result;
						}
						if (argScheme.OptionalValues)
						{
							result = argScheme;
							return result;
						}
						result = null;
						return result;
					}
					}
				}
				else
				{
					if (argScheme.IsOptional)
					{
						result = argScheme;
						return result;
					}
				}
			}
			result = null;
			return result;
		}
		private string[] RemoveItems(int minIndex, int maxIndex, string[] text)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < minIndex; i++)
			{
				list.Add(text[i]);
			}
			for (int j = maxIndex + 1; j < text.Length; j++)
			{
				list.Add(text[j]);
			}
			return list.ToArray<string>();
		}
		private string GetIdentifier(IEnumerable<string> identifiers, string[] args, out int indexIdentifier)
		{
			string[] array = identifiers.Cast<string>().ToArray<string>();
			string result;
			for (int i = 0; i < args.Length; i++)
			{
				int num = this.IsOption(args[i]);
				if (num > -1)
				{
					string text = args[i].Substring(this._separators[num].Length);
					if (identifiers.Contains(text))
					{
						indexIdentifier = i;
						result = text;
						return result;
					}
				}
			}
			indexIdentifier = -1;
			result = "";
			return result;
		}
		private int IsOption(string argument)
		{
			int result;
			for (int i = 0; i < this._separators.Length; i++)
			{
				if (argument.StartsWith(this._separators[i]))
				{
					result = i;
					return result;
				}
			}
			result = -1;
			return result;
		}
		private string GetOptionWithoutSeparator(string option, int separatorIndex)
		{
			return option.Substring(this._separators[separatorIndex].Length - 1);
		}
		private IEnumerable<string> SplitArgs(string args)
		{
			Regex regex = new Regex(this._stringSplitPattern);
			MatchCollection matchCollection = regex.Matches(args);
			foreach (Match match in matchCollection)
			{
				string text = match.Groups[2].ToString();
				if (text != string.Empty)
				{
					yield return text;
				}
				else
				{
					yield return match.Groups[1].ToString();
				}
			}
			yield break;
		}
		public IEnumerable<ArgumentScheme> GetArgumentSchemes(string argumentSchemes)
		{
			argumentSchemes = argumentSchemes.Trim();
			List<ArgumentScheme> list = new List<ArgumentScheme>();
			string[] array = argumentSchemes.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string argumentScheme = array[i];
				ArgumentScheme argument = this.GetArgument(argumentScheme);
				if (argument == null)
				{
					throw new Exception("Das Argumentschema ist ungültig.");
				}
				list.Add(argument);
			}
			return list;
		}
		private ArgumentScheme GetArgument(string argumentScheme)
		{
			bool flag = this.CheckFirstAndLastCharOfString('(', ')', argumentScheme);
			if (flag)
			{
				argumentScheme = this.RemoveLastAndFirstChar(argumentScheme);
			}
			ArgumentScheme result;
			if (this.IsValid(argumentScheme))
			{
				ValueType valueType = ValueType.None;
				int num = argumentScheme.IndexOf(':');
				if (num == -1)
				{
					num = argumentScheme.IndexOf('=');
				}
				if (num > -1)
				{
					string values = argumentScheme.Substring(num + 1);
					bool optionalValues;
					string[] values2 = this.GetValues(values, out optionalValues, out valueType).Cast<string>().ToArray<string>();
					string multipleIdentifierScheme = argumentScheme.Substring(0, num);
					result = new ArgumentScheme(this.GetIdentifiers(multipleIdentifierScheme), values2, valueType, optionalValues, flag);
				}
				else
				{
					string multipleIdentifierScheme = argumentScheme;
					result = new ArgumentScheme(this.GetIdentifiers(multipleIdentifierScheme), flag);
				}
			}
			else
			{
				result = null;
			}
			return result;
		}
		private IEnumerable<string> GetValues(string values, out bool optionalValues, out ValueType valueType)
		{
			values = values.ToLower();
			if (this.CheckFirstAndLastCharOfString('(', ')', values))
			{
				optionalValues = true;
				values = this.RemoveLastAndFirstChar(values);
			}
			else
			{
				optionalValues = false;
			}
			IEnumerable<string> result;
			if (values == "\"list" || values == "\"l")
			{
				valueType = ValueType.List;
				result = new string[0];
			}
			else
			{
				if (values == "\"single" || values == "\"s")
				{
					valueType = ValueType.Single;
					result = new string[0];
				}
				else
				{
					valueType = ValueType.Single;
					result = values.Split(new char[]
					{
						'|'
					});
				}
			}
			return result;
		}
		private bool IsValid(string argument)
		{
			Regex regex = new Regex(this._argumentPattern);
			return regex.IsMatch(argument);
		}
		private IEnumerable<string> GetIdentifiers(string multipleIdentifierScheme)
		{
			List<string> list = new List<string>();
			string[] array = multipleIdentifierScheme.Split(new char[]
			{
				'|'
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string singleIdentifierScheme = array2[i];
				list.AddRange(this.SplitIdentifiers(singleIdentifierScheme));
			}
			return list;
		}
		private IEnumerable<string> SplitIdentifiers(string singleIdentifierScheme)
		{
			List<string> list = new List<string>();
			Regex regex = new Regex(this._identifierPattern);
			Match match = regex.Match(singleIdentifierScheme);
			string[] array = new string[5];
			for (int i = 0; i < 5; i++)
			{
				array[i] = match.Groups[i + 1].ToString();
			}
			if (array[0] != string.Empty)
			{
				if (array[1] != string.Empty)
				{
					if (array[2] != string.Empty)
					{
						list.Add(array[0] + array[2]);
						list.Add(array[0] + this.RemoveLastAndFirstChar(array[1]) + array[2]);
					}
					else
					{
						list.Add(array[0]);
						list.Add(array[0] + this.RemoveLastAndFirstChar(array[1]));
					}
				}
				else
				{
					list.Add(array[0]);
				}
			}
			else
			{
				list.Add(this.RemoveLastAndFirstChar(array[3]) + array[4]);
				list.Add(array[4]);
			}
			return list;
		}
		private string RemoveLastAndFirstChar(string text)
		{
			string result;
			if (text.Length >= 3)
			{
				result = text.Substring(1, text.Length - 2);
			}
			else
			{
				result = text;
			}
			return result;
		}
		private bool CheckFirstAndLastCharOfString(char firstChar, char lastChar, string text)
		{
			return text.Length > 1 && text[0] == firstChar && text[text.Length - 1] == lastChar;
		}
	}
}

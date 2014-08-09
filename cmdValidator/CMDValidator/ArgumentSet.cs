using System;
using System.Collections.Generic;
using System.Linq;
namespace cmdValidator
{
	public class ArgumentSet
	{
		private ArgumentScheme[] _argSchemes;
		public event GetArguments GetArgs;
		public Dictionary<string, ArgumentScheme> ArgSchemeDic
		{
			get
			{
				Dictionary<string, ArgumentScheme> dictionary = new Dictionary<string, ArgumentScheme>();
				ArgumentScheme[] argSchemes = this._argSchemes;
				for (int i = 0; i < argSchemes.Length; i++)
				{
					ArgumentScheme argumentScheme = argSchemes[i];
					foreach (string current in argumentScheme.Identifiers)
					{
						dictionary.Add(current, argumentScheme);
					}
				}
				return dictionary;
			}
		}
		public ArgumentScheme[] ArgSchemes
		{
			get
			{
				return this._argSchemes;
			}
			set
			{
				this._argSchemes = value;
			}
		}
		public ArgumentSet(IEnumerable<ArgumentScheme> argSchemes, GetArguments getArgs)
		{
			this._argSchemes = argSchemes.Cast<ArgumentScheme>().ToArray<ArgumentScheme>();
			if (!this.IsValid())
			{
				throw new Exception("Es muss mindestens ein Argumentschema notwendig sein.");
			}
			this.GetArgs += getArgs;
		}
		public void TriggerEvent(string[] unknownOptions)
		{
			if (this.GetArgs != null)
			{
				this.GetArgs(this.ArgSchemeDic, unknownOptions);
			}
		}
		private bool IsValid()
		{
			ArgumentScheme[] argSchemes = this._argSchemes;
			bool result;
			for (int i = 0; i < argSchemes.Length; i++)
			{
				ArgumentScheme argumentScheme = argSchemes[i];
				if (!argumentScheme.IsOptional)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}
	}
}

using System;
using System.Collections.Generic;
namespace cmdValidator
{
	public delegate void GetArguments(Dictionary<string, ArgumentScheme> argSchemes, string[] unknownParameters);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cmdValidator
{
    public class ParameterValidator
    {
        private const string _argumentPattern = "\\A(?:(?:(?:\\w+)(?:\\[\\w+\\])?(?:\\w*))|(?:(?:\\[\\w+\\])(?:\\w+)))(?:\\|(?:(?:(?:\\w+)(?:\\[\\w+\\])?(?:\\w*))|(?:(?:\\[\\w+\\])(?:\\w+))))*(?:(?::\\([^|]+(?:\\|[^|]+)*\\))|(?:\\s*:[^|]+(?:\\|[^|]+)*))?\\Z";

        public bool Validate(string argumentSchemeString)
        {
            return Regex.IsMatch(argumentSchemeString, ParameterValidator._argumentPattern);
        }
    }
}

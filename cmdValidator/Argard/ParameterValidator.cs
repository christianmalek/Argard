using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Argard
{
    public class ParameterValidator
    {
        public bool Validate(string argumentSchemeString)
        {
            return Regex.IsMatch(argumentSchemeString, Pattern.ParameterPattern);
        }
    }
}

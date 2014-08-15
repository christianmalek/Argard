using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Argard
{
    static class Pattern
    {
        public const string IdentifierPattern = @"(?:(?:(\w+)(\[\w+\])?(\w*))|(?:(\[\w+\])(\w+)))";
        public const string ParameterPattern = @"\A" + IdentifierPattern + @"(?:\s*\|\s*" + IdentifierPattern + @")*(?:(?::\([^|]+(?:\|[^|]+)*\))|(?:\s*:[^|]+(?:\|[^|]+)*))?\Z";
    }
}
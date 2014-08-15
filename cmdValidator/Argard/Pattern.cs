using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Argard
{
    static class Pattern
    {
        public const string IdentifierPattern = @"\A(?:(?:(\w+)(\[\w+\])?(\w*))|(?:(\[\w+\])(\w+)))\Z";
        public const string NonBackReferencingIdentifierPattern = @"(?:(?:(?:\w+)(?:\[\w+\])?(?:\w*))|(?:(?:\[\w+\])(?:\w+)))";

        //important: doesnt match surrounding brackets!
        public const string ParameterPattern = @"\A" + NonBackReferencingIdentifierPattern + @"(?:\s*\|\s*" + NonBackReferencingIdentifierPattern + @")*(?:(?::\([^|]+(?:\|[^|]+)*\))|(?:\s*:[^|]+(?:\|[^|]+)*))?\Z";
    }
}
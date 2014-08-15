using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Argard.Exception
{
    public class InvalidParameterException : System.Exception
    {
        private const string MESSAGE_INVALID_ARGUMENT_SCHEME = "The argument scheme is invalid.";

        public InvalidParameterException(string message)
            : base(message)
        { }

        public InvalidParameterException()
            : base(MESSAGE_INVALID_ARGUMENT_SCHEME)
        { }
    }
}
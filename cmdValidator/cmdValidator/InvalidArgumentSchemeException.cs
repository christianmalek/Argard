using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmdValidator
{
    public class InvalidArgumentSchemeException : Exception
    {
        private const string MESSAGE_INVALID_ARGUMENT_SCHEME = "The argument scheme is invalid.";

        public InvalidArgumentSchemeException(string message)
            : base(message)
        { }

        public InvalidArgumentSchemeException()
            : base(MESSAGE_INVALID_ARGUMENT_SCHEME)
        { }
    }
}
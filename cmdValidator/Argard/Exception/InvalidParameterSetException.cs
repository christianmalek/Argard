using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Argard.Exception
{
    public class InvalidParameterSetException : System.Exception
    {
        private const string MESSAGE_INVALID_ARGUMENT_SET = "The argument set is invalid.";

        public InvalidParameterSetException(string message)
            : base(message)
        { }

        public InvalidParameterSetException()
            : base(MESSAGE_INVALID_ARGUMENT_SET)
        { }
    }
}
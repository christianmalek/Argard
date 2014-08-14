using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmdValidator.Exception
{
    public class InvalidArgumentSetException : System.Exception
    {
        private const string MESSAGE_INVALID_ARGUMENT_SET = "The argument set is invalid.";

        public InvalidArgumentSetException(string message)
            : base(message)
        { }

        public InvalidArgumentSetException()
            : base(MESSAGE_INVALID_ARGUMENT_SET)
        { }
    }
}
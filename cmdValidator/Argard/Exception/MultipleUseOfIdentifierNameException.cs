using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Argard.Exception
{
    public class MultipleUseOfIdentifierNameException : InvalidParameterSetException
    {
        private const string MESSAGE = "Illegal use of same identifier name in multiple argument schemes.";

        public MultipleUseOfIdentifierNameException(string message)
            : base(message)
        { }

        public MultipleUseOfIdentifierNameException()
            : base(MESSAGE)
        { }
    }
}
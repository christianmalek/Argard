using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmdValidator.Exception
{
    public class ConflictingFlagNamesException : InvalidArgumentSetException
    {
        private const string MESSAGE = "Some flag identifiers are in conflict with one or more identifiers.";

        public ConflictingFlagNamesException(string message)
            : base(message)
        { }
    }
}
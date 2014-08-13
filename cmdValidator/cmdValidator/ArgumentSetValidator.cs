using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmdValidator
{
    class ArgumentSetValidator
    {
        public void Validate(ArgumentScheme[] argumentSchemes)
        {
            bool hasRequiredArgumentScheme = HasRequiredArgumentScheme(argumentSchemes);
            bool isCmdRequired = IsCmdRequired(argumentSchemes);

            if(hasRequiredArgumentScheme == false)
                throw new InvalidArgumentSetException("At least one argument scheme is required.");

            if (isCmdRequired == false)
                throw new InvalidArgumentSetException("The command argument scheme must not be optional.");
        }

         //checks if at least one argument scheme is required
        private bool HasRequiredArgumentScheme(ArgumentScheme[] argumentSchemes)
        {
            foreach (var argScheme in argumentSchemes)
                if (argScheme.IsOptional == false)
                    return true;

            return false;
        }

        //checks if the cmd argument scheme is required
        private bool IsCmdRequired(ArgumentScheme[] argumentSchemes)
        {
            return argumentSchemes.Length > 0 && argumentSchemes[0].IsOptional == false;
        }
    }
}

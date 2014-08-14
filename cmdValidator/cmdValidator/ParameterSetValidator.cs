using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cmdValidator.Exception;

namespace cmdValidator
{
    class ParameterSetValidator
    {
        public void Validate(Parameter[] argumentSchemes)
        {
            bool hasRequiredArgumentScheme = HasRequiredArgumentScheme(argumentSchemes);
            bool isCmdRequired = IsCmdRequired(argumentSchemes);
            string collidingNonFlag = AreFlagsCollidingWithNonFlagNames(argumentSchemes);
            string nonUniqueIdentifier = AreIdentifiersUnique(argumentSchemes);

            if (hasRequiredArgumentScheme == false)
                throw new InvalidArgumentSetException("At least one argument scheme is required.");

            if (isCmdRequired == false)
                throw new InvalidArgumentSetException("The command argument scheme must not be optional.");

            if (collidingNonFlag != null)
                throw new ConflictingFlagNamesException("The flag attributes are colliding with following non flag identifier:" + collidingNonFlag);

            if (nonUniqueIdentifier != null)
                throw new MultipleUseOfIdentifierNameException("Following identfier is used in multiple argument schemes:" + nonUniqueIdentifier);
        }

        //checks if at least one argument scheme is required
        private bool HasRequiredArgumentScheme(Parameter[] argumentSchemes)
        {
            foreach (var argScheme in argumentSchemes)
                if (argScheme.IsOptional == false)
                    return true;

            return false;
        }

        //checks if the cmd argument scheme is required
        private bool IsCmdRequired(Parameter[] argumentSchemes)
        {
            return argumentSchemes.Length > 0 && argumentSchemes[0].IsOptional == false;
        }

        //checks if combined flag identifiers could conflict with other identifiers
        //to prevent ambiguous names
        private string AreFlagsCollidingWithNonFlagNames(Parameter[] argumentSchemes)
        {
            List<string> flags = GetFlags(argumentSchemes);
            List<string> nonFlags = GetNonFlags(argumentSchemes);

            foreach (var nonFlag in nonFlags)
            {
                bool isColliding = true;

                foreach (var letter in nonFlag)
                {

                    //if there is at leaste one not matching letter, every thing is ok
                    if (flags.Contains(Convert.ToString(letter)) == false)
                    {
                        isColliding = false;
                        break;
                    }
                }

                if (isColliding)
                    return nonFlag;
            }

            return null;
        }

        private List<string> GetFlags(Parameter[] argumentSchemes)
        {
            List<string> flags = new List<string>();

            foreach (var argScheme in argumentSchemes)
                if (argScheme.IsFlag)
                    foreach (var identifier in argScheme.Identifiers)
                        flags.Add(identifier);

            return flags;
        }

        private List<string> GetNonFlags(Parameter[] argumentSchemes)
        {
            List<string> nonFlags = new List<string>();

            foreach (var argScheme in argumentSchemes)
                if(argScheme.IsFlag == false)
                    foreach (var identifier in argScheme.Identifiers)
                        nonFlags.Add(identifier);

            return nonFlags;
        }

        private string AreIdentifiersUnique(Parameter[] argumentSchemes)
        {
            for (int i = 0; i < argumentSchemes.Length; i++)
            {
                List<string> identifiers = argumentSchemes[i].Identifiers;

                for (int j = 0; j < argumentSchemes.Length; j++)
                {
                    if(i == j)
                        continue;

                    foreach (var identifierToCompare in argumentSchemes[j].Identifiers)
                        if(identifiers.Contains(identifierToCompare))
                            return identifierToCompare;
                }
            }

            return null;
        }
    }
}

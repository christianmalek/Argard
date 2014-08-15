using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Argard
{
    class ValueParser
    {
        public ParameterValues GetValues(string valueScheme)
        {
            bool areValuesOptional = StringHelper.CheckFirstAndLastCharOfString('(', ')', valueScheme);

            if (areValuesOptional)
                valueScheme = StringHelper.RemoveLastAndFirstChar(valueScheme);

            valueScheme = valueScheme.ToLower();

            if (valueScheme == "^list" || valueScheme == "^l")
                return new ParameterValues(ValueType.List, areValuesOptional, new List<string>());
            else if (valueScheme == "^single" || valueScheme == "^s")
                return new ParameterValues(ValueType.Single, areValuesOptional, new List<string>());
            else
            {
                List<string> values = new List<string>(valueScheme.Split(new char[] { '|' }));

                //remove all spaces at the beginning and the end
                for (int i = 0; i < values.Count; i++)
                    values[i] = values[i].Trim();

                return new ParameterValues(ValueType.Single, areValuesOptional, values);
            }
        }
    }
}
